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

    private WordleView? wordleView;

    private string? word;
    private WordList? wordList;
    private readonly HashSet<string> usedWords = new();

    private readonly int wordSize;
    private readonly int rows;

    private GamePage(int wordSize, WordListManager wordListManager)
    {
        this.wordSize = wordSize;
        rows = rowCounts[wordSize];

        InitializeComponent();
        BindingContext = this;

        MainEntry.MaxLength = wordSize;
        MainEntry.TextChanged += (object? sender, TextChangedEventArgs e) => wordleView?.SetRowText(e.NewTextValue, currentRow);
        MainEntry.Completed += (object? sender, EventArgs e) => EnterWord();
    }

    public static async Task<GamePage> CreateGamePageAsync(int wordSize, WordListManager wordListManager)
    {
        GamePage gamePage = new(wordSize, wordListManager);

        await Task.Run(() =>
        {
            gamePage.wordleView = new()
            {
                Rows = gamePage.rows,
                Columns = gamePage.wordSize,
                BoxSize = 50,
            };
            gamePage.MainLayout.Insert(0, gamePage.wordleView);
        });

        gamePage.wordList = await wordListManager.GetWordListAsync(wordSize).ConfigureAwait(false);
        gamePage.word = gamePage.wordList.GetRandomWord();

        return gamePage;
    }

    private void EndGame()
    {
        Debug.Assert(wordleView != null);

        MainEntry.Unfocus();
        MainLayout.Remove(MainEntry);
        MainLayout.Add(new Label()
        {
            Style = (Style)Resources["WordRevealText"],
            Text = word,
        });

        History.AddEntry(wordleView.GetHistoryEntry());
    }

    private void EnterWord()
    {
        Debug.Assert(word != null && wordList != null && wordleView != null);

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
        for (int col = 0; col < wordSize; col++)
        {
            Debug.Assert(col < word.Length && col < enteredWord.Length);

            wordleView.SetChar(currentRow, col, enteredWord[col]);

            if (enteredWord[col] == currentWord[col])
            {
                wordleView.SetTile(currentRow, col, WordleView.WordleTile.Correct);
            }
            else if (currentWord.Contains(enteredWord[col]))
            {
                wordleView.SetTile(currentRow, col, WordleView.WordleTile.Present);
                isCorrect = false;
            }
            else
            {
                wordleView.SetTile(currentRow, col, WordleView.WordleTile.NotFound);
                isCorrect = false;
            }
        }
        currentRow++;

        if (isCorrect || currentRow >= rows)
            EndGame();
    }
}