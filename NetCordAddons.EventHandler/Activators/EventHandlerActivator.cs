using System.Reflection;
using Microsoft.Extensions.Logging;
using NetCordAddons.EventHandler.Common;

namespace NetCordAddons.EventHandler.Activators;

internal class EventHandlerActivator
{
    private readonly IEnumerable<EventModule> _eventModules;
    private readonly ILogger<EventHandlerActivator> _logger;

    public EventHandlerActivator(IEnumerable<EventModule> eventModules,
        ILogger<EventHandlerActivator> logger)
    {
        _eventModules = eventModules;
        _logger = logger;
    }

    public Task StartAsync(object client)
    {
        ModifyEventModules();
        RegisterEvents(client);
        return Task.CompletedTask;
    }

    public Task StopAsync()
    {
        return Task.CompletedTask;
    }

    private void RegisterEvents(object client)
    {
        foreach (var eventModule in _eventModules)
        foreach (var @event in eventModule.Events)
        {
            var eventInfo = client.GetType().GetEvent(@event.EventType.ToString());
            var handlerType = eventInfo!.EventHandlerType!;
            Delegate handler;
            try
            {
                handler = Delegate.CreateDelegate(handlerType, eventModule, @event.MethodInfo);
            }
            catch (ArgumentException)
            {
                _logger.LogError(
                    $"Your {@event.MethodInfo.Name} method from {eventModule.GetType().Name} class has a bad signature.");
                continue;
            }

            eventInfo.AddEventHandler(client, handler);
        }
    }

    private void ModifyEventModules()
    {
        var eventModuleType = typeof(EventModule);
        foreach (var eventModule in _eventModules)
        {
            var events = eventModuleType.GetMethods()
                .Where(m => m.GetCustomAttributes(typeof(EventAttribute), false).Length > 0)
                .Select(m => new Event
                    { MethodInfo = m, EventType = m.GetCustomAttribute<EventAttribute>()!.EventType })
                .ToList();
            eventModule.Events = events;
        }
    }
}