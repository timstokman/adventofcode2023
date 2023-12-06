// See https://aka.ms/new-console-template for more information

using System.Text.RegularExpressions;
using Common;

IEnumerable<long> LongRange(long start, long count) {
    long end = start + count;
    for (long current = start; current < end; current++)
    {
        yield return current;
    }
}

int WaysToWin(long time, long minDistance)
{
    var isWinning = LongRange(0, time + 1).Select(timePressed =>
    {
        long speed = timePressed;
        long moved = (time - timePressed) * speed;
        return moved > minDistance;
    });
    return isWinning.Count(i => i);
}

string puzzleInput = await Util.GetPuzzleInput(6);
string[] puzzleLines = puzzleInput.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

long[] times = new Regex(@"\s+").Split(puzzleLines[0]).Skip(1).Select(long.Parse).ToArray();
long[] distances = new Regex(@"\s+").Split(puzzleLines[1]).Skip(1).Select(long.Parse).ToArray();
int[] waysToWin = LongRange(0, times.Length).Select(i => WaysToWin(times[i], distances[i])).ToArray();
long multWaysToWin = waysToWin.Aggregate(1, (l, r) => l * r);
Console.WriteLine(multWaysToWin);

long realTime = long.Parse(puzzleLines[0].Replace(" ", "")[5..]);
long realDistance = long.Parse(puzzleLines[1].Replace(" ", "")[9..]);
int realWaysToWin = WaysToWin(realTime, realDistance);
Console.WriteLine(realWaysToWin);