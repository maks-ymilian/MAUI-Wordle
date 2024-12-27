namespace Wordle;

public partial class GamePage : ContentPage
{
    private int currentRow = 0;

    public GamePage(int rows, int columns)
    {
        InitializeComponent();
        BindingContext = this;

        BuildGrid(rows, columns);

        MainEntry.TextChanged +=
            (object? sender, TextChangedEventArgs e) => UpdateGridString(e.NewTextValue, currentRow);

        MainEntry.Completed += (object? sender, EventArgs e) =>
            {
                currentRow++;
                MainEntry.Text = "";
            };
    }

    private void BuildGrid(int rows, int columns)
    {
        MainEntry.MaxLength = columns;

        WordleVerticalStack.Clear();

        for (int i = 0; i < rows; i++)
        {
            HorizontalStackLayout layout = new()
            {
                Style = (Style)Resources["WordleVertical"]
            };

            for (int j = 0; j < columns; j++)
            {
                layout.Add(new Label()
                {
                    Style = (Style)Resources["WordleBox"]
                });
            }

            WordleVerticalStack.Add(layout);
        }
    }

    private void UpdateGridString(string str, int row)
    {
        if (row >= WordleVerticalStack.Children.Count)
            return;

        if (WordleVerticalStack.Children[row] is not HorizontalStackLayout layout)
            return;

        for (int i = 0; i < layout.Children.Count; i++)
        {
            if (layout.Children[i] is not Label label)
                continue;

            label.Text = i < str.Length ? str[i].ToString().ToUpper() : "";
        }
    }
}