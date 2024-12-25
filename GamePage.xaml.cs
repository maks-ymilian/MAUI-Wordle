namespace Wordle;

public partial class GamePage : ContentPage
{
    public GamePage()
    {
        int wordSize = 5;
        int rowCount = 6;

        InitializeComponent();

        for (int i = 0; i < rowCount; i++)
        {
            HorizontalStackLayout layout = new()
            {
                Style = (Style)Resources["WordleVertical"]
            };

            for (int j = 0; j < wordSize; j++)
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