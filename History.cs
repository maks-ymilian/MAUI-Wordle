using System.Text.Json;
using System.Text.Json.Serialization;
using static Wordle.WordleView;

namespace Wordle;

public class History
{
    public record HistoryEntry(
        [property: JsonInclude] int Rows,
        [property: JsonInclude] int Columns,
        [property: JsonInclude] string[] TextRows,
        [property: JsonInclude] WordleTile[] Tiles,
        [property: JsonInclude] string Answer,
        [property: JsonInclude] DateTime DateTime
        );

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
