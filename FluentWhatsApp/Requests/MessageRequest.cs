using System.Text.Json.Serialization;

namespace FluentWhatsApp.Requests;

public abstract record MessageRequest
{
    [JsonPropertyName("messaging_product")]
    [JsonInclude]
    public string MessagingProduct { get; private init; } = "whatsapp";

    [JsonPropertyName("recipient_type")]
    public string RecipientType { get; init; } = "individual";

    [JsonPropertyName("to")]
    public required string To { get; init; }

    [JsonPropertyName("type")]
    public abstract string Type { get; }

    [JsonPropertyName("biz_opaque_callback_data")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? BizOpaqueCallbackData { get; init; }

    [JsonPropertyName("context")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public MessageContext? Context { get; init; }
}

public record MessageContext(
    [property: JsonPropertyName("message_id")] string MessageId
);
