using NetCordAddons.Services.Models;

namespace NetCordAddons.Services.Creators;

internal interface IInteractionCreator
{
    void CreateTextCommandInteraction(string prefix, Service service);
    void CreateInteractions(List<Service> services);
}