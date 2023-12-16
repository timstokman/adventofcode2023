using Common;
using Position = (int X, int Y);

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
    Dictionary<Position, List<Direction>> nodeStates = new Dictionary<Position, List<Direction>>();
    Queue<(Position Position, Direction Direction)> queue = new Queue<(Position Position, Direction Direction)>();
    queue.Enqueue((startPosition, startDirection));

    while (queue.Any())
    {
        (Position position, Direction direction) = queue.Dequeue();

        if (position.X < 0 || position.X >= width || position.Y < 0 || position.Y >= height)
        {
            continue;
        }
        
        if (!nodeStates.TryGetValue(position, out List<Direction>? nodeState))
        {
            nodeState = new() { direction };
            nodeStates[position] = nodeState;
        }
        else
        {
            if (nodeState.Contains(direction))
            {
                continue;
            }

            nodeState.Add(direction);
        }

        switch (map[position.Y][position.X])
        {
            case '.':
                var newPosition = direction switch
                {
                    Direction.Top => new Position(position.X, position.Y - 1),
                    Direction.Right => new Position(position.X + 1, position.Y),
                    Direction.Bottom => new Position(position.X, position.Y + 1),
                    Direction.Left => new Position(position.X - 1, position.Y),
                    _ => throw new ArgumentOutOfRangeException()
                };
                queue.Enqueue((newPosition, direction));
                break;
            case '/':
                ((int X, int Y), Direction) nextRightMirror = direction switch
                {
                    Direction.Top => (new Position(position.X + 1, position.Y), Direction.Right),
                    Direction.Right => (new Position(position.X, position.Y - 1), Direction.Top),
                    Direction.Bottom => (new Position(position.X - 1, position.Y), Direction.Left),
                    Direction.Left => (new Position(position.X, position.Y + 1), Direction.Bottom),
                    _ => throw new ArgumentOutOfRangeException()
                };
                queue.Enqueue(nextRightMirror);
                break;
            case '\\':
                ((int X, int Y), Direction) nextLeftMirror = direction switch
                {
                    Direction.Top => (new Position(position.X - 1, position.Y), Direction.Left),
                    Direction.Right => (new Position(position.X, position.Y + 1), Direction.Bottom),
                    Direction.Bottom => (new Position(position.X + 1, position.Y), Direction.Right),
                    Direction.Left => (new Position(position.X, position.Y - 1), Direction.Top),
                    _ => throw new ArgumentOutOfRangeException()
                };
                queue.Enqueue(nextLeftMirror);
                break;
            case '|':
                if (direction == Direction.Top || direction == Direction.Bottom)
                {
                    var newPositionVerticalSplit = direction switch
                    {
                        Direction.Top => new Position(position.X, position.Y - 1),
                        Direction.Bottom => new Position(position.X, position.Y + 1),
                        _ => throw new ArgumentOutOfRangeException()
                    };
                    queue.Enqueue((newPositionVerticalSplit, direction));
                }
                else
                {
                    queue.Enqueue((new Position(position.X, position.Y - 1), Direction.Top));
                    queue.Enqueue((new Position(position.X, position.Y + 1), Direction.Bottom));
                }
                break;
            case '-':
                if (direction == Direction.Left || direction == Direction.Right)
                {
                    var newPositionHorizontalSplit = direction switch
                    {
                        Direction.Right => new Position(position.X + 1, position.Y),
                        Direction.Left => new Position(position.X - 1, position.Y),
                        _ => throw new ArgumentOutOfRangeException()
                    };
                    queue.Enqueue((newPositionHorizontalSplit, direction));
                }
                else
                {
                    queue.Enqueue((new Position(position.X - 1, position.Y), Direction.Left));
                    queue.Enqueue((new Position(position.X + 1, position.Y), Direction.Right));
                }
                break;
            default:
                throw new ArgumentOutOfRangeException();
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