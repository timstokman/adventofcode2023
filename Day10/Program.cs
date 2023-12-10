using Common;
using Microsoft.VisualBasic.CompilerServices;

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

int CountInsideLoop(char[][] map, (int X, int Y)[] loop)
{
    HashSet<(int X, int Y)> loopSet = new(loop);
    Queue<(int X, int Y)> toSearch = new();

    (int X, int Y)[] Sides = { (-1, 0), (1, 0), (0, 1), (0, -1) };

    for (int x = 0; x < map[0].Length; x++)
    {
        foreach (int y in new[] { 0, map.Length - 1 })
        {
            if (!loopSet.Contains((x, y)))
            {
                toSearch.Enqueue((x, y));
            }
        }
    }

    for (int y = 0; y < map.Length; y++)
    {
        foreach (int x in new[] { 0, map[0].Length - 1 })
        {
            if (!loopSet.Contains((x, y)))
            {
                toSearch.Enqueue((x, y));
            }
        }
    }

    var outsideLoop = new HashSet<(int X, int Y)>();
    while (toSearch.Any())
    {
        var toCheck = toSearch.Dequeue();
        if (!loopSet.Contains(toCheck))
        {
            outsideLoop.Add(toCheck);
            foreach (var side in Sides)
            {
                var neighbour = (X: toCheck.X + side.X, Y: toCheck.Y + side.Y);
                if (neighbour.X >= 0 && neighbour.X < map[0].Length && neighbour.Y >= 0 && neighbour.Y < map.Length && !outsideLoop.Contains(neighbour))
                {
                    toSearch.Enqueue(neighbour);
                }
            }
        }
    }

    for (int y = 0; y < map.Length; y++)
    {
        for (int x = 0; x < map[0].Length; x++)
        {
            if (loopSet.Contains((x, y)))
            {
                Console.Write("X");
            }
            else if (outsideLoop.Contains((x, y)))
            {
                Console.Write("O");
            }
            else
            {
                Console.Write("I");
            }
        }

        Console.WriteLine();
    }

    return (map.Length * map[0].Length) - loop.Length - outsideLoop.Count;
}

string puzzleInput = await Util.GetPuzzleInput(10);

char[][] map = puzzleInput.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).Select(l => l.ToCharArray()).ToArray();
var largestLoop = FindLargestConnectingLoop(map);
int insideLoop = CountInsideLoop(map, largestLoop);
Console.WriteLine(largestLoop.Length / 2);
Console.WriteLine(insideLoop);