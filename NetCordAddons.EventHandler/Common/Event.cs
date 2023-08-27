using System.Reflection;

namespace NetCordAddons.EventHandler.Common;

public class Event
{
    internal MethodInfo MethodInfo { get; init; }
    internal EventType EventType { get; init; }
}