using Common;
using Day19;

string puzzleInput = await Util.GetPuzzleInput(19);

string Result(Workflow workflow, Part part)
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

bool IsAccepted(Workflow[] workflows, Part part)
{
    Dictionary<string, Workflow> mapping = workflows.ToDictionary(w => w.Name, w => w);
    Workflow current = mapping["in"];

    while (true)
    {
        string targetFlow = Result(current, part);
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
            current = mapping[targetFlow];
        }
    }
}

string[] split = puzzleInput.Split(Environment.NewLine + Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
Workflow[] workflows = split[0].Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).Select(Workflow.WorkflowFromLine).ToArray();
Part[] parts = split[1].Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).Select(Part.PartFromLine).ToArray();
var accepted = parts.Where(p => IsAccepted(workflows, p)).ToArray();
Console.WriteLine(accepted.Sum(p => p.Sum));