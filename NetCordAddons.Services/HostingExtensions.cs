using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using NetCord.Gateway;
using NetCord.Services.ApplicationCommands;
using NetCord.Services.Commands;
using NetCord.Services.Interactions;
using NetCordAddons.Services.Models;

namespace NetCordAddons.Services;

public static class HostingExtensions
{
    public static IHostBuilder AddGatewayClient(this IHostBuilder hostBuilder,
        Func<IServiceProvider, GatewayClientSettings> settingsFactory, Action? beforeBotStart = null, Action? afterBotStart = null, Action? beforeBotClose = null, Action? afterBotClose = null)
    {
        hostBuilder.ConfigureServices(services =>
        {
            var settings = settingsFactory(services.BuildServiceProvider());
            services.AddSingleton<GatewayClient>(_ =>
                new GatewayClient(settings.Token, settings.GatewayClientConfiguration));
            services.AddSingleton<IServiceCollection>(_ => services);
            services.AddSingleton<BotCallback>(_ => new BotCallback(beforeBotStart, afterBotStart, beforeBotClose, afterBotClose));
            services.AddHostedService<GatewayClientBotService>();
        });
        
        return hostBuilder;
    }

    public static IHostBuilder AddApplicationCommand<TContext>(this IHostBuilder hostBuilder,
        Func<IServiceProvider, ApplicationCommandServiceConfiguration<TContext>>? configurationFactory = null)
        where TContext : IApplicationCommandContext
    {
        hostBuilder
            .TryAddApplicationCommandManager()
            .ConfigureServices(
                services =>
                {
                    ApplicationCommandServiceConfiguration<TContext>? applicationCommandServiceConfiguration = null;
                    if (configurationFactory is not null)
                        applicationCommandServiceConfiguration = configurationFactory(services.BuildServiceProvider());
                    services.TryAddSingleton<ApplicationCommandService<TContext>>(_ =>
                        new ApplicationCommandService<TContext>(applicationCommandServiceConfiguration));
                });

        return hostBuilder;
    }

    public static IHostBuilder AddApplicationCommand<TContext, TAutocompleteContext>(this IHostBuilder hostBuilder,
        Func<IServiceProvider, ApplicationCommandServiceConfiguration<TContext>>? configurationFactory = null)
        where TContext : IApplicationCommandContext
        where TAutocompleteContext : IAutocompleteInteractionContext
    {
        hostBuilder
            .TryAddApplicationCommandManager()
            .ConfigureServices(
                services =>
                {
                    ApplicationCommandServiceConfiguration<TContext>? applicationCommandServiceConfiguration = null;
                    if (configurationFactory is not null)
                        applicationCommandServiceConfiguration = configurationFactory(services.BuildServiceProvider());
                    services.TryAddSingleton<ApplicationCommandService<TContext, TAutocompleteContext>>(_ =>
                        new ApplicationCommandService<TContext, TAutocompleteContext>(
                            applicationCommandServiceConfiguration));
                });
        return hostBuilder;
    }

    public static IHostBuilder AddInteraction<TContext>(this IHostBuilder hostBuilder,
        Func<IServiceProvider, InteractionServiceConfiguration<TContext>>? configurationFactory = null)
        where TContext : InteractionContext
    {
        hostBuilder
            .ConfigureServices(
                services =>
                {
                    InteractionServiceConfiguration<TContext>? interactionServiceConfiguration = null;
                    if (configurationFactory is not null)
                        interactionServiceConfiguration = configurationFactory(services.BuildServiceProvider());
                    services.TryAddSingleton<InteractionService<TContext>>(_ =>
                        new InteractionService<TContext>(interactionServiceConfiguration));
                });
        return hostBuilder;
    }

    public static IHostBuilder AddCommand<TContext>(this IHostBuilder hostBuilder,
        Func<IServiceProvider, CommandServiceConfiguration<TContext>>? configurationFactory = null)
        where TContext : ICommandContext
    {
        hostBuilder
            .ConfigureServices(
                services =>
                {
                    CommandServiceConfiguration<TContext>? commandServiceConfiguration = null;
                    if (configurationFactory is not null)
                        commandServiceConfiguration = configurationFactory(services.BuildServiceProvider());
                    services.TryAddSingleton<CommandService<TContext>>(_ =>
                        new CommandService<TContext>(commandServiceConfiguration));
                });
        return hostBuilder;
    }

    private static IHostBuilder TryAddApplicationCommandManager(this IHostBuilder hostBuilder)
    {
        hostBuilder
            .ConfigureServices(
                services =>
                    services.TryAddSingleton<ApplicationCommandServiceManager>()
            );
        return hostBuilder;
    }
}