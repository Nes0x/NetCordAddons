using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using NetCord.Gateway;
using NetCord.Services.ApplicationCommands;
using NetCord.Services.Commands;
using NetCordAddons.Services.Creators;
using NetCordAddons.Services.Models;
using NetCordAddons.Services.Validators;

namespace NetCordAddons.Services;

internal class ClientBotService
{
    private readonly bool _areCommands;
    private readonly BotCallback _botCallback;
    private readonly IServiceCollection _collection;
    private readonly IInteractionCreator _interactionCreator;
    private readonly IServiceProvider _provider;
    private readonly IServiceValidator _serviceValidator;


    public ClientBotService(IServiceCollection collection, IServiceProvider provider, BotCallback botCallback,
        IInteractionCreator interactionCreator, IServiceValidator serviceValidator)
    {
        _collection = collection;
        _provider = provider;
        _botCallback = botCallback;
        _interactionCreator = interactionCreator;
        _serviceValidator = serviceValidator;
        _areCommands =
            collection.FirstOrDefault(s => s.ServiceType.IsAssignableTo(typeof(ApplicationCommandServiceManager))) is
                not null;
    }

    public async Task StartAsync(object client)
    {
        ConfigureBot();
        if (_botCallback.BeforeBotStart is not null) await _botCallback.BeforeBotStart.Invoke(_provider);

        switch (client)
        {
            case GatewayClient gatewayClient:
                await gatewayClient.StartAsync();
                await RunBot(gatewayClient);
                break;
            case ShardedGatewayClient shardedClient:
                await shardedClient.StartAsync();
                await RunBot(shardedClient[0]);
                break;
        }

        if (_botCallback.AfterBotStart is not null) await _botCallback.AfterBotStart.Invoke(_provider);
    }


    public async Task StopAsync(object client)
    {
        if (_botCallback.BeforeBotClose is not null) await _botCallback.BeforeBotClose.Invoke(_provider);
        switch (client)
        {
            case GatewayClient gatewayClient:
                await gatewayClient.CloseAsync();
                break;
            case ShardedGatewayClient shardedClient:
                await shardedClient.CloseAsync();
                break;
        }

        if (_botCallback.AfterBotClose is not null) await _botCallback.AfterBotClose.Invoke(_provider);
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
            var addModules = _serviceValidator.ShouldBeAddedAsModules(serviceType, _areCommands);
            if (!addModules) continue;
            UpdateCommandData(serviceType, ref textCommandPrefix, ref textCommandService);
            var addModulesMethod = serviceType.GetMethod("AddModules");
            if (addModulesMethod is null) continue;
            var genericType = serviceType.GenericTypeArguments.First();
            var service = _provider.GetRequiredService(serviceType);
            addModulesMethod.Invoke(service, new object[] { assembly });
            services.Add(new Service(serviceType, genericType, service, genericType.Name.Replace("Context", "")));
        }

        if (textCommandPrefix is not null && textCommandService is not null)
            _interactionCreator.CreateTextCommandInteraction(textCommandPrefix, textCommandService);
        _interactionCreator.CreateInteractions(services);
    }

    private void UpdateCommandData(Type serviceType, ref string? textCommandPrefix, ref Service? textCommandService)
    {
        var genericTypeDefinition = serviceType.GetGenericTypeDefinition();
        if (genericTypeDefinition.IsAssignableTo(typeof(CommandService<>).GetGenericTypeDefinition()))
        {
            textCommandService = new Service(serviceType, serviceType.GenericTypeArguments.First(),
                _provider.GetRequiredService(serviceType));
        }
        else if (genericTypeDefinition.IsAssignableTo(typeof(CommandSettings<>).GetGenericTypeDefinition()))
        {
            textCommandPrefix =
                serviceType.GetProperty("Prefix")!.GetValue(_provider.GetRequiredService(serviceType))!
                    .ToString();
        }
        else if (_areCommands &&
                 genericTypeDefinition.IsAssignableTo(typeof(ApplicationCommandService<>).GetGenericTypeDefinition()))
        {
            var applicationCommandServiceManager =
                _provider.GetRequiredService<ApplicationCommandServiceManager>();
            typeof(ApplicationCommandServiceManager).GetMethod("AddService")!
                .MakeGenericMethod(serviceType.GenericTypeArguments).Invoke(applicationCommandServiceManager,
                    new[] { _provider.GetRequiredService(serviceType) });
        }
    }

    private async Task RunBot(GatewayClient client)
    {
        await client.ReadyAsync;
        if (_areCommands)
        {
            var applicationCommandServiceManager = _provider.GetRequiredService<ApplicationCommandServiceManager>();
            await applicationCommandServiceManager.CreateCommandsAsync(client.Rest, client.ApplicationId);
        }
    }
}