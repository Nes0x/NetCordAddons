using Microsoft.Extensions.Logging;
using NetCord.Gateway;
using NetCordAddons.Services.Creators;
using NetCordAddons.Services.Models;

namespace NetCordAddons.Services.Creators;

public class ShardedGatewayInteractionCreator : IInteractionCreator
{
    private readonly ShardedGatewayClient _client;
    private readonly InteractionCreator _interactionCreator;

    public ShardedGatewayInteractionCreator(ShardedGatewayClient client, InteractionCreator interactionCreator)
    {
        _client = client;
        _interactionCreator = interactionCreator;
    }

    public void CreateTextCommandInteraction(string prefix, Service service)
    {
        _client.MessageCreate += (client, message) =>
        {
            _interactionCreator.CreateTextCommandInteraction(message, prefix, service, client);
            return ValueTask.CompletedTask;
        };
    }

    public void CreateInteractions(List<Service> services)
    {
        services.ForEach(s =>
        {
            _client.InteractionCreate += (client, interaction) =>
            {
                _interactionCreator.CreateInteraction(interaction, s, client);
                return ValueTask.CompletedTask;
            };
        });
    }
}