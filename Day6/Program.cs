// See https://aka.ms/new-console-template for more information

using System.Text.RegularExpressions;
using Common;

int WaysToWin(long time, long minDistance)
{
    double minPressedStart = Math.Ceiling(0.5 * (time - Math.Sqrt(time * time - 4 * minDistance)));
    double maxPressedStart = Math.Ceiling(0.5 * (Math.Sqrt(time * time - 4 * minDistance) + time));
    return (int)Math.Round(maxPressedStart - minPressedStart);
}

string puzzleInput = await Util.GetPuzzleInput(6);
string[] puzzleLines = puzzleInput.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

long[] times = new Regex(@"\s+").Split(puzzleLines[0]).Skip(1).Select(long.Parse).ToArray();
long[] distances = new Regex(@"\s+").Split(puzzleLines[1]).Skip(1).Select(long.Parse).ToArray();
int[] waysToWin = Enumerable.Range(0, times.Length).Select(i => WaysToWin(times[i], distances[i])).ToArray();
long multWaysToWin = waysToWin.Aggregate(1, (l, r) => l * r);
Console.WriteLine(multWaysToWin);

long realTime = long.Parse(puzzleLines[0].Replace(" ", "")[5..]);
long realDistance = long.Parse(puzzleLines[1].Replace(" ", "")[9..]);
int realWaysToWin = WaysToWin(realTime, realDistance);
Console.WriteLine(realWaysToWin);