namespace WhatsApp.Net;

public interface IWhatsAppClientFactory
{
    IWhatsAppClient Create(WhatsAppClientOptions options);
    IWhatsAppClient Create(Action<WhatsAppClientOptions> configure);
}
