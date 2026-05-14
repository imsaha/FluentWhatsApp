using System.Text.Json.Serialization;
using WhatsApp.Net.Models;

namespace WhatsApp.Net.Requests;

public record LocationMessageRequest : MessageRequest
{
    [JsonPropertyName("type")]
    public override string Type
    {
        get => "location";
    }

    [JsonPropertyName("location")]
    public required LocationObject Location { get; init; }
}
