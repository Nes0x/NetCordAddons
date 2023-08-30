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
        if (_botCallback.BeforeBotStart is not null) _botCallback.BeforeBotStart(_provider);
        await _client.StartAsync();
        await _client.ReadyAsync;
        if (_areCommands)
        {
            var applicationCommandServiceManager = _provider.GetRequiredService<ApplicationCommandServiceManager>();
            await applicationCommandServiceManager.CreateCommandsAsync(_client.Rest, _client.ApplicationId!);
        }

        if (_botCallback.AfterBotStart is not null) _botCallback.AfterBotStart(_provider);
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        if (_botCallback.BeforeBotClose is not null) _botCallback.BeforeBotClose(_provider);
        await _client.CloseAsync();
        if (_botCallback.AfterBotClose is not null) _botCallback.AfterBotClose(_provider);
    }

    private void ConfigureBot()
    {
        var interactions = new List<Interaction>();
        var assembly = Assembly.GetEntryAssembly()!;
        string? commandPrefix = null;
        Interaction? commandInteraction = null;
        foreach (var serviceDescriptor in _collection)
        {
            var serviceType = serviceDescriptor.ServiceType;
            var serviceName = serviceType.Name.ToLower();
            Type? type = null;
            if (serviceName.Contains("interactionservice"))
            {
                type = typeof(InteractionService<>).MakeGenericType(serviceType.GenericTypeArguments);
            }
            else if (serviceName.Equals("commandservice`1"))
            {
                type = typeof(CommandService<>).MakeGenericType(serviceType.GenericTypeArguments);
                commandInteraction = new Interaction(serviceType, serviceType.GenericTypeArguments.First(),
                    _provider.GetRequiredService(type));
            }
            else if (serviceName.Equals("commandsettings`1"))
            {
                commandPrefix = serviceType.GetProperty("Prefix")!.GetValue(_provider.GetRequiredService(serviceType))!
                    .ToString();
            }
            else if (_areCommands && serviceType.GenericTypeArguments.Length != 0 &&
                     serviceName.Contains("applicationcommandservice"))
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
            interactions.Add(new Interaction(type, genericType, service));
        }

        if (commandPrefix is not null && commandInteraction is not null)
            CreateCommandInteraction(commandPrefix, commandInteraction);
        CreateInteractions(interactions);
    }


    private void CreateCommandInteraction(string prefix, Interaction interaction)
    {
        _client.MessageCreate += message =>
        {
            if (message.Content.StartsWith(prefix))
                try
                {
                    interaction.Type.GetMethod("ExecuteAsync")!.Invoke(interaction.Service, new[]
                    {
                        prefix.Length, Activator.CreateInstance(interaction.GenericType, message, _client), _provider
                    });
                }
                catch (Exception e)
                {
                    _logger.LogError("{E}", e);
                }

            return ValueTask.CompletedTask;
        };
    }

    private void CreateInteractions(List<Interaction> interactions)
    {
        interactions.ForEach(i =>
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
                    _logger.LogError("{E}", e);
                }

                return ValueTask.CompletedTask;
            };
        });
    }
}