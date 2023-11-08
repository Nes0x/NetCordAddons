using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NetCord;
using NetCord.Gateway;
using NetCord.Rest;

namespace NetCordAddons.Services.ErrorHandling;

public class BaseErrorHandler : IErrorHandler
{
    private readonly IEnumerable<EmbedProperties>? _embedsProperties;
    private readonly ILogger<BaseErrorHandler> _logger;
    private readonly string? _message;
    private readonly MessageFlags _messageFlags;

    public BaseErrorHandler(IServiceProvider provider, string? message = null,
        IEnumerable<EmbedProperties>? embedsProperties = null, bool ephemeral = false)
    {
        _logger = provider.GetRequiredService<ILogger<BaseErrorHandler>>();
        _message = message;
        _embedsProperties = embedsProperties;
        if (!ephemeral) return;
        _messageFlags = MessageFlags.Ephemeral;
    }

    public Task SendError(Message message, Exception exception)
    {
        if (string.IsNullOrWhiteSpace(_message) && _embedsProperties is null) return Task.CompletedTask;
        var exceptionMessage = exception.Message;
        message.Channel.SendMessageAsync(new MessageProperties().WithContent(GetChangedMessage(exceptionMessage))
            .WithEmbeds(GetChangedEmbedProperties(exceptionMessage))
            .WithMessageReference(new MessageReferenceProperties(message.Id))
        );
        return Task.CompletedTask;
    }

    public Task SendError(Interaction interaction, Exception exception)
    {
        if (string.IsNullOrWhiteSpace(_message) && _embedsProperties is null) return Task.CompletedTask;
        var exceptionMessage = exception.Message;
        interaction.SendResponseAsync(InteractionCallback.Message(
            new InteractionMessageProperties().WithContent(GetChangedMessage(exceptionMessage))
                .WithEmbeds(GetChangedEmbedProperties(exceptionMessage)).WithFlags(_messageFlags)));
        return Task.CompletedTask;
    }

    public void LogError(string message)
    {
        _logger.LogError("{message}", message);
    }

    private string? GetChangedMessage(string exceptionMessage)
    {
        return _message?.Replace("%exception%", exceptionMessage);
    }

    private IEnumerable<EmbedProperties>? GetChangedEmbedProperties(string exceptionMessage)
    {
        return _embedsProperties?.Select(embedProperties => new EmbedProperties
        {
            Description = string.IsNullOrWhiteSpace(embedProperties.Description)
                ? embedProperties.Description
                : embedProperties.Description.Replace("%exception%", exceptionMessage),
            Author = embedProperties.Author,
            Color = embedProperties.Color,
            Fields = embedProperties.Fields,
            Footer = embedProperties.Footer,
            Image = embedProperties.Image,
            Thumbnail = embedProperties.Thumbnail,
            Timestamp = embedProperties.Timestamp,
            Title = embedProperties.Title,
            Url = embedProperties.Url
        });
    }
}