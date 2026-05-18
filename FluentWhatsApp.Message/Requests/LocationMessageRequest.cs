using System.Text.Json.Serialization;
using FluentWhatsApp.Message.Models;

namespace FluentWhatsApp.Message.Requests;

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
