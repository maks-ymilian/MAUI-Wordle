using static Wordle.WordleView;

namespace Wordle;

public class History
{
    public struct HistoryEntry
    {
        public string[] textRows;
        public WordleTile[] tiles;
    }

    public List<HistoryEntry> List { get; private set; } = new();

    public void AddEntry(HistoryEntry entry) => List.Add(entry);
}
