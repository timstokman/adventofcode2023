using Common;
using Day19;

IEnumerable<Restrictions> FindAcceptedRanges(Dictionary<string, Workflow> workflows, string workflow, Restrictions restrictions)
{
    if (workflow == "A")
    {
        yield return restrictions;
        yield break;
    }
    else if (workflow == "R")
    {
        yield break;
    }

    Restrictions current = restrictions;
    IEnumerable<Restrictions> found = Enumerable.Empty<Restrictions>();
    Workflow foundWorkflow = workflows[workflow];
    
    foreach (Rule rule in foundWorkflow.Rules)
    {
        if (rule.Operator == null)
        {
            foreach (Restrictions r in FindAcceptedRanges(workflows, rule.TargetFlow, current))
            {
                yield return r;
            }
            yield break;
        }
        else
        {
            Restrictions ruleMatchingRestriction = rule.SplitForMatching(current);
            current = rule.SplitForNonMatching(current);
            foreach (Restrictions r in FindAcceptedRanges(workflows, rule.TargetFlow, ruleMatchingRestriction))
            {
                yield return r;
            }
        }
    }
}

string puzzleInput = await Util.GetPuzzleInput(19);

string[] split = puzzleInput.Split(Environment.NewLine + Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
Dictionary<string, Workflow> workflows = split[0].Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).Select(Workflow.WorkflowFromLine).ToDictionary(w => w.Name, w => w);
Part[] parts = split[1].Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).Select(Part.PartFromLine).ToArray();
Restrictions start = new Restrictions(new Restriction(1, 4000), new Restriction(1, 4000), new Restriction(1, 4000), new Restriction(1, 4000));

Restrictions[] acceptedRanges = FindAcceptedRanges(workflows, "in", start).ToArray();
IEnumerable<Part> accepted = parts.Where(part => acceptedRanges.Any(acceptedRange => acceptedRange.InRange(part)));
int ratingNumbersAcceptedParts = accepted.Sum(p => p.Sum);
long numAcceptedCombinations = acceptedRanges.Sum(r => r.NumValues);
Console.WriteLine($"Rating numbers accepted parts: {ratingNumbersAcceptedParts}");
Console.WriteLine($"Number combinations accepted ratings: {numAcceptedCombinations}");