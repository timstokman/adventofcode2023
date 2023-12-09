using Common;

IEnumerable<int[]> Diffs(int[] series)
{
    yield return series;
    int[] lastDiff = series;
    do
    {
        int[] diff = Enumerable.Range(0, lastDiff.Length - 1).Select(i => lastDiff[i + 1] - lastDiff[i]).ToArray();
        yield return diff;
        lastDiff = diff;
    } while (lastDiff.Any(d => d != 0));
}

int NextValue(int[] series)
{
    int lastValue = 0;
    foreach (int[] diff in Diffs(series).Reverse().Skip(1))
    {
        lastValue += diff.Last();
    }
    return lastValue;
}

int PreviousValue(int[] series)
{
    int previousValue = 0;
    foreach (int[] diff in Diffs(series).Reverse().Skip(1))
    {
        previousValue = diff.First() - previousValue;
    }
    return previousValue;
}

string puzzleInput = await Util.GetPuzzleInput(9);
int[][] series = puzzleInput
    .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
    .Select(line => line.Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray())
    .ToArray();
int sumNextValues = series.Select(NextValue).Sum();
Console.WriteLine($"Sum next values: {sumNextValues}");
int sumPreviousValues = series.Select(PreviousValue).Sum();
Console.WriteLine($"Sum previous values: {sumPreviousValues}");
