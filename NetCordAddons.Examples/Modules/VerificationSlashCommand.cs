using NetCord;
using NetCord.Rest;
using NetCord.Services.ApplicationCommands;

namespace NetCordAddons.Examples.Modules;

public class VerificationSlashCommand : ApplicationCommandModule<SlashCommandContext>
{
    [SlashCommand("verification", "Send message.", DMPermission = false,
        DefaultGuildUserPermissions = Permissions.Administrator)]
    public async Task HandleAsync()
    {
        await Context.Channel.SendMessageAsync(new MessageProperties()
            .WithComponents(new[]
            {
                new ActionRowProperties(new[]
                {
                    new ActionButtonProperties("verification", "Verification", ButtonStyle.Success)
                })
            }));
        await RespondAsync(InteractionCallback.ChannelMessageWithSource(
            new InteractionMessageProperties().WithContent("Success").WithFlags(MessageFlags.Ephemeral)));
    }
}