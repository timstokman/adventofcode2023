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
}