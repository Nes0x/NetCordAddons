# First steps

- You must have installed `Microsoft.Extensions.Hosting 7.0.1` in your project.
- You must using `.NET 6` or newer.
- You must install `NetCordAddons.Servies` and `NetCordAddons.EventHandler` if you want have better event handling.

First of all create host and add GatewayClient or ShardedGatewayClient.
```csharp
var host = Host.CreateDefaultBuilder(args);
host.AddGatewayClient(_ => new GatewayClientSettings(new Token(TokenType.Bot, "Your token")))
//or 
host.AddShardedGatewayClient(_ => new ShardedGatewayClientSettings(new Token(TokenType.Bot, "Your token")))
var app = host.Build();
await app.RunAsync();
```
