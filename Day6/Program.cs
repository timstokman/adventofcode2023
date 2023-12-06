// See https://aka.ms/new-console-template for more information

using System.Text.RegularExpressions;
using Common;

long WaysToWin(long time, long minDistance)
{
    double minPressedStart = 0.5 * (time - Math.Sqrt(time * time - 4 * minDistance));
    minPressedStart = minPressedStart - Math.Truncate(minPressedStart) == 0.0 ? Math.Round(minPressedStart) + 1 : Math.Ceiling(minPressedStart);
    double maxPressedStart = 0.5 * (Math.Sqrt(time * time - 4 * minDistance) + time);
    maxPressedStart = maxPressedStart - Math.Truncate(maxPressedStart) == 0.0 ? Math.Round(maxPressedStart) + 1 : Math.Ceiling(maxPressedStart);
    return (long)Math.Round(maxPressedStart - minPressedStart);
}

string puzzleInput = await Util.GetPuzzleInput(6);
string[] puzzleLines = puzzleInput.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

long[] times = new Regex(@"\s+").Split(puzzleLines[0]).Skip(1).Select(long.Parse).ToArray();
long[] distances = new Regex(@"\s+").Split(puzzleLines[1]).Skip(1).Select(long.Parse).ToArray();
long[] waysToWin = Enumerable.Range(0, times.Length).Select(i => WaysToWin(times[i], distances[i])).ToArray();
long multWaysToWin = waysToWin.Aggregate(1l, (l, r) => l * r);
Console.WriteLine(multWaysToWin);

long realTime = long.Parse(puzzleLines[0].Replace(" ", "")[5..]);
long realDistance = long.Parse(puzzleLines[1].Replace(" ", "")[9..]);
long realWaysToWin = WaysToWin(realTime, realDistance);
Console.WriteLine(realWaysToWin);