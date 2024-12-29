namespace Wordle;

public partial class ConfirmationPage : ContentPage
{
    public static readonly BindableProperty ConfirmationTextProperty = BindableProperty.Create(
                nameof(ConfirmationText), typeof(string), typeof(ConfirmationPage), "");

    public string ConfirmationText
    {
        get => (string)GetValue(ConfirmationTextProperty);
        set => SetValue(ConfirmationTextProperty, value);
    }

    public EventHandler<ConfirmationCompletedEventArgs> ConfirmationCompleted;

    public ConfirmationPage()
    {
        InitializeComponent();
        BindingContext = this;

        ConfirmationCompleted += (object? o, ConfirmationCompletedEventArgs e) => { };
    }

    private void ClickedYes(object sender, EventArgs e)
    {
        ConfirmationCompleted.Invoke(this, new ConfirmationCompletedEventArgs(true));
        Navigation.PopModalAsync();
    }

    private void ClickedNo(object sender, EventArgs e)
    {
        ConfirmationCompleted.Invoke(this, new ConfirmationCompletedEventArgs(false));
        Navigation.PopModalAsync();
    }
}

public class ConfirmationCompletedEventArgs(bool yesClicked) : EventArgs
{
    public bool YesClicked { get; } = yesClicked;
}