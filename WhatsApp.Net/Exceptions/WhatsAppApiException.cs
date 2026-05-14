using System.Text.Json.Serialization;

namespace WhatsApp.Net.Exceptions;

public sealed class WhatsAppApiException : Exception
{
    internal WhatsAppApiException(WhatsAppApiError error)
        : base(error.Message ?? $"WhatsApp API error (code {error.Code})")
    {
        ErrorCode = error.Code;
        ErrorType = error.Type;
        FbTraceId = error.FbTraceId;
    }
    public int ErrorCode { get; }
    public string? ErrorType { get; }
    public string? FbTraceId { get; }
}

internal record WhatsAppErrorResponse(
    [property: JsonPropertyName("error")] WhatsAppApiError? Error
);

internal record WhatsAppApiError
{
    [JsonPropertyName("message")]
    public string? Message { get; init; }

    [JsonPropertyName("type")]
    public string? Type { get; init; }

    [JsonPropertyName("code")]
    public int Code { get; init; }

    [JsonPropertyName("fbtrace_id")]
    public string? FbTraceId { get; init; }
}
