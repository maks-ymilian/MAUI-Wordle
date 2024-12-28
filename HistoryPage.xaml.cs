using static Wordle.WordleView;

namespace Wordle;

public struct HistoryEntry
{
    public string[] textRows;
    public WordleTile[] tiles;
}

public partial class HistoryPage : ContentPage
{
    public List<HistoryEntry> List { get; set; } = new()
    {
        new()
        {
            textRows = ["crane", "audio", "igloo"],
            tiles =
            [
                WordleTile.NotFound, WordleTile.NotFound, WordleTile.Correct, WordleTile.NotFound, WordleTile.Present,
                WordleTile.Correct, WordleTile.Present, WordleTile.Correct, WordleTile.NotFound, WordleTile.NotFound,
                WordleTile.NotFound, WordleTile.NotFound, WordleTile.Correct, WordleTile.NotFound, WordleTile.Present,
            ],
        },
        new()
        {
            textRows = ["crane", "audio", "igloo"],
            tiles =
            [
                WordleTile.NotFound, WordleTile.NotFound, WordleTile.Correct, WordleTile.NotFound, WordleTile.Present,
                WordleTile.Correct, WordleTile.Present, WordleTile.Correct, WordleTile.NotFound, WordleTile.NotFound,
                WordleTile.NotFound, WordleTile.NotFound, WordleTile.Correct, WordleTile.NotFound, WordleTile.Present,
            ],
        },
        new()
        {
            textRows = ["crane", "audio", "igloo"],
            tiles =
            [
                WordleTile.NotFound, WordleTile.NotFound, WordleTile.Correct, WordleTile.NotFound, WordleTile.Present,
                WordleTile.Correct, WordleTile.Present, WordleTile.Correct, WordleTile.NotFound, WordleTile.NotFound,
                WordleTile.NotFound, WordleTile.NotFound, WordleTile.Correct, WordleTile.NotFound, WordleTile.Present,
            ],
        },
        new()
        {
            textRows = ["crane", "audio", "igloo"],
            tiles =
            [
                WordleTile.NotFound, WordleTile.NotFound, WordleTile.Correct, WordleTile.NotFound, WordleTile.Present,
                WordleTile.Correct, WordleTile.Present, WordleTile.Correct, WordleTile.NotFound, WordleTile.NotFound,
                WordleTile.NotFound, WordleTile.NotFound, WordleTile.Correct, WordleTile.NotFound, WordleTile.Present,
            ],
        },
        new()
        {
            textRows = ["crane", "audio", "igloo"],
            tiles =
            [
                WordleTile.NotFound, WordleTile.NotFound, WordleTile.Correct, WordleTile.NotFound, WordleTile.Present,
                WordleTile.Correct, WordleTile.Present, WordleTile.Correct, WordleTile.NotFound, WordleTile.NotFound,
                WordleTile.NotFound, WordleTile.NotFound, WordleTile.Correct, WordleTile.NotFound, WordleTile.Present,
            ],
        },
    };

    public HistoryPage()
    {
        InitializeComponent();
        BindingContext = this;
    }
}