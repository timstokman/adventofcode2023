using Common;

IEnumerable<long> LongRange(long start, long count) {
    long end = start + count;
    for (long current = start; current < end; current++)
    {
        yield return current;
    }
}

IEnumerable<long> LocationNumbers(IEnumerable<long> seedNumbers, long[][][] maps)
{
    return seedNumbers.Select(seed =>
    {
        long val = seed;
        foreach (long[][] map in maps)
        {
            var matchingRange = map.FirstOrDefault(ran => val >= ran[1] && val < ran[1] + ran[2]);
            if (matchingRange != null)
            {
                val = val - matchingRange[1] + matchingRange[0];
            }
        }

        return val;
    });
}

string puzzleInput = await Util.GetPuzzleInput(5);

string[] sections = puzzleInput.Split(Environment.NewLine + Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
long[] seeds = sections[0][7..].Split(" ", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Select(long.Parse).ToArray();
long[][][] maps = sections[1..].Select(sec => sec.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)[1..].Select(secLine => secLine.Split(" ").Select(long.Parse).ToArray()).ToArray()).ToArray();
IEnumerable<long> locationNumbers = LocationNumbers(seeds, maps);
long minLocation = locationNumbers.Min();
IEnumerable<long> realSeedNumbers = Enumerable.Range(0, seeds.Length / 2).SelectMany(r => LongRange(seeds[r * 2], seeds[r * 2 + 1]));
IEnumerable<long> realLocationNumbers = LocationNumbers(realSeedNumbers, maps);
long realMinLocation = realLocationNumbers.Min();
Console.WriteLine(minLocation);
Console.WriteLine(realMinLocation);
