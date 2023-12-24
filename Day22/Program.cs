using Common;
using Day22;

Dictionary<int, int[]> IsSupported(List<Brick> bricks)
{
    bricks = bricks.ToList();
    List<Brick> resolved = new();
    Dictionary<int, int[]> isSupportedBy = new();

    while (bricks.Count > 0)
    {
        Brick current = bricks.First();

        while (current.MinLayer > 1)
        {
            Brick tryMove = current.OneDown;
            Brick[] collisions = resolved.Where(r => tryMove.MinLayer <= r.MaxLayer && r.MinLayer <= tryMove.MaxLayer && r.Positions().Intersect(tryMove.Positions()).Any()).ToArray();
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
    }

    return isSupportedBy;
}

int SumChainReaction(Dictionary<int, int[]> isSupportedBy)
{
    Dictionary<int, int[]> supports = isSupportedBy.Values.SelectMany(v => v).Distinct().ToDictionary(i => i, i => isSupportedBy.Where(s => s.Value.Contains(i)).Select(s => s.Key).ToArray());
    int sumBricks = 0;

    foreach (int index in supports.Keys)
    {
        var eliminated = new HashSet<int>() { index };

        while (true)
        {
            var unsupported = isSupportedBy.Where(s => !eliminated.Contains(s.Key) && !s.Value.Except(eliminated).Any()).ToArray();
            if (unsupported.Length == 0)
            {
                break;
            }
            foreach (var u in unsupported)
            {
                eliminated.Add(u.Key);
                sumBricks++;
            }
        }
    }
    
    return sumBricks;
}

string puzzleInput = await Util.GetPuzzleInput(22);

List<Brick> bricks = puzzleInput.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).Select((l, i) => Brick.FromLine(i, l)).OrderBy(b => b.MinLayer).ToList();
Dictionary<int, int[]> isSupportedBy = IsSupported(bricks);
int canBeEliminated = bricks.Count(r => !isSupportedBy.Any(s => s.Value.Contains(r.Index) && s.Value.Count() == 1));
int sumChain = SumChainReaction(isSupportedBy);
Console.WriteLine(canBeEliminated);
Console.WriteLine(sumChain);