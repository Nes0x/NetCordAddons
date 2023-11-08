using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NetCord;
using NetCord.Gateway;
using NetCord.Rest;
using NetCord.Services.ApplicationCommands;
using NetCord.Services.Commands;
using NetCord.Services.Interactions;
using NetCordAddons.EventHandler.Extensions;
using NetCordAddons.Examples.Services;
using NetCordAddons.Services.ErrorHandling;
using NetCordAddons.Services.Extensions;
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
        },
        new BotCallback
        {
            BeforeBotStart = _ =>
            {
                Console.WriteLine("Working");
                return Task.CompletedTask;
            }
        }, provider => new BaseErrorHandler(provider, embedsProperties: new[]
        {
            new EmbedProperties().WithColor(new Color(255, 0, 0)).WithTitle("Error!")
                .WithDescription("Exception was thrown: %exception%")
        }))
    .AddEventHandler()
    .AddInteraction<StringMenuInteractionContext>()
    .AddInteraction<ModalSubmitInteractionContext>()
    .AddCommand<CommandContext>(_ => new CommandSettings<CommandContext>("!"))
    .AddApplicationCommand<SlashCommandContext>()
    .AddApplicationCommand<UserCommandContext>();

var app = host.Build();
await app.RunAsync();