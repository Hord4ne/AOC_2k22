using AOC_2k22_Common;

namespace AOC_2k22_12;

internal class Solver : ChallangeSolver
{
    private const int LetterAValue = 97;

    protected override void SolvePart1(
        string[] input)
    {
        var start = (-1, -1);
        var end = (-1, -1);

        var grid = new int[input.Length, input[0].Length];

        for (int i = 0; i < input.Length; i++)
        {
            string? line = input[i];
            for (int j = 0; j < line.Length; j++)
            {
                char letter = line[j];
                if (letter == 'S')
                {
                    start = (i, j);
                    grid[i, j] = (int)'a' - LetterAValue;
                }
                else if (letter == 'E')
                {
                    end = (i, j);
                    grid[i, j] = (int)'z' - LetterAValue;
                }
                else
                {
                    grid[i, j] = (int)letter - LetterAValue;
                }
            }
        }

        var result = FindShortestPathFromTo(start, end, grid);

        Console.WriteLine(result);
    }

    private object FindShortestPathFromTo(
        (int, int) start,
        (int, int) end,
        int[,] grid)
    {
        var seen = new HashSet<(int, int)>();
        var queue = new Queue<(int, int)>();
        var stepsCounter = new Dictionary<(int,int), int>();

        queue.Enqueue(start);
        stepsCounter[start] = 0;

        while (queue.Count > 0)
        {
            var item = queue.Dequeue();

            if (item == end)
            {
                return stepsCounter[end];
            }

            var currentItemCounter = stepsCounter[item];

            foreach (var neighbour in GetNeighbours(item, grid))
            {
                if (seen.Add(neighbour))
                {
                    if (stepsCounter.TryGetValue(neighbour, out var neighbourCounter))
                    {
                        stepsCounter[neighbour] = Math.Min(neighbourCounter, currentItemCounter + 1);
                    }
                    else
                    {
                        stepsCounter[neighbour] = currentItemCounter + 1;
                    }

                    queue.Enqueue(neighbour);
                }
            }
        }

        throw new NotImplementedException("Unsolvable!");
    }

    private IEnumerable<(int, int)> GetNeighbours(
        (int, int) item,
        int[,] grid)
    {
        var (itemX, itemY) = item;

        var itemValue = grid[itemX, itemY];

        foreach (var possibleNeighbourShift in GetPossibleNeighboursShifts())
        {
            var (possibleShiftX, possibleShiftY) = possibleNeighbourShift;
            var possibleItem = (itemX + possibleShiftX, itemY + possibleShiftY);
            var (possibleItemX, possibleItemY) = possibleItem;

            if (IsInGrid(possibleItem, grid)
                && grid[possibleItemX, possibleItemY] - 1 <= itemValue)
            {
                yield return possibleItem;
            }
        }
    }

    private bool IsInGrid(
        (int, int) value,
        int[,] grid)
    {
        var (x, y) = value;

        return x < grid.GetLength(0)
            && y < grid.GetLength(1)
            && x >= 0
            && y >= 0;
    }

    private static (int, int)[] GetPossibleNeighboursShifts()
    {
        return new[] { (-1, 0), (1, 0), (0, -1), (0, 1) };
    }

    protected override void SolvePart2(
        string[] input)
    {
        var startPositions = new List<(int,int)>();
        var end = (-1, -1);

        var grid = new int[input.Length, input[0].Length];

        for (int i = 0; i < input.Length; i++)
        {
            string? line = input[i];
            for (int j = 0; j < line.Length; j++)
            {
                char letter = line[j];
                if (letter == 'a')
                {
                    startPositions.Add((i, j));
                }
                if (letter == 'S')
                {
                    startPositions.Add((i, j));
                    grid[i, j] = (int)'a' - LetterAValue;
                }
                else if (letter == 'E')
                {
                    end = (i, j);
                    grid[i, j] = (int)'z' - LetterAValue;
                }
                
                else
                {
                    grid[i, j] = (int)letter - LetterAValue;
                }
            }
        }

        var result = FindShortestPathFromToPart2(startPositions, end, grid);

        Console.WriteLine(result);
    }

    private object FindShortestPathFromToPart2(
    List<(int, int)> startPositions,
    (int, int) end,
    int[,] grid)
    {
        var seen = new HashSet<(int, int)>();
        var queue = new Queue<(int, int)>();
        var stepsCounter = new Dictionary<(int, int), int>();

        foreach (var start in startPositions)
        {
            queue.Enqueue(start);
            stepsCounter[start] = 0;
        }

        while (queue.Count > 0)
        {
            var item = queue.Dequeue();

            if (item == end)
            {
                return stepsCounter[end];
            }

            var currentItemCounter = stepsCounter[item];

            foreach (var neighbour in GetNeighbours(item, grid))
            {
                if (seen.Add(neighbour))
                {
                    if (stepsCounter.TryGetValue(neighbour, out var neighbourCounter))
                    {
                        stepsCounter[neighbour] = Math.Min(neighbourCounter, currentItemCounter + 1);
                    }
                    else
                    {
                        stepsCounter[neighbour] = currentItemCounter + 1;
                    }

                    queue.Enqueue(neighbour);
                }
            }
        }

        throw new NotImplementedException("Unsolvable!");
    }
}