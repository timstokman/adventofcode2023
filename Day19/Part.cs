namespace Day19;

public record Part(int X, int M, int A, int S)
{
    public static Part PartFromLine(string line)
    {
        Dictionary<Rating, int> partItem = line[1..^1].Split(",").Select(rat => rat.Split('=')).ToDictionary(rat => (Rating)rat[0][0], rat => int.Parse(rat[1]));
        return new Part(partItem[Rating.X], partItem[Rating.M], partItem[Rating.A], partItem[Rating.S]);
    }

    public int Sum
        => X + M + A + S;
}