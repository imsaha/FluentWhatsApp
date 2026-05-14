using System.Text.Json.Serialization;
using WhatsApp.Net.Models;

namespace WhatsApp.Net.Requests;

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
