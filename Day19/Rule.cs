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
    
    public Restrictions SplitForMatching(Restrictions current)
    {
        Restriction ratingRestriction = current.ByRating(Rating.Value);
        return current.With(Rating.Value,
            Operator == Day19.Operator.Greater ?
                ratingRestriction with { Start = Math.Max(ratingRestriction.Start, Comparison.Value + 1) } :
                ratingRestriction with { End = Math.Min(ratingRestriction.End, Comparison.Value - 1) });
    }
    
    public Restrictions SplitForNonMatching(Restrictions current)
    {
        Restriction ratingRestriction = current.ByRating(Rating.Value);
        return current.With(Rating.Value,
            Operator == Day19.Operator.Greater ? 
                ratingRestriction with { End = Math.Min(ratingRestriction.End, Comparison.Value) } :
                ratingRestriction with { Start = Math.Max(ratingRestriction.Start, Comparison.Value) });
    }
}