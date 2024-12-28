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

    private VerticalStackLayout vertical;

    private readonly int rows;
    private readonly int columns;

    private readonly Color emptyColor = Color.FromRgb(0, 0, 0);
    private readonly Color notFoundColor = Color.FromRgb(68, 68, 68);
    private readonly Color presentColor = Color.FromRgb(255, 255, 0);
    private readonly Color correctColor = Color.FromRgb(0, 255, 0);

    public WordleView(int rows, int columns, float boxSize)
    {
        this.rows = rows;
        this.columns = columns;

        vertical = new()
        {
            VerticalOptions = LayoutOptions.Center,
            HorizontalOptions = LayoutOptions.Center,
            Spacing = 5,
        };
        Add(vertical);

        BuildGrid(boxSize);
    }

    public void SetRowText(string str, int row)
    {
        for (int i = 0; i < columns; i++)
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

    private void BuildGrid(float boxSize)
    {
        for (int i = 0; i < rows; i++)
        {
            HorizontalStackLayout layout = new() { Spacing = 5, };
            for (int j = 0; j < columns; j++)
            {
                layout.Add(new Label()
                {
                    WidthRequest = boxSize,
                    HeightRequest = boxSize,
                    BackgroundColor = emptyColor,
                    TextColor = Colors.White,
                    FontSize = 25,
                    TextTransform = TextTransform.Uppercase,
                    VerticalTextAlignment = TextAlignment.Center,
                    HorizontalTextAlignment = TextAlignment.Center,
                    FontAttributes = FontAttributes.Bold,
                });
            }
            vertical.Add(layout);
        }
    }
}
