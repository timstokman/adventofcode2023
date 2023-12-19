namespace Day19;

public record Part(Dictionary<Rating, int> Ratings)
{
    public static Part PartFromLine(string line)
        => new Part(line[1..^1].Split(",").Select(rat => rat.Split('=')).ToDictionary(rat => (Rating)rat[0][0], rat => int.Parse(rat[1])));

    public int Sum
        => Ratings.Values.Sum();
}