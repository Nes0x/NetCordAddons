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
            foreach (var type in types)
                if (type.IsAssignableTo(targetType) && !type.IsAbstract)
                    services.AddScoped(targetType, type);
            services.AddSingleton<EventHandlerActivatorService>();

            if (services.FirstOrDefault(s => s.ServiceType.IsAssignableTo(typeof(GatewayClient))) is not null)
                services.AddHostedService<GatewayClientEventHandlerActivatorService>();
            else if (services.FirstOrDefault(s => s.ServiceType.IsAssignableTo(typeof(ShardedGatewayClient))) is not
                     null)
                services.AddHostedService<ShardedGatewayClientEventHandlerActivatorService>();
        });

        return hostBuilder;
    }
}