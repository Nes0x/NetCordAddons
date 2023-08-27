using System.Reflection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NetCord.Gateway;
using NetCordAddons.EventHandler.Common;

namespace NetCordAddons.EventHandler;

public class EventHandlerActivatorService : IHostedService
{
    private readonly GatewayClient _client;
    private readonly IEnumerable<EventModule> _eventModules;
    private readonly ILogger<EventHandlerActivatorService> _logger;

    public EventHandlerActivatorService(IEnumerable<EventModule> eventModules,
        ILogger<EventHandlerActivatorService> logger, GatewayClient client)
    {
        _eventModules = eventModules;
        _logger = logger;
        _client = client;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        ModifyEventModules();
        try
        {
            RegisterEvents();
        }
        catch (Exception e)
        {
            _logger.LogError("Your event implementation threw an exception. Class name {DeclaringTypeName}",
                e.InnerException.TargetSite.DeclaringType.Name);
        }

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    private void RegisterEvents()
    {
        foreach (var eventModule in _eventModules)
        foreach (var @event in eventModule.Events)
            switch (@event.EventType)
            {
                case EventType.Ready:
                    _client.Ready += args =>
                    {
                        return (ValueTask)@event.MethodInfo.Invoke(eventModule, new[]
                        {
                            args
                        });
                    };
                    break;
                case EventType.ApplicationCommandPermissionsUpdate:
                    _client.ApplicationCommandPermissionsUpdate += args =>
                    {
                        return (ValueTask)@event.MethodInfo.Invoke(eventModule, new[]
                        {
                            args
                        });
                    };
                    break;
                case EventType.AutoModerationRuleCreate:
                    _client.AutoModerationRuleCreate += args =>
                    {
                        return (ValueTask)@event.MethodInfo.Invoke(eventModule, new[]
                        {
                            args
                        });
                    };
                    break;
                case EventType.AutoModerationRuleUpdate:
                    _client.AutoModerationRuleCreate += args =>
                    {
                        return (ValueTask)@event.MethodInfo.Invoke(eventModule, new[]
                        {
                            args
                        });
                    };
                    break;
                case EventType.AutoModerationRuleDelete:
                    _client.AutoModerationRuleDelete += args =>
                    {
                        return (ValueTask)@event.MethodInfo.Invoke(eventModule, new[]
                        {
                            args
                        });
                    };
                    break;
                case EventType.AutoModerationActionExecution:
                    _client.AutoModerationActionExecution += args =>
                    {
                        return (ValueTask)@event.MethodInfo.Invoke(eventModule, new[]
                        {
                            args
                        });
                    };
                    break;
                case EventType.GuildChannelCreate:
                    _client.GuildChannelCreate += args =>
                    {
                        return (ValueTask)@event.MethodInfo.Invoke(eventModule, new[]
                        {
                            args
                        });
                    };
                    break;
                case EventType.GuildChannelUpdate:
                    _client.GuildChannelUpdate += args =>
                    {
                        return (ValueTask)@event.MethodInfo.Invoke(eventModule, new[]
                        {
                            args
                        });
                    };
                    break;
                case EventType.GuildChannelDelete:
                    _client.GuildChannelDelete += args =>
                    {
                        return (ValueTask)@event.MethodInfo.Invoke(eventModule, new[]
                        {
                            args
                        });
                    };
                    break;
                case EventType.ChannelPinsUpdate:
                    _client.ChannelPinsUpdate += args =>
                    {
                        return (ValueTask)@event.MethodInfo.Invoke(eventModule, new[]
                        {
                            args
                        });
                    };
                    break;
                case EventType.GuildThreadCreate:
                    _client.GuildThreadCreate += args =>
                    {
                        return (ValueTask)@event.MethodInfo.Invoke(eventModule, new[]
                        {
                            args
                        });
                    };
                    break;
                case EventType.GuildThreadUpdate:
                    _client.GuildThreadUpdate += args =>
                    {
                        return (ValueTask)@event.MethodInfo.Invoke(eventModule, new[]
                        {
                            args
                        });
                    };
                    break;
                case EventType.GuildThreadDelete:
                    _client.GuildThreadDelete += args =>
                    {
                        return (ValueTask)@event.MethodInfo.Invoke(eventModule, new[]
                        {
                            args
                        });
                    };
                    break;
                case EventType.GuildThreadListSync:
                    _client.GuildThreadListSync += args =>
                    {
                        return (ValueTask)@event.MethodInfo.Invoke(eventModule, new[]
                        {
                            args
                        });
                    };
                    break;
                case EventType.GuildThreadUserUpdate:
                    _client.GuildThreadUserUpdate += args =>
                    {
                        return (ValueTask)@event.MethodInfo.Invoke(eventModule, new[]
                        {
                            args
                        });
                    };
                    break;
                case EventType.GuildThreadUsersUpdate:
                    _client.GuildThreadUsersUpdate += args =>
                    {
                        return (ValueTask)@event.MethodInfo.Invoke(eventModule, new[]
                        {
                            args
                        });
                    };
                    break;
                case EventType.GuildCreate:
                    _client.GuildCreate += args =>
                    {
                        return (ValueTask)@event.MethodInfo.Invoke(eventModule, new[]
                        {
                            args
                        });
                    };
                    break;
                case EventType.GuildUpdate:
                    _client.GuildUpdate += args =>
                    {
                        return (ValueTask)@event.MethodInfo.Invoke(eventModule, new[]
                        {
                            args
                        });
                    };
                    break;
                case EventType.GuildDelete:
                    _client.GuildDelete += args =>
                    {
                        return (ValueTask)@event.MethodInfo.Invoke(eventModule, new[]
                        {
                            args
                        });
                    };
                    break;
                case EventType.GuildAuditLogEntryCreate:
                    _client.GuildAuditLogEntryCreate += args =>
                    {
                        return (ValueTask)@event.MethodInfo.Invoke(eventModule, new[]
                        {
                            args
                        });
                    };
                    break;
                case EventType.GuildBanAdd:
                    _client.GuildBanAdd += args =>
                    {
                        return (ValueTask)@event.MethodInfo.Invoke(eventModule, new[]
                        {
                            args
                        });
                    };
                    break;
                case EventType.GuildBanRemove:
                    _client.GuildBanRemove += args =>
                    {
                        return (ValueTask)@event.MethodInfo.Invoke(eventModule, new[]
                        {
                            args
                        });
                    };
                    break;
                case EventType.GuildEmojisUpdate:
                    _client.GuildEmojisUpdate += args =>
                    {
                        return (ValueTask)@event.MethodInfo.Invoke(eventModule, new[]
                        {
                            args
                        });
                    };
                    break;
                case EventType.GuildStickersUpdate:
                    _client.GuildStickersUpdate += args =>
                    {
                        return (ValueTask)@event.MethodInfo.Invoke(eventModule, new[]
                        {
                            args
                        });
                    };
                    break;
                case EventType.GuildIntegrationsUpdate:
                    _client.GuildIntegrationsUpdate += args =>
                    {
                        return (ValueTask)@event.MethodInfo.Invoke(eventModule, new[]
                        {
                            args
                        });
                    };
                    break;
                case EventType.GuildUserAdd:
                    _client.GuildUserAdd += args =>
                    {
                        return (ValueTask)@event.MethodInfo.Invoke(eventModule, new[]
                        {
                            args
                        });
                    };
                    break;
                case EventType.GuildUserUpdate:
                    _client.GuildUserUpdate += args =>
                    {
                        return (ValueTask)@event.MethodInfo.Invoke(eventModule, new[]
                        {
                            args
                        });
                    };
                    break;
                case EventType.GuildUserRemove:
                    _client.GuildUserRemove += args =>
                    {
                        return (ValueTask)@event.MethodInfo.Invoke(eventModule, new[]
                        {
                            args
                        });
                    };
                    break;
                case EventType.GuildUserChunk:
                    _client.GuildUserChunk += args =>
                    {
                        return (ValueTask)@event.MethodInfo.Invoke(eventModule, new[]
                        {
                            args
                        });
                    };
                    break;
                case EventType.RoleCreate:
                    _client.RoleCreate += args =>
                    {
                        return (ValueTask)@event.MethodInfo.Invoke(eventModule, new[]
                        {
                            args
                        });
                    };
                    break;
                case EventType.RoleUpdate:
                    _client.RoleUpdate += args =>
                    {
                        return (ValueTask)@event.MethodInfo.Invoke(eventModule, new[]
                        {
                            args
                        });
                    };
                    break;
                case EventType.RoleDelete:
                    _client.RoleDelete += args =>
                    {
                        return (ValueTask)@event.MethodInfo.Invoke(eventModule, new[]
                        {
                            args
                        });
                    };
                    break;
                case EventType.GuildScheduledEventCreate:
                    _client.GuildScheduledEventCreate += args =>
                    {
                        return (ValueTask)@event.MethodInfo.Invoke(eventModule, new[]
                        {
                            args
                        });
                    };
                    break;
                case EventType.GuildScheduledEventUpdate:
                    _client.GuildScheduledEventUpdate += args =>
                    {
                        return (ValueTask)@event.MethodInfo.Invoke(eventModule, new[]
                        {
                            args
                        });
                    };
                    break;
                case EventType.GuildScheduledEventDelete:
                    _client.GuildScheduledEventDelete += args =>
                    {
                        return (ValueTask)@event.MethodInfo.Invoke(eventModule, new[]
                        {
                            args
                        });
                    };
                    break;
                case EventType.GuildScheduledEventUserAdd:
                    _client.GuildScheduledEventUserAdd += args =>
                    {
                        return (ValueTask)@event.MethodInfo.Invoke(eventModule, new[]
                        {
                            args
                        });
                    };
                    break;
                case EventType.GuildScheduledEventUserRemove:
                    _client.GuildScheduledEventUserRemove += args =>
                    {
                        return (ValueTask)@event.MethodInfo.Invoke(eventModule, new[]
                        {
                            args
                        });
                    };
                    break;
                case EventType.GuildIntegrationCreate:
                    _client.GuildIntegrationCreate += args =>
                    {
                        return (ValueTask)@event.MethodInfo.Invoke(eventModule, new[]
                        {
                            args
                        });
                    };
                    break;
                case EventType.GuildIntegrationUpdate:
                    _client.GuildIntegrationUpdate += args =>
                    {
                        return (ValueTask)@event.MethodInfo.Invoke(eventModule, new[]
                        {
                            args
                        });
                    };
                    break;
                case EventType.GuildIntegrationDelete:
                    _client.GuildIntegrationDelete += args =>
                    {
                        return (ValueTask)@event.MethodInfo.Invoke(eventModule, new[]
                        {
                            args
                        });
                    };
                    break;
                case EventType.GuildInviteCreate:
                    _client.GuildInviteCreate += args =>
                    {
                        return (ValueTask)@event.MethodInfo.Invoke(eventModule, new[]
                        {
                            args
                        });
                    };
                    break;
                case EventType.GuildInviteDelete:
                    _client.GuildInviteDelete += args =>
                    {
                        return (ValueTask)@event.MethodInfo.Invoke(eventModule, new[]
                        {
                            args
                        });
                    };
                    break;
                case EventType.MessageCreate:
                    _client.MessageCreate += args =>
                    {
                        return (ValueTask)@event.MethodInfo.Invoke(eventModule, new[]
                        {
                            args
                        });
                    };
                    break;
                case EventType.MessageUpdate:
                    _client.MessageUpdate += args =>
                    {
                        return (ValueTask)@event.MethodInfo.Invoke(eventModule, new[]
                        {
                            args
                        });
                    };
                    break;
                case EventType.MessageDelete:
                    _client.MessageDelete += args =>
                    {
                        return (ValueTask)@event.MethodInfo.Invoke(eventModule, new[]
                        {
                            args
                        });
                    };
                    break;
                case EventType.MessageDeleteBulk:
                    _client.MessageDeleteBulk += args =>
                    {
                        return (ValueTask)@event.MethodInfo.Invoke(eventModule, new[]
                        {
                            args
                        });
                    };
                    break;
                case EventType.MessageReactionAdd:
                    _client.MessageReactionAdd += args =>
                    {
                        return (ValueTask)@event.MethodInfo.Invoke(eventModule, new[]
                        {
                            args
                        });
                    };
                    break;
                case EventType.MessageReactionRemove:
                    _client.MessageReactionRemove += args =>
                    {
                        return (ValueTask)@event.MethodInfo.Invoke(eventModule, new[]
                        {
                            args
                        });
                    };
                    break;
                case EventType.MessageReactionRemoveAll:
                    _client.MessageReactionRemoveAll += args =>
                    {
                        return (ValueTask)@event.MethodInfo.Invoke(eventModule, new[]
                        {
                            args
                        });
                    };
                    break;
                case EventType.MessageReactionRemoveEmoji:
                    _client.MessageReactionRemoveEmoji += args =>
                    {
                        return (ValueTask)@event.MethodInfo.Invoke(eventModule, new[]
                        {
                            args
                        });
                    };
                    break;
                case EventType.PresenceUpdate:
                    _client.PresenceUpdate += args =>
                    {
                        return (ValueTask)@event.MethodInfo.Invoke(eventModule, new[]
                        {
                            args
                        });
                    };
                    break;
                case EventType.TypingStart:
                    _client.TypingStart += args =>
                    {
                        return (ValueTask)@event.MethodInfo.Invoke(eventModule, new[]
                        {
                            args
                        });
                    };
                    break;
                case EventType.CurrentUserUpdate:
                    _client.CurrentUserUpdate += args =>
                    {
                        return (ValueTask)@event.MethodInfo.Invoke(eventModule, new[]
                        {
                            args
                        });
                    };
                    break;
                case EventType.VoiceStateUpdate:
                    _client.VoiceStateUpdate += args =>
                    {
                        return (ValueTask)@event.MethodInfo.Invoke(eventModule, new[]
                        {
                            args
                        });
                    };
                    break;
                case EventType.VoiceServerUpdate:
                    _client.VoiceServerUpdate += args =>
                    {
                        return (ValueTask)@event.MethodInfo.Invoke(eventModule, new[]
                        {
                            args
                        });
                    };
                    break;
                case EventType.WebhooksUpdate:
                    _client.WebhooksUpdate += args =>
                    {
                        return (ValueTask)@event.MethodInfo.Invoke(eventModule, new[]
                        {
                            args
                        });
                    };
                    break;
                case EventType.InteractionCreate:
                    _client.InteractionCreate += args =>
                    {
                        return (ValueTask)@event.MethodInfo.Invoke(eventModule, new[]
                        {
                            args
                        });
                    };
                    break;
                case EventType.StageInstanceCreate:
                    _client.AutoModerationRuleDelete += args =>
                    {
                        return (ValueTask)@event.MethodInfo.Invoke(eventModule, new[]
                        {
                            args
                        });
                    };
                    break;
                case EventType.StageInstanceUpdate:
                    _client.StageInstanceUpdate += args =>
                    {
                        return (ValueTask)@event.MethodInfo.Invoke(eventModule, new[]
                        {
                            args
                        });
                    };
                    break;
                case EventType.StageInstanceDelete:
                    _client.StageInstanceDelete += args =>
                    {
                        return (ValueTask)@event.MethodInfo.Invoke(eventModule, new[]
                        {
                            args
                        });
                    };
                    break;
                case EventType.UnknownEvent:
                    _client.UnknownEvent += args =>
                    {
                        return (ValueTask)@event.MethodInfo.Invoke(eventModule, new[]
                        {
                            args
                        });
                    };
                    break;
            }
    }

    private void ModifyEventModules()
    {
        foreach (var eventModule in _eventModules)
        {
            var events = eventModule.GetType().GetMethods()
                .Where(m => m.GetCustomAttributes(typeof(EventAttribute), false).Length > 0)
                .Select(m => new Event
                    { MethodInfo = m, EventType = m.GetCustomAttribute<EventAttribute>()!.EventType })
                .ToList();
            eventModule.Events = events;
        }
    }
}