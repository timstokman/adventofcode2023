﻿using Common;

int GetCalibrationValue(string line)
{
    IEnumerable<int?> digits = line.Select(c => int.TryParse(c.ToString(), out int n) ? (int?)n : null)
                                   .Where(d => d != null);
    return digits.First().Value * 10 + digits.Last().Value;
}

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
    IEnumerable<int> digits = Enumerable.Range(0, line.Length)
                                        .Select(i => mapping.Keys.FirstOrDefault(digit => line[i..].StartsWith(digit)))
                                        .Where(digit => digit != null)
                                        .Select(digit => mapping[digit]);
    return digits.First() * 10 + digits.Last();
}

string puzzleInput = await Util.GetPuzzleInput(1);
IEnumerable<string> lines = puzzleInput.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
int sumCalibrationValues = lines.Sum(GetCalibrationValue);
int sumRealCalibrationValues = lines.Sum(GetRealCalibrationValue);

Console.WriteLine($"Sum calibration values: {sumCalibrationValues}");
Console.WriteLine($"Sum real calibration values: {sumRealCalibrationValues}");