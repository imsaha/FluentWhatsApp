using FluentWhatsApp.Models.Interactive;
using FluentWhatsApp.Requests;

namespace FluentWhatsApp.Builders;

public sealed class InteractiveMessageBuilder
{
    private readonly string _bodyText;
    private readonly MessageBuilder _root;
    private InteractiveFooter? _footer;
    private InteractiveHeader? _header;

    internal InteractiveMessageBuilder(MessageBuilder root, string bodyText)
    {
        _root = root;
        _bodyText = bodyText;
    }

    public InteractiveMessageBuilder WithHeader(string text)
    {
        _header = new InteractiveHeader { Type = "text", Text = text };
        return this;
    }

    public InteractiveMessageBuilder WithImageHeader(string? link = null, string? mediaId = null)
    {
        _header = new InteractiveHeader
        {
            Type = "image",
            Image = new HeaderMedia { Id = mediaId, Link = link }
        };
        return this;
    }

    public InteractiveMessageBuilder WithVideoHeader(string? link = null, string? mediaId = null)
    {
        _header = new InteractiveHeader
        {
            Type = "video",
            Video = new HeaderMedia { Id = mediaId, Link = link }
        };
        return this;
    }

    public InteractiveMessageBuilder WithDocumentHeader(string? link = null, string? mediaId = null)
    {
        _header = new InteractiveHeader
        {
            Type = "document",
            Document = new HeaderMedia { Id = mediaId, Link = link }
        };
        return this;
    }

    public InteractiveMessageBuilder WithFooter(string text)
    {
        _footer = new InteractiveFooter(text);
        return this;
    }

    public ReplyButtonsBuilder Buttons()
    {
        return new ReplyButtonsBuilder(this);
    }

    public ListMessageBuilder List(string buttonText)
    {
        return new ListMessageBuilder(this, buttonText);
    }

    public InteractiveMessageRequest CtaUrl(string displayText, string url)
    {
        return BuildWith("button", new CtaUrlAction { Parameters = new CtaUrlParameters(displayText, url) });
    }

    public InteractiveMessageRequest Flow(string flowToken, string flowId, string flowCta,
        string flowActionType = "navigate", string? mode = null)
    {
        return BuildWith("flow", new FlowAction
        {
            Parameters = new FlowActionParameters
            {
                FlowToken = flowToken,
                FlowId = flowId,
                FlowCta = flowCta,
                FlowActionType = flowActionType,
                Mode = mode
            }
        });
    }

    public InteractiveMessageRequest Catalog(string? thumbnailProductRetailerId = null)
    {
        return BuildWith("catalog_message", new CatalogAction
        {
            Parameters = thumbnailProductRetailerId is null
                ? null
                : new CatalogActionParameters(thumbnailProductRetailerId)
        });
    }

    internal InteractiveMessageRequest BuildWith(string interactiveType, object action)
    {
        var request = new InteractiveMessageRequest
        {
            To = _root.Recipient,
            RecipientType = _root.RecipientType,
            Interactive = new InteractiveObject
            {
                InteractiveType = interactiveType,
                Header = _header,
                Body = new InteractiveBody(_bodyText),
                Footer = _footer,
                Action = action
            }
        };
        return _root.Apply(request);
    }
}
