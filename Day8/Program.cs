using Common;

int StepsNeeded(string instructions, Dictionary<string, (string Left, string Right)> nodes)
{
    int steps = 0;
    string current = "AAA";

    while (true)
    {
        char step = instructions[steps % instructions.Length];
        var node = nodes[current];
        current = step == 'L' ? node.Left : node.Right;
        steps++;
        if (current == "ZZZ")
        {
            break;
        }
    }

    return steps;
}

string puzzleInput = await Util.GetPuzzleInput(8);
string[] puzzleLines = puzzleInput.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

string instructions = puzzleLines[0];
Dictionary<string, (string Left, string Right)> nodes = puzzleLines[1..].ToDictionary(line => line[0..3], line => (line[7..10], line[12..15]));
int stepsNeeded = StepsNeeded(instructions, nodes);
Console.WriteLine(stepsNeeded);