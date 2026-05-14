using System.Text.Json.Serialization;
using WhatsApp.Net.Models.Template;

namespace WhatsApp.Net.Requests;

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
