using System.Text.RegularExpressions;
using Common;

List<(int lineIndex, Match match)> NumberMatches(string[] puzzleLines)
{
   Regex numberMatch = new Regex(@"\d+");
   return puzzleLines.SelectMany((line, lineIndex) => numberMatch.Matches(line).Select(m => (lineIndex, m))).ToList();
}

List<(int lineIndex, Match match)> GearMatches(string[] puzzleLines)
{
   Regex gearMatch = new Regex(@"\*");
   return puzzleLines.SelectMany((line, lineIndex) => gearMatch.Matches(line).Select(m => (lineIndex, m))).ToList();
}

IEnumerable<int> PartNumbers(List<(int lineIndex, Match match)> numberMatches, string[] puzzleLines)
{
   return numberMatches.Where(i =>
   {
      (int lineIndex, var match) = i;
      var indexesToCheck = new List<(int Y, int X)>()
      {
         (lineIndex, match.Index - 1),
         (lineIndex - 1, match.Index - 1),
         (lineIndex + 1, match.Index - 1),

         (lineIndex, match.Index + match.Length),
         (lineIndex - 1, match.Index + match.Length),
         (lineIndex + 1, match.Index + match.Length)
      };
      indexesToCheck.AddRange(Enumerable.Range(match.Index, match.Length).Select(i => (lineIndex + 1, i)));
      indexesToCheck.AddRange(Enumerable.Range(match.Index, match.Length).Select(i => (lineIndex - 1, i)));
      var symbolIndexes = indexesToCheck.Where(toCheck => toCheck.X >= 0 && toCheck.X < puzzleLines[0].Length && toCheck.Y >= 0 && toCheck.Y < puzzleLines.Length && puzzleLines[toCheck.Y][toCheck.X] != '.' && !char.IsDigit(puzzleLines[toCheck.Y][toCheck.X]));
      return symbolIndexes.Any();
   }).Select(num => int.Parse(num.match.Value));
}

IEnumerable<int> GearRatios(IEnumerable<(int lineIndex, Match match)> gearMatches, string[] puzzleLines, List<(int lineIndex, Match match)> numberMatches)
{
   return gearMatches.Select(i =>
   {
      (int lineIndex, var match) = i;
      var indexesToCheck = new List<(int Y, int X)>()
      {
         (lineIndex, match.Index - 1),
         (lineIndex - 1, match.Index - 1),
         (lineIndex + 1, match.Index - 1),

         (lineIndex + 1, match.Index),
         (lineIndex - 1, match.Index),

         (lineIndex, match.Index + match.Length),
         (lineIndex - 1, match.Index + match.Length),
         (lineIndex + 1, match.Index + match.Length)
      };
      var digitIndexes = indexesToCheck.Where(toCheck => toCheck.X >= 0 && toCheck.X < puzzleLines[0].Length && toCheck.Y >= 0 && toCheck.Y < puzzleLines.Length && char.IsDigit(puzzleLines[toCheck.Y][toCheck.X]));
      List<(int lineIndex, Match match)?> matchingDigits = digitIndexes.Select(matchingDigit => ((int lineIndex, Match m)?)numberMatches.FirstOrDefault(numberMatch => matchingDigit.X >= numberMatch.match.Index && matchingDigit.X < numberMatch.match.Index + numberMatch.match.Length && numberMatch.lineIndex == matchingDigit.Y)).Where(m => m != null).DistinctBy(item => (item.Value.lineIndex, item.Value.m.Index)).ToList();
      if (matchingDigits.Count() == 2)
      {
         return int.Parse(matchingDigits.First().Value.match.Value) * int.Parse(matchingDigits.Last().Value.match.Value);
      }
      else
      {
         return 0;
      }
   });
}

string puzzleInput = await Util.GetPuzzleInput(3);
string[] puzzleLines = puzzleInput.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

var numberMatches = NumberMatches(puzzleLines);
var partNumbers = PartNumbers(numberMatches, puzzleLines);
int sumPartNumbers = partNumbers.Sum();

var gearMatches = GearMatches(puzzleLines);
var gearRatios = GearRatios(gearMatches, puzzleLines, numberMatches);
int sumGearRatios = gearRatios.Sum();

Console.WriteLine($"Sum part numbers: {sumPartNumbers}");
Console.WriteLine($"Sum gear ratios: {sumGearRatios}");
