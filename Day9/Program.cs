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
    => Diffs(series).Sum(diff => diff.Last());

int PreviousValue(int[] series)
    => NextValue(series.Reverse().ToArray());

string puzzleInput = await Util.GetPuzzleInput(9);
int[][] series = puzzleInput
    .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
    .Select(line => line.Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray())
    .ToArray();
int sumNextValues = series.Sum(NextValue);
Console.WriteLine($"Sum next values: {sumNextValues}");
int sumPreviousValues = series.Sum(PreviousValue);
Console.WriteLine($"Sum previous values: {sumPreviousValues}");