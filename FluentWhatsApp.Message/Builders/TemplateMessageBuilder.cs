using FluentWhatsApp.Message.Models.Template;
using FluentWhatsApp.Message.Requests;

namespace FluentWhatsApp.Message.Builders;

public sealed class TemplateMessageBuilder
{
    private readonly List<TemplateComponent> _components = [];
    private readonly string _languageCode;
    private readonly string _name;
    private readonly MessageBuilder _root;

    internal TemplateMessageBuilder(MessageBuilder root, string name, string languageCode)
    {
        _root = root;
        _name = name;
        _languageCode = languageCode;
    }

    public TemplateMessageBuilder AddBodyParameters(Action<TemplateParameterBuilder> configure)
    {
        return AddComponent("body", null, null, configure);
    }

    public TemplateMessageBuilder AddHeaderParameter(Action<TemplateParameterBuilder> configure)
    {
        return AddComponent("header", null, null, configure);
    }

    /// <param name="index">0-based button index.</param>
    /// <param name="subType">"url" | "quick_reply" | "copy_code" | "catalog"</param>
    /// <param name="configure"></param>
    public TemplateMessageBuilder AddButton(int index, string subType,
        Action<TemplateParameterBuilder> configure)
    {
        return AddComponent("button", subType, index, configure);
    }

    private TemplateMessageBuilder AddComponent(string type, string? subType, int? index,
        Action<TemplateParameterBuilder> configure)
    {
        var pb = new TemplateParameterBuilder();
        configure(pb);
        _components.Add(new TemplateComponent
        {
            Type = type,
            SubType = subType,
            Index = index,
            Parameters = pb.Build()
        });
        return this;
    }

    public TemplateMessageRequest Build()
    {
        return _root.Apply(new TemplateMessageRequest
        {
            To = _root.Recipient,
            RecipientType = _root.RecipientType,
            Template = new TemplateObject
            {
                Name = _name,
                Language = new TemplateLanguage(_languageCode),
                Components = _components.Count > 0 ? _components.AsReadOnly() : null
            }
        });
    }
}

public sealed class TemplateParameterBuilder
{
    private readonly List<TemplateParameter> _params = [];

    public TemplateParameterBuilder Text(string value)
    {
        _params.Add(new TemplateParameter { Type = "text", Text = value });
        return this;
    }

    /// <summary>Quick-reply button payload (the value sent back when the user taps).</summary>
    public TemplateParameterBuilder Payload(string payload)
    {
        _params.Add(new TemplateParameter { Type = "payload", Payload = payload });
        return this;
    }

    public TemplateParameterBuilder Image(string? link = null, string? mediaId = null)
    {
        _params.Add(new TemplateParameter
        {
            Type = "image",
            Image = new TemplateMediaObject { Id = mediaId, Link = link }
        });
        return this;
    }

    public TemplateParameterBuilder Video(string? link = null, string? mediaId = null)
    {
        _params.Add(new TemplateParameter
        {
            Type = "video",
            Video = new TemplateMediaObject { Id = mediaId, Link = link }
        });
        return this;
    }

    public TemplateParameterBuilder Document(string? link = null, string? mediaId = null,
        string? caption = null, string? filename = null)
    {
        _params.Add(new TemplateParameter
        {
            Type = "document",
            Document = new TemplateMediaObject { Id = mediaId, Link = link, Caption = caption, Filename = filename }
        });
        return this;
    }

    public TemplateParameterBuilder Currency(string fallbackValue, string currencyCode, long amount1000)
    {
        _params.Add(new TemplateParameter
        {
            Type = "currency",
            Currency = new CurrencyParameter(fallbackValue, currencyCode, amount1000)
        });
        return this;
    }

    public TemplateParameterBuilder DateTime(string fallbackValue)
    {
        _params.Add(new TemplateParameter
        {
            Type = "date_time",
            DateTime = new DateTimeParameter(fallbackValue)
        });
        return this;
    }

    public TemplateParameterBuilder PhoneNumber(string phoneNumber)
    {
        _params.Add(new TemplateParameter
        {
            Type = "phone_number",
            PhoneNumber = phoneNumber
        });
        return this;
    }

    public TemplateParameterBuilder Url(string url)
    {
        _params.Add(new TemplateParameter
        {
            Type = "url",
            Url = url
        });
        return this;
    }

    internal IReadOnlyList<TemplateParameter> Build()
    {
        return _params.AsReadOnly();
    }
}
