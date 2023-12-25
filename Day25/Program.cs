using Common;

bool HasPath(Dictionary<string, HashSet<string>> edges, Dictionary<string, Dictionary<string, int>> capacity, string start, string end, Dictionary<string, string> parent)
{
    HashSet<string> visited = new();

    Queue<string> queue = new();
    queue.Enqueue(start);
    visited.Add(start);

    while (queue.Count > 0)
    {
        string u = queue.Dequeue();

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

int FindGroupsWithEdmondsKarp(string[] nodes, Dictionary<string, HashSet<string>> edges)
{
    Random r = new();
    while (true)
    {
        string start = nodes[r.Next(nodes.Length)];
        string end = nodes[r.Next(nodes.Length)];
        Dictionary<string, Dictionary<string, int>> capacity = edges.ToDictionary(e => e.Key, e => e.Value.ToDictionary(v => v, v => 1));
        Dictionary<string, string> parent = new();
        int maxFlow = 0;

        while (HasPath(edges, capacity, start, end, parent))
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
                string previous = parent[v];
                capacity[previous][v] -= pathFlow;
                capacity[v][previous] += pathFlow;
                v = previous;
            }
        }

        if (maxFlow != 3)
        {
            continue;
        }

        int firstGroupSize = nodes.Count(n => HasPath(edges, capacity, start, n, parent));
        int secondGroupSize = nodes.Length - firstGroupSize;

        return firstGroupSize * secondGroupSize;
    }
}

string puzzleInput = await Util.GetPuzzleInput(25);

(string Left, string Right)[] edges = puzzleInput.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).SelectMany(l =>
{
    string[] split = l.Split(": ");
    return split[1].Split(" ").Select(s => (Left: split[0], Right: s));
}).ToArray();
string[] nodes = edges.SelectMany(e => new[] { e.Left, e.Right }).Distinct().ToArray();
Dictionary<string, HashSet<string>> dictEdges = nodes.ToDictionary(n => n, n => new HashSet<string>(edges.Where(e => e.Left == n).Select(e => e.Right).Concat(edges.Where(e => e.Right == n).Select(e => e.Left))));

Console.WriteLine(FindGroupsWithEdmondsKarp(nodes, dictEdges));