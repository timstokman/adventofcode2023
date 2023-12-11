using Common;
using Position = (int X, int Y);

IEnumerable<IEnumerable<T>> GetCombinations<T>(IEnumerable<T> list, int length = 2) where T : IComparable
{
    if (length == 1)
    {
        return list.Select(t => new T[] { t });
    }

    return GetCombinations(list, length - 1).SelectMany(t => list.Where(o => o.CompareTo(t.Last()) > 0), (t1, t2) => t1.Concat(new T[] { t2 }));
}

long ShortestPath(Position start, Position end, int emptySpaceSize, int[] emptyRows, int[] emptyColumns)
{
    int numEmptyRows = emptyRows.Count(r => (start.Y < r && end.Y > r) || (end.Y < r && start.Y > r));
    int numEmptyColumns = emptyColumns.Count(c => (start.X < c && end.X > c) || (end.X < c && start.X > c));
    return Math.Abs(end.Y - start.Y) + Math.Abs(end.X - start.X) + numEmptyColumns * (emptySpaceSize - 1) + numEmptyRows * (emptySpaceSize - 1);
}

string puzzleInput = await Util.GetPuzzleInput(11);

string[] map = puzzleInput.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
Position[] galaxies = map.SelectMany((row, r) => row.Select((i, c) => (i, c)).Where(i => i.i == '#').Select(i => new Position(i.c, r))).ToArray();
int[] emptyRows = map.Select((row, rowIndex) => (row, rowIndex)).Where(row => row.row.All(c => c == '.')).Select(row => row.rowIndex).ToArray();
int[] emptyColumns = Enumerable.Range(0, map[0].Length).Select(c => (Column: Enumerable.Range(0, map.Length).Select(r => map[r][c]).ToArray(), Index: c)).Where(c => c.Column.All(c => c == '.')).Select(c => c.Index).ToArray();
IEnumerable<IEnumerable<Position>> combinations = GetCombinations(galaxies);
long sumDistances = combinations.Sum(pair => ShortestPath(pair.First(), pair.Last(), 2, emptyRows, emptyColumns));
long sumDistancesOlder = combinations.Sum(pair => ShortestPath(pair.First(), pair.Last(), 1000000, emptyRows, emptyColumns));

Console.WriteLine($"Sum distances: {sumDistances}");
Console.WriteLine($"Sum distances older: {sumDistancesOlder}");
