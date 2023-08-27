using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NetCord.Gateway;
using NetCord.Services.ApplicationCommands;
using NetCord.Services.Commands;
using NetCord.Services.Interactions;
using NetCordAddons.Services.Models;

namespace NetCordAddons.Services;

public class GatewayClientBotService : IHostedService
{
    private readonly bool _areCommands;
    private readonly GatewayClient _client;
    private readonly IServiceCollection _collection;
    private readonly List<Interaction> _interactions;
    private readonly ILogger<GatewayClientBotService> _logger;
    private readonly IServiceProvider _provider;


    public GatewayClientBotService(GatewayClient client, IServiceCollection collection, IServiceProvider provider,
        ILogger<GatewayClientBotService> logger)
    {
        _client = client;
        _collection = collection;
        _provider = provider;
        _logger = logger;
        _areCommands =
            collection.FirstOrDefault(s => s.ServiceType.Name.ToLower().Contains("applicationcommandservicemanager")) is
                null
                ? false
                : true;
        _interactions = new List<Interaction>();
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        ConfigureBot();
        CreateInteractions();
        await _client.StartAsync();
        await _client.ReadyAsync;
        if (_areCommands)
        {
            var applicationCommandServiceManager = _provider.GetRequiredService<ApplicationCommandServiceManager>();
            await applicationCommandServiceManager.CreateCommandsAsync(_client.Rest, _client.ApplicationId!);
        }
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await _client.CloseAsync();
    }

    private void ConfigureBot()
    {
        var assembly = Assembly.GetEntryAssembly()!;
        foreach (var serviceDescriptor in _collection)
        {
            var serviceType = serviceDescriptor.ServiceType;
            var serviceName = serviceType.Name.ToLower();
            Type? type = null;
            if (serviceName.Contains("interactionservice"))
                type = typeof(InteractionService<>).MakeGenericType(serviceType.GenericTypeArguments);
            else if (serviceName.Equals("commandservice`1"))
                type = typeof(CommandService<>).MakeGenericType(serviceType.GenericTypeArguments);
            else if (serviceType.GenericTypeArguments.Length != 0 && serviceName.Contains("applicationcommandservice"))
                if (_areCommands)
                {
                    var applicationCommandServiceManager =
                        _provider.GetRequiredService<ApplicationCommandServiceManager>();
                    type = typeof(ApplicationCommandService<>).MakeGenericType(serviceType.GenericTypeArguments);
                    typeof(ApplicationCommandServiceManager).GetMethod("AddService")!
                        .MakeGenericMethod(serviceType.GenericTypeArguments).Invoke(applicationCommandServiceManager,
                            new[] { _provider.GetRequiredService(type) });
                }

            if (type is null) continue;
            var genericType = serviceType.GenericTypeArguments.First();
            var service = _provider.GetRequiredService(type);
            type.GetMethod("AddModules")!.Invoke(service, new object[] { assembly });
            _interactions.Add(new Interaction(type, genericType, service));
        }
    }

    private void CreateInteractions()
    {
        _interactions.ForEach(i =>
        {
            _client.InteractionCreate += interaction =>
            {
                try
                {
                    i.Type.GetMethod("ExecuteAsync")!.Invoke(i.Service, new[]
                    {
                        Activator.CreateInstance(i.GenericType, interaction, _client), _provider
                    });
                }
                catch (Exception e)
                {
                    _logger.LogError("{}", e);
                }

                return ValueTask.CompletedTask;
            };
        });
    }
}