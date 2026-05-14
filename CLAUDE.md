# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

`WhatsApp.Net` is a .NET 10 class library wrapping the WhatsApp Cloud API (Meta). It targets `net10.0` with nullable reference types and implicit usings enabled. No external dependencies — serialization uses `System.Text.Json`.

## Build & Run Commands

```powershell
dotnet build WhatsApp.Net/WhatsApp.Net.csproj
dotnet restore WhatsApp.Net/WhatsApp.Net.csproj

# Run tests (once a test project exists)
dotnet test
dotnet test --filter "FullyQualifiedName~TestClassName.TestMethodName"
```

## Architecture

```
WhatsApp.Net/
├── Models/
│   ├── MediaObject.cs          shared media reference (id or link + caption + filename)
│   ├── LocationObject.cs
│   ├── ReactionObject.cs
│   ├── Contacts/ContactModels.cs     ContactData + nested sub-types
│   ├── Interactive/InteractiveModels.cs  InteractiveObject + all action variants
│   └── Template/TemplateModels.cs    TemplateObject + components + parameters
├── Requests/
│   ├── MessageRequest.cs       abstract base (messaging_product, to, type, context, biz_opaque_callback_data)
│   ├── TextMessageRequest.cs
│   ├── MediaMessageRequests.cs  Image / Audio / Video / Document / Sticker (one file, 5 records)
│   ├── LocationMessageRequest.cs
│   ├── ContactsMessageRequest.cs
│   ├── ReactionMessageRequest.cs
│   ├── InteractiveMessageRequest.cs
│   └── TemplateMessageRequest.cs
└── Builders/
    ├── MessageBuilder.cs        static entry point `Message.To()` + root `MessageBuilder`
    ├── TextMessageBuilder.cs
    ├── MediaMessageBuilder.cs   handles all 5 media types via internal MediaKind enum
    ├── LocationMessageBuilder.cs
    ├── ContactsMessageBuilder.cs  includes ContactBuilder (nested contact fluent builder)
    ├── InteractiveMessageBuilder.cs  header/footer setters + branches to sub-builders
    ├── ReplyButtonsBuilder.cs
    ├── ListMessageBuilder.cs    stateful section/row accumulator
    └── TemplateMessageBuilder.cs   includes TemplateParameterBuilder
```

### Request model pattern

`MessageRequest` is an abstract record. All concrete types override the abstract `Type` property to return the WhatsApp API type string and carry a payload property named after the type (e.g. `Image`, `Text`, `Location`).

`MessagingProduct` is always `"whatsapp"` (private init). `RecipientType` defaults to `"individual"`.

Serialization uses `System.Text.Json` with `[JsonPropertyName]`. Optional fields use `[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]`.

`InteractiveMessageRequest` holds an `InteractiveObject` whose `Action` property is typed as `object` — `System.Text.Json` serialises it by runtime type, so each action record (e.g. `ReplyButtonsAction`, `ListAction`, `CtaUrlAction`, `FlowAction`, `CatalogAction`) serialises correctly without a custom converter.

### Fluent builder pattern

Entry point is the static `Message` class:

```csharp
// Text
Message.To("+1234567890").Text("Hello!").Build()

// Media
Message.To("+1234567890").Image(link: "https://…", caption: "Hi").Build()
Message.To("+1234567890").Document(link: "https://…").WithFilename("doc.pdf").Build()

// Location
Message.To("+1234567890").Location(37.7749, -122.4194).WithName("SF").Build()

// Reaction
Message.To("+1234567890").Reaction("wamid.xxx", "👍")   // returns MessageRequest directly

// Reply buttons (max 3)
Message.To("+1234567890")
    .Interactive("Pick one").WithFooter("Tap to reply")
    .ReplyButtons().Add("y", "Yes").Add("n", "No").Build()

// List
Message.To("+1234567890")
    .Interactive("Choose a plan").List("View Plans")
    .Section("Monthly").Row("m1", "Basic", "$9/mo").Row("m2", "Pro", "$19/mo")
    .Section("Annual") .Row("a1", "Basic Annual", "$90/yr")
    .Build()

// CTA URL / Flow / Catalog
Message.To("+1234567890").Interactive("Visit us").CtaUrl("Open", "https://…")
Message.To("+1234567890").Interactive("Fill form").Flow("token", "flowId", "Start")
Message.To("+1234567890").Interactive("Shop").Catalog()

// Template
Message.To("+1234567890")
    .Template("order_confirm", "en_US")
    .AddBodyParameters(p => p.Text("John").Text("Order #42"))
    .AddButton(0, "url", p => p.Text("/order/42"))
    .Build()

// Contacts
Message.To("+1234567890")
    .Contacts()
    .Add(c => c.Name("John Doe", firstName: "John").Phone("+1999…").Email("j@…"))
    .Build()

// Reply context + callback data (set on root before type method)
Message.To("+1234567890").ReplyTo("wamid.…").WithCallbackData("ref_123").Text("Got it").Build()
```

`MessageBuilder.Apply<T>()` stamps `Context` and `BizOpaqueCallbackData` onto any completed request using a `with` expression.

`Message.ToGroup(groupId)` creates a builder with `recipient_type = "group"`.
