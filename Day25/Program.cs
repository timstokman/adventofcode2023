using Common;

bool HasPath(string[] nodes, Dictionary<string, HashSet<string>> edges, Dictionary<string, Dictionary<string, int>> capacity, string start, string end, Dictionary<string, string> parent)
{
    HashSet<string> visited = new();

    var queue = new Queue<string>();
    queue.Enqueue(start);
    visited.Add(start);

    while (queue.Count > 0)
    {
        var u = queue.Dequeue();

        foreach (string e in edges[u])
        {
            if (!visited.Contains(e) && capacity[u][e] > 0)
            {
                queue.Enqueue(e);
                visited.Add(e);
                parent[e] = u;
            }
        }
    }

    return visited.Contains(end);
}

int EdmondsKarp(string[] nodes, Dictionary<string, HashSet<string>> edges, string start, string end)
{
    Dictionary<string, Dictionary<string, int>> capacity = edges.ToDictionary(e => e.Key, e => e.Value.ToDictionary(v => v, v => 1));
    Dictionary<string, string> parent = new();
    int maxFlow = 0;

    while (HasPath(nodes, edges, capacity, start, end, parent))
    {
        int pathFlow = int.MaxValue;
        string s = end;
        while (s != start)
        {
            string previous = parent[s];
            pathFlow = Math.Min(pathFlow, capacity[previous][s]);
            s = previous;
        }

        maxFlow += pathFlow;

        string v = end;
        while (v != start)
        {
            var previous = parent[v];
            capacity[previous][v] -= pathFlow;
            capacity[v][previous] += pathFlow;
            v = previous;
        }
    }

    if (maxFlow != 3)
    {
        throw new ArgumentOutOfRangeException();
    }

    int firstGroupSize = nodes.Count(n => HasPath(nodes, edges, capacity, start, n, parent));
    int secondGroupSize = nodes.Length - firstGroupSize;

    return firstGroupSize * secondGroupSize;
}

/*
void GlobalMinCut(string[] nodes, Dictionary<string, HashSet<string>> adjecent)
{
    Dictionary<string, Dictionary<string, int>> co = new();
    foreach (var node in nodes)
    {
        co[node] = new 
    }
}
*/

string puzzleInput = await Util.GetPuzzleInput(25);

(string Left, string Right)[] edges = puzzleInput.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).SelectMany(l =>
{
    var split = l.Split(": ");
    return split[1].Split(" ").Select(s => (Left: split[0], Right: s));
}).ToArray();
string[] nodes = edges.SelectMany(e => new[] { e.Left, e.Right }).Distinct().ToArray();
Dictionary<string, HashSet<string>> dictEdges = nodes.ToDictionary(n => n, n => new HashSet<string>(edges.Where(e => e.Left == n).Select(e => e.Right).Concat(edges.Where(e => e.Right == n).Select(e => e.Left))));

Console.WriteLine(EdmondsKarp(nodes, dictEdges, "fdb", "mnl"));