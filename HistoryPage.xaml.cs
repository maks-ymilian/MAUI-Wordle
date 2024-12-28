namespace Wordle;

public partial class HistoryPage : ContentPage
{
	public List<Color> List { get; set; } = new()
	{
		Colors.White,
		Colors.Black,
		Colors.White,
		Colors.Black,
		Colors.White,
		Colors.Black,
		Colors.White,
		Colors.Black,
		Colors.White,
		Colors.Black,
		Colors.White,
		Colors.Black,
		Colors.White,
		Colors.Black,
		Colors.White,
		Colors.Black,
	};

	public HistoryPage()
	{
		InitializeComponent();
		BindingContext = this;
	}
}