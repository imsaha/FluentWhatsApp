using System.Text.Json.Serialization;

namespace FluentWhatsApp.Message.Models;

public record SendMessageResponse
{
    [JsonPropertyName("messaging_product")]
    public string? MessagingProduct { get; init; }

    [JsonPropertyName("contacts")]
    public IReadOnlyList<ResponseContact> Contacts { get; init; } = [];

    [JsonPropertyName("messages")]
    public IReadOnlyList<SentMessageId> Messages { get; init; } = [];

    public bool IsSuccessful
    {
        get => !string.IsNullOrWhiteSpace(FirstMessageId);
    }

    public string? FirstMessageId
    {
        get => Messages.Where(x => !string.IsNullOrWhiteSpace(x.Id)).Select(x => x.Id).FirstOrDefault();
    }
}

public record ResponseContact(
    [property: JsonPropertyName("input")] string Input,
    [property: JsonPropertyName("wa_id")] string WaId
);

public record SentMessageId(
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("message_status")] string? MessageStatus
);
