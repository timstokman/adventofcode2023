using Common;
using Day7;
using Microsoft.VisualBasic.CompilerServices;

string puzzle = await Util.GetPuzzleInput(7);
var rounds = puzzle.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).Select(Round.FromLine).ToList();
rounds.Sort();
int winnings = rounds.Select((round, i) => (i + 1) * round.Bid).Sum();
Console.WriteLine(winnings);