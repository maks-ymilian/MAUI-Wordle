using System.Diagnostics;

namespace Wordle;

public partial class SettingsPage : ContentPage
{
    public AppTheme ChosenTheme
    {
        get
        {
            Debug.Assert(Application.Current != null);
            return Application.Current.UserAppTheme;
        }
        set
        {
            OnPropertyChanged();
            Debug.Assert(Application.Current != null);
            Application.Current.UserAppTheme = value;
        }
    }

    public AppTheme[] ThemePickerItems { get; } = { AppTheme.Dark, AppTheme.Light };

    private readonly History history;

    public SettingsPage(History history)
    {
        this.history = history;

        InitializeComponent();
        BindingContext = this;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        Debug.Assert(Application.Current != null);
        ChosenTheme = Application.Current.UserAppTheme;
    }

    private async void ClearHistoryClicked(object sender, EventArgs e)
    {
        ConfirmationPage page = new ConfirmationPage()
        {
            ConfirmationText = "Are you sure you want to clear your history?",
        };
        page.ConfirmationCompleted += (object? o, ConfirmationCompletedEventArgs e) =>
        {
            if (e.YesClicked)
                history.ClearHistory();
        };

        await Navigation.PushModalAsync(page);
    }
}