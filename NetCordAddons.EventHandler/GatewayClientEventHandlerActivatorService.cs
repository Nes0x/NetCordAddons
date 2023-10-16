using Microsoft.Extensions.Hosting;
using NetCord.Gateway;

namespace NetCordAddons.EventHandler;

public class GatewayClientEventHandlerActivatorService : IHostedService
{
    private readonly GatewayClient _client;
    private readonly EventHandlerActivatorService _handlerActivator;

    public GatewayClientEventHandlerActivatorService(GatewayClient client,
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