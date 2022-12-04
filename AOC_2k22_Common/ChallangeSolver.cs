namespace AOC_2k22_0;

public abstract class ChallangeSolver
{
    protected abstract void SolvePart1(
        string[] input);

    protected abstract void SolvePart2(
        string[] input);

    public async Task Part1()
    {
        SolvePart1(await ReadInput());
    }
    public async Task Part2()
    {
        SolvePart2(await ReadInput());
    }

    private Task<string[]> ReadInput(
        string filename = "input.txt")
    {
        return File.ReadAllLinesAsync(filename);
    }
}