using NetCord;
using NetCord.Rest;
using NetCord.Services.ApplicationCommands;
using NetCordAddons.Examples.Modals;

namespace NetCordAddons.Examples.Modules;

public class VerificationSlashCommand : ApplicationCommandModule<SlashCommandContext>
{
    [SlashCommand("verification", "Send a message.", DMPermission = false,
        DefaultGuildUserPermissions = Permissions.Administrator)]
    public async Task HandleAsync()
    {
        var exampleModal = new ExampleModal();
        exampleModal.AddParameterToId(29482949249249);
        await RespondAsync(InteractionCallback.Modal(exampleModal.ToModalProperties()));
    }
}