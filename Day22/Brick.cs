using System.Text.RegularExpressions;

namespace Day22;

public record Brick(int Index, Position Start, Position End)
{
    public static Brick FromLine(int index, string line)
    {
        string[] split = new Regex(@",|~").Split(line);
        return new Brick(index, new Position(int.Parse(split[0]), int.Parse(split[1]), int.Parse(split[2])), new Position(int.Parse(split[3]), int.Parse(split[4]), int.Parse(split[5])));
    }

    public Brick OneDown
        => new(Index, Start with { Z = Start.Z - 1 }, End with { Z = End.Z - 1 });

    public Brick OneUp
        => new(Index, Start with { Z = Start.Z + 1 }, End with { Z = End.Z + 1 });

    public int Layer
        => Math.Min(Start.Z, End.Z);

    public Dimension Orientation
    {
        get
        {
            if (Start.X - End.X != 0)
            {
                return Dimension.X;
            }
            else if (Start.Y - End.Y != 0)
            {
                return Dimension.Y;
            }
            else
            {
                return Dimension.Z;
            }
        }
    }

    public bool Direction
    {
        get
        {
            switch (Orientation)
            {
                case Dimension.X:
                    return Start.X <= End.X;
                case Dimension.Y:
                    return Start.Y <= End.Y;
                case Dimension.Z:
                    return Start.Z <= End.Z;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    public int NumBricks
        => Math.Max(Math.Abs(Start.X - End.X), Math.Max(Math.Abs(Start.Y - End.Y), Math.Abs(Start.Z - End.Z))) + 1;

    public IEnumerable<Position> Positions()
    {
        var position = Start;
        yield return position;
        for (int i = 0; i < NumBricks - 1; i++)
        {
            int diff = Direction ? 1 : -1;
            position = new Position(position.X + (Orientation == Dimension.X ? diff : 0), position.Y + (Orientation == Dimension.Y ? diff : 0), position.Z + (Orientation == Dimension.Z ? diff : 0));
            yield return position;
        }
    }

    public override string ToString()
        => $"Brick {{ Index: {Index}, Start: {Start}, End: {End} }}";
}