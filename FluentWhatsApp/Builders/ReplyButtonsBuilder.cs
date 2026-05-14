using FluentWhatsApp.Models.Interactive;
using FluentWhatsApp.Requests;

namespace FluentWhatsApp.Builders;

public sealed class ReplyButtonsBuilder
{
    private readonly List<ReplyButton> _buttons = [];
    private readonly InteractiveMessageBuilder _interactive;

    internal ReplyButtonsBuilder(InteractiveMessageBuilder interactive)
    {
        _interactive = interactive;
    }

    /// <param name="id">Unique button identifier (max 256 chars).</param>
    /// <param name="title">Button label shown to the user (max 20 chars).</param>
    public ReplyButtonsBuilder Add(string id, string title)
    {
        if (_buttons.Count >= 3)
            throw new InvalidOperationException("Reply buttons are limited to 3 per message.");

        _buttons.Add(ReplyButton.Create(id, title));
        return this;
    }

    public InteractiveMessageRequest Build()
    {
        if (_buttons.Count == 0)
            throw new InvalidOperationException("At least one button is required.");

        return _interactive.BuildWith("button",
            new ReplyButtonsAction(_buttons.AsReadOnly()));
    }
}
