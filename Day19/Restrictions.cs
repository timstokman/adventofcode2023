namespace Day19;

public record Restrictions(Restriction X, Restriction M, Restriction A, Restriction S)
{
    public long NumValues
        => X.NumValues * M.NumValues * A.NumValues * S.NumValues;

    public bool Valid
        => X.Valid && M.Valid && A.Valid && S.Valid;

    public bool InRange(Part part)
        => X.InRange(part.X) && M.InRange(part.M) && A.InRange(part.A) && S.InRange(part.S);

    public Restriction ByRating(Rating rating)
    {
        return rating switch
        {
            Rating.X => X,
            Rating.M => M,
            Rating.A => A,
            Rating.S => S,
            _ => throw new ArgumentOutOfRangeException(nameof(rating), rating, null)
        };
    }

    public Restrictions With(Rating rating, Restriction newRestriction)
    {
        return rating switch
        {
            Rating.X => this with { X = newRestriction },
            Rating.M => this with { M = newRestriction },
            Rating.A => this with { A = newRestriction },
            Rating.S => this with { S = newRestriction },
            _ => throw new ArgumentOutOfRangeException(nameof(rating), rating, null)
        };
    }
}