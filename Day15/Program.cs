using System.Text.RegularExpressions;
using Common;

int HashIt(string s)
{
    int v = 0;
    foreach (char c in s.ToCharArray())
    {
        v += c;
        v = v * 17 % 256;
    }

    return v;
}

string puzzleInput = await Util.GetPuzzleInput(15);

string[] puzzle = puzzleInput.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)[0].Split(",", StringSplitOptions.RemoveEmptyEntries);
Console.WriteLine(puzzle.Sum(HashIt));

var map = Enumerable.Range(0, 256).Select(i => new List<(string Label, int Focus)>()).ToArray();
var r = new Regex(@"-|=");
foreach (string p in puzzle)
{
    string label = r.Split(p)[0];
    int boxNum = HashIt(label);
    var box = map[boxNum];
    int ind = box.FindIndex(m => m.Label == label);

    if (p.EndsWith("-"))
    {
        if (ind >= 0)
        {
            box.RemoveAt(ind);
        }
    }
    else
    {
        int focus = int.Parse(r.Split(p)[1]);
        if (ind >= 0)
        {
            box[ind] = (label, focus);
        }
        else
        {
            box.Add((label, focus));
        }
    }
}

long focusTotal = map.SelectMany((box, boxNum) => box.Select((lensItem, lensIndex) => (boxNum + 1) * (lensIndex + 1) * lensItem.Focus)).Sum();

Console.WriteLine(focusTotal);