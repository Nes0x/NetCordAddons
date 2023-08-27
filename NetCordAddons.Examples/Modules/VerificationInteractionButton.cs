using NetCord;
using NetCord.Rest;
using NetCord.Services.Interactions;

namespace NetCordAddons.Examples.Modules;

public class VerificationInteractionButton : InteractionModule<ButtonInteractionContext>
{
    [Interaction("verification")]
    public async Task HandleAsync()
    {
        await RespondAsync(InteractionCallback.ChannelMessageWithSource(
            new InteractionMessageProperties().WithContent("Verification").WithFlags(MessageFlags.Ephemeral)));
    }
}