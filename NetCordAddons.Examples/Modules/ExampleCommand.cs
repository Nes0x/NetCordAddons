using NetCord.Services.Commands;

namespace NetCordAddons.Examples.Modules;

public class ExampleCommand : CommandModule<CommandContext>
{
    [Command("ping")]
    public Task PingAsync()
    {
        return ReplyAsync("pong!");
    }
}