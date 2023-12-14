using Common;

string Reverse(string toReverse)
    => new(toReverse.Reverse().ToArray());

string[] TransposePattern(string[] pattern)
    => Enumerable.Range(0, pattern[0].Length)
                 .Select(newRowIndex => 
                     new string(
                         Enumerable.Range(0, pattern.Length)
                                   .Select(newColumnIndex => pattern[newColumnIndex][newRowIndex]).ToArray())).ToArray();

IEnumerable<int> VerticalMirrorIndex(string[] pattern, int smudge)
    => Enumerable.Range(0, pattern[0].Length - 1).Where(mirrorIndex =>
    {
        int rangeToCheck = Math.Min(mirrorIndex + 1, pattern[0].Length - mirrorIndex - 1);
        int numNotMirrored = Enumerable
            .Range(0, pattern.Length)
            .Sum(verticalIndex =>
            {
                string left = pattern[verticalIndex][(mirrorIndex + 1 - rangeToCheck)..(mirrorIndex + 1)];
                string right = Reverse(pattern[verticalIndex][(mirrorIndex + 1)..(mirrorIndex + 1 + rangeToCheck)]);
                return Enumerable.Range(0, left.Length).Count(i => left[i] != right[i]);
            });
        return numNotMirrored == smudge;
    });

IEnumerable<int> HorizontalMirrorIndex(string[] pattern, int smudge)
    => VerticalMirrorIndex(TransposePattern(pattern), smudge);

int SummarizePattern(string[] pattern, int smudge)
    => VerticalMirrorIndex(pattern, smudge).Sum(v => v + 1) + HorizontalMirrorIndex(pattern, smudge).Sum(h => (h + 1) * 100);

string puzzleInput = await Util.GetPuzzleInput(13);

string [][] patterns = puzzleInput.Split(Environment.NewLine + Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).Select(p => p.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)).ToArray();
int summary = patterns.Sum(pattern => SummarizePattern(pattern, 0));
int summarySmudge = patterns.Sum(pattern => SummarizePattern(pattern, 1));
Console.WriteLine($"Summary: {summary}");
Console.WriteLine($"Summary with smudge: {summarySmudge}");
