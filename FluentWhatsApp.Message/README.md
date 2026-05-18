# FluentWhatsApp.Message

Fluent message-building API for the [WhatsApp Cloud API](https://developers.facebook.com/docs/whatsapp/cloud-api) (Meta).
Build any WhatsApp message request type using a chainable builder — with zero external dependencies.

This package **only builds** `MessageRequest` objects (plain serializable records). It does not make any HTTP calls.
You can serialize the result with `System.Text.Json` and send it with any HTTP client you already have.

> If you want a ready-made HTTP client with DI, resilience, and `IWhatsAppClient`, use the
> [`FluentWhatsApp`](https://www.nuget.org/packages/FluentWhatsApp) package instead — it depends on this one.

## Installation

```bash
dotnet add package FluentWhatsApp.Message
```

---

## Usage

Every message starts from `WhatsAppMessage.To(phoneNumber)` and ends with `.Build()` (or returns a `MessageRequest` directly for simple types like Reaction and CTA URL).

Build the request, serialize it, and POST it to the WhatsApp Cloud API with your own `HttpClient`:

```csharp
var request = WhatsAppMessage.To("+1234567890").Text("Hello!").Build();

var json = JsonSerializer.Serialize(request, new JsonSerializerOptions
{
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
});

using var http = new HttpClient();
http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ACCESS_TOKEN);

var response = await http.PostAsync(
    $"https://graph.facebook.com/v22.0/{PHONE_NUMBER_ID}/messages",
    new StringContent(json, Encoding.UTF8, "application/json"));
```

---

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

// By media ID (uploaded via the Media API)
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
// Returns a MessageRequest directly — no .Build() needed
var request = WhatsAppMessage
    .To("+1234567890")
    .Interactive("Visit our website")
    .CtaUrl("Open site", "https://example.com");
```

---

### Interactive — Flow

```csharp
// Returns a MessageRequest directly — no .Build() needed
var request = WhatsAppMessage
    .To("+1234567890")
    .Interactive("Fill in your details")
    .Flow("FLOW_TOKEN", "FLOW_ID", "Start");
```

---

### Interactive — Catalog

```csharp
// Returns a MessageRequest directly — no .Build() needed
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

```csharp
var request = WhatsAppMessage
    .To("+1234567890")
    .TypingIndicator("wamid.INCOMING_MESSAGE_ID")
    .Build();
```

---

## Reply context and callback data

Chain `.ReplyTo()` and/or `.WithCallbackData()` before the message-type method:

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
```

---

## License

MIT
