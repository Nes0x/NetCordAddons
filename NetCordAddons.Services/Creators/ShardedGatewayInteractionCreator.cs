using NetCord.Gateway;
using NetCordAddons.Services.Models;

namespace NetCordAddons.Services.Creators;

internal class ShardedGatewayInteractionCreator : IInteractionCreator
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
        services.ForEach(service =>
        {
            _client.InteractionCreate += (client, interaction) =>
            {
                _interactionCreator.CreateInteraction(interaction, service, client);
                return ValueTask.CompletedTask;
            };
        });
    }
}