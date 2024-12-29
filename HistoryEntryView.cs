using System.Diagnostics;
using static Wordle.History;

namespace Wordle
{
    class HistoryEntryView : Grid
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

                ColumnSpacing = 25;

                ColumnDefinitions.Add(new(GridLength.Star));
                ColumnDefinitions.Add(new(GridLength.Star));

                wordleView = new()
                {
                    Rows = 6,
                    Columns = 5,
                    HistoryEntry = HistoryEntry,
                    HorizontalOptions = LayoutOptions.End,
                    HeightRequest = Height - (Padding.Top + Padding.Bottom),
                };
                Grid.SetColumn(wordleView, 0);
                Add(wordleView);

                VerticalStackLayout vertical = new()
                {
                    Spacing = 10,
                };
                vertical.Add(new Label()
                {
                    Text = HistoryEntry.answer,
                    FontSize = 25,
                    HorizontalOptions = LayoutOptions.Start,
                    FontAttributes = FontAttributes.Bold,
                    TextTransform = TextTransform.Uppercase,
                });
                vertical.Add(new Label()
                {
                    Text = "whenever",
                    FontSize = 15,
                    HorizontalOptions = LayoutOptions.Start,
                });
                Grid.SetColumn(vertical, 1);
                Add(vertical);
            };
        }
    }
}
