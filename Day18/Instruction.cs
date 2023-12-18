namespace Day18;

record Instruction(Direction Direction, int Amount)
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
        return new Instruction(direction, int.Parse(split[1]));
    }

    public static Instruction FromLineReal(string line)
    {
        var split = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);
        return new Instruction((Direction)int.Parse(split[2][7].ToString()), Convert.ToInt32("0x" + split[2][2..^2], 16));
    }
}