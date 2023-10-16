using Microsoft.Extensions.Logging;
using NetCord;
using NetCord.Gateway;
using NetCordAddons.Services.Models;

namespace NetCordAddons.Services.Creators;

public class InteractionCreator
{
    private readonly ILogger<InteractionCreator> _logger;
    private readonly IServiceProvider _provider;

    public InteractionCreator(IServiceProvider provider, ILogger<InteractionCreator> logger)
    {
        _provider = provider;
        _logger = logger;
    }

    public void CreateTextCommandInteraction(Message message, string prefix, Service service, object client)
    {
        if (!message.Content.StartsWith(prefix)) return;
        try
        {
            service.Type.GetMethod("ExecuteAsync")!.Invoke(service.ServiceInstance, new[]
            {
                prefix.Length, Activator.CreateInstance(service.GenericType, message, client), _provider
            });
        }
        catch (Exception e)
        {
            _logger.LogError("{E}", e);
        }
    }

    public void CreateInteraction(Interaction interaction, Service service, object client)
    {
        try
        {
            if (interaction.GetType().Name.StartsWith(service.InteractionName!))
                service.Type.GetMethod("ExecuteAsync")!.Invoke(service.ServiceInstance, new[]
                {
                    Activator.CreateInstance(service.GenericType, interaction, client), _provider
                });
        }
        catch (Exception e)
        {
            _logger.LogError("{E}", e);
        }
    }
}