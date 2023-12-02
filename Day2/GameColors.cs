namespace Day2;

record GameColors(int Red, int Green, int Blue)
{
    public bool IsPossibleWith(GameColors total)
        => Red <= total.Red && Blue <= total.Blue && Green <= total.Green;

    public int Power
        => Red * Blue * Green;
}