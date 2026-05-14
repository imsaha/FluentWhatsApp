using WhatsApp.Net.Models.Interactive;
using WhatsApp.Net.Requests;

namespace WhatsApp.Net.Builders;

public sealed class ListMessageBuilder
{
    private readonly string _buttonText;
    private readonly InteractiveMessageBuilder _interactive;

    private readonly List<ListSection> _sections = [];
    private List<ListRow>? _currentRows;
    private string? _currentTitle;

    internal ListMessageBuilder(InteractiveMessageBuilder interactive, string buttonText)
    {
        _interactive = interactive;
        _buttonText = buttonText;
    }

    /// <summary>Starts a new section. A previous open section is committed automatically.</summary>
    /// <param name="title">Optional section title (max 24 chars).</param>
    public ListMessageBuilder Section(string? title = null)
    {
        CommitCurrent();
        _currentTitle = title;
        _currentRows = [];
        return this;
    }

    /// <param name="id">Unique row identifier (max 200 chars).</param>
    /// <param name="title">Row title shown to the user (max 24 chars).</param>
    /// <param name="description">Optional description (max 72 chars).</param>
    public ListMessageBuilder Row(string id, string title, string? description = null)
    {
        _currentRows ??= [];
        _currentRows.Add(new ListRow { Id = id, Title = title, Description = description });
        return this;
    }

    public InteractiveMessageRequest Build()
    {
        CommitCurrent();

        if (_sections.Count == 0)
            throw new InvalidOperationException("A list message requires at least one section with rows.");

        return _interactive.BuildWith("list", new ListAction
        {
            Button = _buttonText,
            Sections = _sections.AsReadOnly()
        });
    }

    private void CommitCurrent()
    {
        if (_currentRows is { Count: > 0 })
            _sections.Add(new ListSection { Title = _currentTitle, Rows = _currentRows.AsReadOnly() });

        _currentTitle = null;
        _currentRows = null;
    }
}
