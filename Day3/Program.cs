using System.Text.RegularExpressions;
using Common;

string puzzleInput = await Util.GetPuzzleInput(3);
string[] puzzleLines = puzzleInput.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

Regex numberMatch = new Regex(@"\d+");
List<(int lineIndex, Match match)> numberMatches = puzzleLines.SelectMany((line, lineIndex) => numberMatch.Matches(line).Select(m => (lineIndex, m))).ToList();
IEnumerable<int> partNumbers = numberMatches.Where(i =>
{
   (int lineIndex, var match) = i;
   var indexesToCheck = new List<(int Y, int X)>()
   {
      ( lineIndex, match.Index - 1 ), 
      ( lineIndex - 1, match.Index - 1 ), 
      ( lineIndex + 1, match.Index - 1 ),

      ( lineIndex, match.Index + match.Length ), 
      ( lineIndex - 1, match.Index + match.Length ),
      ( lineIndex + 1, match.Index + match.Length )
   };
   indexesToCheck.AddRange(Enumerable.Range(match.Index, match.Length).Select(i => (lineIndex + 1, i)));
   indexesToCheck.AddRange(Enumerable.Range(match.Index, match.Length).Select(i => (lineIndex - 1, i)));
   var symbolIndexes = indexesToCheck.Where(toCheck => toCheck.X >= 0 && toCheck.X < puzzleLines[0].Length && toCheck.Y >= 0 && toCheck.Y < puzzleLines.Length && puzzleLines[toCheck.Y][toCheck.X] != '.' && !char.IsDigit(puzzleLines[toCheck.Y][toCheck.X]));
   return symbolIndexes.Any();
}).Select(num => int.Parse(num.match.Value));
int sumPartNumbers = partNumbers.Sum();


Regex gearMatch = new Regex(@"\*");
IEnumerable<(int lineIndex, Match match)> gearMatches = puzzleLines.SelectMany((line, lineIndex) => gearMatch.Matches(line).Select(m => (lineIndex, m)));
IEnumerable<int> gearRatios = gearMatches.Select(i =>
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
int sumGearRatios = gearRatios.Sum();


Console.WriteLine(sumPartNumbers);
Console.WriteLine(sumGearRatios);
