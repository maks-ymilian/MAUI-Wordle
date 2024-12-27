using System.Diagnostics;

namespace Wordle;

public partial class GameStartPage : ContentPage
{
    public static readonly BindableProperty WordSizeProperty =
                BindableProperty.Create(nameof(WordSize), typeof(int), typeof(GamePage), 5);

    private static readonly WordListManager wordListManager = new();

    public int WordSize
    {
        get => (int)GetValue(WordSizeProperty);
        set => SetValue(WordSizeProperty, value);
    }

    public GameStartPage()
    {
        InitializeComponent();
        BindingContext = this;
    }

    private async void PlayButtonClicked(object sender, EventArgs e) =>
        await Navigation.PushAsync(new GamePage(WordSize, wordListManager));

    private async void HistoryButtonClicked(object sender, EventArgs e) =>
        await Navigation.PushAsync(new HistoryPage());

    private async void SettingsButtonClicked(object sender, EventArgs e) =>
        await Navigation.PushAsync(new SettingsPage());
}