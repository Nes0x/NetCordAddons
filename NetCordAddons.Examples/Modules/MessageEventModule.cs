using NetCord.Gateway;
using NetCordAddons.EventHandler.Common;

namespace NetCordAddons.Examples.Modules;

public class MessageEventModule : EventModule
{
    [Event(EventType.MessageCreate)]
    public async ValueTask HandleMessageTyped(Message message)
    {
        if (!message.Author.IsBot) await message.ReplyAsync("Hi!");
    }
}