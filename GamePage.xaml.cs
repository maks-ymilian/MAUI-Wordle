using System.Diagnostics;

namespace Wordle;

public partial class GamePage : ContentPage
{
    private readonly string word;
    private HashSet<string> usedWords = new();

    private int currentRow = 0;

    public GamePage(int rows, int columns, WordListManager wordListManager)
    {
        WordList wordList = wordListManager.GetWordList(columns).Result;
        word = wordList.GetRandomWord();

        InitializeComponent();
        BindingContext = this;

        BuildGrid(rows, columns);

        MainEntry.TextChanged += (object? sender, TextChangedEventArgs e) => UpdateGridString(e.NewTextValue);

        MainEntry.Completed += (object? sender, EventArgs e) =>
            {
                if (MainEntry.Text.Length != columns)
                    return;
                if (!MainEntry.Text.All(char.IsAsciiLetter))
                    return;
                if (usedWords.Contains(MainEntry.Text.ToLower()))
                    return;
                if (!wordList.IsValidWord(MainEntry.Text))
                    return;

                usedWords.Add(MainEntry.Text.ToLower());

                UpdateGridWord();
                MainEntry.Text = "";
            };
    }

    private void BuildGrid(int rows, int columns)
    {
        MainEntry.MaxLength = columns;

        WordleVerticalStack.Clear();

        for (int i = 0; i < rows; i++)
        {
            HorizontalStackLayout layout = new()
            {
                Style = (Style)Resources["WordleVertical"]
            };

            for (int j = 0; j < columns; j++)
            {
                layout.Add(new Label()
                {
                    Style = (Style)Resources["WordleBoxEmpty"]
                });
            }

            WordleVerticalStack.Add(layout);
        }
    }

    private void IterateBoxes(Action<Label, int> action)
    {
        if (currentRow >= WordleVerticalStack.Children.Count)
            return;

        if (WordleVerticalStack.Children[currentRow] is not HorizontalStackLayout layout)
            return;

        for (int i = 0; i < layout.Children.Count; i++)
        {
            if (layout.Children[i] is not Label label)
                continue;

            action(label, i);
        }
    }

    private void UpdateGridString(string str)
    {
        IterateBoxes((Label label, int col) =>
        {
            label.Text = col < str.Length ? str[col].ToString().ToUpper() : "";
        });
    }

    private void UpdateGridWord()
    {
        IterateBoxes((Label label, int col) =>
        {
            Debug.Assert(col < word.Length);

            string currentWord = word.ToLower();
            char currentChar = label.Text.ToLower()[0];

            if (currentChar == currentWord[col])
                label.Style = (Style)Resources["WordleBoxCorrect"];
            else if (currentWord.Contains(currentChar))
                label.Style = (Style)Resources["WordleBoxPresent"];
            else
                label.Style = (Style)Resources["WordleBoxNotFound"];
        });
        currentRow++;
    }
}