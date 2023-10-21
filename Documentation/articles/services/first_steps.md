## Requirements 
- Installed NetCordAddons.Services

# Services
- You can add any interaction, application command, command service 
- You can add many of services - like two called .AddInteraction, but they must be unique in generic type.
- You can also using your custom modules and contexts.

Example: 
```csharp
var host = Host.CreateDefaultBuilder(args);
host.AddGatewayClient(_ => new GatewayClientSettings(new Token(TokenType.Bot, "Your token")))
//by adding this, you added handling text command, like !ping
host.AddCommand<CommandContext>(_ => new CommandSettings<CommandContext>("!"))
//by adding this, you added handling slash command, like /ping
host.AddApplicationCommand<SlashCommandContext>()
//by adding this, you added handling buttons 
host.AddInteraction<ButtonInteractionContext>()
//by adding this, you added handling string menus 
host.AddInteraction<StringMenuInteractionContext>()
var app = host.Build();
await app.RunAsync();
```
