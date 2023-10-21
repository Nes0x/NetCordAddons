using Microsoft.Extensions.Hosting;
using NetCord.Gateway;

namespace NetCordAddons.EventHandler.Activators;

public class ShardedGatewayClientEventHandlerActivatorService : IHostedService
{
    private readonly ShardedGatewayClient _client;
    private readonly EventHandlerActivatorService _handlerActivator;

    public ShardedGatewayClientEventHandlerActivatorService(ShardedGatewayClient client,
        EventHandlerActivatorService handlerActivator)
    {
        _client = client;
        _handlerActivator = handlerActivator;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await _handlerActivator.StartAsync(_client);
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await _handlerActivator.StopAsync();
    }
}