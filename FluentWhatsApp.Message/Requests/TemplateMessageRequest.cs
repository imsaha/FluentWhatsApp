using System.Text.Json.Serialization;
using FluentWhatsApp.Message.Models.Template;

namespace FluentWhatsApp.Message.Requests;

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
