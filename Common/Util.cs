namespace Common;

public static class Util
{
    public static async Task<string> GetPuzzleInput(int day)
    {
        string cookieHeader = DotEnv.Load()["COOKIE"];
        using HttpClient client = new();
        client.DefaultRequestHeaders.Add("Cookie", cookieHeader);
        HttpResponseMessage result = await client.GetAsync($"https://adventofcode.com/2023/day/{day}/input");
        return await result.Content.ReadAsStringAsync();
    }

    public static IEnumerable<string> SplitInLines(this string input, bool removeEmptyLines=true)
        => input.Split(Environment.NewLine).Where(l => !removeEmptyLines || !string.IsNullOrWhiteSpace(l));
}