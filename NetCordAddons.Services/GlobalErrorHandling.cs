using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NetCord;
using NetCord.Gateway;
using NetCord.Rest;

namespace NetCordAddons.Services;

public class GlobalErrorHandling
{
    private readonly ILogger<GlobalErrorHandling> _logger;

    public GlobalErrorHandling(IServiceProvider provider, string? message = null,
        IEnumerable<EmbedProperties>? embedProperties = null)
    {
        _logger = provider.GetRequiredService<ILogger<GlobalErrorHandling>>();
        Message = message;
        EmbedProperties = embedProperties;
    }

    public string? Message { get; }
    public IEnumerable<EmbedProperties>? EmbedProperties { get; }

    public Task SendAndLogError(Message message, Exception exception)
    {
        _logger.LogError("{exception}", exception);
        if (string.IsNullOrWhiteSpace(Message) && EmbedProperties is null) return Task.CompletedTask;

        var exceptionMessage = exception.Message;
        message.Channel.SendMessageAsync(new MessageProperties().WithContent(GetChangedMessage(exceptionMessage))
            .WithEmbeds(GetChangedEmbedProperties(exceptionMessage))
            .WithMessageReference(new MessageReferenceProperties(message.Id)));
        return Task.CompletedTask;
    }

    public Task SendAndLogError(Interaction interaction, Exception exception)
    {
        _logger.LogError("{exception}", exception);
        if (string.IsNullOrWhiteSpace(Message) && EmbedProperties is null) return Task.CompletedTask;

        var exceptionMessage = exception.Message;
        interaction.SendResponseAsync(InteractionCallback.ChannelMessageWithSource(
            new InteractionMessageProperties().WithContent(GetChangedMessage(exceptionMessage))
                .WithEmbeds(GetChangedEmbedProperties(exceptionMessage))));
        return Task.CompletedTask;
    }

    private string GetChangedMessage(string exceptionMessage)
    {
        return Message.Replace("%exception%", exceptionMessage);
    }

    private IEnumerable<EmbedProperties> GetChangedEmbedProperties(string exceptionMessage)
    {
        return EmbedProperties.Select(e => new EmbedProperties
        {
            Description = string.IsNullOrWhiteSpace(e.Description)
                ? e.Description
                : e.Description.Replace("%exception%", exceptionMessage),
            Author = e.Author,
            Color = e.Color,
            Fields = e.Fields,
            Footer = e.Footer,
            Image = e.Image,
            Thumbnail = e.Thumbnail,
            Timestamp = e.Timestamp,
            Title = e.Title,
            Url = e.Url
        });
    }
}