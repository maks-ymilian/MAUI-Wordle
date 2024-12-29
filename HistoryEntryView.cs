using System.Diagnostics;
using static Wordle.History;

namespace Wordle
{
    class HistoryEntryView : VerticalStackLayout
    {
        public static readonly BindableProperty HistoryEntryProperty = BindableProperty.Create(
            nameof(HistoryEntry), typeof(HistoryEntry), typeof(WordleView), null);

        public HistoryEntry? HistoryEntry
        {
            get => (HistoryEntry?)GetValue(HistoryEntryProperty);
            set => SetValue(HistoryEntryProperty, value);
        }

        private WordleView? wordleView;

        public HistoryEntryView()
        {
            Loaded += (object? o, EventArgs e) =>
            {
                Debug.Assert(HistoryEntry != null);

                Spacing = 10;

                wordleView = new()
                {
                    HistoryEntry = HistoryEntry,
                    HorizontalOptions = LayoutOptions.Center,
                    HeightRequest = 250,
                };
                Grid.SetColumn(wordleView, 0);
                Add(wordleView);

                Add(new Label()
                {
                    Text = HistoryEntry.Answer,
                    FontSize = 25,
                    HorizontalOptions = LayoutOptions.Center,
                    FontAttributes = FontAttributes.Bold,
                    TextTransform = TextTransform.Uppercase,
                });
                Add(new Label()
                {
                    Text = "whenever",
                    FontSize = 15,
                    HorizontalOptions = LayoutOptions.Center,
                });
            };
        }
    }
}
