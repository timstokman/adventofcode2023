namespace Day19;

public record Rule(Operator? Operator, Rating? Rating, int? Comparison, string TargetFlow)
{
    public static Rule FromString(string str)
    {
        if (str.Contains(":"))
        {
            var split = str.Split(":");
            return new Rule((Operator)split[0][1], (Rating)split[0][0], int.Parse(split[0][2..]), split[1]);
        }
        else
        {
            return new Rule(null, null, null, str);
        }
    }
}