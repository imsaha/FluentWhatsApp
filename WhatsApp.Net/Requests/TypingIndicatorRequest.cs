using System.Text.Json.Serialization;

namespace WhatsApp.Net.Requests;

public record TypingIndicatorRequest(
    [property: JsonPropertyName("message_id")] string MessageId) : MessageRequest
{
    [JsonPropertyName("type")]
    public override string Type
    {
        get => "";
    }

    [JsonPropertyName("status")]
    public string Status
    {
        get => "read";
    }

    [JsonPropertyName("typing_indicator")]
    public WhatsAppTypingIndicator TypingIndicator { get; set; } = new WhatsAppTypingIndicator();
}
