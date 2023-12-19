using Common;
using Day19;

string puzzleInput = await Util.GetPuzzleInput(19);

string WorkflowResult(Workflow workflow, Part part)
{
    foreach (var rule in workflow.Rules)
    {
        if (rule.Rating == null)
        {
            return rule.TargetFlow;
        }
        else
        {
            int toCompare = part.Ratings[rule.Rating.Value];
            if (rule.Operator == Operator.Greater && toCompare > rule.Comparison)
            {
                return rule.TargetFlow;
            }
            else if (rule.Operator == Operator.Lesser && toCompare < rule.Comparison)
            {
                return rule.TargetFlow;
            }
        }
    }

    throw new ArgumentOutOfRangeException();
}

bool IsAccepted(Dictionary<string, Workflow> workflows, Part part)
{
    Workflow current = workflows["in"];

    while (true)
    {
        string targetFlow = WorkflowResult(current, part);
        if (targetFlow == "A")
        {
            return true;
        }
        else if (targetFlow == "R")
        {
            return false;
        }
        else
        {
            current = workflows[targetFlow];
        }
    }
}

long NumCombinations(Dictionary<string, Workflow> workflows)
{
    Restrictions start = new Restrictions(new Restriction(1, 4000), new Restriction(1, 4000), new Restriction(1, 4000), new Restriction(1, 4000));
    var accepted = AcceptedRanges(workflows, "in", start);
    return accepted.Sum(a => a.NumValues);
}

IEnumerable<Restrictions> AcceptedRanges(Dictionary<string, Workflow> workflows, string workflow, Restrictions restrictions)
{
    if (workflow == "A")
    {
        return new[] { restrictions };
    }
    else if (workflow == "R")
    {
        return Enumerable.Empty<Restrictions>();
    }

    Restrictions current = restrictions;
    List<Restrictions> found = new List<Restrictions>();
    var foundWorkflow = workflows[workflow];
    
    foreach (Rule rule in foundWorkflow.Rules)
    {
        if (!current.Valid)
        {
            break;
        }

        if (rule.Operator == null)
        {
            found.AddRange(AcceptedRanges(workflows, rule.TargetFlow, current));
            break;
        }
        else
        {
            Restriction ratingRestriction = current.ByRating(rule.Rating.Value);
            Restrictions ruleMatchingRestriction = 
                current.With(rule.Rating.Value,
                    rule.Operator == Operator.Greater ?
                        ratingRestriction with { Start = Math.Max(ratingRestriction.Start, rule.Comparison.Value + 1) } :
                        ratingRestriction with { End = Math.Min(ratingRestriction.End, rule.Comparison.Value - 1) });
            found.AddRange(AcceptedRanges(workflows, rule.TargetFlow, ruleMatchingRestriction));
            current = current.With(rule.Rating.Value,
                rule.Operator == Operator.Greater ? 
                    ratingRestriction with { End = Math.Min(ratingRestriction.End, rule.Comparison.Value) } :
                    ratingRestriction with { Start = Math.Max(ratingRestriction.Start, rule.Comparison.Value) });
        }
    }

    return found;
}

string[] split = puzzleInput.Split(Environment.NewLine + Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
Dictionary<string, Workflow> workflows = split[0].Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).Select(Workflow.WorkflowFromLine).ToDictionary(w => w.Name, w => w);
Part[] parts = split[1].Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).Select(Part.PartFromLine).ToArray();
var accepted = parts.Where(p => IsAccepted(workflows, p)).ToArray();
Console.WriteLine(accepted.Sum(p => p.Sum));
Console.WriteLine(NumCombinations(workflows));