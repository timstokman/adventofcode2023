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
    => Diffs(series).Select(diff => diff.Last()).Sum();

string puzzleInput = await Util.GetPuzzleInput(9);
int[][] series = puzzleInput
    .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
    .Select(line => line.Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray())
    .ToArray();
int sumNextValues = series.Select(NextValue).Sum();
Console.WriteLine($"Sum next values: {sumNextValues}");
int sumPreviousValues = series.Select(s => s.Reverse().ToArray()).Select(NextValue).Sum();
Console.WriteLine($"Sum previous values: {sumPreviousValues}");