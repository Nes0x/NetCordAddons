using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NetCordAddons.EventHandler.Common;

namespace NetCordAddons.EventHandler;

public static class HostingExtensions
{
    public static IHostBuilder AddEventHandler(this IHostBuilder hostBuilder, ServiceLifetime serviceLifetime)
    {
        var assembly = Assembly.GetEntryAssembly()!;
        var types = assembly.GetTypes();
        var targetType = typeof(EventModule);


        hostBuilder.ConfigureServices(services =>
        {
            foreach (var type in types)
                if (type.IsAssignableTo(targetType) && !type.IsAbstract)
                    switch (serviceLifetime)
                    {
                        case ServiceLifetime.Transient:
                            services.AddTransient(targetType, type);
                            break;
                        case ServiceLifetime.Scoped:
                            services.AddScoped(targetType, type);
                            break;
                        case ServiceLifetime.Singleton:
                            services.AddSingleton(targetType, type);
                            break;
                    }


            services.AddHostedService<EventHandlerActivatorService>();
        });

        return hostBuilder;
    }
}