using System.Text.Json.Serialization;
using FluentWhatsApp.Message.Models.Contacts;

namespace FluentWhatsApp.Message.Requests;

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
