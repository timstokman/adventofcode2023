using Common;
using Position = (int X, int Y);

Position MoveInDirection(Position position, Direction direction)
{
    return direction switch
    {
        Direction.Top => new Position(position.X, position.Y - 1),
        Direction.Right => new Position(position.X + 1, position.Y),
        Direction.Bottom => new Position(position.X, position.Y + 1),
        Direction.Left => new Position(position.X - 1, position.Y),
        _ => throw new ArgumentOutOfRangeException()
    };
}

IEnumerable<Direction> NextDirection(char encountered, Direction direction)
{
    switch (encountered)
    {
        case '.':
            yield return direction;
            break;
        case '/':
            yield return direction switch
            {
                Direction.Top => Direction.Right,
                Direction.Right => Direction.Top,
                Direction.Bottom => Direction.Left,
                Direction.Left => Direction.Bottom,
                _ => throw new ArgumentOutOfRangeException()
            };
            break;
        case '\\':
            yield return direction switch
            {
                Direction.Top => Direction.Left,
                Direction.Right => Direction.Bottom,
                Direction.Bottom => Direction.Right,
                Direction.Left => Direction.Top,
                _ => throw new ArgumentOutOfRangeException()
            };
            break;
        case '|':
            if (direction is Direction.Top or Direction.Bottom)
            {
                yield return direction;
            }
            else
            {
                yield return Direction.Top;
                yield return Direction.Bottom;
            }
            break;
        case '-':
            if (direction is Direction.Left or Direction.Right)
            {
                yield return direction;
            }
            else
            {
                yield return Direction.Left;
                yield return Direction.Right;
            }
            break;
        default:
            throw new ArgumentOutOfRangeException(nameof(encountered));
    }
}

IEnumerable<((int X, int Y) Position, Direction Direction)> PossibleEntryPositions(int height, int width)
{
    return Enumerable.Range(0, height).Select(i => (new Position(i, 0), Direction.Bottom)).Concat(
           Enumerable.Range(0, height).Select(i => (new Position(i, height - 1), Direction.Top))).Concat(
           Enumerable.Range(0, width).Select(i => (new Position(0, i), Direction.Right))).Concat(
           Enumerable.Range(0, width).Select(i => (new Position(width - 1, i), Direction.Left)));
}

int TrackEnergize(char[][] map, Position startPosition, Direction startDirection)
{
    int height = map.Length;
    int width = map[0].Length;
    Dictionary<Position, HashSet<Direction>> nodeStates = new();
    Queue<(Position Position, Direction Direction)> queue = new();
    queue.Enqueue((startPosition, startDirection));

    while (queue.Count != 0)
    {
        (Position position, Direction direction) = queue.Dequeue();

        if (position.X < 0 || position.X >= width || position.Y < 0 || position.Y >= height)
        {
            continue;
        }
        
        if (!nodeStates.TryGetValue(position, out HashSet<Direction>? nodeState))
        {
            nodeStates[position] = [direction];
        }
        else if (!nodeState.Add(direction))
        {
            continue;
        }

        foreach (Direction nextDirection in NextDirection(map[position.Y][position.X], direction))
        {
            queue.Enqueue((MoveInDirection(position, nextDirection), nextDirection));
        }
    }

    return nodeStates.Keys.Count;
}

string puzzleInput = await Util.GetPuzzleInput(16);

char[][] map = puzzleInput.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).Select(l => l.ToCharArray()).ToArray();
int numEnergized = TrackEnergize(map, new Position(0, 0), Direction.Right);
Console.WriteLine($"Num energized: {numEnergized}");

var possiblePositions = PossibleEntryPositions(map.Length, map[0].Length);
int maxEnergized = possiblePositions.Max(p => TrackEnergize(map, p.Position, p.Direction));
Console.WriteLine($"Max energized: {maxEnergized}");