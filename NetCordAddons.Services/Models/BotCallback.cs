namespace NetCordAddons.Services.Models;

public class BotCallback
{
    public BotCallback(Action<IServiceProvider>? beforeBotStart, Action<IServiceProvider>? afterBotStart,
        Action<IServiceProvider>? beforeBotClose, Action<IServiceProvider>? afterBotClose)
    {
        BeforeBotStart = beforeBotStart;
        AfterBotStart = afterBotStart;
        BeforeBotClose = beforeBotClose;
        AfterBotClose = afterBotClose;
    }

    public Action<IServiceProvider>? BeforeBotStart { get; }
    public Action<IServiceProvider>? AfterBotStart { get; }
    public Action<IServiceProvider>? BeforeBotClose { get; }
    public Action<IServiceProvider>? AfterBotClose { get; }
}