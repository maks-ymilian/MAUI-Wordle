using System.Diagnostics;

namespace Wordle;

public partial class GamePage : ContentPage
{
    private int currentRow = 0;

    private static readonly Dictionary<int, int> rowCounts = new()
    {
        { 3, 5 },
        { 4, 5 },
        { 5, 6 },
        { 6, 7 },
        { 7, 7 },
        { 8, 7 },
    };

    private string? word;
    private WordList? wordList;
    private HashSet<string> usedWords = new();

    private readonly int wordSize;
    private readonly int rows;

    private GamePage(int wordSize, WordListManager wordListManager)
    {
        this.wordSize = wordSize;
        rows = rowCounts[wordSize];

        InitializeComponent();
        BindingContext = this;

        BuildGrid(rows, wordSize);

        MainEntry.TextChanged += (object? sender, TextChangedEventArgs e) => UpdateGridText(e.NewTextValue);
        MainEntry.Completed += (object? sender, EventArgs e) => EnterWord();
    }

    public static async Task<GamePage> CreateGamePageAsync(int wordSize, WordListManager wordListManager)
    {
        GamePage gamePage = new(wordSize, wordListManager);
        gamePage.wordList = await wordListManager.GetWordList(wordSize).ConfigureAwait(false);
        gamePage.word = gamePage.wordList.GetRandomWord();
        return gamePage;
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

    private void EndGame()
    {
        MainEntry.Unfocus();
        MainLayout.Remove(MainEntry);
        MainLayout.Add(new Label()
        {
            Style = (Style)Resources["WordRevealText"],
            Text = word,
        });
    }

    private void EnterWord()
    {
        Debug.Assert(word != null && wordList != null);

        string enteredWord = MainEntry.Text.ToLower();
        string currentWord = word.ToLower();

        if (enteredWord.Length != wordSize)
            return;
        if (!enteredWord.All(char.IsAsciiLetter))
            return;
        if (usedWords.Contains(enteredWord))
            return;
        if (!wordList.IsValidWord(enteredWord))
            return;

        usedWords.Add(enteredWord);
        MainEntry.Text = "";

        bool isCorrect = true;
        IterateCurrentRow((Label label, int col) =>
        {
            Debug.Assert(col < word.Length && col < enteredWord.Length);

            char currentChar = enteredWord[col];
            label.Text = currentChar.ToString();

            if (currentChar == currentWord[col])
            {
                label.Style = (Style)Resources["WordleBoxCorrect"];
            }
            else if (currentWord.Contains(currentChar))
            {
                label.Style = (Style)Resources["WordleBoxPresent"];
                isCorrect = false;
            }
            else
            {
                label.Style = (Style)Resources["WordleBoxNotFound"];
                isCorrect = false;
            }
        });
        currentRow++;

        if (isCorrect || currentRow >= rows)
            EndGame();
    }

    private void UpdateGridText(string str)
    {
        IterateCurrentRow((Label label, int col) =>
        {
            label.Text = col < str.Length ? str[col].ToString() : "";
        });
    }

    private void IterateCurrentRow(Action<Label, int> action)
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
}