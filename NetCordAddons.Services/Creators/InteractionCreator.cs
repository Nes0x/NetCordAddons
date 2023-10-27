using NetCord;
using NetCord.Gateway;
using NetCordAddons.Services.ErrorHandling;
using NetCordAddons.Services.Models;

namespace NetCordAddons.Services.Creators;

public class InteractionCreator
{
    private readonly GlobalErrorHandling _globalErrorHandling;
    private readonly IServiceProvider _provider;

    public InteractionCreator(IServiceProvider provider, GlobalErrorHandling globalErrorHandling)
    {
        _provider = provider;
        _globalErrorHandling = globalErrorHandling;
    }

    public async Task CreateTextCommandInteraction(Message message, string prefix, Service service, object client)
    {
        if (!message.Content.StartsWith(prefix)) return;
        try
        {
            await (Task)service.Type.GetMethod("ExecuteAsync")!.Invoke(service.ServiceInstance, new[]
            {
                prefix.Length, Activator.CreateInstance(service.GenericType, message, client), _provider
            });
        }
        catch (Exception e)
        {
            _globalErrorHandling.SendAndLogError(message, e);
        }
    }

    public async Task CreateInteraction(Interaction interaction, Service service, object client)
    {
        if (!interaction.GetType().Name.StartsWith(service.InteractionName!)) return;
        try
        {
            await (Task)service.Type.GetMethod("ExecuteAsync")!.Invoke(service.ServiceInstance, new[]
            {
                Activator.CreateInstance(service.GenericType, interaction, client), _provider
            });
        }
        catch (Exception e)
        {
            _globalErrorHandling.SendAndLogError(interaction, e);
        }
    }
}