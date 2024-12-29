using static Wordle.History;

namespace Wordle;

public partial class HistoryPage : ContentPage
{
    public List<HistoryEntry> List { get => history.List; }

    private readonly History history;

    public HistoryPage(History history)
    {
        this.history = history;

        InitializeComponent();
        BindingContext = this;
    }
}