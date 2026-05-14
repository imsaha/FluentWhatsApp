using WhatsApp.Net.Models;
using WhatsApp.Net.Requests;

namespace WhatsApp.Net.Builders;

/// <summary>
///     Entry point for the fluent message-building API.
/// </summary>
/// <example>
///     var msg = Message.To("+1234567890").Text("Hello!").Build();
///     var img = Message.To("+1234567890").Image(link: "https://…").WithCaption("Hi").Build();
/// </example>
public static class WhatsAppMessage
{
    /// <summary>
    ///     Specifies the recipient of the message as an individual based on their phone number.
    /// </summary>
    /// <param name="phoneNumber">The phone number of the recipient in international format.</param>
    /// <returns>A <see cref="MessageBuilder" /> instance to continue building the message.</returns>
    public static MessageBuilder To(string phoneNumber)
    {
        return new MessageBuilder(phoneNumber, "individual");
    }

    /// <summary>
    ///     Specifies the recipient of the message as a group based on the group ID.
    /// </summary>
    /// <param name="groupId">The unique identifier of the group where the message will be sent.</param>
    /// <returns>A <see cref="MessageBuilder" /> instance to continue building the message.</returns>
    public static MessageBuilder ToGroup(string groupId)
    {
        return new MessageBuilder(groupId, "group");
    }
}

public sealed class MessageBuilder
{

    internal MessageBuilder(string recipient, string recipientType)
    {
        Recipient = recipient;
        RecipientType = recipientType;
    }

    internal string Recipient { get; private set; }
    internal string RecipientType { get; }
    internal MessageContext? Context { get; private set; }
    internal string? BizOpaqueCallbackData { get; private set; }

    public MessageBuilder To(string phoneNumber)
    {
        Recipient = phoneNumber;
        return this;
    }

    public MessageBuilder ReplyTo(string messageId)
    {
        Context = new MessageContext(messageId);
        return this;
    }

    public MessageBuilder WithCallbackData(string data)
    {
        BizOpaqueCallbackData = data;
        return this;
    }

    public TextMessageBuilder Text(string body, bool previewUrl = false)
    {
        return new TextMessageBuilder(this, body, previewUrl);
    }

    public ImageMessageBuilder Image(string? link = null, string? mediaId = null, string? caption = null)
    {
        return new ImageMessageBuilder(this, link, mediaId, caption);
    }

    public AudioMessageBuilder Audio(string? link = null, string? mediaId = null)
    {
        return new AudioMessageBuilder(this, link, mediaId);
    }

    public VideoMessageBuilder Video(string? link = null, string? mediaId = null, string? caption = null)
    {
        return new VideoMessageBuilder(this, link, mediaId, caption);
    }

    public DocumentMessageBuilder Document(string? link = null, string? mediaId = null,
        string? caption = null, string? filename = null)
    {
        return new DocumentMessageBuilder(this, link, mediaId, caption, filename);
    }

    public StickerMessageBuilder Sticker(string? link = null, string? mediaId = null)
    {
        return new StickerMessageBuilder(this, link, mediaId);
    }

    public LocationMessageBuilder Location(double latitude, double longitude)
    {
        return new LocationMessageBuilder(this, latitude, longitude);
    }

    public ReactionMessageRequest Reaction(string messageId, string emoji)
    {
        return Apply(new ReactionMessageRequest
        {
            To = Recipient,
            RecipientType = RecipientType,
            Reaction = new ReactionObject(messageId, emoji)
        });
    }

    public ContactsMessageBuilder Contacts()
    {
        return new ContactsMessageBuilder(this);
    }

    public InteractiveMessageBuilder Interactive(string bodyText)
    {
        return new InteractiveMessageBuilder(this, bodyText);
    }

    public TemplateMessageBuilder Template(string name, string languageCode)
    {
        return new TemplateMessageBuilder(this, name, languageCode);
    }

    internal T Apply<T>(T request) where T : MessageRequest
    {
        return request with
        {
            BizOpaqueCallbackData = BizOpaqueCallbackData,
            Context = Context
        };
    }

    public TypingIndicatorMessageBuilder TypingIndicator(string messageId)
    {
        return new TypingIndicatorMessageBuilder(this, messageId);
    }
}
