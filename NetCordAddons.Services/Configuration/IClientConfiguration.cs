using NetCord.Gateway;

namespace NetCordAddons.Services.Configuration;

internal interface IClientConfiguration
{
    void ConfigureBot();
    Task AddCommandsIfAdded(GatewayClient client);
}