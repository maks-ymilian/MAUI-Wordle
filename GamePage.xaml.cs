
namespace Wordle;

public partial class GamePage : ContentPage
{
    public static readonly BindableProperty WordSizeProperty =
            BindableProperty.Create(nameof(WordSize), typeof(int), typeof(GamePage), 5, propertyChanged: OnGridSizeChanged);

    public static readonly BindableProperty RowCountProperty =
        BindableProperty.Create(nameof(RowCount), typeof(int), typeof(GamePage), 6, propertyChanged: OnGridSizeChanged);

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
    }

    private static void OnGridSizeChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is GamePage page)
            page.RebuildGrid();
    }

    private void RebuildGrid()
    {
        WordleVerticalStack.Clear();

        for (int i = 0; i < RowCount; i++)
        {
            HorizontalStackLayout layout = new()
            {
                Style = (Style)Resources["WordleVertical"]
            };

            for (int j = 0; j < WordSize; j++)
            {
                layout.Add(new BoxView()
                {
                    Style = (Style)Resources["WordleBox"]
                });
            }

            WordleVerticalStack.Add(layout);
        }
    }
}