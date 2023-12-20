namespace Day20;

public record Module(string Name, ModuleType Type, string[] Outputs)
{
    public static Module FromLine(string line)
    {
        string[] split = line.Split(" ");
        ModuleType type = line.StartsWith("broadcaster") ? ModuleType.Broadcaster : line.StartsWith("&") ? ModuleType.Conjunction : ModuleType.FlipFlop;
        string name = type == ModuleType.Broadcaster ? "broadcaster" : split[0][1..];
        string[] outputs = split[2..].Select(o => o.Replace(",", "")).ToArray();
        return new Module(name, type, outputs);
    }

    public override string ToString()
        => $"Module {{ Name = {Name}, Type = {Type}, Outputs = {string.Join(", ", Outputs)} }}";
}