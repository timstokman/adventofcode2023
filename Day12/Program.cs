using Common;
using Day12;

string puzzleInput = await Util.GetPuzzleInput(12);
string[] puzzleLines = puzzleInput.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

IEnumerable<OperationalRecord> records = puzzleLines.Select(OperationalRecord.FromLine);
IEnumerable<long> numPossibilities = records.Select(r => r.MatchingOperationalRecords());
Console.WriteLine($"Sum possibilities: {numPossibilities.Sum()}");

IEnumerable<OperationalRecord> unfoldedRecords = records.Select(r => r.Unfolded(5));
IEnumerable<long> numPossibilitiesUnfolded = unfoldedRecords.Select(r => r.MatchingOperationalRecords());
Console.WriteLine($"Sum possibilities unfolded: {numPossibilitiesUnfolded.Sum()}");