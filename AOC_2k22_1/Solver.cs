using AOC_2k22_Common;

namespace AOC_2k22_1;

internal class Solver : ChallangeSolver
{
    protected override void SolvePart1(
        string[] input)
    {
        var maxCalories = -1;
        var currentCalories = 0;

        foreach (var line in input)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                maxCalories = Math.Max(maxCalories, currentCalories);
                currentCalories = 0;
                continue;
            }

            currentCalories += int.Parse(line);
        }

        maxCalories = Math.Max(maxCalories, currentCalories);
        Console.WriteLine(maxCalories);
    }

    protected override void SolvePart2(
        string[] input)
    {
        var priorityQueue = new PriorityQueue<int, int>();

        var currentCalories = 0;
        foreach (var line in input)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                priorityQueue.Enqueue(currentCalories, -currentCalories);
                currentCalories = 0;
                continue;
            }

            currentCalories += int.Parse(line);
        }

        if (currentCalories != 0)
        {
            priorityQueue.Enqueue(currentCalories, -currentCalories);
        }

        var top3 = DequeueRange(priorityQueue, 3).Sum();

        Console.WriteLine(top3);
    }

    private IEnumerable<TK> DequeueRange<TK, TP>(
        PriorityQueue<TK, TP> priorityQyeye,
        int range)
    {
        for (int i = 0; i < range; i++)
        {
            yield return priorityQyeye.Dequeue();
        }
    }
}
