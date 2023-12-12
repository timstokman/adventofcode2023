using Common;
using Day12;

string puzzleInput = await Util.GetPuzzleInput(12);

string[] puzzleLines = puzzleInput.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
Console.WriteLine(string.Join(Environment.NewLine, puzzleLines.Select((l, i) => $"{i} {l}")));
OperationalRecord[] records = puzzleLines.Select(OperationalRecord.FromLine).ToArray();
long[] numPossibilities = records.Select((r, i) =>
{ 
    long num = r.MatchingOperationalRecords();
    Console.WriteLine($"{i} {num}");
    return num;
}).ToArray();
Console.WriteLine(numPossibilities.Sum());

OperationalRecord[] unfoldedRecords = records.Select(r => r.Unfolded(5)).ToArray();
long[] numPossibilitiesUnfolded = unfoldedRecords.Select((r, i) =>
{ 
    long num = r.MatchingOperationalRecords();
    Console.WriteLine($"{i} {num}");
    return num;
}).ToArray();
Console.WriteLine(numPossibilitiesUnfolded.Sum());