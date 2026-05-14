using System.Text.Json.Serialization;
using FluentWhatsApp.Models;

namespace FluentWhatsApp.Requests;

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
