// See https://aka.ms/new-console-template for more information

using Common;
using Day2;

string input = await Util.GetPuzzleInput(2);
List<Game> games = Util.SplitInLines(input).Select(Game.GameFromLine).ToList();
GameColors total = new(12, 13, 14);
int sumPossible = games.Where(game => game.IsPossibleWith(total)).Sum(game => game.Num);
int sumPowers = games.Select(g => g.MinColors).Sum(c => c.Power);
Console.WriteLine($"Sum possible game numbers: {sumPossible}");
Console.WriteLine($"Sum powers: {sumPowers}");