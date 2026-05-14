using System.Text.Json.Serialization;
using FluentWhatsApp.Models.Template;

namespace FluentWhatsApp.Requests;

public record TemplateMessageRequest : MessageRequest
{
    [JsonPropertyName("type")]
    public override string Type
    {
        get => "template";
    }

    [JsonPropertyName("template")]
    public required TemplateObject Template { get; init; }
}
