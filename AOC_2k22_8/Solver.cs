using AOC_2k22_Common;

namespace AOC_2k22_8;

internal class Solver : ChallangeSolver
{
    protected override void SolvePart1(
        string[] input)
    {
        var grid = input
            .Select(
                r => r.AsEnumerable()
                    .Select(c => int.Parse(c.ToString()))
                    .ToArray())
            .ToArray();

        var seenTrees = new HashSet<(int, int)>();

        // ->
        for (var i = 0; i < input.Length; i++)
        {
            var leftMost = (i, 0);
            AddVisibleTrees(
                leftMost,
                GetNextToTheRight,
                seenTrees,
                grid);
        }

        // <-
        for (var i = input.Length - 1; i >= 0 ; i--)
        {
            var rightMost = (i, grid[0].Length - 1);
            AddVisibleTrees(
                rightMost,
                GetNextToTheLeft,
                seenTrees,
                grid);
        }

        // ^
        for (var i = 0; i < input.Length; i++)
        {
            var topMost = (0, i);
            AddVisibleTrees(
                topMost,
                GetNextUp,
                seenTrees,
                grid);
        }

        // v
        for (var i = input.Length - 1; i >= 0; i--)
        {
            var bottomMost = (grid.Length - 1, i);
            AddVisibleTrees(
                bottomMost,
                GetNextDown,
                seenTrees,
                grid);
        }

        Console.WriteLine(seenTrees.Count);
    }

    private void AddVisibleTrees(
        (int, int) current,
        Func<(int, int), (int, int)> getNext,
        HashSet<(int, int)> seenTrees,
        int[][] grid,
        int lastTreeSize = -1)
    {
        if (!IsInGrid(current, grid))
        {
            return;
        }

        var (x, y) = current;
        var currentTreeSize = grid[x][y];

        if (currentTreeSize > lastTreeSize)
        {
            lastTreeSize = currentTreeSize;
            seenTrees.Add(current);
        }

        var nextTreePosition = getNext(current);
        AddVisibleTrees(nextTreePosition, getNext, seenTrees, grid, lastTreeSize);
    }

    private void AddVisibleNotBiggerTrees(
        (int, int) current,
        Func<(int, int), (int, int)> getNext,
        List<(int, int)> seenTrees,
        int[][] grid,
        int treeHouseTreeSize)
    {
        if (!IsInGrid(current, grid))
        {
            return;
        }

        var (x, y) = current;
        var currentTreeSize = grid[x][y];

        if (currentTreeSize < treeHouseTreeSize)
        {
            seenTrees.Add(current);
        }
        else
        {
            seenTrees.Add(current);
            return;
        }

        var nextTreePosition = getNext(current);
        AddVisibleNotBiggerTrees(nextTreePosition, getNext, seenTrees, grid, treeHouseTreeSize);
    }

    protected override void SolvePart2(
        string[] input)
    {
        var grid = input
            .Select(
                r => r.AsEnumerable()
                    .Select(c => int.Parse(c.ToString()))
                    .ToArray())
            .ToArray();

        var best = 0;
        var trees = new List<(int, int)>();
        for (int i = 0; i < grid.Length; i++)
        {
            for (int j = 0; j < grid[0].Length; j++)
            {
                var res = 1;
                var current = (i, j);

                trees.Clear();
                AddVisibleNotBiggerTrees(
                    GetNextToTheLeft(current),
                    GetNextToTheLeft,
                    trees,
                    grid,
                    grid[i][j]);
                res *= trees.Count;

                trees.Clear();
                AddVisibleNotBiggerTrees(
                    GetNextToTheRight(current),
                    GetNextToTheRight,
                    trees,
                    grid,
                    grid[i][j]);
                res *= trees.Count;

                trees.Clear();
                AddVisibleNotBiggerTrees(
                    GetNextUp(current),
                    GetNextUp,
                    trees,
                    grid,
                    grid[i][j]);
                res *= trees.Count;

                trees.Clear();
                AddVisibleNotBiggerTrees(
                    GetNextDown(current),
                    GetNextDown,
                    trees,
                    grid,
                    grid[i][j]);
                res *= trees.Count;

                best = Math.Max(best, res);
            }
        }

        Console.WriteLine(best);
    }

    private (int, int) GetNextToTheRight(
        (int, int) current)
    {
        var (x, y) = current;
        return (x, y + 1);
    }

    private (int, int) GetNextToTheLeft(
        (int, int) current)
    {
        var (x, y) = current;
        return (x, y - 1);
    }

    private (int, int) GetNextUp(
        (int, int) current)
    {
        var (x, y) = current;
        return (x + 1, y);
    }

    private (int, int) GetNextDown(
        (int, int) current)
    {
        var (x, y) = current;
        return (x - 1, y);
    }
    private bool IsInGrid(
        (int, int) treePosition,
        int[][] grid)
    {
        return IsInGrid(treePosition, grid.Length, grid[0].Length);
    }

    private bool IsInGrid(
        (int,int) current,
        int maxX,
        int maxY)
    {
        var (x, y) = current;
        return x >= 0
            && x < maxX 
            && y >= 0
            && y < maxY;
    }
}
