using Common;
using Position = (int X, int Y);

string puzzleInput = await Util.GetPuzzleInput(23);

IEnumerable<Position> Surrounding(Position position)
{
    yield return position with { X = position.X + 1 };
    yield return position with { X = position.X - 1 };
    yield return position with { Y = position.Y + 1 };
    yield return position with { Y = position.Y - 1 };
}

IEnumerable<Position> SurroundingWithBounds(Position position, int height, int width)
{
    return Surrounding(position).Where(s => s.X >= 0 && s.X < width && s.Y >= 0 && s.Y < height);
}

Position InDirection(Position position, char slope)
{
    return slope switch
    {
        '^' => position with { Y = position.Y - 1 },
        'v' => position with { Y = position.Y + 1 },
        '>' => position with { X = position.X + 1 },
        '<' => position with { X = position.X - 1 },
        _ => throw new ArgumentOutOfRangeException()
    };
}

int LongestPathDfs(Dictionary<(Position Start, Position End), int> edges, List<Position> visited, Position end)
{
    var current = visited.Last();
    if (current == end)
    {
        return 0;
    }

    var possibleEdges = edges.Where(e => e.Key.Start == current && !visited.Contains(e.Key.End));
    return possibleEdges.Max(e => e.Value + LongestPathDfs(edges, visited.Concat(new[] { e.Key.End }).ToList(), end));
}

(int Steps, Position? FoundNode) FollowPath(Position previous, Position next, List<Position> nodes, char[][] map, int height, int width)
{
    char[] slopes = new[] { '^', 'v', '>', '<' };
    int steps = 1;
    while (!nodes.Contains(next))
    {
        var surrounding = SurroundingWithBounds(next, height, width).Where(s => map[s.Y][s.X] != '#' && s != previous).ToArray();
        bool isSlope = slopes.Contains(map[next.Y][next.X]);
        if (isSlope)
        {
            var inDirection = InDirection(next, map[next.Y][next.X]);
            if (surrounding.Length == 1 && surrounding[0] == inDirection)
            {
                previous = next;
                next = inDirection;
                steps++;
            }
            else
            {
                return (-1, null);
            }
        }
        else
        {
            if (surrounding.Length == 1)
            {
                previous = next;
                next = surrounding[0];
                steps++;
            }
            else
            {
                throw new ArgumentOutOfRangeException();
            }
        }
    }

    return (steps, next);
}

int LongestPath(char[][] map)
{
    int height = map.Length;
    int width = map[0].Length;
    List<Position> nodes = new() { new(1, 0), new(width - 2, height - 1) };
    for (int y = 0; y < height; y++)
    {
        for (int x = 0; x < width; x++)
        {
            if (map[y][x] != '#' && SurroundingWithBounds(new Position(x, y), height, width).Count(s => map[s.Y][s.X] != '#') > 2)
            {
                nodes.Add(new(x, y));
            }
        }
    }

    Dictionary<(Position From, Position To), int> edges = new();
    foreach (Position node in nodes)
    {
        var possibleDirections = SurroundingWithBounds(node, height, width).Where(s => map[s.Y][s.X] != '#');
        foreach (var d in possibleDirections)
        {
            (int steps, Position? foundNode) = FollowPath(node, d, nodes, map, height, width);
            if (foundNode != null)
            {
                edges[(node, foundNode.Value)] = steps;
            }
        }
    }

    return LongestPathDfs(edges, new List<Position>() { nodes[0] }, nodes[1]);
}

char[][] map = puzzleInput.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).Select(l => l.ToCharArray()).ToArray();
Console.WriteLine(LongestPath(map));