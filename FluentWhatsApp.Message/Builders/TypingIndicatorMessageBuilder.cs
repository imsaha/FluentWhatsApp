using FluentWhatsApp.Message.Requests;

namespace FluentWhatsApp.Message.Builders;

public sealed class TypingIndicatorMessageBuilder
{
    private readonly string _messageId;
    private readonly MessageBuilder _root;

    internal TypingIndicatorMessageBuilder(MessageBuilder root, string messageId)
    {
        _root = root;
        _messageId = messageId;
    }

    public TypingIndicatorRequest Build()
    {
        return _root.Apply(new TypingIndicatorRequest(_messageId)
        {
            To = _root.Recipient,
            RecipientType = _root.RecipientType,
            TypingIndicator = new WhatsAppTypingIndicator()
        });
    }
}
