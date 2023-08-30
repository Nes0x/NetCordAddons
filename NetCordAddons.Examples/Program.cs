using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NetCord;
using NetCord.Gateway;
using NetCord.Services.ApplicationCommands;
using NetCord.Services.Commands;
using NetCordAddons.EventHandler;
using NetCordAddons.Examples.Services;
using NetCordAddons.Services;
using NetCordAddons.Services.Models;

var host = Host.CreateDefaultBuilder(args);
host
    .ConfigureServices(services => services.AddSingleton(ConfigService.Create()))
    .AddGatewayClient(provider =>
    {
        var config = provider.GetRequiredService<ConfigService>();
        return new GatewayClientSettings(new Token(TokenType.Bot, config.Token), new GatewayClientConfiguration
        {
            Intents = GatewayIntents.All,
            ConnectionProperties = ConnectionPropertiesProperties.IOS,
            Presence = new PresenceProperties(UserStatusType.Online)
            {
                Activities = new[] { new UserActivityProperties("Hey", UserActivityType.Streaming) }
            }
        });
    }, _ => { Console.WriteLine("Working"); })
    .AddEventHandler(ServiceLifetime.Singleton)
    .AddCommand<CommandContext>(_ => new CommandSettings<CommandContext>("!"))
    .AddApplicationCommand<SlashCommandContext>()
    .AddApplicationCommand<UserCommandContext>();

var app = host.Build();
await app.RunAsync();