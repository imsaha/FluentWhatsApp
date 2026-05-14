using FluentWhatsApp.Models;
using FluentWhatsApp.Requests;

namespace FluentWhatsApp.Builders;

public sealed class LocationMessageBuilder
{
    private readonly double _lat;
    private readonly double _lon;
    private readonly MessageBuilder _root;
    private string? _address;
    private string? _name;

    internal LocationMessageBuilder(MessageBuilder root, double lat, double lon)
    {
        _root = root;
        _lat = lat;
        _lon = lon;
    }

    public LocationMessageBuilder WithName(string name)
    {
        _name = name;
        return this;
    }
    public LocationMessageBuilder WithAddress(string address)
    {
        _address = address;
        return this;
    }

    public LocationMessageRequest Build()
    {
        return _root.Apply(new LocationMessageRequest
        {
            To = _root.Recipient,
            RecipientType = _root.RecipientType,
            Location = new LocationObject
            {
                Latitude = _lat,
                Longitude = _lon,
                Name = _name,
                Address = _address
            }
        });
    }
}
