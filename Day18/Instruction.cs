namespace Day18;

record Instruction(Direction Direction, int Amount, string HexColor)
{
    public static Instruction FromLine(string line)
    {
        var split = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);
        var direction = split[0] switch
        {
            "D" => Direction.Bottom,
            "U" => Direction.Top,
            "R" => Direction.Right,
            "L" => Direction.Left,
            _ => throw new ArgumentOutOfRangeException(),
        };
        return new Instruction(direction, int.Parse(split[1]), split[2][2..^1]);
    }
}