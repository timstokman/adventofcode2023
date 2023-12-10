using System.Collections;
using Common;

Dictionary<char, (int X, int Y)[]> connectors = new()
{
    { '|', new []{ (0, -1), (0, 1) }},
    { '-', new []{ (-1, 0), (1, 0) }},
    { 'L', new []{ (0, -1), (1, 0) }},
    { 'J', new []{ (0, -1), (-1, 0) }},
    { 'F', new []{ (0, 1), (1, 0) }},
    { '7', new []{ (0, 1), (-1, 0) }},
    { 'S', new[] { (0, -1), (0, 1), (-1, 0), (1, 0) }},
};

void PrintLoop(char[][] map, (int X, int Y)[] loop)
{
    HashSet<(int X, int Y)> loopSet = new(loop);
    for (int y = 0; y < map.Length; y++)
    {
        for (int x = 0; x < map[0].Length; x++)
        {
            if (loopSet.Contains((x, y)))
            {
                Console.Write("X");
            }
            else
            {
                Console.Write(".");
            }
        }

        Console.WriteLine();
    }
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
        return new (int X, int Y)[] { };
    }
}

IEnumerable<(int X, int Y)> GetLoop(char[][] map, (int X, int Y) start, (int X, int Y) next)
{
    yield return start;
    var previous = start;
    while (true)
    {
        yield return next;
        var connecting = GetConnectingPipes(map, next).Where(c => c != previous).ToArray();
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

(int X, int Y)[] FindLargestConnectingLoop(char[][] map)
{
    int startX = 0;
    int startY = 0;
    for (startY = 0; startY < map.Length; startY++)
    {
        startX = Array.IndexOf(map[startY], 'S');
        if (startX >= 0)
        {
            break;
        }
    }

    (int X, int Y)[][] loops = GetConnectingPipes(map, (startX, startY)).Select(connection => GetLoop(map, (startX, startY), (connection.X, connection.Y)).ToArray()).ToArray();
    loops = loops.Where(loop => loop.Last() == (startX, startY)).ToArray();
    int maxLength = loops.Max(l => l.Count());
    return loops.First(l => l.Count() == maxLength).Take(maxLength - 1).ToArray();
}

int Mod(int x, int m) {
    int r = x%m;
    return r<0 ? r+m : r;
}

int CountInsideLoop(char[][] map, (int X, int Y)[] loop)
{
    HashSet<(int X, int Y)> loopSet = new(loop);
    HashSet<(int X, int Y)> insideSet = new();

    // Dumb way to find start point
    int startY = 0;
    int startX;
    for (startX = 0; startX < map[0].Length; startX++)
    {
        for (startY = 0; startY < map.Length; startY++)
        {
            if (map[startY][startX] == '|' && loopSet.Contains((startX, startY)))
            {
                goto GETOUT;
            }
        }
    }
    
    GETOUT:

    (int X, int Y)[] Sides = { (-1, 0), (1, 0), (0, 1), (0, -1) };

    int loopStartIndex = Array.IndexOf(loop, (startX, startY));
    Direction insideLoop = Direction.Right;
    for (int i = 0; i < loop.Length; i++)
    {
        var item = loop[Mod(loopStartIndex + i, loop.Length)];
        var previous = loop[Mod(loopStartIndex + i - 1, loop.Length)];
        var next = loop[Mod(loopStartIndex + i + 1, loop.Length)];
        bool turning = Math.Abs(previous.X - next.X) != 2 && Math.Abs(previous.Y - next.Y) != 2;
        bool turningClockwise =
            turning &&
            (
                (item.X - previous.X == 1 && item.Y - previous.Y == 0 && next.X - item.X == 0 && next.Y - item.Y == 1) ||
                (item.X - previous.X == 0 && item.Y - previous.Y == 1 && next.X - item.X == -1 && next.Y - item.Y == 0) ||
                (item.X - previous.X == -1 && item.Y - previous.Y == 0 && next.X - item.X == 0 && next.Y - item.Y == -1) ||
                (item.X - previous.X == 0 && item.Y - previous.Y == -1 && next.X - item.X == 1 && next.Y - item.Y == 0)
            );
        bool turningCounterclockwise = turning && !turningClockwise;
        var newDirection = turning ? (Direction)Mod((int)(turningClockwise ? insideLoop + 1 : insideLoop - 1), 4) : insideLoop;
        
        // "Flood" inside section
        var startFlood = insideLoop switch
        {
            Direction.Left => (item.X - 1, item.Y),
            Direction.Top => (item.X, item.Y - 1),
            Direction.Right => (item.X + 1, item.Y),
            Direction.Bottom => (item.X, item.Y + 1),
            _ => throw new ArgumentOutOfRangeException()
        };
        var otherStartFlood = newDirection switch
        {
            Direction.Left => (item.X - 1, item.Y),
            Direction.Top => (item.X, item.Y - 1),
            Direction.Right => (item.X + 1, item.Y),
            Direction.Bottom => (item.X, item.Y + 1),
            _ => throw new ArgumentOutOfRangeException()
        };

        var toSearch = new Queue<(int X, int Y)>(new[] { startFlood, otherStartFlood });

        while (toSearch.Any())
        {
            var searchItem = toSearch.Dequeue();
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

    for (int y = 0; y < map.Length; y++)
    {
        for (int x = 0; x < map[0].Length; x++)
        {
            if (loopSet.Contains((x, y)))
            {
                Console.Write("L");
            }
            else if (insideSet.Contains((x, y)))
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

    return insideSet.Count;
}

string puzzleInput = await Util.GetPuzzleInput(10);

char[][] map = puzzleInput.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).Select(l => l.ToCharArray()).ToArray();
var largestLoop = FindLargestConnectingLoop(map);
int insideLoop = CountInsideLoop(map, largestLoop);
Console.WriteLine(largestLoop.Length / 2);
Console.WriteLine(insideLoop);