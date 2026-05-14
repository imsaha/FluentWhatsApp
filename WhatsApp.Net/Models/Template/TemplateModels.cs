using System.Text.Json.Serialization;

namespace WhatsApp.Net.Models.Template;

public record TemplateObject
{
    [JsonPropertyName("name")]
    public required string Name { get; init; }

    [JsonPropertyName("language")]
    public required TemplateLanguage Language { get; init; }

    [JsonPropertyName("components")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<TemplateComponent>? Components { get; init; }
}

public record TemplateLanguage(
    [property: JsonPropertyName("code")] string Code
);

public record TemplateComponent
{
    // "header" | "body" | "button"
    [JsonPropertyName("type")]
    public required string Type { get; init; }

    // button only: "url" | "quick_reply" | "copy_code" | "catalog"
    [JsonPropertyName("sub_type")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? SubType { get; init; }

    // Required for button components (0-based index)
    [JsonPropertyName("index")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Index { get; init; }

    [JsonPropertyName("parameters")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<TemplateParameter> Parameters { get; init; } = [];
}

public record TemplateParameter
{
    // "text" | "image" | "document" | "video" | "currency" | "date_time" | "payload" | "phone_number" | "url"
    [JsonPropertyName("type")]
    public required string Type { get; init; }

    [JsonPropertyName("text")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Text { get; init; }

    [JsonPropertyName("payload")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Payload { get; init; }

    [JsonPropertyName("image")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public TemplateMediaObject? Image { get; init; }

    [JsonPropertyName("document")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public TemplateMediaObject? Document { get; init; }

    [JsonPropertyName("video")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public TemplateMediaObject? Video { get; init; }

    [JsonPropertyName("currency")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public CurrencyParameter? Currency { get; init; }

    [JsonPropertyName("date_time")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public DateTimeParameter? DateTime { get; init; }

    [JsonPropertyName("phone_number")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? PhoneNumber { get; init; }

    [JsonPropertyName("url")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Url { get; init; }
}

public record TemplateMediaObject
{
    [JsonPropertyName("id")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Id { get; init; }

    [JsonPropertyName("link")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Link { get; init; }

    [JsonPropertyName("caption")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Caption { get; init; }

    [JsonPropertyName("filename")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Filename { get; init; }
}

public record CurrencyParameter(
    [property: JsonPropertyName("fallback_value")] string FallbackValue,
    [property: JsonPropertyName("code")] string Code,
    [property: JsonPropertyName("amount_1000")] long Amount1000
);

public record DateTimeParameter(
    [property: JsonPropertyName("fallback_value")] string FallbackValue
);
