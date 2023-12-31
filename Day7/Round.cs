namespace Day7;

public sealed class Round : IComparable<Round>
{
    public static char[] CardStrength = { 'A', 'K', 'Q', 'J', 'T', '9', '8', '7', '6', '5', '4', '3', '2' };

    public Round(char[] hand, int bid)
    {
        Hand = hand;
        Bid = bid;
        Frequency = Hand.GroupBy(h => h).ToDictionary(h => h.Key, h => h.Count());
        FrequencySorted = Frequency.Values.OrderDescending();
    }

    public char[] Hand { get; set; }

    public int Bid { get; set; }

    public Dictionary<char, int> Frequency { get; }

    public IEnumerable<int> FrequencySorted { get; }

    public static Round FromLine(string line)
    {
        string[] split = line.Split(" ");
        return new Round(split[0].ToCharArray(), int.Parse(split[1]));
    }

    public bool IsFiveOfAKind()
        => Frequency.Count == 1;

    public bool IsFourOfAKind()
        => FrequencySorted.First() == 4;

    public bool IsFullHouse()
        => FrequencySorted.SequenceEqual(new[] { 3, 2 });

    public bool IsThreeOfAKind()
        => FrequencySorted.SequenceEqual(new[] { 3, 1, 1 });

    public bool IsTwoPair()
        => FrequencySorted.SequenceEqual(new[] { 2, 2, 1 });

    public bool IsOnePair()
        => FrequencySorted.SequenceEqual(new[] { 2, 1, 1, 1 });

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

    public int CompareTo(Round? other)
    {
        if (other == null)
        {
            return -1;
        }

        int typeDiff = Type() - other.Type();
        return typeDiff != 0 ? typeDiff : Enumerable.Range(0, Hand.Length).Select(i => Array.IndexOf(CardStrength, other.Hand[i]) - Array.IndexOf(CardStrength, Hand[i])).FirstOrDefault(diff => diff != 0);
    }
}