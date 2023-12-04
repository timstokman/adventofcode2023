namespace Day4;

public record Card(int Number, List<int> WinningNumbers, List<int> OwningNumbers)
{
    public static Card FromLine(string line)
    {
        string[] split = line.Split(": ");
        int num = int.Parse(split[0][5..]);
        string[] splitSections = split[1].Split(" | ", StringSplitOptions.RemoveEmptyEntries);
        return new Card(num, splitSections[0].Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList(), splitSections[1].Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList());
    }

    public IEnumerable<int> MatchingNumbers
        => WinningNumbers.Intersect(OwningNumbers);

    public int Score
        => MatchingNumbers.Any() ? (int)Math.Pow(2, MatchingNumbers.Count() - 1) : 0;
}