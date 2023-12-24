using Common;
using Position = (int X, int Y);

string puzzleInput = await Util.GetPuzzleInput(23);

char[] slopes = new[] { '^', 'v', '>', '<' };

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

(int Steps, List<Position>? Visited) LongestPathDfs(Dictionary<Position, Dictionary<Position, int>> edges, List<Position> visited, Position end)
{
    var current = visited.Last();
    if (current == end)
    {
        return (0, visited);
    }

    IEnumerable<KeyValuePair<Position, int>> possibleEdges = edges[current].Where(e => !visited.Contains(e.Key));
    return possibleEdges.Select(e =>
    {
        var longest = LongestPathDfs(edges, visited.Concat(new[] { e.Key }).ToList(), end);
        return (Steps: longest.Steps + e.Value, visited: longest.Visited);
    }).Concat(new[] { (Steps: int.MinValue, Visited: (List<Position>?)null) }).MaxBy(v => v.Steps);
}

(int Steps, Position? FoundNode) FollowPath(Position previous, Position next, List<Position> nodes, char[][] map, int height, int width)
{
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

    Dictionary<Position, Dictionary<Position, int>> edges = new();
    foreach (Position node in nodes)
    {
        var possibleDirections = SurroundingWithBounds(node, height, width).Where(s => map[s.Y][s.X] != '#');
        foreach (var d in possibleDirections)
        {
            (int steps, Position? foundNode) = FollowPath(node, d, nodes, map, height, width);
            if (foundNode != null)
            {
                if (edges.TryGetValue(node, out Dictionary<Position, int> edge))
                {
                    edge[foundNode.Value] = steps;
                }
                else
                {
                    edges[node] = new Dictionary<Position, int>() { { foundNode.Value, steps } };
                }
            }
        }
    }

    var longest = LongestPathDfs(edges, new List<Position>() { nodes[0] }, nodes[1]);

    for (int y = 0; y < height; y++)
    {
        for (int x = 0; x < width; x++)
        {
            int visitNum = longest.Visited.IndexOf(new Position(x, y));
            if (visitNum >= 0)
            {
                Console.Write(visitNum.ToString());
            }
            else
            {
                Console.Write(map[y][x]);
            }
        }

        Console.WriteLine();
    }

    return longest.Steps;
}

char[][] map = puzzleInput.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).Select(l => l.ToCharArray()).ToArray();
char[][] mapWithoutSlopes = puzzleInput.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).Select(l => l.Replace('^', '.').Replace('v', '.').Replace('>', '.').Replace('<', '.').ToCharArray()).ToArray();
Console.WriteLine(LongestPath(map));
Console.WriteLine(LongestPath(mapWithoutSlopes));
