using Day2;

record Game(int Num, List<GameColors> Rounds)
{
    public static Game GameFromLine(string line)
    {
        string[] split = line.Split(": ");
        int gameNum = int.Parse(split[0][5..].Trim());
        string[] roundSplit = split[1].Split("; ");
        List<GameColors> rounds = roundSplit.Select(roundSpec =>
        {
            Dictionary<string, int> colorMap = roundSpec
                .Split(", ")
                .Select(colorSpec => colorSpec.Split(" "))
                .ToDictionary(colorSpec => colorSpec[1], colorSpec => int.Parse(colorSpec[0]));
            colorMap.TryGetValue("red", out int red);
            colorMap.TryGetValue("green", out int green);
            colorMap.TryGetValue("blue", out int blue);
            return new GameColors(red, green, blue);
        }).ToList();
        return new Game(gameNum, rounds);
    }

    public bool IsPossibleWith(GameColors totals)
        => Rounds.All(r => r.IsPossibleWith(totals));

    public GameColors MinColors
        => new GameColors(Rounds.Max(r => r.Red), Rounds.Max(r => r.Green), Rounds.Max(r => r.Blue));
}