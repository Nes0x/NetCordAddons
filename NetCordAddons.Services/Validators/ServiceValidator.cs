using NetCord.Services.ApplicationCommands;
using NetCord.Services.Commands;
using NetCord.Services.Interactions;
using NetCordAddons.Services.Models;

namespace NetCordAddons.Services.Validators;

public class ServiceValidator
{
    public bool ShouldBeAddedAsModules(Type serviceType, bool areCommands)
    {
        if (!serviceType.IsGenericType) return false;
        var genericTypeDefinition = serviceType.GetGenericTypeDefinition();
        return genericTypeDefinition.IsAssignableTo(typeof(InteractionService<>).GetGenericTypeDefinition()) ||
               genericTypeDefinition.IsAssignableTo(typeof(CommandService<>).GetGenericTypeDefinition()) ||
               genericTypeDefinition.IsAssignableTo(typeof(CommandSettings<>).GetGenericTypeDefinition()) ||
               (areCommands &&
                genericTypeDefinition.IsAssignableTo(typeof(ApplicationCommandService<>).GetGenericTypeDefinition()));
    }
}