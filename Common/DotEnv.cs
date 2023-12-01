namespace Common;

public static class DotEnv
{
    public static Dictionary<string, string> Load()
    {
        const string FilePath = ".env";

        if (!File.Exists(FilePath))
        {
            throw new FileNotFoundException(".env not found", FilePath);
        }

        Dictionary<string, string> results = new();
        foreach (string line in File.ReadAllLines(FilePath))
        {
            int separatorIndex = line.IndexOf('=');
            results.Add(line[0..separatorIndex], line[(separatorIndex + 1)..]);
        }

        return results;
    }
} 