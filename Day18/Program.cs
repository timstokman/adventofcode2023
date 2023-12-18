using Common;
using Day18;

string puzzleInput = await Util.GetPuzzleInput(18);

Position MoveInDirection(Position position, Direction direction, double amount)
{
    return direction switch
    {
        Direction.Top => position with { Y = position.Y - amount },
        Direction.Right => position with { X = position.X + amount },
        Direction.Bottom => position with { Y = position.Y + amount },
        Direction.Left => position with { X = position.X - amount },
        _ => throw new ArgumentOutOfRangeException()
    };
}

IEnumerable<Edge> ToGraph(Instruction[] instructions)
{
    var current = new Position(0, 0);
    foreach (var instruction in instructions)
    {
        var next = MoveInDirection(current, instruction.Direction, instruction.Amount);
        yield return new Edge(current, next, instruction.Direction);
        current = next;
    }
}

bool IsClockwiseTurn(Direction start, Direction end)
{
    return (start, end) switch
    {
        (Direction.Bottom, Direction.Left) => true,
        (Direction.Left, Direction.Top) => true,
        (Direction.Top, Direction.Right) => true,
        (Direction.Right, Direction.Bottom) => true,
        (Direction.Bottom, Direction.Right) => false,
        (Direction.Left, Direction.Bottom) => false,
        (Direction.Top, Direction.Left) => false,
        (Direction.Right, Direction.Top) => false,
        _ => throw new ArgumentOutOfRangeException(),
    };
}

Direction Inverse(Direction direction)
{
    return direction switch
    {
        Direction.Bottom => Direction.Top,
        Direction.Left => Direction.Right,
        Direction.Top => Direction.Bottom,
        Direction.Right => Direction.Left,
        _ => throw new ArgumentOutOfRangeException(),
    };
}

Position SnapToGrid(Position position)
    => new Position(Math.Round(position.X), Math.Round(position.Y));

Direction Turn(Direction direction, bool clockwise)
{
    return (direction, clockwise) switch
    {
        (Direction.Bottom, true) => Direction.Left,
        (Direction.Left, true) => Direction.Top,
        (Direction.Top, true) => Direction.Right,
        (Direction.Right, true) => Direction.Bottom,
        (Direction.Bottom, false) => Direction.Right,
        (Direction.Left, false) => Direction.Bottom,
        (Direction.Top, false) => Direction.Left,
        (Direction.Right, false) => Direction.Top,
        _ => throw new ArgumentOutOfRangeException(),
    };
}

IEnumerable<Position> GetEdges(Instruction[] instructions)
{
    var graph = ToGraph(instructions).ToList();
    int maxX = (int)graph.Max(e => e.First.X);
    int startEdge = graph.IndexOf(graph.First(e => e.First.X == maxX && e.First.Y != e.Last.Y));
    Direction outsideGraph = Direction.Right;

    for (int i = 0; i < graph.Count; i++)
    {
        var edge = graph[(i + startEdge) % graph.Count];
        var nextEdge = graph[(i + startEdge + 1) % graph.Count];
        bool clockwise = IsClockwiseTurn(edge.Direction, nextEdge.Direction);
        Direction nextOutsideGraph = Turn(outsideGraph, clockwise);
        yield return SnapToGrid(MoveInDirection(MoveInDirection(MoveInDirection(MoveInDirection(edge.Last, outsideGraph, 0.5), nextOutsideGraph, 0.5), Direction.Top, 0.5), Direction.Right, 0.5));
        outsideGraph = nextOutsideGraph;
    }
}

long Area(Position[] points)
{
    return Enumerable.Range(0, points.Length).Sum(i =>
    {
        var current = points[i];
        var next = points[(i + 1) % points.Length];
        return (long)Math.Round((current.Y + next.Y) * (current.X - next.X));
    }) / 2;
    /*
    int[] changePointsX = edges.SelectMany(e => new[] { e.First.X - 1, e.First.X, e.First.X + 1 }).Distinct().OrderBy(x => x).ToArray();

    long area = 0;
    int? previousChangePointX = null;
    long areaForInflectionPoint = 0;
    foreach (int changePointX in changePointsX)
    {
        if (previousChangePointX != null && changePointX - previousChangePointX.Value > 1)
        {
            area += (changePointX - previousChangePointX.Value - 1) * areaForInflectionPoint;
        }

        List<Edge> matchingEdges = edges.Where(e => e.First.X == changePointX && e.Last.X == changePointX).OrderByDescending(e => Math.Min(e.First.Y, e.Last.Y)).ToList();
        List<Edge> crossingEdges = edges.Where(e => (e.First.X < changePointX && e.Last.X > changePointX) || (e.Last.X < changePointX && e.First.X > changePointX)).OrderByDescending(e => e.First.Y).ToList();

        areaForInflectionPoint = 0;
        bool inPolygon = false;
        while (matchingEdges.Count > 0 && crossingEdges.Count > 0)
        {
            
        }

        area += areaForInflectionPoint;

        previousChangePointX = changePointX;
    }

    return area;
    */
}

Instruction[] instructions = puzzleInput.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).Select(l => Instruction.FromLine(l)).ToArray();
Instruction[] realInstructions = puzzleInput.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).Select(l => Instruction.FromLineReal(l)).ToArray();
Console.WriteLine(Area(GetEdges(instructions).ToArray()));
Console.WriteLine(Area(GetEdges(realInstructions).ToArray()));
