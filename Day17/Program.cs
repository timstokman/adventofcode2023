using Common;
using Point = (int X, int Y, Direction? LastDirection);

int ShortestPath(int[][] nodeCosts, int maxStraightLine = 3)
{
    int[,,] dist = new int[nodeCosts.Length, nodeCosts[0].Length, 4];
    Dictionary<Point, Point> prev = new();
    Dictionary<Point, int> queue = new();

    for (int y = 0; y < nodeCosts.Length; y++)
    {
        for (int x = 0; x < nodeCosts[0].Length; x++)
        {
            for (int d = 0; d < maxStraightLine + 1; d++)
            {
                if (!(x == 0 && y == 0))
                {
                    dist[y, x, d] = int.MaxValue;
                    queue[new Point(x, y, (Direction)d)] = dist[y, x, d];
                }
            }
        }
    }

    queue[new Point(0, 0, null)] = 0;

    while (queue.Count > 0)
    {
        IEnumerable<(Point Neighbour, int Cost)> Neighbours(Point point)
        {
            var surrounding = new (int X, int Y, Direction Direction)[]
            {
                (-1, 0, Direction.Left),
                (1, 0, Direction.Right),
                (0, -1, Direction.Top),
                (0, 1, Direction.Bottom),
            };
            Direction? inverse = point.LastDirection switch
            {
                Direction.Top => Direction.Bottom,
                Direction.Right => Direction.Left,
                Direction.Bottom => Direction.Top,
                Direction.Left => Direction.Right,
                null => null,
                _ => throw new ArgumentOutOfRangeException()
            };
            foreach (var s in surrounding)
            {
                if (s.Direction != point.LastDirection && s.Direction != inverse)
                {
                    int cost = 0;
                    for (int i = 1; i <= maxStraightLine; i++)
                    {
                        Point newPoint = new(point.X + s.X * i, point.Y + s.Y * i, s.Direction);

                        if (newPoint.X >= 0 && newPoint.X < nodeCosts[0].Length && newPoint.Y >= 0 && newPoint.Y < nodeCosts.Length)
                        {
                            cost += nodeCosts[newPoint.Y][newPoint.X];
                            if (queue.ContainsKey(newPoint))
                            {
                                yield return (newPoint, cost);
                            }
                        }
                    }
                }
            }
        }

        IEnumerable<Point> Path(Point last)
        {
            Point c = last;
            while (prev.ContainsKey(c))
            {
                yield return c;
                c = prev[c];
            }
        }

        Point u = queue.MinBy(kv => kv.Value).Key;
        queue.Remove(u);

        if (u.Y == nodeCosts.Length - 1 && u.X == nodeCosts[0].Length - 1)
        {
            var path = Path(u).Reverse().ToArray();
            int cost = dist[u.Y, u.X, (int)(u.LastDirection ?? Direction.Top)];

            for (int y = 0; y < nodeCosts.Length; y++)
            {
                for (int x = 0; x < nodeCosts[0].Length; x++)
                {
                    if (path.Any(p => p.X == x && p.Y == y))
                    {
                        Console.Write("#");
                    }
                    else
                    {
                        Console.Write(nodeCosts[y][x]);
                    }
                }

                Console.WriteLine();
            }
            
            return cost;
        }

        var neighbours = Neighbours(u);
        foreach ((Point v, int cost) in neighbours)
        {
            int alt = dist[u.Y, u.X, (int)(u.LastDirection ?? Direction.Top)] + cost;
            if (alt < dist[v.Y, v.X, (int)(v.LastDirection!)])
            {
                dist[v.Y, v.X, (int)(v.LastDirection!)] = alt;
                prev[v] = u;
                queue[v] = alt;
            }
        }
    }

    return -1;
}

string puzzleInput = await Util.GetPuzzleInput(17);

int[][] nodeCosts = puzzleInput.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).Select(l => l.ToCharArray().Select(c => int.Parse(c.ToString())).ToArray()).ToArray();
Console.WriteLine(ShortestPath(nodeCosts));