namespace Day12;

public record OperationalRecord(char[] Springs, int[] Groups)
{
    public static OperationalRecord FromLine(string line)
    {
        string[] split = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        return new OperationalRecord(split[0].ToCharArray(), split[1].Split(",", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray());
    }

    public OperationalRecord Unfolded(int num = 5)
        => new OperationalRecord(string.Join("?", Enumerable.Repeat(string.Join("", Springs), num)).ToCharArray(), Enumerable.Repeat(Groups, 5).SelectMany(g => g).ToArray());

    public long MatchingOperationalRecords()
    {
        Dictionary<(int GroupIndex, int Index), long> cache = new();
        return MatchingOperationalRecords(cache, 0, 0);
    }
    
    private long MatchingOperationalRecords(Dictionary<(int GroupIndex, int Index), long> cache, int groupIndex, int index)
    {
        if (cache.ContainsKey((groupIndex, index)))
        {
            return cache[(groupIndex, index)];
        }

        long count;

        if (groupIndex >= Groups.Length || index >= Springs.Length)
        {
            if (groupIndex == Groups.Length && (index >= Springs.Length || Springs[index..].All(s => s == '.' || s == '?')))
            {
                count = 1;
            }
            else
            {
                count = 0;
            }
        }
        else
        {
            bool canPutGroup = Springs.Length - index >= Groups[groupIndex] &&
                               Springs[index..].Take(Groups[groupIndex]).All(c => c == '#' || c == '?') &&
                               (index + Groups[groupIndex] >= Springs.Length || Springs[index + Groups[groupIndex]] == '.' || Springs[index + Groups[groupIndex]] == '?');
            bool isInvalid = (!canPutGroup && Springs[index] == '#') || Springs[index..].Count(c => c == '#') > Groups[groupIndex..].Sum() || Springs[index..].Count(c => c == '#' || c == '?') < Groups[groupIndex..].Sum();
            bool canAdvance = Springs[index] == '.' || Springs[index] == '?';

            if (isInvalid)
            {
                count = 0;
            }
            else
            {
                count = (canAdvance ? MatchingOperationalRecords(cache, groupIndex, index + 1 /*, solution.Concat(new[] { '.' })*/) : 0) + (canPutGroup ? MatchingOperationalRecords(cache, groupIndex + 1, index + Groups[groupIndex] + 1 /*, solution.Concat(Enumerable.Repeat('#', Groups[groupIndex])).Concat(new[] { '.' })*/) : 0);
            }
        }

        cache[(groupIndex, index)] = count;
        return count;
    }
}