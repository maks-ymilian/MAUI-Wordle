namespace Wordle;

public class WordleView : Grid
{
    public enum WordleTile
    {
        Empty,
        NotFound,
        Present,
        Correct,
    }

    public static readonly BindableProperty HistoryEntryProperty = BindableProperty.Create(
        nameof(HistoryEntry), typeof(HistoryEntry?), typeof(WordleView), null);

    private readonly VerticalStackLayout vertical;

    private readonly Color emptyColor = Color.FromRgb(0, 0, 0);
    private readonly Color notFoundColor = Color.FromRgb(68, 68, 68);
    private readonly Color presentColor = Color.FromRgb(255, 255, 0);
    private readonly Color correctColor = Color.FromRgb(0, 255, 0);

    public int Rows { set; get; }
    public int Columns { set; get; }

    public float BoxSize { set; get; }

    public HistoryEntry? HistoryEntry
    {
        get => (HistoryEntry?)GetValue(HistoryEntryProperty);
        set => SetValue(HistoryEntryProperty, value);
    }

    public WordleView()
    {
        vertical = new()
        {
            VerticalOptions = LayoutOptions.Center,
            HorizontalOptions = LayoutOptions.Center,
        };
        Add(vertical);

        Loaded += (object? o, EventArgs e) => InitializeProperties();
    }

    public void SetRowText(string str, int row)
    {
        for (int i = 0; i < Columns; i++)
        {
            if (i < str.Length)
                SetChar(row, i, str[i]);
            else
                SetChar(row, i, null);
        }
    }

    public void SetChar(int row, int col, char? character)
    {
        var layout = (HorizontalStackLayout)vertical.Children[row];

        Label label = (Label)layout.Children[col];
        if (character != null)
            label.Text = character.ToString();
        else
            label.Text = null;
    }

    public char? GetChar(int row, int col)
    {
        var layout = (HorizontalStackLayout)vertical.Children[row];

        Label label = (Label)layout.Children[col];
        if (label.Text == null)
            return null;
        else
            return label.Text.ToLower()[0];
    }

    public void SetTile(int row, int col, WordleTile type)
    {
        var layout = (HorizontalStackLayout)vertical.Children[row];
        Label label = (Label)layout.Children[col];

        label.BackgroundColor = type switch
        {
            WordleTile.Empty => emptyColor,
            WordleTile.NotFound => notFoundColor,
            WordleTile.Present => presentColor,
            WordleTile.Correct => correctColor,
            _ => throw new InvalidOperationException(),
        };
    }

    private void InitializeProperties()
    {
        vertical.Spacing = BoxSize / 10f;

        BuildGrid(BoxSize);

        if (HistoryEntry != null)
            SetFromHistoryEntry((HistoryEntry)HistoryEntry);
    }

    private void BuildGrid(float boxSize)
    {
        for (int i = 0; i < Rows; i++)
        {
            HorizontalStackLayout layout = new() { Spacing = boxSize / 10f, };
            for (int j = 0; j < Columns; j++)
            {
                layout.Add(new Label()
                {
                    WidthRequest = boxSize,
                    HeightRequest = boxSize,
                    BackgroundColor = emptyColor,
                    TextColor = Colors.White,
                    FontSize = boxSize / 2f,
                    TextTransform = TextTransform.Uppercase,
                    VerticalTextAlignment = TextAlignment.Center,
                    HorizontalTextAlignment = TextAlignment.Center,
                    FontAttributes = FontAttributes.Bold,
                });
            }
            vertical.Add(layout);
        }
    }

    private void SetFromHistoryEntry(HistoryEntry entry)
    {
        for (int row = 0; row < Rows; row++)
        {
            if (row < entry.textRows.Length)
                SetRowText(entry.textRows[row], row);

            for (int column = 0; column < Columns; column++)
            {
                WordleTile tile = WordleTile.Empty;
                int index = row * Columns + column;
                if (index < entry.tiles.Length)
                    tile = entry.tiles[index];

                SetTile(row, column, tile);
            }
        }
    }
}
