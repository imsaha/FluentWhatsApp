using FluentWhatsApp.Requests;

namespace FluentWhatsApp.Builders;

public sealed class TextMessageBuilder
{
    private readonly string _body;
    private readonly bool _previewUrl;
    private readonly MessageBuilder _root;

    internal TextMessageBuilder(MessageBuilder root, string body, bool previewUrl)
    {
        _root = root;
        _body = body;
        _previewUrl = previewUrl;
    }

    public TextMessageRequest Build()
    {
        return _root.Apply(new TextMessageRequest
        {
            To = _root.Recipient,
            RecipientType = _root.RecipientType,
            Text = new WhatsAppText(_body, _previewUrl)
        });
    }
}
