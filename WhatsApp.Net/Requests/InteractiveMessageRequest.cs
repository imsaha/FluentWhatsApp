using System.Text.Json.Serialization;
using WhatsApp.Net.Models.Interactive;

namespace WhatsApp.Net.Requests;

public record InteractiveMessageRequest : MessageRequest
{
    [JsonPropertyName("type")]
    public override string Type
    {
        get => "interactive";
    }

    [JsonPropertyName("interactive")]
    public required InteractiveObject Interactive { get; init; }
}
