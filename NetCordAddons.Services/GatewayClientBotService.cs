using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NetCord.Gateway;
using NetCord.Services.ApplicationCommands;
using NetCordAddons.Services.Models;

namespace NetCordAddons.Services;

public class GatewayClientBotService : IHostedService
{
    private readonly bool _areCommands;
    private readonly BotCallback _botCallback;
    private readonly GatewayClient _client;
    private readonly IServiceCollection _collection;
    private readonly ILogger<GatewayClientBotService> _logger;
    private readonly IServiceProvider _provider;


    public GatewayClientBotService(GatewayClient client, IServiceCollection collection, IServiceProvider provider,
        ILogger<GatewayClientBotService> logger, BotCallback botCallback)
    {
        _client = client;
        _collection = collection;
        _provider = provider;
        _logger = logger;
        _botCallback = botCallback;
        _areCommands =
            collection.FirstOrDefault(s => s.ServiceType.Name.ToLower().Contains("applicationcommandservicemanager")) is
                null
                ? false
                : true;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        ConfigureBot();
        _botCallback.BeforeBotStart?.Invoke(_provider);
        await _client.StartAsync();
        await _client.ReadyAsync;
        if (_areCommands)
        {
            var applicationCommandServiceManager = _provider.GetRequiredService<ApplicationCommandServiceManager>();
            await applicationCommandServiceManager.CreateCommandsAsync(_client.Rest, _client.ApplicationId!);
        }

        _botCallback.AfterBotStart?.Invoke(_provider);
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        _botCallback.BeforeBotClose?.Invoke(_provider);
        await _client.CloseAsync();
        _botCallback.AfterBotClose?.Invoke(_provider);
    }

    private void ConfigureBot()
    {
        var services = new List<Service>();
        var assembly = Assembly.GetEntryAssembly()!;
        string? textCommandPrefix = null;
        Service? textCommandService = null;
        foreach (var serviceDescriptor in _collection)
        {
            var serviceType = serviceDescriptor.ServiceType;
            var serviceName = serviceType.Name.ToLower();
            var addModules = ShouldBeAddedAsModules(serviceName, serviceType);
            if (addModules)
            {
                UpdateCommandData(serviceType, serviceName, ref textCommandPrefix, ref textCommandService);
                var addModulesMethod = serviceType.GetMethod("AddModules");
                if (addModulesMethod is null) continue;
                var genericType = serviceType.GenericTypeArguments.First();
                var service = _provider.GetRequiredService(serviceType);
                addModulesMethod.Invoke(service, new object[] { assembly });
                services.Add(new Service(serviceType, genericType, service, genericType.Name.Replace("Context", "")));
            }
           
        }
        if (textCommandPrefix is not null && textCommandService is not null)
            CreateTextCommandInteraction(textCommandPrefix, textCommandService);
        CreateInteractions(services);
    }

    private bool ShouldBeAddedAsModules(string serviceName, Type serviceType)
    {
        return serviceName.Contains("interactionservice") || serviceName.Equals("commandservice`1") ||
               serviceName.Equals("commandsettings`1")
               || _areCommands && serviceType.GenericTypeArguments.Length != 0 &&
               serviceName.Contains("applicationcommandservice");
    }

    private void UpdateCommandData(Type serviceType, string serviceName, ref string? textCommandPrefix, ref Service? textCommandService)
    {
        if (serviceName.Equals("commandservice`1")) textCommandService = new Service(serviceType, serviceType.GenericTypeArguments.First(),
            _provider.GetRequiredService(serviceType));
        else if (serviceName.Equals("commandsettings`1"))
            textCommandPrefix =
                serviceType.GetProperty("Prefix")!.GetValue(_provider.GetRequiredService(serviceType))!
                    .ToString();
        else if (_areCommands && serviceType.GenericTypeArguments.Length != 0 &&
                 serviceName.Contains("applicationcommandservice"))
        {
            var applicationCommandServiceManager =
                _provider.GetRequiredService<ApplicationCommandServiceManager>();
            typeof(ApplicationCommandServiceManager).GetMethod("AddService")!
                .MakeGenericMethod(serviceType.GenericTypeArguments).Invoke(applicationCommandServiceManager,
                    new[] { _provider.GetRequiredService(serviceType) });
        }
    }

    private void CreateTextCommandInteraction(string prefix, Service service)
    {
        _client.MessageCreate += message =>
        {
            if (message.Content.StartsWith(prefix))
                try
                {
                    service.Type.GetMethod("ExecuteAsync")!.Invoke(service.ServiceInstance, new[]
                    {
                        prefix.Length, Activator.CreateInstance(service.GenericType, message, _client), _provider
                    });
                }
                catch (Exception e)
                {
                    _logger.LogError("{E}", e);
                }

            return ValueTask.CompletedTask;
        };
    }

    private void CreateInteractions(List<Service> services)
    {
        services.ForEach(s =>
        {
            _client.InteractionCreate += interaction =>
            {
                try
                {
                     if (interaction.GetType().Name.StartsWith(s.InteractionName!))
                     {
                         s.Type.GetMethod("ExecuteAsync")!.Invoke(s.ServiceInstance, new[]
                         {
                             Activator.CreateInstance(s.GenericType, interaction, _client), _provider
                         });
                     }
                }
                catch (Exception e)
                {
                    _logger.LogError("{E}", e);
                }

                return ValueTask.CompletedTask;
            };
        });
    }
}