namespace Wordle;

public partial class GameStartPage : ContentPage
{
    private static readonly WordListManager wordListManager = new();
    private static readonly History history = new();

    public static readonly BindableProperty WordSizeProperty =
                BindableProperty.Create(nameof(WordSize), typeof(int), typeof(GamePage), 5);

    public static readonly BindableProperty IsLoadingProperty =
                BindableProperty.Create(nameof(IsLoading), typeof(bool), typeof(GamePage), false);

    public bool IsLoading
    {
        get => (bool)GetValue(IsLoadingProperty);
        set => SetValue(IsLoadingProperty, value);
    }

    public int WordSize
    {
        get => (int)GetValue(WordSizeProperty);
        set => SetValue(WordSizeProperty, value);
    }

    public GameStartPage()
    {
        InitializeComponent();
        BindingContext = this;

        Disappearing += async (object? o, EventArgs e) => await history.WriteToFileAsync();
    }

    private async void PlayButtonClicked(object sender, EventArgs e)
    {
        IsLoading = true;

        try
        {
            GamePage page = await GamePage.CreateGamePageAsync(WordSize, wordListManager, history);
            await Navigation.PushAsync(page);
        }
        catch (HttpRequestException) { }
        finally
        {
            IsLoading = false;
        }
    }

    private async void HistoryButtonClicked(object sender, EventArgs e)
    {
        HistoryPage? page = null;

        IsLoading = true;
        await Task.Run(() => page = new(history));
        IsLoading = false;

        await Navigation.PushAsync(page);
    }

    private void SettingsButtonClicked(object sender, EventArgs e) => Navigation.PushAsync(new SettingsPage(history));
}