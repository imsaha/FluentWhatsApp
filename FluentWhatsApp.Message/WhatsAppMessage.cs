using FluentWhatsApp.Message.Builders;

namespace FluentWhatsApp.Message;

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
