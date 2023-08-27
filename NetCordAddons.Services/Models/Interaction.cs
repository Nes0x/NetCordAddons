namespace NetCordAddons.Services.Models;

public class Interaction
{
    public Interaction(Type type, Type genericType, object service)
    {
        Type = type;
        GenericType = genericType;
        Service = service;
    }

    public Type Type { get; }
    public Type GenericType { get; }
    public object Service { get; }
}