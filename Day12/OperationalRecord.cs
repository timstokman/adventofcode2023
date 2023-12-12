namespace Day12;

public record OperationalRecord(char[] Springs, int[] Groups)
{
    public static OperationalRecord FromLine(string line)
    {
        string[] split = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        return new OperationalRecord(split[0].ToCharArray(), split[1].Split(",", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray());
    }

    public IEnumerable<int[]> WaysToSplit(int numBucket, int numItem)
    {
        if (numBucket == 1)
        {
            yield return [ numItem ];
        }
        else if (numBucket > 1 && numItem > 0)
        {
            for (int i = 0; i <= numItem; i++)
            {
                int[] start = [ i ];
                foreach (int[] nextBucketSplit in WaysToSplit(numBucket - 1, numItem - i))
                {
                    yield return start.Concat(nextBucketSplit).ToArray();
                }
            }
        }
    }

    public IEnumerable<char> ToEffectiveSpring(int[] split)
    {
        int splitIndex = 0;
        foreach (int s in split)
        {
            for (int i = 0; i < s; i++)
            {
                yield return '.';
            }

            if (splitIndex < split.Length - 1)
            {
                for (int i = 0; i < Groups[splitIndex]; i++)
                {
                    yield return '#';
                }
            }

            splitIndex++;
        }
    }

    public bool IsMatchingSplit(int[] split)
    {
        if (split.Skip(1).Take(split.Length - 2).Any(s => s == 0)) // At least one between
        {
            return false;
        }

        char[] effectiveSpring = ToEffectiveSpring(split).ToArray();
        return Springs.Select((spring, i) => spring == '?' || effectiveSpring[i] == spring).All(m => m);
    }

    public int MatchingOperationalRecords()
    {
        int numBuckets = Groups.Length + 1;
        int numItems = Springs.Length - Groups.Sum();

        return WaysToSplit(numBuckets, numItems).Count(IsMatchingSplit);
    }
}