using Common;

void PrintLoop(char[][] map, (int X, int Y)[] loop)
{
    HashSet<(int X, int Y)> loopSet = new(loop);
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

void PrintInside(char[][] map, HashSet<(int X, int Y)> loop, HashSet<(int X, int Y)> inside)
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

(int X, int Y)[] Sides = { (0, -1), (0, 1), (-1, 0), (1, 0) };

Dictionary<char, (int X, int Y)[]> connectors = new()
{
    { '|', new []{ (0, -1), (0, 1) }},
    { '-', new []{ (-1, 0), (1, 0) }},
    { 'L', new []{ (0, -1), (1, 0) }},
    { 'J', new []{ (0, -1), (-1, 0) }},
    { 'F', new []{ (0, 1), (1, 0) }},
    { '7', new []{ (0, 1), (-1, 0) }},
    { 'S', Sides },
};

bool IsTurningClockwise((int X, int Y) item, (int X, int Y) previous, (int X, int Y) next)
{
    return (item.X - previous.X == 1 && item.Y - previous.Y == 0 && next.X - item.X == 0 && next.Y - item.Y == 1) ||
           (item.X - previous.X == 0 && item.Y - previous.Y == 1 && next.X - item.X == -1 && next.Y - item.Y == 0) ||
           (item.X - previous.X == -1 && item.Y - previous.Y == 0 && next.X - item.X == 0 && next.Y - item.Y == -1) ||
           (item.X - previous.X == 0 && item.Y - previous.Y == -1 && next.X - item.X == 1 && next.Y - item.Y == 0);
}

(int, int) MoveDirection(Direction direction, (int X, int Y) node)
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

(int X, int Y)[] GetConnectingPipes(char[][] map, (int x, int y) position)
{
    if (connectors.TryGetValue(map[position.y][position.x], out var connections))
    {
        return connections.Where(connection =>
        {
            (int netX, int netY) = (position.x + connection.X, position.y + connection.Y);
            if (netX >= map[0].Length || netX < 0 || netY >= map.Length || netY < 0)
            {
                return false;
            }

            return connectors.ContainsKey(map[netY][netX]) && connectors[map[netY][netX]].Any(backConnection => connection.X == -1 * backConnection.X && connection.Y == -1 * backConnection.Y);
        }).Select(connection =>
        {
            return (X: position.x + connection.X, Y: position.y + connection.Y);
        }).ToArray();
    }
    else
    {
        return Array.Empty<(int X, int Y)>();
    }
}

IEnumerable<(int X, int Y)> GetLoop(char[][] map, (int X, int Y) start, (int X, int Y) next)
{
    yield return start;
    (int X, int Y) previous = start;
    while (true)
    {
        yield return next;
        (int X, int Y)[] connecting = GetConnectingPipes(map, next).Where(c => c != previous).ToArray();
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

(int X, int Y) StartNode(char[][] map)
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

(int X, int Y)[] FindLargestConnectingLoop(char[][] map)
{
    var start = StartNode(map);
    (int X, int Y)[][] loops = GetConnectingPipes(map, (start.X, start.Y)).Select(connection => GetLoop(map, (start.X, start.Y), (connection.X, connection.Y)).ToArray()).Where(loop => loop.Last() == (start.X, start.Y)).ToArray();
    int maxLength = loops.Max(l => l.Length);
    return loops.First(l => l.Length == maxLength).Take(maxLength - 1).ToArray();
}

int Mod(int x, int m) {
    int r = x % m;
    return r < 0 ? r + m : r;
}

(int X, int Y) StartNodeForInsideTrace(char[][] map, HashSet<(int X, int Y)> loopSet)
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

HashSet<(int X, int Y)> InsideNodes(char[][] map, (int X, int Y)[] loop)
{
    HashSet<(int X, int Y)> loopSet = new(loop);
    HashSet<(int X, int Y)> insideSet = new();

    (int X, int Y) startNode = StartNodeForInsideTrace(map, loopSet);

    int loopStartIndex = Array.IndexOf(loop, startNode);
    Direction insideLoop = Direction.Right;
    for (int i = 0; i < loop.Length; i++)
    {
        (int X, int Y) item = loop[Mod(loopStartIndex + i, loop.Length)];
        (int X, int Y) previous = loop[Mod(loopStartIndex + i - 1, loop.Length)];
        (int X, int Y) next = loop[Mod(loopStartIndex + i + 1, loop.Length)];
        bool turning = Math.Abs(previous.X - next.X) != 2 && Math.Abs(previous.Y - next.Y) != 2;
        bool turningClockwise = turning && IsTurningClockwise(item, previous, next);
        Direction newDirection = turning ? (Direction)Mod((int)(turningClockwise ? insideLoop + 1 : insideLoop - 1), 4) : insideLoop;
        
        // "Flood" inside section
        var toSearch = new Queue<(int X, int Y)>(new[] { MoveDirection(insideLoop, item), MoveDirection(newDirection, item) });

        while (toSearch.Any())
        {
            (int X, int Y) searchItem = toSearch.Dequeue();
            if (searchItem.X >= 0 && searchItem.Y >= 0 && searchItem.X < map[0].Length && searchItem.Y < map.Length && !loopSet.Contains(searchItem) && !insideSet.Contains(searchItem))
            {
                insideSet.Add(searchItem);
                foreach (var side in Sides)
                {
                    toSearch.Enqueue((searchItem.X + side.X, searchItem.Y + side.Y));
                }
            }
        }

        insideLoop = newDirection;
    }

    return insideSet;
}

string puzzleInput = await Util.GetPuzzleInput(10);

char[][] map = puzzleInput.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).Select(l => l.ToCharArray()).ToArray();
(int X, int Y)[] largestLoop = FindLargestConnectingLoop(map);
HashSet<(int X, int Y)> insideLoop = InsideNodes(map, largestLoop);
Console.WriteLine($"Maximum distance beast: {largestLoop.Length / 2}");
Console.WriteLine($"Area inside loop: {insideLoop.Count}");
PrintLoop(map, largestLoop);
PrintInside(map, new HashSet<(int X, int Y)> (largestLoop), insideLoop);