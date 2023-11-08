using NetCord;
using NetCord.Gateway;

namespace NetCordAddons.Services.ErrorHandling;

public interface IErrorHandler
{
    Task SendError(Message message, Exception exception);
    Task SendError(Interaction interaction, Exception exception);
    void LogError(string message);
}