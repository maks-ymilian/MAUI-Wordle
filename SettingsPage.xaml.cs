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

    public SettingsPage()
    {
        InitializeComponent();
        BindingContext = this;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        Debug.Assert(Application.Current != null);
        ChosenTheme = Application.Current.UserAppTheme;
    }
}