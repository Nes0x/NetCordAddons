using NetCord;
using NetCord.Rest;
using NetCord.Services.ApplicationCommands;

namespace NetCordAddons.Examples.Modules;

public class UserProfileCommand : ApplicationCommandModule<UserCommandContext>
{
    [UserCommand("Profile")]
    public async Task HandleAsync()
    {
        await RespondAsync(InteractionCallback.Message(
            new InteractionMessageProperties().WithContent("Your profile").WithFlags(MessageFlags.Ephemeral)));
    }
}