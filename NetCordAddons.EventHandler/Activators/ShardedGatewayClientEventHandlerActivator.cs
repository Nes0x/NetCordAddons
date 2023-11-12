using Microsoft.Extensions.Hosting;
using NetCord.Gateway;

namespace NetCordAddons.EventHandler.Activators;

internal class ShardedGatewayClientEventHandlerActivator : IHostedService
{
    private readonly ShardedGatewayClient _client;
    private readonly EventHandlerActivator _handlerActivator;

    public ShardedGatewayClientEventHandlerActivator(ShardedGatewayClient client,
        EventHandlerActivator handlerActivator)
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