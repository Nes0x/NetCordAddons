namespace NetCordAddons.EventHandler.Common;

[AttributeUsage(AttributeTargets.Method)]
public class EventAttribute : Attribute
{
    public EventAttribute(EventType eventType)
    {
        EventType = eventType;
    }

    public EventType EventType { get; }
}