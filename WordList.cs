using System.Diagnostics;

namespace Wordle
{
    public class WordList(List<string> list, HashSet<string> hashSet)
    {
        private static readonly Random random = new();

        private readonly List<string> list = list;
        private readonly HashSet<string> hashSet = hashSet;

        public string GetRandomWord() => list[(int)random.NextInt64(list.Count)];
        public bool IsValidWord(string word) => hashSet.Contains(word.ToLower());
    }

    public class WordListManager
    {
        private class WordListLoader(string filePath, string url)
        {
            private static readonly HttpClient client = new();

            private WordList? wordList;

            public async Task<WordList> GetWordListAsync()
            {
                if (wordList != null)
                    return wordList;

                List<string> list = new(3000);
                HashSet<string> hashSet = new(3000);

                if (!File.Exists(filePath))
                {
                    using Stream wordStream = await client.GetStreamAsync(url).ConfigureAwait(false);
                    using FileStream fileStream = File.OpenWrite(filePath);

                    await wordStream.CopyToAsync(fileStream);

                    Trace.WriteLine($"Downloaded from {url} to {filePath}");
                }

                using FileStream stream = File.OpenRead(filePath);
                using StreamReader reader = new(stream);
                while (!reader.EndOfStream)
                {
                    string? line = reader.ReadLine();
                    if (line == null)
                        break;

                    line = line.ToLower();

                    list.Add(line);
                    if (!hashSet.Add(line))
                        Trace.WriteLine($"Found duplicate word \"{line}\"");
                }

                Trace.WriteLine($"Loaded {stream.Length} bytes ({list.Count} lines) from {filePath}");

                wordList = new(list, hashSet);
                return wordList;
            }
        }

        private readonly Dictionary<int, WordListLoader> loaders;

        public WordListManager()
        {
            static string GetPath(string name) => Path.Combine(FileSystem.CacheDirectory, name);

            loaders = new()
            {
                {
                    3,
                    new WordListLoader(GetPath("3-letter-words.txt"),
                        "https://raw.githubusercontent.com/Fj00/CEL/refs/heads/master/2-15/3.txt")
                },
                {
                    4,
                    new WordListLoader(GetPath("4-letter-words.txt"),
                        "https://raw.githubusercontent.com/Fj00/CEL/refs/heads/master/2-15/4.txt")
                },
                {
                    5,
                    new WordListLoader(GetPath("5-letter-words.txt"),
                        "https://raw.githubusercontent.com/DonH-ITS/jsonfiles/main/words.txt")
                },
                {
                    6,
                    new WordListLoader(GetPath("6-letter-words.txt"),
                        "https://raw.githubusercontent.com/Fj00/CEL/refs/heads/master/2-15/6.txt")
                },
                {
                    7,
                    new WordListLoader(GetPath("7-letter-words.txt"),
                        "https://raw.githubusercontent.com/Fj00/CEL/refs/heads/master/2-15/7.txt")
                },
                {
                    8,
                    new WordListLoader(GetPath("8-letter-words.txt"),
                        "https://raw.githubusercontent.com/Fj00/CEL/refs/heads/master/2-15/8.txt")
                },
            };
        }

        public async Task<WordList> GetWordList(int numLetters)
        {
            loaders.TryGetValue(numLetters, out WordListLoader? loader);
            if (loader == null)
                throw new InvalidOperationException("Unsupported number of letters");

            return await loader.GetWordListAsync().ConfigureAwait(false);
        }
    }
}