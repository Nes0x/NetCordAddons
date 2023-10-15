namespace NetCordAddons.Services.Models;

public class BotCallback
{
    public Func<IServiceProvider, Task>? BeforeBotStart { get; init; }
    public Func<IServiceProvider, Task>? AfterBotStart { get; init; }
    public Func<IServiceProvider, Task>? BeforeBotClose { get; init; }
    public Func<IServiceProvider, Task>? AfterBotClose { get; init; }
}