using static Wordle.History;

namespace Wordle;

public partial class HistoryPage : ContentPage
{
    public List<HistoryEntry> List { get => History.List; }

    public HistoryPage()
    {
        InitializeComponent();
        BindingContext = this;
    }
}