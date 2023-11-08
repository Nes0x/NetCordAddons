using NetCord.Rest;
using NetCord.Services.Interactions;
using NetCordAddons.Examples.Modals;

namespace NetCordAddons.Examples.Modules;

public class EmbedModal : InteractionModule<ModalSubmitInteractionContext>
{
    [Interaction("example")]
    public async Task HandleAsync()
    {
        var modal = new ExampleModal();
        modal.Load(Context.Components);
        await RespondAsync(InteractionCallback.Message($"You typed: {modal.ExampleProperty}"));
    }
}