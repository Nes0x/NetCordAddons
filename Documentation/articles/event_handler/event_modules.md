# Event modules
- To create event module, you must derive from EventModule class.
- Methods must have attribute Event which declaring what type of event will be handled.
- You can add many methods, which handling events, in one class.
- Methods must take parameters such as those returned by the event and must returning ValueTask.

GatewayClient example:  
```csharp
public class MessageEventModule : EventModule
{
    [Event(EventType.MessageCreate)]
    public ValueTask HandleMessageTyped(Message message)
    {
        return ValueTask.CompletedTask;
    }
    
    [Event(EventType.MessageUpdate)]
    public ValueTask HandleMessageUpdated(Message message)
    {
        return ValueTask.CompletedTask;
    }
}
```

ShardedGatewayClient example:
```csharp
public class MessageEventModule : EventModule
{
    [Event(EventType.MessageCreate)]
    public ValueTask HandleMessageTyped(GatewayClient client, Message message)
    {
        return ValueTask.CompletedTask;
    }
    
    [Event(EventType.MessageUpdate)]
    public ValueTask HandleMessageUpdated(GatewayClient client, Message message)
    {
        return ValueTask.CompletedTask;
    }
}
```