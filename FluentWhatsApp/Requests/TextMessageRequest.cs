using System.Text.Json.Serialization;

namespace FluentWhatsApp.Requests;

public record TextMessageRequest : MessageRequest
{
    [JsonPropertyName("type")]
    public override string Type
    {
        get => "text";
    }

    [JsonPropertyName("text")]
    public required WhatsAppText Text { get; init; }
}

public record WhatsAppText(
    [property: JsonPropertyName("body")] string Body,
    [property: JsonPropertyName("preview_url")] bool PreviewUrl = false
);

public record WhatsAppTypingIndicator
{
    public string Type
    {
        get => "text";
    }
}
