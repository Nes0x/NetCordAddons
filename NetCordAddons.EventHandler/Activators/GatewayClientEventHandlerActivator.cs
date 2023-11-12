using Microsoft.Extensions.Hosting;
using NetCord.Gateway;

namespace NetCordAddons.EventHandler.Activators;

internal class GatewayClientEventHandlerActivator : IHostedService
{
    private readonly GatewayClient _client;
    private readonly EventHandlerActivator _handlerActivator;

    public GatewayClientEventHandlerActivator(GatewayClient client,
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