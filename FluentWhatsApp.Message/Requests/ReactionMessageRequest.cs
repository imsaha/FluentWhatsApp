using System.Text.Json.Serialization;
using FluentWhatsApp.Message.Models;

namespace FluentWhatsApp.Message.Requests;

public record ReactionMessageRequest : MessageRequest
{
    [JsonPropertyName("type")]
    public override string Type
    {
        get => "reaction";
    }

    [JsonPropertyName("reaction")]
    public required ReactionObject Reaction { get; init; }
}
