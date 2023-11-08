using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using NetCord.Gateway;
using NetCord.Services.ApplicationCommands;
using NetCord.Services.Commands;
using NetCord.Services.Interactions;
using NetCordAddons.Services.Creators;
using NetCordAddons.Services.ErrorHandling;
using NetCordAddons.Services.Models;
using NetCordAddons.Services.Validators;

namespace NetCordAddons.Services.Extensions;

public static class HostingExtensions
{
    private static bool _isClientAdded;

    public static IHostBuilder AddGatewayClient(this IHostBuilder hostBuilder,
        Func<IServiceProvider, GatewayClientSettings> settingsFactory, BotCallback? botCallback = null,
        Func<IServiceProvider, IErrorHandler>? errorHandler = null)
    {
        if (_isClientAdded) return hostBuilder;
        _isClientAdded = !_isClientAdded;
        hostBuilder.ConfigureServices(services =>
        {
            var settings = settingsFactory(services.BuildServiceProvider());
            services.AddClientServices(botCallback, errorHandler);
            services.AddSingleton<GatewayClient>(_ =>
                new GatewayClient(settings.Token, settings.GatewayClientConfiguration));
            services.AddSingleton<IInteractionCreator, GatewayInteractionCreator>();
            services.AddHostedService<GatewayClientBotService>();
        });

        return hostBuilder;
    }

    public static IHostBuilder AddShardedGatewayClient(this IHostBuilder hostBuilder,
        Func<IServiceProvider, ShardedGatewayClientSettings> settingsFactory,
        BotCallback? botCallback = null, Func<IServiceProvider, IErrorHandler>? globalErrorHandling = null)
    {
        if (_isClientAdded) return hostBuilder;
        _isClientAdded = !_isClientAdded;
        hostBuilder.ConfigureServices(services =>
        {
            var settings = settingsFactory(services.BuildServiceProvider());
            services.AddClientServices(botCallback, globalErrorHandling);
            services.AddSingleton<ShardedGatewayClient>(_ =>
                new ShardedGatewayClient(settings.Token, settings.ShardedGatewayClientConfiguration));
            services.AddSingleton<IInteractionCreator, ShardedGatewayInteractionCreator>();
            services.AddHostedService<ShardedGatewayClientBotService>();
        });

        return hostBuilder;
    }

    public static IHostBuilder AddApplicationCommand<TContext>(this IHostBuilder hostBuilder,
        Func<IServiceProvider, ApplicationCommandServiceConfiguration<TContext>>? configurationFactory = null)
        where TContext : IApplicationCommandContext
    {
        if (!_isClientAdded) return hostBuilder;
        hostBuilder
            .TryAddRequiredServices()
            .ConfigureServices(
                services =>
                {
                    services.TryAddSingleton<ApplicationCommandServiceManager>();
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
        if (!_isClientAdded) return hostBuilder;
        hostBuilder
            .TryAddRequiredServices()
            .ConfigureServices(
                services =>
                {
                    services.TryAddSingleton<ApplicationCommandServiceManager>();
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
        if (!_isClientAdded) return hostBuilder;
        hostBuilder
            .TryAddRequiredServices()
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
        Func<IServiceProvider, CommandSettings<TContext>> configurationFactory)
        where TContext : ICommandContext
    {
        if (!_isClientAdded) return hostBuilder;
        hostBuilder
            .TryAddRequiredServices()
            .ConfigureServices(
                services =>
                {
                    var commandSettings = configurationFactory(services.BuildServiceProvider());
                    services.TryAddSingleton<CommandSettings<TContext>>(_ => commandSettings);
                    services.TryAddSingleton<CommandService<TContext>>(_ =>
                        new CommandService<TContext>(commandSettings.CommandServiceConfiguration));
                });
        return hostBuilder;
    }

    private static IHostBuilder TryAddRequiredServices(this IHostBuilder hostBuilder)
    {
        hostBuilder
            .ConfigureServices(
                services =>
                {
                    services.TryAddSingleton<InteractionCreator>();
                    services.TryAddSingleton<IServiceValidator, ServiceValidator>();
                }
            );
        return hostBuilder;
    }

    private static IServiceCollection AddClientServices(this IServiceCollection services, BotCallback? botCallback,
        Func<IServiceProvider, IErrorHandler>? errorHandlerFunc)
    {
        botCallback ??= new BotCallback();
        services.AddSingleton<IServiceCollection>(_ => services);
        services.AddSingleton<BotCallback>(_ => botCallback);
        errorHandlerFunc ??= provider => new BaseErrorHandler(provider);
        services.AddSingleton(errorHandlerFunc);
        services.AddSingleton<ClientBotService>();
        return services;
    }
}