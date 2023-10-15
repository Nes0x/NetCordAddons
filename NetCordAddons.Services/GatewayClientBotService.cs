using Microsoft.Extensions.Hosting;
using NetCord.Gateway;

namespace NetCordAddons.Services;

public class GatewayClientBotService : IHostedService
{
    private readonly GatewayClient _client;
    private readonly ClientBotService _clientBot;

    public GatewayClientBotService(GatewayClient client, ClientBotService clientBot)
    {
        _client = client;
        _clientBot = clientBot;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _clientBot.StartAsync(_client);
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _clientBot.StopAsync(_client);
        return Task.CompletedTask;
    }
}