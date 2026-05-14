using FluentWhatsApp.Models;
using FluentWhatsApp.Requests;

namespace FluentWhatsApp.Builders;

public sealed class ImageMessageBuilder
{
    private readonly string? _link;
    private readonly string? _mediaId;
    private readonly MessageBuilder _root;
    private string? _caption;

    internal ImageMessageBuilder(MessageBuilder root, string? link, string? mediaId, string? caption)
    {
        _root = root;
        _link = link;
        _mediaId = mediaId;
        _caption = caption;
    }

    public ImageMessageBuilder WithCaption(string caption)
    {
        _caption = caption;
        return this;
    }

    public ImageMessageRequest Build()
    {
        return _root.Apply(new ImageMessageRequest
        {
            To = _root.Recipient,
            RecipientType = _root.RecipientType,
            Image = new MediaObject { Id = _mediaId, Link = _link, Caption = _caption }
        });
    }
}

public sealed class AudioMessageBuilder
{
    private readonly string? _link;
    private readonly string? _mediaId;
    private readonly MessageBuilder _root;

    internal AudioMessageBuilder(MessageBuilder root, string? link, string? mediaId)
    {
        _root = root;
        _link = link;
        _mediaId = mediaId;
    }

    public AudioMessageRequest Build()
    {
        return _root.Apply(new AudioMessageRequest
        {
            To = _root.Recipient,
            RecipientType = _root.RecipientType,
            Audio = new MediaObject { Id = _mediaId, Link = _link }
        });
    }
}

public sealed class VideoMessageBuilder
{
    private readonly string? _link;
    private readonly string? _mediaId;
    private readonly MessageBuilder _root;
    private string? _caption;

    internal VideoMessageBuilder(MessageBuilder root, string? link, string? mediaId, string? caption)
    {
        _root = root;
        _link = link;
        _mediaId = mediaId;
        _caption = caption;
    }

    public VideoMessageBuilder WithCaption(string caption)
    {
        _caption = caption;
        return this;
    }

    public VideoMessageRequest Build()
    {
        return _root.Apply(new VideoMessageRequest
        {
            To = _root.Recipient,
            RecipientType = _root.RecipientType,
            Video = new MediaObject { Id = _mediaId, Link = _link, Caption = _caption }
        });
    }
}

public sealed class DocumentMessageBuilder
{
    private readonly string? _link;
    private readonly string? _mediaId;
    private readonly MessageBuilder _root;
    private string? _caption;
    private string? _filename;

    internal DocumentMessageBuilder(MessageBuilder root, string? link, string? mediaId,
        string? caption, string? filename)
    {
        _root = root;
        _link = link;
        _mediaId = mediaId;
        _caption = caption;
        _filename = filename;
    }

    public DocumentMessageBuilder WithCaption(string caption)
    {
        _caption = caption;
        return this;
    }
    public DocumentMessageBuilder WithFilename(string filename)
    {
        _filename = filename;
        return this;
    }

    public DocumentMessageRequest Build()
    {
        return _root.Apply(new DocumentMessageRequest
        {
            To = _root.Recipient,
            RecipientType = _root.RecipientType,
            Document = new MediaObject { Id = _mediaId, Link = _link, Caption = _caption, Filename = _filename }
        });
    }
}

public sealed class StickerMessageBuilder
{
    private readonly string? _link;
    private readonly string? _mediaId;
    private readonly MessageBuilder _root;

    internal StickerMessageBuilder(MessageBuilder root, string? link, string? mediaId)
    {
        _root = root;
        _link = link;
        _mediaId = mediaId;
    }

    public StickerMessageRequest Build()
    {
        return _root.Apply(new StickerMessageRequest
        {
            To = _root.Recipient,
            RecipientType = _root.RecipientType,
            Sticker = new MediaObject { Id = _mediaId, Link = _link }
        });
    }
}
