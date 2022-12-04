using AOC_2k22_0;

namespace AOC_2k22_3;

internal class Solver : ChallangeSolver
{
    protected override void SolvePart1(
        string[] input)
    {
        var totalScore = 0;

        foreach (var packing in input)
        {
            var totalPackingLength = packing.Length;

            var packingAsSpan = packing.AsSpan();

            var leftCompartment = new HashSet<char>(packingAsSpan.Slice(0, packing.Length / 2).ToArray());
            var rightCompartment = new HashSet<char>(packingAsSpan.Slice(packing.Length / 2).ToArray());

            totalScore += leftCompartment.Intersect(rightCompartment).Sum(x => GetLetterPriority(x.ToString()));
        }

        Console.WriteLine(totalScore);
    }

    protected override void SolvePart2(
        string[] input)
    {
        var totalScore = 0;

        for (var i = 0; i < input.Length; i += 3)
        {
            var firstElfBag = new HashSet<char>(input[i]);
            var secondElfBag = new HashSet<char>(input[i + 1]);
            var thirdElfBag = new HashSet<char>(input[i + 2]);

            totalScore += firstElfBag
                .Intersect(secondElfBag)
                .Intersect(thirdElfBag)
                .Sum(x => GetLetterPriority(x.ToString()));
        }

        Console.WriteLine(totalScore);
    }

    private int GetLetterPriority(string letter)
    {
        if (letter.ToLower()[0] == letter[0])
        {
            return (int)letter[0] - 96;
        }
        else
        {
            return (int)letter[0] - 64 + 26;
        }
    }
}
