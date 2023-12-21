using Common;
using Position = (int X, int Y);

int StepsReachable(bool[][] map, Position start, int steps)
{
    Queue<(int Steps, Position)> toVisit = new();
    toVisit.Enqueue((0, start));
    HashSet<Position> visited = new();
    int height = map.Length;
    int width = map[0].Length;

    while (toVisit.Count > 0)
    {
        (int positionSteps, Position position) = toVisit.Dequeue();

        if (positionSteps > steps || visited.Contains(position))
        {
            continue;
        }

        visited.Add(position);
        var surrounding = new[]
        {
            new Position(position.X + 1, position.Y),
            new Position(position.X - 1, position.Y),
            new Position(position.X, position.Y + 1),
            new Position(position.X, position.Y - 1),
        };

        foreach (var surround in surrounding)
        {
            if (surround.Y >= 0 && surround.Y < height && surround.X >= 0 && surround.X < width && map[surround.Y][surround.X] && !visited.Contains(surround))
            {
                toVisit.Enqueue((positionSteps + 1, surround));
            }
        }
    }

    return visited.Where(v => (Math.Abs(v.X - start.X) + Math.Abs(v.Y - start.Y)) % 2 == steps % 2).Count();
}

string puzzleInput = await Util.GetPuzzleInput(21);

var split = puzzleInput.Split(Environment.NewLine, StringSplitOptions.TrimEntries);
bool[][] map = split.Select(l => l.ToCharArray().Select(c => c is '.' or 'S').ToArray()).ToArray();
(string row, int rowIndex) startRow = split.Select((Row, RowIndex) => (row: Row, rowIndex: RowIndex)).Where(r => r.row.Contains("S")).Single();
Position start = new Position(startRow.row.IndexOf("S"), startRow.rowIndex);

Console.WriteLine(StepsReachable(map, start, 64));