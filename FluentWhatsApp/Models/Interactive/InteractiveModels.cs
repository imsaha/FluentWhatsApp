using System.Text.Json.Serialization;

namespace FluentWhatsApp.Models.Interactive;

public record InteractiveObject
{
    // "button" | "list" | "flow" | "catalog_message"
    [JsonPropertyName("type")]
    public required string InteractiveType { get; init; }

    [JsonPropertyName("header")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public InteractiveHeader? Header { get; init; }

    [JsonPropertyName("body")]
    public required InteractiveBody Body { get; init; }

    [JsonPropertyName("footer")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public InteractiveFooter? Footer { get; init; }

    // Runtime type drives serialization — each action variant is a different record.
    [JsonPropertyName("action")]
    public required object Action { get; init; }
}

public record InteractiveHeader
{
    // "text" | "image" | "video" | "document"
    [JsonPropertyName("type")]
    public required string Type { get; init; }

    [JsonPropertyName("text")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Text { get; init; }

    [JsonPropertyName("image")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public HeaderMedia? Image { get; init; }

    [JsonPropertyName("video")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public HeaderMedia? Video { get; init; }

    [JsonPropertyName("document")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public HeaderMedia? Document { get; init; }
}

public record HeaderMedia
{
    [JsonPropertyName("id")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Id { get; init; }

    [JsonPropertyName("link")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Link { get; init; }
}

public record InteractiveBody(
    [property: JsonPropertyName("text")] string Text
);

public record InteractiveFooter(
    [property: JsonPropertyName("text")] string Text
);

internal record ReplyButtonsAction(
    [property: JsonPropertyName("buttons")] IReadOnlyList<ReplyButton> Buttons
);

internal record ReplyButton(
    [property: JsonPropertyName("type")] string Type,
    [property: JsonPropertyName("reply")] ReplyButtonPayload Reply
)
{
    public static ReplyButton Create(string id, string title)
    {
        return new ReplyButton("reply", new ReplyButtonPayload(id, title));
    }
}

internal record ReplyButtonPayload(
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("title")] string Title
);

internal record ListAction
{
    [JsonPropertyName("button")]
    public required string Button { get; init; }

    [JsonPropertyName("sections")]
    public required IReadOnlyList<ListSection> Sections { get; init; }
}

internal record ListSection
{
    [JsonPropertyName("title")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Title { get; init; }

    [JsonPropertyName("rows")]
    public required IReadOnlyList<ListRow> Rows { get; init; }
}

internal record ListRow
{
    [JsonPropertyName("id")]
    public required string Id { get; init; }

    [JsonPropertyName("title")]
    public required string Title { get; init; }

    [JsonPropertyName("description")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Description { get; init; }
}

internal record CtaUrlAction
{
    [JsonPropertyName("name")]
    public string Name { get; } = "cta_url";

    [JsonPropertyName("parameters")]
    public required CtaUrlParameters Parameters { get; init; }
}

internal record CtaUrlParameters(
    [property: JsonPropertyName("display_text")] string DisplayText,
    [property: JsonPropertyName("url")] string Url
);

internal record FlowAction
{
    [JsonPropertyName("name")]
    public string Name { get; } = "flow";

    [JsonPropertyName("parameters")]
    public required FlowActionParameters Parameters { get; init; }
}

internal record FlowActionParameters
{
    [JsonPropertyName("flow_message_version")]
    public string FlowMessageVersion { get; init; } = "3";

    [JsonPropertyName("flow_token")]
    public required string FlowToken { get; init; }

    [JsonPropertyName("flow_id")]
    public required string FlowId { get; init; }

    [JsonPropertyName("flow_cta")]
    public required string FlowCta { get; init; }

    // "navigate" | "data_exchange"
    [JsonPropertyName("flow_action")]
    public string FlowActionType { get; init; } = "navigate";

    // "draft" omits this field (null = published)
    [JsonPropertyName("mode")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Mode { get; init; }
}

internal record CatalogAction
{
    [JsonPropertyName("name")]
    public string Name { get; } = "catalog_message";

    [JsonPropertyName("parameters")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public CatalogActionParameters? Parameters { get; init; }
}

internal record CatalogActionParameters(
    [property: JsonPropertyName("thumbnail_product_retailer_id")]
    string ThumbnailProductRetailerId
);
