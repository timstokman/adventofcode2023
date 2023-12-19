using Common;
using Day19;

IEnumerable<Restrictions> FindAcceptedRanges(Dictionary<string, Workflow> workflows, string workflow, Restrictions restrictions)
{
    if (!restrictions.Valid)
    {
        return Enumerable.Empty<Restrictions>();
    }
    else if (workflow == "A")
    {
        return new[] { restrictions };
    }
    else if (workflow == "R")
    {
        return Enumerable.Empty<Restrictions>();
    }

    Restrictions current = restrictions;
    IEnumerable<Restrictions> found = Enumerable.Empty<Restrictions>();
    Workflow foundWorkflow = workflows[workflow];
    
    foreach (Rule rule in foundWorkflow.Rules)
    {
        if (!current.Valid)
        {
            break;
        }

        if (rule.Operator == null)
        {
            found = found.Concat(FindAcceptedRanges(workflows, rule.TargetFlow, current));
            break;
        }
        else
        {
            Restrictions ruleMatchingRestriction = rule.SplitForMatching(current);
            found = found.Concat(FindAcceptedRanges(workflows, rule.TargetFlow, ruleMatchingRestriction));
            current = rule.SplitForNonMatching(current);
        }
    }

    return found;
}

string puzzleInput = await Util.GetPuzzleInput(19);

string[] split = puzzleInput.Split(Environment.NewLine + Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
Dictionary<string, Workflow> workflows = split[0].Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).Select(Workflow.WorkflowFromLine).ToDictionary(w => w.Name, w => w);
Part[] parts = split[1].Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).Select(Part.PartFromLine).ToArray();
Restrictions start = new Restrictions(new Restriction(1, 4000), new Restriction(1, 4000), new Restriction(1, 4000), new Restriction(1, 4000));

Restrictions[] acceptedRanges = FindAcceptedRanges(workflows, "in", start).ToArray();
IEnumerable<Part> accepted = parts.Where(part => acceptedRanges.Any(acceptedRange => acceptedRange.InRange(part)));
Console.WriteLine(accepted.Sum(p => p.Sum));
Console.WriteLine(acceptedRanges.Sum(r => r.NumValues));