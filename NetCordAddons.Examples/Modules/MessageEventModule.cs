using NetCord.Gateway;
using NetCordAddons.EventHandler.Common;

namespace NetCordAddons.Examples.Modules;

public class MessageEventModule : EventModule
{
    [Event(EventType.MessageCreate)]
    public ValueTask HandleMessageTyped(Message message)
    {
        Console.WriteLine(message.Content);
        return ValueTask.CompletedTask;
    }
}