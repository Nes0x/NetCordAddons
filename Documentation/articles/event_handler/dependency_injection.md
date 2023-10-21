# Dependency injection

If you don't want to use all services as singleton (even if they are added as scoped or transient), you must use IServiceProvider. 

Example: 
```csharp
public class MessageEventModule : EventModule
{
    private readonly IServiceProvider _provider;

    public MessageEventModule(IServiceProvider provider)
    {
        _provider = provider;
    }
    
    [Event(EventType.MessageCreate)]
    public ValueTask HandleMessageTyped(Message message)
    {
        var config = _provider.GetRequiredService<ConfigService>();
        return ValueTask.CompletedTask;
    }
    
    [Event(EventType.MessageUpdate)]
    public ValueTask HandleMessageUpdated(Message message)
    {
        return ValueTask.CompletedTask;
    }
}
```

Or if you want use all of those as singletons, you can inject them by constructor.

Example: 
```csharp
public class MessageEventModule : EventModule
{
    private readonly ConfigService _config;

    public MessageEventModule(ConfigService config)
    {
        _config = config;
    }
    
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
