using System.Diagnostics;
using static Wordle.History;

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
        nameof(HistoryEntry), typeof(HistoryEntry), typeof(WordleView), null);

    private readonly VerticalStackLayout vertical;

    private readonly Color emptyColor = Color.FromRgb(0, 0, 0);
    private readonly Color notFoundColor = Color.FromRgb(68, 68, 68);
    private readonly Color presentColor = Color.FromRgb(255, 255, 0);
    private readonly Color correctColor = Color.FromRgb(0, 255, 0);

    private WordleTile[]? tiles;

    public int Rows { set; get; }
    public int Columns { set; get; }

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

        Loaded += (object? o, EventArgs e) =>
        {
            if (Rows == 0 && Columns == 0 && HistoryEntry != null)
            {
                Rows = HistoryEntry.rows;
                Columns = HistoryEntry.columns;
            }

            Debug.Assert(Rows > 0 && Columns > 0);

            // calculate boxSize and spacing for requested height
            double spacingRatio = 0.1f;
            double boxSize = HeightRequest / (Rows + (Rows - 1) * spacingRatio);
            double spacing = boxSize * spacingRatio;

            tiles = new WordleTile[Rows * Columns];

            vertical.Spacing = spacing;

            BuildGrid(boxSize, spacing);

            if (HistoryEntry != null)
                SetFromHistoryEntry(HistoryEntry);
        };
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

    public string GetRowText(int row)
    {
        string str = "";
        for (int i = 0; i < Columns; i++)
        {
            char? character = GetChar(row, i);
            if (character != null)
                str += character;
        }
        return str;
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
        Debug.Assert(tiles != null);
        tiles[col + row * Columns] = type;

        var layout = (HorizontalStackLayout)vertical.Children[row];
        Label label = (Label)layout.Children[col];

        switch (type)
        {
            case WordleTile.Empty: label.BackgroundColor = emptyColor; break;
            case WordleTile.NotFound: label.BackgroundColor = notFoundColor; break;
            case WordleTile.Present: label.BackgroundColor = presentColor; break;
            case WordleTile.Correct: label.BackgroundColor = correctColor; break;
        };
    }

    public WordleTile[] GetTiles()
    {
        Debug.Assert(tiles != null);
        return tiles;
    }

    private void BuildGrid(double boxSize, double spacing)
    {
        for (int i = 0; i < Rows; i++)
        {
            HorizontalStackLayout layout = new() { Spacing = spacing, };
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
