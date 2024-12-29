using System.Text.Json;
using System.Text.Json.Serialization;
using static Wordle.WordleView;

namespace Wordle;

public class History
{
    public record HistoryEntry(string[] textRows, WordleTile[] tiles)
    {
        [JsonInclude] public readonly string[] textRows = textRows;
        [JsonInclude] public readonly WordleTile[] tiles = tiles;
    }

    public List<HistoryEntry> List { get; private set; } = new();

    private static readonly string filePath = Path.Combine(FileSystem.AppDataDirectory, "history.txt");

    public History()
    {
        if (!File.Exists(filePath))
            return;

        using FileStream stream = File.OpenRead(filePath);
        var obj = JsonSerializer.Deserialize(stream, typeof(List<HistoryEntry>));
        if (obj != null)
            List = (List<HistoryEntry>)obj;
    }

    public void AddEntry(HistoryEntry entry) => List.Insert(0, entry);

    public async Task WriteToFileAsync()
    {
        using FileStream stream = File.OpenWrite(filePath);
        await JsonSerializer.SerializeAsync(stream, List);
    }
}
