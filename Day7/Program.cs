using Common;
using Day7;

string puzzle = await Util.GetPuzzleInput(7);
string[] puzzleLines = puzzle.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

int winnings = puzzleLines
    .Select(Round.FromLine)
    .OrderBy(r => r)
    .Select((round, i) => (i + 1) * round.Bid)
    .Sum();
Console.WriteLine($"Winnings: {winnings}");

int winningsWithJRules = puzzleLines
    .Select(RoundWithJRules.FromLine)
    .OrderBy(r => r)
    .Select((round, i) => (i + 1) * round.Bid)
    .Sum();
Console.WriteLine($"Winnings with joker rules: {winningsWithJRules}");
