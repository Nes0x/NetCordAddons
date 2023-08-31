namespace NetCordAddons.Services.Models;

public class Interaction
{
    public Interaction(Type type, Type genericType, object service, string? interactionName = null)
    {
        Type = type;
        GenericType = genericType;
        Service = service;
        InteractionName = interactionName;
    }

    public Type Type { get; }
    public Type GenericType { get; }
    public object Service { get; }
    public string? InteractionName { get; }
}