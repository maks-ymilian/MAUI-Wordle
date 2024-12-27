using System.Diagnostics;

namespace Wordle;

public partial class GameStartPage : ContentPage
{
    public static readonly BindableProperty ColumnsProperty =
                BindableProperty.Create(nameof(Columns), typeof(int), typeof(GamePage), 5);

    public static readonly BindableProperty RowsProperty =
        BindableProperty.Create(nameof(Rows), typeof(int), typeof(GamePage), 6);

    private static readonly WordListManager wordListManager = new();

    public int Columns
    {
        get => (int)GetValue(ColumnsProperty);
        set => SetValue(ColumnsProperty, value);
    }

    public int Rows
    {
        get => (int)GetValue(RowsProperty);
        set => SetValue(RowsProperty, value);
    }

    public GameStartPage()
    {
        InitializeComponent();
        BindingContext = this;
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new GamePage(Rows, Columns, wordListManager));
    }
}