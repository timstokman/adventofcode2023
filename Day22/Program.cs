using Common;
using Day22;

int CanBeEliminated(List<Brick> bricks)
{
    List<Brick> resolved = new();
    Dictionary<int, IEnumerable<int>> isSupportedBy = new();

    while (bricks.Count > 0)
    {
        Brick current = bricks.First();

        while (current.Layer > 1)
        {
            Brick tryMove = current.OneDown;
            Brick[] collisions = resolved.Where(r => r.Positions().Intersect(tryMove.Positions()).Any()).ToArray();
            if (collisions.Length == 0)
            {
                current = tryMove;
            }
            else
            {
                isSupportedBy[current.Index] = collisions.Select(c => c.Index).ToArray();
                break;
            }
        }

        bricks.Remove(bricks.First());
        resolved.Add(current);
        Console.WriteLine($"Resolved: {bricks.Count}, {resolved.Count}");
    }

    return resolved.Count - resolved.Count(r => isSupportedBy.Any(s => s.Value.Contains(r.Index) && s.Value.Count() == 1));
}

string puzzleInput = await Util.GetPuzzleInput(22);

var bricks = puzzleInput.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).Select((l, i) => Brick.FromLine(i, l)).OrderBy(b => b.Layer).ToList();
Console.WriteLine(CanBeEliminated(bricks));