namespace Day14;

public sealed class Map : IEquatable<Map>
{
    public Map(char[][] positions)
    {
        Positions = positions;
    }
    
    public char[][] Positions { get; }

    public int Load
        => Positions.Select((row, rowIndex) => (Positions.Length - rowIndex) * row.Count(r => r == 'O')).Sum();
    
    public Map TiltNorth()
    {
        char[][] newMap = Positions.Select(row => row.Select(c => c is '.' or '#' ? c : '.').ToArray()).ToArray();
        for (int column = 0; column < Positions[0].Length; column++)
        {
            int[] solidRockIndexes = new[] { -1}.Concat(Positions.Select((rock, row) => (rock, row)).Where(r => r.rock[column] == '#').Select(r => r.row)).Concat(new[] { Positions.Length }).ToArray();
            for (int solidRockIndex = 1; solidRockIndex < solidRockIndexes.Length; solidRockIndex++)
            {
                int startRow = solidRockIndexes[solidRockIndex - 1] + 1;
                int endRow = solidRockIndexes[solidRockIndex];
                int roundRocks = Positions[startRow..endRow].Count(r => r[column] == 'O');

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
        char[][] newMap = Enumerable.Range(0, Positions[0].Length).Select(r => Enumerable.Range(0, Positions.Length).Select(i => ' ').ToArray()).ToArray();

        for (int r = 0; r < Positions.Length; r++)
        {
            for (int c = 0; c < Positions[0].Length; c++)
            {
                newMap[c][Positions.Length - r - 1] = Positions[r][c];
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
        => other != null && Enumerable.Range(0, Positions.Length).All(i => Positions[i].SequenceEqual(other.Positions[i]));
}