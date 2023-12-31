﻿using Common;
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
    Position current = new Position(0, 0);
    foreach (Instruction instruction in instructions)
    {
        Position next = MoveInDirection(current, instruction.Direction, instruction.Amount);
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

IEnumerable<Position> GetNodesIncludingEdges(Instruction[] instructions)
{
    List<Edge> graph = ToGraph(instructions).ToList();
    int maxX = (int)graph.Max(e => e.First.X);
    int startEdge = graph.IndexOf(graph.First(e => e.First.X == maxX && e.First.Y != e.Last.Y));
    Direction outsideGraph = Direction.Right;

    for (int i = 0; i < graph.Count; i++)
    {
        Edge edge = graph[(i + startEdge) % graph.Count];
        Edge nextEdge = graph[(i + startEdge + 1) % graph.Count];
        bool clockwise = IsClockwiseTurn(edge.Direction, nextEdge.Direction);
        Direction nextOutsideGraph = Turn(outsideGraph, clockwise);
        yield return MoveInDirection(MoveInDirection(edge.Last, outsideGraph, 0.5), nextOutsideGraph, 0.5);
        outsideGraph = nextOutsideGraph;
    }
}

long Area(Instruction[] instructions)
{
    Position[] points = GetNodesIncludingEdges(instructions).ToArray();
    return (long)Math.Round(Enumerable.Range(0, points.Length).Sum(i =>
    {
        Position current = points[i];
        Position next = points[(i + 1) % points.Length];
        return current.X * next.Y - next.X * current.Y;
    }) / 2.0);
}

Instruction[] instructions = puzzleInput.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).Select(l => Instruction.GetInstructionFromLine(l)).ToArray();
Instruction[] realInstructions = puzzleInput.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).Select(l => Instruction.GetRealInstructionFromLine(l)).ToArray();
Console.WriteLine($"Area given instructions: {Area(instructions)}");
Console.WriteLine($"Area given real instructions: {Area(realInstructions)}");
