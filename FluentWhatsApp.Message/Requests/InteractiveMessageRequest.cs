using System.Text.Json.Serialization;
using FluentWhatsApp.Message.Models.Interactive;

namespace FluentWhatsApp.Message.Requests;

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
