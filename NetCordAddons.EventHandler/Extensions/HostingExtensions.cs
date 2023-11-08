using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NetCord.Gateway;
using NetCordAddons.EventHandler.Activators;
using NetCordAddons.EventHandler.Common;

namespace NetCordAddons.EventHandler.Extensions;

public static class HostingExtensions
{
    public static IHostBuilder AddEventHandler(this IHostBuilder hostBuilder)
    {
        var assembly = Assembly.GetEntryAssembly()!;
        var types = assembly.GetTypes();
        var targetType = typeof(EventModule);

        hostBuilder.ConfigureServices(services =>
        {
            var gatewayClient = services.FirstOrDefault(s => s.ServiceType.IsAssignableTo(typeof(GatewayClient)));
            var shardedGatewayClient =
                services.FirstOrDefault(s => s.ServiceType.IsAssignableTo(typeof(ShardedGatewayClient)));
            if (gatewayClient is null && shardedGatewayClient is null) return;
            foreach (var type in types)
                if (type.IsAssignableTo(targetType) && !type.IsAbstract)
                    services.AddSingleton(targetType, type);
            services.AddSingleton<EventHandlerActivatorService>();

            if (gatewayClient is not null)
                services.AddHostedService<GatewayClientEventHandlerActivatorService>();
            else if (shardedGatewayClient is not null)
                services.AddHostedService<ShardedGatewayClientEventHandlerActivatorService>();
        });

        return hostBuilder;
    }
}