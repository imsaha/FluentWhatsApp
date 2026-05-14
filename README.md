# FluentWhatsApp

A modern, fluent .NET 10 SDK for sending WhatsApp messages through
the [WhatsApp Cloud API](https://developers.facebook.com/docs/whatsapp/cloud-api?utm_source=chatgpt.com).
Built as a lightweight class library using `System.Text.Json` for serialization — with no external JSON dependencies.

## Installation

```bash
dotnet add package FluentWhatsApp
```

---

## Sending messages

Every message is built using the fluent `WhatsAppMessage` API and sent via `IWhatsAppClient`.

```csharp
// Build a request
var request = WhatsAppMessage.To("+1234567890").Text("Hello!").Build();

// Send it
SendMessageResponse response = await client.SendAsync(request);
Console.WriteLine(response.FirstMessageId); // wamid.xxx
```

### Using the interface helpers

```csharp
await client.SendToIndividualAsync("+1234567890", b =>
    b.Text("Hello!").Build());

await client.SendToGroupAsync("GROUP_ID", b =>
    b.Text("Hello group!").Build());
```

---

## Setup

Register the client in your DI container with your phone number ID and access token:

```csharp
builder.Services.AddWhatsAppClient(options =>
{
    options.PhoneNumberId = "YOUR_PHONE_NUMBER_ID";
    options.AccessToken   = "YOUR_ACCESS_TOKEN";
    // options.ApiVersion = "v22.0"; // default
});
```

Then inject `IWhatsAppClient` wherever you need it:

```csharp
public class NotificationService(IWhatsAppClient whatsApp)
{
    public Task NotifyAsync(string phone) =>
        whatsApp.SendToIndividualAsync(phone, b => b.Text("Hello from FluentWhatsApp!").Build());
}
```

### Resilience

The library registers a standard resilience pipeline (retry, circuit-breaker, timeouts) via
`Microsoft.Extensions.Http.Resilience`. Customise it through the optional second parameter:

```csharp
builder.Services.AddWhatsAppClient(
    options =>
    {
        options.PhoneNumberId = "...";
        options.AccessToken   = "...";
    },
    resilience =>
    {
        resilience.Retry.MaxRetryAttempts = 5;
    });
```

### Custom client

Bring your own `IWhatsAppClient` implementation (e.g., for testing):

```csharp
builder.Services.AddWhatsAppClient<MyCustomClient>();
```

## Message types

### Text

```csharp
var request = WhatsAppMessage
    .To("+1234567890")
    .Text("Hello, World!")
    .Build();
```

With URL preview:

```csharp
var request = WhatsAppMessage
    .To("+1234567890")
    .Text("Check this out: https://example.com", previewUrl: true)
    .Build();
```

---

### Image

```csharp
// By URL
var request = WhatsAppMessage
    .To("+1234567890")
    .Image(link: "https://example.com/image.jpg", caption: "Look at this!")
    .Build();

// By media ID (uploaded via Media API)
var request = WhatsAppMessage
    .To("+1234567890")
    .Image(mediaId: "MEDIA_ID")
    .Build();
```

---

### Audio

```csharp
var request = WhatsAppMessage
    .To("+1234567890")
    .Audio(link: "https://example.com/audio.mp3")
    .Build();
```

---

### Video

```csharp
var request = WhatsAppMessage
    .To("+1234567890")
    .Video(link: "https://example.com/video.mp4", caption: "Watch this")
    .Build();
```

---

### Document

```csharp
var request = WhatsAppMessage
    .To("+1234567890")
    .Document(link: "https://example.com/file.pdf", caption: "Your invoice")
    .WithFilename("invoice.pdf")
    .Build();
```

---

### Sticker

```csharp
var request = WhatsAppMessage
    .To("+1234567890")
    .Sticker(link: "https://example.com/sticker.webp")
    .Build();
```

---

### Location

```csharp
var request = WhatsAppMessage
    .To("+1234567890")
    .Location(37.7749, -122.4194)
    .WithName("San Francisco")
    .WithAddress("San Francisco, CA, USA")
    .Build();
```

---

### Reaction

```csharp
// Returns a MessageRequest directly — no .Build() needed
var request = WhatsAppMessage
    .To("+1234567890")
    .Reaction("wamid.MESSAGE_ID", "👍");

await client.SendAsync(request);
```

---

### Contacts

```csharp
var request = WhatsAppMessage
    .To("+1234567890")
    .Contacts()
    .Add(c => c
        .Name("John Doe", firstName: "John", lastName: "Doe")
        .Phone("+19998887777")
        .Email("john@example.com"))
    .Add(c => c
        .Name("Jane Doe", firstName: "Jane")
        .Phone("+19997776666", type: "WORK"))
    .Build();
```

---

### Interactive — Reply buttons

Up to 3 buttons.

```csharp
var request = WhatsAppMessage
    .To("+1234567890")
    .Interactive("Would you like to proceed?")
    .WithFooter("Tap a button to respond")
    .ReplyButtons()
    .Add("yes", "Yes, continue")
    .Add("no",  "No, cancel")
    .Build();
```

---

### Interactive — List

```csharp
var request = WhatsAppMessage
    .To("+1234567890")
    .Interactive("Choose a subscription plan")
    .WithHeader("Our Plans")
    .WithFooter("Prices are monthly")
    .List("View Plans")
    .Section("Monthly")
        .Row("m_basic", "Basic",   "$9/mo")
        .Row("m_pro",   "Pro",     "$19/mo")
        .Row("m_ultra", "Ultra",   "$49/mo")
    .Section("Annual")
        .Row("a_basic", "Basic Annual",  "$90/yr")
        .Row("a_pro",   "Pro Annual",    "$190/yr")
    .Build();
```

---

### Interactive — CTA URL button

```csharp
var request = WhatsAppMessage
    .To("+1234567890")
    .Interactive("Visit our website")
    .CtaUrl("Open site", "https://example.com");
```

---

### Interactive — Flow

```csharp
var request = WhatsAppMessage
    .To("+1234567890")
    .Interactive("Fill in your details")
    .Flow("FLOW_TOKEN", "FLOW_ID", "Start");
```

---

### Interactive — Catalog

```csharp
var request = WhatsAppMessage
    .To("+1234567890")
    .Interactive("Browse our catalog")
    .Catalog();
```

---

### Template

```csharp
var request = WhatsAppMessage
    .To("+1234567890")
    .Template("order_confirmation", "en_US")
    .AddBodyParameters(p => p
        .Text("John")
        .Text("Order #42")
        .Text("$59.99"))
    .AddButton(0, "url", p => p.Text("/orders/42"))
    .Build();
```

---

### Typing indicator

Show a "typing…" indicator before sending a message:

```csharp
var request = WhatsAppMessage
    .To("+1234567890")
    .TypingIndicator("wamid.INCOMING_MESSAGE_ID")
    .Build();

await client.SendAsync(request);
```

---

## Reply context and callback data

Chain `.ReplyTo()` and/or `.WithCallbackData()` before the message type method to attach a reply context or a business
opaque callback string:

```csharp
var request = WhatsAppMessage
    .To("+1234567890")
    .ReplyTo("wamid.ORIGINAL_MESSAGE_ID")
    .WithCallbackData("order_ref_42")
    .Text("Got your message!")
    .Build();
```

---

## Group messages

```csharp
var request = WhatsAppMessage
    .ToGroup("GROUP_ID")
    .Text("Hello everyone!")
    .Build();

await client.SendAsync(request);
```

Or use the helper:

```csharp
await client.SendToGroupAsync("GROUP_ID", b =>
    b.Text("Hello everyone!").Build());
```

---

## Multi-tenant / factory usage

When you need to send from multiple phone numbers at runtime, inject `IWhatsAppClientFactory`:

```csharp
public class MultiTenantSender(IWhatsAppClientFactory factory)
{
    public Task SendAsync(string phoneNumberId, string token, string to, string text)
    {
        var client = factory.Create(o =>
        {
            o.PhoneNumberId = phoneNumberId;
            o.AccessToken   = token;
        });

        return client.SendAsync(WhatsAppMessage.To(to).Text(text).Build());
    }
}
```

Register the factory-only overload (no static options required):

```csharp
builder.Services.AddWhatsAppClient();
```

---

## Error handling

Failed API calls throw `WhatsAppApiException`:

```csharp
try
{
    await client.SendAsync(request);
}
catch (WhatsAppApiException ex)
{
    Console.WriteLine($"Code: {ex.ErrorCode}");
    Console.WriteLine($"Type: {ex.ErrorType}");
    Console.WriteLine($"Trace: {ex.FbTraceId}");
    Console.WriteLine($"Message: {ex.Message}");
}
```

---

## Response

`SendMessageResponse` exposes the sent message ID and contact info returned by the API:

```csharp
var response = await client.SendAsync(request);

if (response.IsSuccessful)
    Console.WriteLine($"Sent: {response.FirstMessageId}");
```

---

## License

MIT
