namespace Wordle;

public partial class GamePage : ContentPage
{
    public static readonly BindableProperty WordSizeProperty =
            BindableProperty.Create(nameof(WordSize), typeof(int), typeof(GamePage), 5, propertyChanged: OnGridSizeChanged);

    public static readonly BindableProperty RowCountProperty =
        BindableProperty.Create(nameof(RowCount), typeof(int), typeof(GamePage), 6, propertyChanged: OnGridSizeChanged);

    private int currentRow = 0;

    public int WordSize
    {
        get => (int)GetValue(WordSizeProperty);
        set => SetValue(WordSizeProperty, value);
    }

    public int RowCount
    {
        get => (int)GetValue(RowCountProperty);
        set => SetValue(RowCountProperty, value);
    }

    public GamePage()
    {
        InitializeComponent();
        BindingContext = this;

        RebuildGrid();

        MainEntry.TextChanged +=
            (object? sender, TextChangedEventArgs e) => UpdateGridString(e.NewTextValue, currentRow);

        MainEntry.Completed += (object? sender, EventArgs e) =>
            {
                currentRow++;
                MainEntry.Text = "";
            };
    }

    private static void OnGridSizeChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is GamePage page)
            page.RebuildGrid();
    }

    private void RebuildGrid()
    {
        MainEntry.MaxLength = WordSize;

        WordleVerticalStack.Clear();

        for (int i = 0; i < RowCount; i++)
        {
            HorizontalStackLayout layout = new()
            {
                Style = (Style)Resources["WordleVertical"]
            };

            for (int j = 0; j < WordSize; j++)
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