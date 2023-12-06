using Common;

long WaysToWin(long time, long minDistance)
{
    double sqrt = Math.Sqrt(time * time - 4 * minDistance);
    long minPressed = (long)Math.Floor(0.5 * (time - sqrt));
    minPressed += (time - minPressed) * minPressed > minDistance ? 0 : 1;
    long maxPressed = (long)Math.Floor(0.5 * (sqrt + time));
    maxPressed += (time - maxPressed) * maxPressed > minDistance ? 1 : 0;
    return Math.Max(0, maxPressed - minPressed);
}

string puzzleInput = await Util.GetPuzzleInput(6);
string[] puzzleLines = puzzleInput.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

long[] times = puzzleLines[0].Split(" ", StringSplitOptions.RemoveEmptyEntries).Skip(1).Select(long.Parse).ToArray();
long[] distances = puzzleLines[1].Split(" ", StringSplitOptions.RemoveEmptyEntries).Skip(1).Select(long.Parse).ToArray();
IEnumerable<long> waysToWin = Enumerable.Range(0, times.Length).Select(i => WaysToWin(times[i], distances[i]));
long multWaysToWin = waysToWin.Aggregate(1L, (l, r) => l * r);
Console.WriteLine($"Multiplied ways to win: {multWaysToWin}");

long realTime = long.Parse(puzzleLines[0].Replace(" ", "")[5..]);
long realDistance = long.Parse(puzzleLines[1].Replace(" ", "")[9..]);
long realWaysToWin = WaysToWin(realTime, realDistance);
Console.WriteLine($"Real ways to win: {realWaysToWin}");