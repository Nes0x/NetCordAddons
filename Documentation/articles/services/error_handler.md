# Error handler

If you want to handle exceptions from interactions, you can do this by using base event handler.
You can also create your error handler, but you must implement IErrorHandler.

Example:
```csharp
var host = Host.CreateDefaultBuilder(args);
host
    .AddGatewayClient(provider =>
        {
            return new GatewayClientSettings(new Token(TokenType.Bot, ""), new GatewayClientConfiguration
            {
                Intents = GatewayIntents.All,
                ConnectionProperties = ConnectionPropertiesProperties.IOS,
                Presence = new PresenceProperties(UserStatusType.Online)
                {
                    Activities = new[] { new UserActivityProperties("Hey", UserActivityType.Streaming) }
                }
            });
        },
        errorHandler: provider => new BaseErrorHandler(provider, embedsProperties: new[]
        {
            new EmbedProperties().WithColor(new Color(255, 0, 0)).WithTitle("Error!")
                .WithDescription("Exception was thrown: %exception%")
        }));
```
