using System.Text.Json.Serialization;
using FluentWhatsApp.Models.Contacts;

namespace FluentWhatsApp.Requests;

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
