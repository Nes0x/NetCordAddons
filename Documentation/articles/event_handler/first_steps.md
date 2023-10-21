## Requirements
- Installed NetCordAddons.EventHandler

First of all add EventHandler to host.
```csharp
var host = Host.CreateDefaultBuilder(args);
host.AddGatewayClient(_ => new GatewayClientSettings(new Token(TokenType.Bot, "Your token")))
host.AddEventHandler();
var app = host.Build();
await app.RunAsync();
```
