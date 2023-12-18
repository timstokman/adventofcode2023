using Common;
using Day18;

string puzzleInput = await Util.GetPuzzleInput(18);

Position MoveInDirection(Position position, Direction direction, int amount)
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

IEnumerable<Edge> ToGraph(Instruction[] instructions)
{
    var current = new Position(0, 0);
    foreach (var instruction in instructions)
    {
        var next = MoveInDirection(current, instruction.Direction, instruction.Amount);
        yield return new Edge(current, next);
        current = next;
    }
}

IEnumerable<Edge> ToNormalizedGraph(Instruction[] instructions)
{
    var graph = ToGraph(instructions).ToArray();
    int minX = graph.Min(e => e.First.X);
    int minY = graph.Min(e => e.First.Y);
    return graph.Select(e => new Edge(new Position(e.First.X - minX, e.First.Y - minY), new Position(e.Last.X - minX, e.Last.Y - minY)));
}

int Area(Edge[] edges)
{
    return 0;
}

Instruction[] instructions = puzzleInput.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).Select(l => Instruction.FromLine(l)).ToArray();
Instruction[] realInstructions = puzzleInput.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).Select(l => Instruction.FromLineReal(l)).ToArray();
Console.WriteLine(Area(ToNormalizedGraph(instructions).ToArray()));
Console.WriteLine(Area(ToNormalizedGraph(realInstructions).ToArray()));
