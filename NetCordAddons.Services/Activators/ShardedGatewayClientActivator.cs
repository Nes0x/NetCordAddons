using Microsoft.Extensions.Hosting;
using NetCord.Gateway;

namespace NetCordAddons.Services.Activators;

internal class ShardedGatewayClientActivator : IHostedService
{
    private readonly ShardedGatewayClient _client;
    private readonly ClientActivator _clientActivator;

    public ShardedGatewayClientActivator(ShardedGatewayClient client, ClientActivator clientActivator)
    {
        _client = client;
        _clientActivator = clientActivator;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await _clientActivator.StartAsync(_client);
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await _clientActivator.StopAsync(_client);
    }
}