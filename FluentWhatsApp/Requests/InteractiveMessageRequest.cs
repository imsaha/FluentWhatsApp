using System.Text.Json.Serialization;
using FluentWhatsApp.Models.Interactive;

namespace FluentWhatsApp.Requests;

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
