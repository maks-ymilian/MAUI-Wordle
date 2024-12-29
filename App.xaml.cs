namespace Wordle
{
    public partial class App : Application
    {
        public App()
        {
            if (UserAppTheme != AppTheme.Dark && UserAppTheme != AppTheme.Light)
                UserAppTheme = AppTheme.Dark;

            InitializeComponent();

            MainPage = new AppShell();
        }
    }
}
