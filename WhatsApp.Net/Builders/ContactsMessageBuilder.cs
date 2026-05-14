using WhatsApp.Net.Models.Contacts;
using WhatsApp.Net.Requests;

namespace WhatsApp.Net.Builders;

public sealed class ContactsMessageBuilder
{
    private readonly List<ContactData> _contacts = [];
    private readonly MessageBuilder _root;

    internal ContactsMessageBuilder(MessageBuilder root)
    {
        _root = root;
    }

    public ContactsMessageBuilder Add(Action<ContactBuilder> configure)
    {
        var builder = new ContactBuilder();
        configure(builder);
        _contacts.Add(builder.Build());
        return this;
    }

    public ContactsMessageRequest Build()
    {
        return _root.Apply(new ContactsMessageRequest
        {
            To = _root.Recipient,
            RecipientType = _root.RecipientType,
            Contacts = _contacts.AsReadOnly()
        });
    }
}

public sealed class ContactBuilder
{
    private readonly List<ContactAddress> _addresses = [];
    private readonly List<ContactEmail> _emails = [];
    private readonly List<ContactPhone> _phones = [];
    private readonly List<ContactUrl> _urls = [];
    private string? _birthday;
    private ContactName? _name;
    private ContactOrg? _org;

    public ContactBuilder Name(string formattedName,
        string? firstName = null, string? lastName = null,
        string? middleName = null, string? prefix = null, string? suffix = null)
    {
        _name = new ContactName
        {
            FormattedName = formattedName,
            FirstName = firstName,
            LastName = lastName,
            MiddleName = middleName,
            Prefix = prefix,
            Suffix = suffix
        };
        return this;
    }

    public ContactBuilder Birthday(string date)
    {
        _birthday = date;
        return this;
    }

    public ContactBuilder Phone(string phone, string? type = null, string? waId = null)
    {
        _phones.Add(new ContactPhone { Phone = phone, Type = type, WaId = waId });
        return this;
    }

    public ContactBuilder Email(string email, string? type = null)
    {
        _emails.Add(new ContactEmail { Email = email, Type = type });
        return this;
    }

    public ContactBuilder Address(string? street = null, string? city = null,
        string? state = null, string? zip = null, string? country = null,
        string? countryCode = null, string? type = null)
    {
        _addresses.Add(new ContactAddress
        {
            Street = street,
            City = city,
            State = state,
            Zip = zip,
            Country = country,
            CountryCode = countryCode,
            Type = type
        });
        return this;
    }

    public ContactBuilder Organization(string? company = null,
        string? department = null, string? title = null)
    {
        _org = new ContactOrg { Company = company, Department = department, Title = title };
        return this;
    }

    public ContactBuilder Url(string url, string? type = null)
    {
        _urls.Add(new ContactUrl { Url = url, Type = type });
        return this;
    }

    internal ContactData Build()
    {
        if (_name is null)
        {
            throw new InvalidOperationException("A contact must have a name. Call Name() before Build().");
        }

        return new ContactData
        {
            Name = _name,
            Birthday = _birthday,
            Phones = _phones.Count > 0 ? _phones.AsReadOnly() : null,
            Emails = _emails.Count > 0 ? _emails.AsReadOnly() : null,
            Addresses = _addresses.Count > 0 ? _addresses.AsReadOnly() : null,
            Urls = _urls.Count > 0 ? _urls.AsReadOnly() : null,
            Org = _org
        };
    }
}
