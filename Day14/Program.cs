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

int Load(char[][] map)
    => map.Select((row, rowIndex) => (map.Length - rowIndex) * row.Count(r => r == 'O')).Sum();

void Print(char[][] map)
    => Console.WriteLine(string.Join(Environment.NewLine, map.Select(r => string.Join("", r))));

char[][] map = puzzleInput.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).Select(line => line.ToCharArray()).ToArray();
var north = TiltNorth(map);
Print(north);
int load = Load(north);
Console.WriteLine(load);