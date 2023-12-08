using Common;

int StepsNeeded(string instructions, Dictionary<string, (string Left, string Right)> nodes, string startNode, string endNode)
{
    int steps = 0;
    string current = startNode;

    while (true)
    {
        char step = instructions[steps % instructions.Length];
        (string Left, string Right) node = nodes[current];
        current = step == 'L' ? node.Left : node.Right;
        steps++;
        if (current == endNode)
        {
            break;
        }
    }

    return steps;
}

int LoopLength(string instructions, Dictionary<string, (string Left, string Right)> nodes, string startNode, char endLetter)
{
    int steps = 0;
    string current = startNode;
    List<(string Node, int InstructionCount)> reachedNodes = new List<(string Node, int InstructionCount)>() { (current, 0) };

    while (true)
    {
        char step = instructions[steps % instructions.Length];
        (string Left, string Right) node = nodes[current];
        current = step == 'L' ? node.Left : node.Right;
        steps++;
        if (reachedNodes.Contains((current, steps % instructions.Length)))
        {
            int startLoop = reachedNodes.IndexOf((current, steps % instructions.Length));
            int lengthLoop = reachedNodes.Count - startLoop;
            int endIndex = reachedNodes.IndexOf(reachedNodes.First(r => r.Node.EndsWith(endLetter)));
            if (lengthLoop != endIndex)
            {
                throw new Exception("This would be difficult to solve :/");
            }

            return lengthLoop;
        }

        reachedNodes.Add((current, steps % instructions.Length));
    }
}

long Gcd(long a, long b) 
{
    if (b == 0)
    {
        return a;
    }

    return Gcd(b, a % b); 
} 
  
long Lcm(long a, long b) 
{ 
    return (a / Gcd(a, b)) * b; 
} 


string puzzleInput = await Util.GetPuzzleInput(8);
string[] puzzleLines = puzzleInput.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

string instructions = puzzleLines[0];
Dictionary<string, (string Left, string Right)> nodes = puzzleLines[1..].ToDictionary(line => line[0..3], line => (line[7..10], line[12..15]));
int stepsNeeded = StepsNeeded(instructions, nodes, "AAA", "ZZZ");
Console.WriteLine($"Steps needed: {stepsNeeded}");
string[] startNodes = nodes.Keys.Where(n => n.EndsWith("A")).ToArray();
int[] loops = startNodes.Select(startNode => LoopLength(instructions, nodes, startNode, 'Z')).ToArray();
long stepsNeededGhost = loops.Aggregate(1L, (l, r) => Lcm(l, r));
Console.WriteLine($"Steps needed ghost: {stepsNeededGhost}");