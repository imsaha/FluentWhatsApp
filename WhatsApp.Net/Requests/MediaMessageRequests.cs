using System.Text.Json.Serialization;
using WhatsApp.Net.Models;

namespace WhatsApp.Net.Requests;

public record ImageMessageRequest : MessageRequest
{
    [JsonPropertyName("type")]
    public override string Type
    {
        get => "image";
    }

    [JsonPropertyName("image")]
    public required MediaObject Image { get; init; }
}

public record AudioMessageRequest : MessageRequest
{
    [JsonPropertyName("type")]
    public override string Type
    {
        get => "audio";
    }

    [JsonPropertyName("audio")]
    public required MediaObject Audio { get; init; }
}

public record VideoMessageRequest : MessageRequest
{
    [JsonPropertyName("type")]
    public override string Type
    {
        get => "video";
    }

    [JsonPropertyName("video")]
    public required MediaObject Video { get; init; }
}

public record DocumentMessageRequest : MessageRequest
{
    [JsonPropertyName("type")]
    public override string Type
    {
        get => "document";
    }

    [JsonPropertyName("document")]
    public required MediaObject Document { get; init; }
}

public record StickerMessageRequest : MessageRequest
{
    [JsonPropertyName("type")]
    public override string Type
    {
        get => "sticker";
    }

    [JsonPropertyName("sticker")]
    public required MediaObject Sticker { get; init; }
}
