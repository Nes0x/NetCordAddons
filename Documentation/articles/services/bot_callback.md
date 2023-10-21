# Bot callback 

If you want to do something before, after bot will be started or closed, you can do this.

Example: 
```csharp
var host = Host.CreateDefaultBuilder(args);
host.AddGatewayClient(_ => new GatewayClientSettings(new Token(TokenType.Bot, "Your token")),  
    new BotCallback
    {
        BeforeBotStart = _ =>
        {
            Console.WriteLine("Working");
            return Task.CompletedTask;
        }
    })
var app = host.Build();
await app.RunAsync();
```

