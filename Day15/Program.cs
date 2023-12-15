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

int GetFocusTotal(string[] instructions)
{
    List<(string Label, int Focus)>[] map = Enumerable.Range(0, 256).Select(i => new List<(string Label, int Focus)>()).ToArray();
    Regex r = new Regex(@"-|=");
    foreach (string p in instructions)
    {
        string label = r.Split(p)[0];
        int boxNum = HashIt(label);
        List<(string Label, int Focus)> box = map[boxNum];
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

    return map.SelectMany((box, boxNum) => box.Select((lensItem, lensIndex) => (boxNum + 1) * (lensIndex + 1) * lensItem.Focus)).Sum();
}

string puzzleInput = await Util.GetPuzzleInput(15);

string[] puzzle = puzzleInput.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)[0].Split(",", StringSplitOptions.RemoveEmptyEntries);
Console.WriteLine(puzzle.Sum(HashIt));
Console.WriteLine(GetFocusTotal(puzzle));