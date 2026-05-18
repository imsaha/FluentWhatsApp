using FluentWhatsApp.Message.Builders;
using FluentWhatsApp.Message.Models;
using FluentWhatsApp.Message.Requests;

namespace FluentWhatsApp;

public interface IWhatsAppClient
{
    Task<SendMessageResponse> SendAsync(MessageRequest request, CancellationToken cancellationToken = default);

    async Task<SendMessageResponse> SendToIndividualAsync(
        string phoneNumber,
        Func<MessageBuilder, MessageRequest> action,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(phoneNumber))
            throw new ArgumentException("Phone number cannot be null or empty", nameof(phoneNumber));

        var request = action(new MessageBuilder(phoneNumber, "individual"));
        return await SendAsync(request, cancellationToken);
    }

    async Task<SendMessageResponse> SendToGroupAsync(
        string groupId,
        Func<MessageBuilder, MessageRequest> action,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(groupId))
            throw new ArgumentException("Group ID cannot be null or empty", nameof(groupId));

        var request = action(new MessageBuilder(groupId, "group"));
        return await SendAsync(request, cancellationToken);
    }
}
