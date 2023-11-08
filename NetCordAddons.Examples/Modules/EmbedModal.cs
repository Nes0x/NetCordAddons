using NetCord.Services.Interactions;
using NetCordAddons.Examples.Modals;

namespace NetCordAddons.Examples.Modules;

public class EmbedModal : InteractionModule<ModalSubmitInteractionContext>
{
    [Interaction("example")]
    public async Task HandleAsync(ulong id, string? image, string? thumbnailImage)
    {
        var modal = new ExampleModal();
        modal.Load(Context.Components, modal);
    }
}