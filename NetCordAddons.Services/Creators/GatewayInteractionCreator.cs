using NetCord.Gateway;
using NetCordAddons.Services.Models;

namespace NetCordAddons.Services.Creators;

public class GatewayInteractionCreator : IInteractionCreator
{
    private readonly GatewayClient _client;
    private readonly InteractionCreator _interactionCreator;

    public GatewayInteractionCreator(GatewayClient client, InteractionCreator interactionCreator)
    {
        _client = client;
        _interactionCreator = interactionCreator;
    }

    public void CreateTextCommandInteraction(string prefix, Service service)
    {
        _client.MessageCreate += message =>
        {
            _interactionCreator.CreateTextCommandInteraction(message, prefix, service, _client);
            return ValueTask.CompletedTask;
        };
    }

    public void CreateInteractions(List<Service> services)
    {
        services.ForEach(s =>
        {
            _client.InteractionCreate += interaction =>
            {
                _interactionCreator.CreateInteraction(interaction, s, _client);
                return ValueTask.CompletedTask;
            };
        });
    }
}