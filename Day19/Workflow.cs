namespace Day19;

public record Workflow(string Name, Rule[] Rules)
{
    public static Workflow WorkflowFromLine(string line)
    {
        int indexBracket = line.IndexOf('{');
        string name = line[0..indexBracket];
        string rules = line[(indexBracket + 1)..^1];
        return new Workflow(name, rules.Split(",").Select(Rule.FromString).ToArray());
    }
}