using Common;

int NextValue(int[] series)
{
    List<int[]> diffs = new() { series };
    while (diffs.Last().Any(d => d != 0))
    {
        var lastDiff = diffs.Last();
        var diff = Enumerable.Range(0, lastDiff.Length - 1).Select(i => lastDiff[i + 1] - lastDiff[i]).ToArray();
        diffs.Add(diff);
    }

    int lastValue = 0;
    diffs.Reverse();
    foreach (var diff in diffs.Skip(1))
    {
        lastValue += diff.Last();
    }
    return lastValue;
}

int PreviousValue(int[] series)
{
    List<int[]> diffs = new() { series };
    while (diffs.Last().Any(d => d != 0))
    {
        var lastDiff = diffs.Last();
        var diff = Enumerable.Range(0, lastDiff.Length - 1).Select(i => lastDiff[i + 1] - lastDiff[i]).ToArray();
        diffs.Add(diff);
    }

    int previousValue = 0;
    diffs.Reverse();
    foreach (var diff in diffs.Skip(1))
    {
        previousValue = diff.First() - previousValue;
    }
    return previousValue;
}

string puzzleInput = await Util.GetPuzzleInput(9);
string[] puzzleLines = puzzleInput.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
int[][] series = puzzleLines.Select(line => line.Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray()).ToArray();
int sumNextValues = series.Select(NextValue).Sum();
Console.WriteLine(sumNextValues);
int sumPreviousValues = series.Select(PreviousValue).Sum();
Console.WriteLine(sumPreviousValues);
