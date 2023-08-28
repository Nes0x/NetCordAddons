namespace NetCordAddons.Services.Models;

public class BotCallback
{
    public Action? BeforeBotStart { get; }
    public Action? AfterBotStart { get; }
    public Action? BeforeBotClose { get; }
    public Action? AfterBotClose { get; }
    
    public BotCallback(Action? beforeBotStart, Action? afterBotStart, Action? beforeBotClose, Action? afterBotClose)
    {
        BeforeBotStart = beforeBotStart;
        AfterBotStart = afterBotStart;
        BeforeBotClose = beforeBotClose;
        AfterBotClose = afterBotClose;
    }

}