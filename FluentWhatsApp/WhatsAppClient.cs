using System.Net.Http.Headers;
using System.Net.Http.Json;
using FluentWhatsApp.Exceptions;
using FluentWhatsApp.Models;
using FluentWhatsApp.Requests;

namespace FluentWhatsApp;

internal sealed class WhatsAppClient : IWhatsAppClient
{
    internal const string HttpClientName = "WhatsAppNet";

    private readonly HttpClient _http;
    private readonly WhatsAppClientOptions _options;

    public WhatsAppClient(HttpClient httpClient, WhatsAppClientOptions options)
    {
        _http = httpClient;
        _options = options;
    }

    public async Task<SendMessageResponse> SendAsync(
        MessageRequest request,
        CancellationToken cancellationToken = default)
    {
        using var req = new HttpRequestMessage(HttpMethod.Post, $"{_options.ApiVersion}/{_options.PhoneNumberId}/messages");
        req.Content = JsonContent.Create(request, request.GetType());
        req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _options.AccessToken);

        var response = await _http.SendAsync(req, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            var errorBody = await response.Content.ReadFromJsonAsync<WhatsAppErrorResponse>(cancellationToken);
            throw new WhatsAppApiException(errorBody?.Error ?? new WhatsAppApiError { Code = (int) response.StatusCode });
        }

        return (await response.Content.ReadFromJsonAsync<SendMessageResponse>(cancellationToken))!;
    }
}
