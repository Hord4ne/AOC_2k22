using AOC_2k22_Common;

namespace AOC_2k22_10;

internal class Solver : ChallangeSolver
{
    protected override void SolvePart1(
        string[] input)
    {
        List<int> cycleValues = GetCycleValues(input);

        var crucialCycles = new[]
        {
            20,
            60,
            100,
            140,
            180,
            220
        };

        var result = 0;

        foreach (var crucialCycle in crucialCycles)
        {
            result += crucialCycle * cycleValues[crucialCycle];
        }

        Console.WriteLine(result);
    }

    protected override void SolvePart2(
        string[] input)
    {
        List<int> cycleValues = GetCycleValues(input);

        for (int i = 0; i < 240; i++)
        {
            if (i % 40 == 0)
            {
                Console.WriteLine();
            }
            if (IsCloseBy(i, cycleValues[i + 1]))
            {
                Console.Write("#");
            }
            else
            {
                Console.Write(".");
            }
        }
    }

    private bool IsCloseBy(
        int crt,
        int middleOfTheSprite)
    {
        var crtInRow = crt % 40;

        return Math.Abs(middleOfTheSprite - crtInRow) <= 1;
    }

    private List<int> GetCycleValues(
        string[] input)
    {
        var cycle = 0;
        var value = 1;

        var cycleValues = new List<int>();
        cycleValues.Add(value);

        foreach (var signal in input)
        {
            if (IsNoopSignal(signal))
            {
                cycle++;
                cycleValues.Add(value);
            }
            else
            {
                var addValue = int.Parse(signal.Split(' ')[1]);

                cycleValues.Add(value);
                cycleValues.Add(value);
                value += addValue;
            }
        }

        return cycleValues;
    }


    private bool IsNoopSignal(
        string singal)
    {
        return singal.StartsWith("noop");
    }
}