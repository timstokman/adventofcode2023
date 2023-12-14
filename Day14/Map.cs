namespace Day14;

public sealed class Map : IEquatable<Map>
{
    private readonly char[][] map;

    public Map(char[][] map)
    {
        this.map = map;
    }
    
    public int Load
        => map.Select((row, rowIndex) => (map.Length - rowIndex) * row.Count(r => r == 'O')).Sum();
    
    public Map TiltNorth()
    {
        char[][] newMap = map.Select(row => row.Select(c => c is '.' or '#' ? c : '.').ToArray()).ToArray();
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

        return new(newMap);
    }

    public Map TurnClockwise()
    {
        char[][] newMap = Enumerable.Range(0, map[0].Length).Select(r => Enumerable.Range(0, map.Length).Select(i => ' ').ToArray()).ToArray();

        for (int r = 0; r < map.Length; r++)
        {
            for (int c = 0; c < map[0].Length; c++)
            {
                newMap[c][map.Length - r - 1] = map[r][c];
            }
        }

        return new(newMap);
    }

    public Map RunCycle()
    {
        Map newMap = this;
        
        for (int i = 0; i < 4; i++)
        {
            newMap = newMap.TiltNorth();
            newMap = newMap.TurnClockwise();
        }

        return newMap;
    }

    public Map RunCycles(int cycles)
    {
        List<Map> history = new List<Map>();
        Map current = this;
        for (int i = 0; i < cycles; i++)
        {
            current = current.RunCycle();
            int historyIndex = history.IndexOf(current);
            if (historyIndex >= 0)
            {
                int cycleLength = i - historyIndex;
                int toGo = cycles - historyIndex - 1;
                return history[historyIndex + (toGo % cycleLength)];
            }
            history.Add(current);
        }

        return current;
    }

    public bool Equals(Map? other)
    {
        if (ReferenceEquals(null, other))
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return Enumerable.Range(0, map.Length).All(i => map[i].SequenceEqual(other.map[i]));
    }

    public override bool Equals(object? obj)
        => ReferenceEquals(this, obj) || obj is Map other && Equals(other);

    public override int GetHashCode()
        => map.GetHashCode();
}