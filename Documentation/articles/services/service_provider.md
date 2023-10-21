# IServiceProvider

You can use services from your host in any of extensions from this library.

``WARNING: if you want use your own services, you must add those before using it``

Example:  
```csharp
var host = Host.CreateDefaultBuilder(args);
host
    .ConfigureServices(services => services.AddSingleton(ConfigService.Create()))
    //provider
    .AddGatewayClient(provider =>
    {
        var config = provider.GetRequiredService<ConfigService>();
        return new GatewayClientSettings(new Token(TokenType.Bot, config.Token));
    }, new BotCallback
    {
        //provider
        BeforeBotStart = provider =>
        {
            Console.WriteLine("Working");
            return Task.CompletedTask;
        }
    })
    //provider
    .AddCommand<CommandContext>(provider => new CommandSettings<CommandContext>("!"));
```