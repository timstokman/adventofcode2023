namespace Day7;

public sealed class RoundWithJRules : IComparable<RoundWithJRules>
{
    public static char[] CardStrength = { 'A', 'K', 'Q', 'T', '9', '8', '7', '6', '5', '4', '3', '2', 'J' };

    public RoundWithJRules(char[] hand, int bid)
    {
        Hand = hand;
        Bid = bid;
        Frequency = Hand.GroupBy(h => h).ToDictionary(h => h.Key, h => h.Count());
        FrequencyWithoutJ = Hand.Where(h => h != 'J').GroupBy(h => h).ToDictionary(h => h.Key, h => h.Count());
        FrequencySorted = Frequency.Values.OrderDescending();
        FrequencySortedWithoutJ = FrequencyWithoutJ.Values.OrderDescending();
    }

    public int Bid { get; set; }

    public char[] Hand { get; set; }

    public static RoundWithJRules FromLine(string line)
    {
        string[] split = line.Split(" ");
        return new RoundWithJRules(split[0].ToCharArray(), int.Parse(split[1]));
    }

    public Dictionary<char, int> Frequency { get; }

    public Dictionary<char, int> FrequencyWithoutJ { get; }

    public IEnumerable<int> FrequencySorted { get; }

    public IEnumerable<int> FrequencySortedWithoutJ { get; }
    
    public int NumJokers
    {
        get
        {
            Frequency.TryGetValue('J', out int j);
            return j;
        }
    }

    public bool IsFiveOfAKind()
        => FrequencyWithoutJ.Count <= 1;

    public bool IsFourOfAKind()
        => FrequencyWithoutJ.Count == 2 && FrequencySortedWithoutJ.Last() == 1;

    public bool IsFullHouse()
        => !IsFourOfAKind() && FrequencyWithoutJ.Count == 2 && FrequencySortedWithoutJ.First() < 4;

    public bool IsThreeOfAKind()
        => !IsFullHouse() && !IsFourOfAKind() && !IsFiveOfAKind() && FrequencySortedWithoutJ.First() + NumJokers == 3;

    public bool IsTwoPair()
        => !IsFiveOfAKind() && !IsFourOfAKind() && !IsFullHouse() && !IsThreeOfAKind() && FrequencySortedWithoutJ.SequenceEqual(new[] { 2, 2, 1 });

    public bool IsOnePair()
        => FrequencySortedWithoutJ.First() + NumJokers == 2;

    public int Type()
    {
        if (IsFiveOfAKind())
        {
            return 6;
        }
        else if (IsFourOfAKind())
        {
            return 5;
        }
        else if (IsFullHouse())
        {
            return 4;
        }
        else if (IsThreeOfAKind())
        {
            return 3;
        }
        else if (IsTwoPair())
        {
            return 2;
        }
        else if (IsOnePair())
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }

    public int CompareTo(RoundWithJRules? other)
    {
        if (other == null)
        {
            return -1;
        }

        int typeDiff = Type() - other.Type();
        return typeDiff != 0 ? typeDiff : Enumerable.Range(0, Hand.Length).Select(i => Array.IndexOf(CardStrength, other.Hand[i]) - Array.IndexOf(CardStrength, Hand[i])).FirstOrDefault(diff => diff != 0);
    }
}