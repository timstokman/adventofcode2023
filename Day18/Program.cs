using Common;
using Day18;
using Position = (int X, int Y);

string puzzleInput = await Util.GetPuzzleInput(18);

Position MoveInDirection(Position position, Direction direction)
{
    return direction switch
    {
        Direction.Top => new Position(position.X, position.Y - 1),
        Direction.Right => new Position(position.X + 1, position.Y),
        Direction.Bottom => new Position(position.X, position.Y + 1),
        Direction.Left => new Position(position.X - 1, position.Y),
        _ => throw new ArgumentOutOfRangeException()
    };
}

int Dug(Instruction[] instructions)
{
    Dictionary<Position, int> dug = new Dictionary<Position, int>();
    Position current = new Position(0, 0);
    dug[current] = 1;
    foreach (var instruction in instructions)
    {
        for (int i = 1; i <= instruction.Amount; i++)
        {
            current = MoveInDirection(current, instruction.Direction);
            dug[current] = 1;
        }
    }

    int minY = -208;
    int maxY = 15;
    int minX = -65;
    int maxX = 248;

    Position avgPosition = new Position((int)Math.Round(dug.Keys.Average(k => k.X)), (int)Math.Round(dug.Keys.Average(k => k.Y)));
    for (int y = 0; y <= maxY - minY; y++)
    {
        for (int x = 0; x <= maxX - minX; x++)
        {
            if (dug.ContainsKey((x + minX, y + minY)))
            {
                Console.Write('#');
            }
            else if (x + minX == avgPosition.X && y + minY == avgPosition.Y)
            {
                Console.Write("!");
            }
            else
            {
                Console.Write('.');
            }
        }

        Console.WriteLine();
    }

    // Dumb way to start floodfill, will improve later
    Queue<Position> toFill = new();
    toFill.Enqueue(avgPosition);

    while (toFill.Count > 0)
    {
        var item = toFill.Dequeue();
        if (!dug.TryAdd(item, 1))
        {
            continue;
        }

        foreach (var d in Enum.GetValues<Direction>())
        {
            toFill.Enqueue(MoveInDirection(item, d));
        }
    }

    return dug.Count;
}

Instruction[] instructions = puzzleInput.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).Select(l => Instruction.FromLine(l)).ToArray();
Console.WriteLine(Dug(instructions));