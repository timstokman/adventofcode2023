using Common;
using Day12;

string puzzleInput = await Util.GetPuzzleInput(12);

string[] puzzleLines = puzzleInput.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
OperationalRecord[] records = puzzleLines.Select(OperationalRecord.FromLine).Select(r => r.Unfolded()).ToArray();
long[] numPossibilities = records.Select(r => r.MatchingOperationalRecords()).ToArray();
Console.WriteLine(numPossibilities.Sum());