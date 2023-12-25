using System.Text.RegularExpressions;
using Common;

string puzzleInput = await Util.GetPuzzleInput(24);

int NumCollisions(long[][] hailPaths, long minAxis, long maxAxis)
{
    int count = 0;
    for (int i = 0; i < hailPaths.Length; i++)
    {
        for (int j = i + 1; j < hailPaths.Length; j++)
        {
            checked
            {
                long[] left = hailPaths[i];
                long[] right = hailPaths[j];
                (long leftX,  long leftY,  long leftvelX,  long leftvelY) =  (left[0],  left[1],  left[3],  left[4]);
                (long rightX, long rightY, long rightvelX, long rightvelY) = (right[0], right[1], right[3], right[4]);

                double slopeLeft = (double)leftvelY / leftvelX;
                double slopeRight = (double)rightvelY / rightvelX;
                if (slopeLeft == slopeRight) // Parallel
                {
                    continue;
                }

                double xP = (rightY - (slopeRight * rightX) - (leftY - (slopeLeft * leftX))) / (slopeLeft - slopeRight);
                double yP = slopeLeft * xP + (leftY - (slopeLeft * leftX));
                if (xP >= minAxis && xP <= maxAxis && yP >= minAxis && yP <= maxAxis && !((xP < leftX && leftvelX > 0) || (xP > leftX && leftvelX < 0) || (xP < rightX && rightvelX > 0) || (xP > rightX && rightvelX < 0)))
                {
                    count++;
                }
            }
        }
    }

    return count;
}

Regex split = new Regex(@"\@|,");
long[][] hailPaths = puzzleInput.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).Select(l => split.Split(l.Replace(" ", "")).Select(long.Parse).ToArray()).ToArray();
long minAxis = 200000000000000L;
long maxAxis = 400000000000000L;
Console.WriteLine(NumCollisions(hailPaths, minAxis, maxAxis));