using Common;
using Day12;

string puzzleInput = await Util.GetPuzzleInput(12);

string[] puzzleLines = puzzleInput.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
OperationalRecord[] records = puzzleLines.Select(OperationalRecord.FromLine).ToArray();
long[] numPossibilities = records.Select(r => r.MatchingOperationalRecords()).ToArray();
Console.WriteLine($"Sum possibilities: {numPossibilities.Sum()}");

OperationalRecord[] unfoldedRecords = records.Select(r => r.Unfolded(5)).ToArray();
long[] numPossibilitiesUnfolded = unfoldedRecords.Select(r => r.MatchingOperationalRecords()).ToArray();
Console.WriteLine($"Sum possibilities unfolded: {numPossibilitiesUnfolded.Sum()}");