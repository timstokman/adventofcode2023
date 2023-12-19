namespace Day19;

public record Restriction(int Start, int End)
{
    public long NumValues
        => End - Start + 1;

    public bool Valid
        => End >= Start;
}