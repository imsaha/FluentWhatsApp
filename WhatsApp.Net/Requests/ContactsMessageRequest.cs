using System.Text.Json.Serialization;
using WhatsApp.Net.Models.Contacts;

namespace WhatsApp.Net.Requests;

public record ContactsMessageRequest : MessageRequest
{
    [JsonPropertyName("type")]
    public override string Type
    {
        get => "contacts";
    }

    [JsonPropertyName("contacts")]
    public required IReadOnlyList<ContactData> Contacts { get; init; }
}
