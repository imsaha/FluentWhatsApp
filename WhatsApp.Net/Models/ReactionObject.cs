using System.Text.Json.Serialization;

namespace WhatsApp.Net.Models;

public record ReactionObject(
    [property: JsonPropertyName("message_id")] string MessageId,
    [property: JsonPropertyName("emoji")] string Emoji
);
