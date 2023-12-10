using Common;
using Position = (int X, int Y);

void PrintLoop(char[][] map, Position[] loop)
{
    HashSet<Position> loopSet = new(loop);
    Console.WriteLine("Loop:");
    for (int y = 0; y < map.Length; y++)
    {
        for (int x = 0; x < map[0].Length; x++)
        {
            if (loopSet.Contains((x, y)))
            {
                Console.Write("L");
            }
            else
            {
                Console.Write(".");
            }
        }

        Console.WriteLine();
    }
}

void PrintInside(char[][] map, HashSet<Position> loop, HashSet<Position> inside)
{
    Console.WriteLine("Fill:");
    for (int y = 0; y < map.Length; y++)
    {
        for (int x = 0; x < map[0].Length; x++)
        {
            if (loop.Contains((x, y)))
            {
                Console.Write("L");
            }
            else if (inside.Contains((x, y)))
            {
                Console.Write("I");
            }
            else
            {
                Console.Write("O");
            }
        }

        Console.WriteLine();
    }
}

Position[] sides = { (0, -1), (0, 1), (-1, 0), (1, 0) };

Dictionary<char, Position[]> connectors = new()
{
    { '|', new []{ (0, -1), (0, 1) }},
    { '-', new []{ (-1, 0), (1, 0) }},
    { 'L', new []{ (0, -1), (1, 0) }},
    { 'J', new []{ (0, -1), (-1, 0) }},
    { 'F', new []{ (0, 1), (1, 0) }},
    { '7', new []{ (0, 1), (-1, 0) }},
    { 'S', sides },
};

Direction TurnDirection(bool turningClockwise, Direction direction)
{
    return (Direction)Mod((int)(turningClockwise ? direction + 1 : direction - 1), 4);
}

int Mod(int x, int m) {
    int r = x % m;
    return r < 0 ? r + m : r;
}

bool IsTurningClockwise(Position item, Position previous, Position next)
{
    return (item.X - previous.X == 1 && item.Y - previous.Y == 0 && next.X - item.X == 0 && next.Y - item.Y == 1) ||
           (item.X - previous.X == 0 && item.Y - previous.Y == 1 && next.X - item.X == -1 && next.Y - item.Y == 0) ||
           (item.X - previous.X == -1 && item.Y - previous.Y == 0 && next.X - item.X == 0 && next.Y - item.Y == -1) ||
           (item.X - previous.X == 0 && item.Y - previous.Y == -1 && next.X - item.X == 1 && next.Y - item.Y == 0);
}

(int, int) MoveDirection(Direction direction, Position node)
{
    return direction switch
    {
        Direction.Left => (node.X - 1, node.Y),
        Direction.Top => (node.X, node.Y - 1),
        Direction.Right => (node.X + 1, node.Y),
        Direction.Bottom => (node.X, node.Y + 1),
        _ => throw new ArgumentOutOfRangeException()
    };
}

Position[] GetConnectingPipes(char[][] map, Position position)
{
    if (connectors.TryGetValue(map[position.Y][position.X], out Position[]? connections))
    {
        return connections.Where(connection =>
        {
            (int netX, int netY) = (position.X + connection.X, position.Y + connection.Y);
            if (netX >= map[0].Length || netX < 0 || netY >= map.Length || netY < 0)
            {
                return false;
            }

            return connectors.ContainsKey(map[netY][netX]) && connectors[map[netY][netX]].Any(backConnection => connection.X == -1 * backConnection.X && connection.Y == -1 * backConnection.Y);
        }).Select(connection =>
        {
            return (X: position.X + connection.X, Y: position.Y + connection.Y);
        }).ToArray();
    }
    else
    {
        return Array.Empty<Position>();
    }
}

IEnumerable<Position> WalkPath(char[][] map, Position start, Position next)
{
    yield return start;
    Position previous = start;
    while (true)
    {
        yield return next;
        Position[] connecting = GetConnectingPipes(map, next).Where(c => c != previous).ToArray();
        if (connecting.Length != 1)
        {
            break;
        }
        
        previous = next;
        next = connecting.First();

        if (next == start)
        {
            yield return start;
            break;
        }
    }
}

Position StartNode(char[][] map)
{
    for (int startY = 0; startY < map.Length; startY++)
    {
        int startX = Array.IndexOf(map[startY], 'S');
        if (startX >= 0)
        {
            return (startX, startY);
        }
    }

    return (-1, -1);
}

Position[] FindLargestConnectingLoop(char[][] map)
{
    Position start = StartNode(map);
    IEnumerable<Position[]> paths = GetConnectingPipes(map, (start.X, start.Y)).Select(connection => WalkPath(map, (start.X, start.Y), (connection.X, connection.Y)).ToArray());
    Position[][] loops = paths.Where(loop => loop.Last() == (start.X, start.Y)).ToArray();
    int maxLength = loops.Max(l => l.Length);
    return loops.First(l => l.Length == maxLength).Take(maxLength - 1).ToArray();
}

Position StartNodeForInsideTrace(char[][] map, HashSet<Position> loopSet)
{
    for (int startX = 0; startX < map[0].Length; startX++)
    {
        for (int startY = 0; startY < map.Length; startY++)
        {
            if (loopSet.Contains((startX, startY)))
            {
                return (startX, startY);
            }
        }
    }

    return (-1, -1);
}

HashSet<Position> InsideNodes(char[][] map, Position[] loop)
{
    HashSet<Position> loopSet = new(loop);
    HashSet<Position> insideSet = new();

    Position startNode = StartNodeForInsideTrace(map, loopSet);
    int loopStartIndex = Array.IndexOf(loop, startNode);
    Direction insideLoop = Direction.Right;
    for (int i = 0; i < loop.Length; i++)
    {
        Position item = loop[Mod(loopStartIndex + i, loop.Length)];
        Position previous = loop[Mod(loopStartIndex + i - 1, loop.Length)];
        Position next = loop[Mod(loopStartIndex + i + 1, loop.Length)];
        bool turning = Math.Abs(previous.X - next.X) != 2 && Math.Abs(previous.Y - next.Y) != 2;
        bool turningClockwise = turning && IsTurningClockwise(item, previous, next);
        Direction newDirectionInside = turning ? TurnDirection(turningClockwise, insideLoop) : insideLoop;
        
        // "Flood" inside section
        Queue<Position> toSearch = new(new[] { MoveDirection(insideLoop, item), MoveDirection(newDirectionInside, item) });

        while (toSearch.Any())
        {
            Position searchItem = toSearch.Dequeue();
            if (searchItem.X >= 0 && searchItem.Y >= 0 && searchItem.X < map[0].Length && searchItem.Y < map.Length && !loopSet.Contains(searchItem) && !insideSet.Contains(searchItem))
            {
                insideSet.Add(searchItem);
                foreach (Position side in sides)
                {
                    toSearch.Enqueue((searchItem.X + side.X, searchItem.Y + side.Y));
                }
            }
        }

        insideLoop = newDirectionInside;
    }

    return insideSet;
}

string puzzleInput = await Util.GetPuzzleInput(10);

char[][] map = puzzleInput.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).Select(l => l.ToCharArray()).ToArray();
Position[] largestLoop = FindLargestConnectingLoop(map);
HashSet<Position> insideLoop = InsideNodes(map, largestLoop);
Console.WriteLine($"Maximum distance beast: {largestLoop.Length / 2}");
Console.WriteLine($"Area inside loop: {insideLoop.Count}");
PrintLoop(map, largestLoop);
PrintInside(map, new HashSet<Position> (largestLoop), insideLoop);