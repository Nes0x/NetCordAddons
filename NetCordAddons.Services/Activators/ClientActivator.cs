using NetCord.Gateway;
using NetCordAddons.Services.Configuration;
using NetCordAddons.Services.Models;

namespace NetCordAddons.Services.Activators;

internal class ClientActivator
{
    private readonly BotCallback _botCallback;
    private readonly IClientConfiguration _clientConfiguration;
    private readonly IServiceProvider _provider;

    public ClientActivator(IClientConfiguration clientConfiguration, BotCallback botCallback, IServiceProvider provider)
    {
        _clientConfiguration = clientConfiguration;
        _botCallback = botCallback;
        _provider = provider;
    }

    public async Task StartAsync(object clientAsObject)
    {
        _clientConfiguration.ConfigureBot();
        if (_botCallback.BeforeBotStart is not null) await _botCallback.BeforeBotStart.Invoke(_provider);
        GatewayClient clientToCommands = null!;
        switch (clientAsObject)
        {
            case GatewayClient client:
                await client.StartAsync();
                clientToCommands = client;
                break;
            case ShardedGatewayClient client:
                await client.StartAsync();
                clientToCommands = client[0];
                break;
        }

        await _clientConfiguration.AddCommandsIfAdded(clientToCommands);
        if (_botCallback.AfterBotStart is not null) await _botCallback.AfterBotStart.Invoke(_provider);
    }

    public async Task StopAsync(object clientAsObject)
    {
        if (_botCallback.BeforeBotClose is not null) await _botCallback.BeforeBotClose.Invoke(_provider);
        switch (clientAsObject)
        {
            case GatewayClient client:
                await client.CloseAsync();
                break;
            case ShardedGatewayClient client:
                await client.CloseAsync();
                break;
        }

        if (_botCallback.AfterBotClose is not null) await _botCallback.AfterBotClose.Invoke(_provider);
    }
}