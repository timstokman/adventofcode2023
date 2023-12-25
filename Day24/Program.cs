using System.Text.RegularExpressions;
using Common;
using Microsoft.Z3;

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

long SumPerfect(long[][] hailPaths)
{
    using Context c = new Context();
    using RealExpr pointX = c.MkRealConst("x");
    using RealExpr pointY = c.MkRealConst("y");
    using RealExpr pointZ = c.MkRealConst("z");
    using RealExpr velocX = c.MkRealConst("dx");
    using RealExpr velocY = c.MkRealConst("dy");
    using RealExpr velocZ = c.MkRealConst("dz");

    using Solver s = c.MkSolver();
    for (int i = 0; i < hailPaths.Length; i++)
    {
        long[] hail = hailPaths[i];
        RealExpr hailT = c.MkRealConst($"t{i}");
        s.Add(c.MkEq(c.MkAdd(pointX, c.MkMul(hailT, velocX)), c.MkAdd(c.MkReal(hail[0]), c.MkMul(hailT, c.MkReal(hail[3])))));
        s.Add(c.MkEq(c.MkAdd(pointY, c.MkMul(hailT, velocY)), c.MkAdd(c.MkReal(hail[1]), c.MkMul(hailT, c.MkReal(hail[4])))));
        s.Add(c.MkEq(c.MkAdd(pointZ, c.MkMul(hailT, velocZ)), c.MkAdd(c.MkReal(hail[2]), c.MkMul(hailT, c.MkReal(hail[5])))));
        s.Add(c.MkGe(hailT, c.MkReal(0)));
    }

    if (s.Check() == Status.SATISFIABLE)
    {
        return new[] { pointX, pointY, pointZ }.Sum(p => long.Parse(s.Model.Eval(p).ToString()));
    }
    
    return -1;
}

Regex split = new Regex(@"\@|,");
long[][] hailPaths = puzzleInput.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).Select(l => split.Split(l.Replace(" ", "")).Select(long.Parse).ToArray()).ToArray();
long minAxis = 200000000000000L;
long maxAxis = 400000000000000L;
Console.WriteLine(NumCollisions(hailPaths, minAxis, maxAxis));
Console.WriteLine(SumPerfect(hailPaths));
