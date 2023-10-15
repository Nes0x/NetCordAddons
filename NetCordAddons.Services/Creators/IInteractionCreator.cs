using NetCordAddons.Services.Models;

namespace NetCordAddons.Services.Creators;

public interface IInteractionCreator
{
    void CreateTextCommandInteraction(string prefix, Service service);
    void CreateInteractions(List<Service> services);
}