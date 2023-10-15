using Microsoft.Extensions.Hosting;
using NetCord.Gateway;

namespace NetCordAddons.Services;

public class ShardedGatewayClientBotService : IHostedService
{
    private readonly ShardedGatewayClient _client;
    private readonly ClientBotService _clientBot;

    public ShardedGatewayClientBotService(ShardedGatewayClient client, ClientBotService clientBot)
    {
        _client = client;
        _clientBot = clientBot;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await _clientBot.StartAsync(_client);
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await _clientBot.StopAsync(_client);
    }
}