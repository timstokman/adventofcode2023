using Common;
using Day21;
using MathNet.Numerics;

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

        foreach (var surround in position.Surrounding(width, height))
        {
            if (!visited.Contains(surround) && map[surround.Y][surround.X])
            {
                toVisit.Enqueue((positionSteps + 1, surround));
            }
        }
    }

    return visited.Count(v => (Math.Abs(v.X - start.X) + Math.Abs(v.Y - start.Y)) % 2 == steps % 2);
}

IEnumerable<(int Steps, int Reachable)> StepsReachableInfinite(bool[][] map, PositionGrid start, int maxSteps, int goal)
{
    Queue<(int Steps, PositionGrid Position)> toVisit = new();
    toVisit.Enqueue((0, start));
    HashSet<PositionGrid> visited = new();
    int height = map.Length;
    int width = map[0].Length;

    for (int step = 1; step < maxSteps; step++)
    {
        Queue<(int Steps, PositionGrid Position)> outsideReach = new();
        while (toVisit.Count > 0)
        {
            (int positionSteps, PositionGrid position) = toVisit.Dequeue();

            if (visited.Contains(position))
            {
                continue;
            }
            else if (positionSteps > step)
            {
                outsideReach.Enqueue((positionSteps, position));
                continue;
            }

            visited.Add(position);

            foreach (var surround in position.Surrounding(width, height))
            {
                if (!visited.Contains(surround) && map[surround.Y][surround.X])
                {
                    toVisit.Enqueue((positionSteps + 1, surround));
                }
            }
        }

        if (step % width == goal % width)
        {
            int oddEven = step % 2;
            yield return (step, visited.Count(v => (Math.Abs(v.X + width * v.XGrid - start.X) + Math.Abs(v.Y + height * v.YGrid - start.Y)) % 2 == oddEven));
        }

        toVisit = outsideReach;
    }
}

long Eval(long[] fit, int x)
{
    checked
    {
        return fit[2] * x * x + fit[1] * x + fit[0];
    }
}

string puzzleInput = await Util.GetPuzzleInput(21);

var split = puzzleInput.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
bool[][] map = split.Select(l => l.ToCharArray().Select(c => c is '.' or 'S').ToArray()).ToArray();
(string row, int rowIndex) startRow = split.Select((Row, RowIndex) => (row: Row, rowIndex: RowIndex)).Where(r => r.row.Contains("S")).Single();
Position start = new Position(startRow.row.IndexOf("S"), startRow.rowIndex);

Console.WriteLine(StepsReachable(map, start, 64));
int goal = 26501365;
(int Steps, int Reachable)[] interpolationPoints = StepsReachableInfinite(map, start.Grid, 1000, goal).ToArray();
long[] fit = Fit.Polynomial(Enumerable.Range(1, interpolationPoints.Length).Select(i => (double)i).ToArray(), interpolationPoints.Select(i => (double)i.Reachable).ToArray(), 2).Select(f => (long)Math.Round(f)).ToArray();
int toInterpolate = 1 + (goal - interpolationPoints[0].Steps) / (interpolationPoints[1].Steps - interpolationPoints[0].Steps);
Console.WriteLine(Eval(fit, toInterpolate));