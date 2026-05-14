using System.Text.Json.Serialization;

namespace FluentWhatsApp.Models;

public record ReactionObject(
    [property: JsonPropertyName("message_id")] string MessageId,
    [property: JsonPropertyName("emoji")] string Emoji
);
