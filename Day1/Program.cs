using System.Text.RegularExpressions;
using Common;

int GetCalibrationValue(string line)
{
    IEnumerable<int?> digits = line.Select(c => int.TryParse(c.ToString(), out int n) ? (int?)n : null).Where(d => d != null);
    return digits.First().Value * 10 + digits.Last().Value;
}

int GetSumCalibrationValues(string input)
    => input.Split(Environment.NewLine).Where(l => !string.IsNullOrWhiteSpace(l)).Select(GetCalibrationValue).Sum();

int GetRealCalibrationValue(string line)
{
    Dictionary<string, int> mapping = new()
    {
        { "one", 1 },
        { "two", 2 },
        { "three", 3 },
        { "four", 4 },
        { "five", 5 },
        { "six", 6 },
        { "seven", 7 },
        { "eight", 8 },
        { "nine", 9 },
        { "0", 0 },
        { "1", 1 },
        { "2", 2 },
        { "3", 3 },
        { "4", 4 },
        { "5", 5 },
        { "6", 6 },
        { "7", 7 },
        { "8", 8 },
        { "9", 9 },
    };
    Regex digitsRegex = new Regex($"(?={string.Join("|", mapping.Keys)})");
    var matches = digitsRegex.Matches(line);
    IEnumerable<int> digits = matches.Select(m => mapping.First(map => line[m.Index..].StartsWith(map.Key)).Value).ToList();
    return digits.First() * 10 + digits.Last();
}

int GetSumRealCalibrationValues(string input)
    => input.Split(Environment.NewLine).Where(l => !string.IsNullOrWhiteSpace(l)).Select(GetRealCalibrationValue).Sum();

string puzzleInput = await Util.GetPuzzleInput(1);
int sumCalibrationValues = GetSumCalibrationValues(puzzleInput);
int sumRealCalibrationValues = GetSumRealCalibrationValues(puzzleInput);
Console.WriteLine($"Sum calibration values: {sumCalibrationValues}");
Console.WriteLine($"Sum real calibration values: {sumRealCalibrationValues}");