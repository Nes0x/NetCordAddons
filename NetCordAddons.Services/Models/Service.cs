namespace NetCordAddons.Services.Models;

public class Service
{
    public Service(Type type, Type genericType, object serviceInstance, string? interactionName = null)
    {
        Type = type;
        GenericType = genericType;
        ServiceInstance = serviceInstance;
        InteractionName = interactionName;
    }

    public Type Type { get; }
    public Type GenericType { get; }
    public object ServiceInstance { get; }
    public string? InteractionName { get; }
}