using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NetCordAddons.EventHandler.Common;

namespace NetCordAddons.EventHandler;

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
                    services.AddSingleton(targetType, type);

            services.AddHostedService<EventHandlerActivatorService>();
        });

        return hostBuilder;
    }
}