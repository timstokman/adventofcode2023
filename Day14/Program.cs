using Common;

string puzzleInput = await Util.GetPuzzleInput(14);

char[][] TiltNorth(char[][] map)
{
    var newMap = map.Select(row => row.Select(c => c is '.' or '#' ? c : '.').ToArray()).ToArray();
    for (int column = 0; column < map[0].Length; column++)
    {
        int[] solidRockIndexes = new[] { -1}.Concat(map.Select((rock, row) => (rock, row)).Where(r => r.rock[column] == '#').Select(r => r.row)).Concat(new[] { map.Length }).ToArray();
        for (int solidRockIndex = 1; solidRockIndex < solidRockIndexes.Length; solidRockIndex++)
        {
            int startRow = solidRockIndexes[solidRockIndex - 1] + 1;
            int endRow = solidRockIndexes[solidRockIndex];
            int roundRocks = map[startRow..endRow].Count(r => r[column] == 'O');

            for (int r = startRow; r < startRow + roundRocks; r++)
            {
                newMap[r][column] = 'O';
            }
        }
    }

    return newMap;
}

char[][] TurnClockwise(char[][] map)
{
    char[][] newMap = Enumerable.Range(0, map[0].Length).Select(r => Enumerable.Range(0, map.Length).Select(i => ' ').ToArray()).ToArray();

    for (int r = 0; r < map.Length; r++)
    {
        for (int c = 0; c < map[0].Length; c++)
        {
            newMap[c][map.Length - r - 1] = map[r][c];
        }
    }

    return newMap;
}

char[][] RunCycle(char[][] map)
{
    for (int i = 0; i < 4; i++)
    {
        map = TiltNorth(map);
        map = TurnClockwise(map);
    }

    return map;
}

bool MapEquals(char[][] left, char[][] right)
    => Enumerable.Range(0, left.Length).All(i => left[i].SequenceEqual(right[i]));

char[][] RunCycles(char[][] map, int cycles)
{
    int skip = 0;
    var history = new List<char[][]>();
    for (int i = 0; i < cycles; i++)
    {
        map = RunCycle(map);
        Print(map);
        Console.WriteLine();
        int historyIndex = history.Select((map, i) => (map, i)).Where(r => MapEquals(r.map, map)).Select(r => r.i).FirstOrDefault(-1);
        if (historyIndex >= 0)
        {
            int cycleLength = i - historyIndex;
            int toGo = cycles - historyIndex - 1;
            return history[historyIndex + (toGo % cycleLength)];
        }
        history.Add(map);
    }

    return map;
}

int Load(char[][] map)
    => map.Select((row, rowIndex) => (map.Length - rowIndex) * row.Count(r => r == 'O')).Sum();

void Print(char[][] map)
    => Console.WriteLine(string.Join(Environment.NewLine, map.Select(r => string.Join("", r))));

char[][] map = puzzleInput.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).Select(line => line.ToCharArray()).ToArray();
Print(map);
Console.WriteLine();
// Print(north);
// int load = Load(map);
// Console.WriteLine(load);
map = RunCycles(map, 1000000000);
Console.WriteLine(Load(map));