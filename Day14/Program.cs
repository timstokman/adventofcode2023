using Common;
using Day14;

string puzzleInput = await Util.GetPuzzleInput(14);

Map map = new(puzzleInput.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).Select(line => line.ToCharArray()).ToArray());
Console.WriteLine($"Load tilted: {map.TiltNorth().Load}");
Console.WriteLine($"Load cycled: {map.RunCycles(1000000000).Load}");