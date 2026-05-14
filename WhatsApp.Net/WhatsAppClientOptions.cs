namespace WhatsApp.Net;

public sealed class WhatsAppClientOptions
{
    public string PhoneNumberId { get; set; } = "";
    public string AccessToken { get; set; } = "";
    public string ApiVersion { get; set; } = "v22.0";
}
