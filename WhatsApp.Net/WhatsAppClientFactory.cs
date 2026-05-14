namespace WhatsApp.Net;

internal sealed class WhatsAppClientFactory : IWhatsAppClientFactory
{
    private readonly IHttpClientFactory _httpClientFactory;

    public WhatsAppClientFactory(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public IWhatsAppClient Create(WhatsAppClientOptions options)
    {
        return new WhatsAppClient(
            _httpClientFactory.CreateClient(WhatsAppClient.HttpClientName),
            options);
    }

    public IWhatsAppClient Create(Action<WhatsAppClientOptions> configure)
    {
        var options = new WhatsAppClientOptions();
        configure(options);
        return Create(options);
    }
}
