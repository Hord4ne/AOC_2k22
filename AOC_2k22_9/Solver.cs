using AOC_2k22_Common;

namespace AOC_2k22_9;

internal class Solver : ChallangeSolver
{
    private const string Up = "U";
    private const string Down = "D";
    private const string Left = "L";
    private const string Right = "R";

    private readonly bool _debugPrint = true;

    protected override void SolvePart1(
        string[] input)
    {
        var headPosition = (0, 0);
        var tailPosition = (0, 0);

        var visitedByTail = new HashSet<(int, int)>();
        visitedByTail.Add(tailPosition);

        foreach (var instruction in input)
        {
            var instructionSplitted = instruction.Split(' ');
            
            var direction = instructionSplitted[0];
            var count = int.Parse(instructionSplitted[1]);

            for (var i = 0; i < count; i++)
            {
                headPosition = MoveHeadPosition(headPosition, direction);
                if (!IsTailStill(headPosition, tailPosition))
                {
                    tailPosition = MoveTailPositionSmarter(headPosition, tailPosition);
                    visitedByTail.Add(tailPosition);
                }          
            }
        }

        Console.WriteLine(visitedByTail.Count);
    }

    protected override void SolvePart2(
        string[] input)
    {
        var positions = new List<(int, int)>();
        var headPosition = (0, 0);
        for (var i = 0; i < 9; i++)
        {
            positions.Add((0, 0));
        }

        var visitedByTail = new HashSet<(int, int)>();
        visitedByTail.Add((0, 0));

        foreach (var instruction in input)
        {
            var instructionSplitted = instruction.Split(' ');

            var direction = instructionSplitted[0];
            var count = int.Parse(instructionSplitted[1]);

            for (var i = 0; i < count; i++)
            {
                headPosition = MoveHeadPosition(headPosition, direction);

                var prev = headPosition;

                for (int j = 0; j < positions.Count; j++)
                {
                    (int, int) tailKnot = positions[j];

                    if (!IsTailStill(prev, tailKnot))
                    {
                        var newKnotPosition = MoveTailPositionSmarter(prev, tailKnot);
                        
                        if (j == positions.Count - 1)
                        {
                            visitedByTail.Add(newKnotPosition);
                        }

                        positions[j] = newKnotPosition;
                    }

                    prev = positions[j];
                }
                if (_debugPrint)
                {
                    Print(headPosition, positions);
                    Console.WriteLine();
                    Console.WriteLine(direction);
                    Console.ReadKey();
                }
            }
        }

        Console.WriteLine(visitedByTail.Count);
    }

    private void Print(
        (int, int) head,
        List<(int,int)> knots)
    {
        var (startX, startY) = (7, 12);

        var grid = new string[30,30];
        for (int i = 0; i < 30; i++)
        {
            for (int j = 0; j < 30; j++)
            {
                grid[i, j] = ".";
            }
        }

        grid[startX, startY] = "s";

        for (int i = knots.Count - 1; i >= 0; i--)
        {
            var (knotX, knotY) = knots[i];
            grid[knotX + startX, knotY + startY] = (i+1).ToString();
        }

        var (headX, headY) = head;
        grid[headX + startX, headY + startY] = "H";

        for (int i = 0; i < 30; i++)
        {
            for (int j = 0; j < 30; j++)
            {
                Console.Write(grid[i, j]);
            }
            Console.WriteLine();
        }
    }

    private (int, int) MoveHeadPosition(
    (int, int) headPosition,
    string direction)
    {
        var (headPositionX, headPositionY) = headPosition;

        if (direction == Up)
        {
            return (headPositionX + 1, headPositionY);
        }
        if (direction == Down)
        {
            return (headPositionX - 1, headPositionY);
        }
        if (direction == Left)
        {
            return (headPositionX, headPositionY - 1);
        }
        if (direction == Right)
        {
            return (headPositionX, headPositionY + 1);
        }

        throw new NotSupportedException();
    }

    private bool IsTailStill(
        (int, int) headPosition,
        (int, int) tailPosition)
    {
        var (headPositionX, headPositionY) = headPosition;
        var (tailPositionX, tailPositionY) = tailPosition;

        return Math.Abs(headPositionX - tailPositionX) <= 1
            && Math.Abs(headPositionY - tailPositionY) <= 1;
    }

    private (int, int) MoveTailPositionSmarter(
        (int, int) headPosition,
        (int, int) tailPosition)
    {
        var (headPositionX, headPositionY) = headPosition;
        var (tailPositionX, tailPositionY) = tailPosition;

        if (headPositionX != tailPositionX
            && headPositionY != tailPositionY)
        {
            return (tailPositionX + Math.Sign(headPositionX - tailPositionX), tailPositionY + Math.Sign(headPositionY - tailPositionY));
        }
        else if (headPositionX != tailPositionX)
        {
            return (tailPositionX + Math.Sign(headPositionX - tailPositionX), tailPositionY);
        }
        else
        {
            return (tailPositionX, tailPositionY + Math.Sign(headPositionY - tailPositionY));
        }
    }

    private (int, int) MoveTailPosition(
        (int, int) headPosition,
        string direction)
    {
        var (headPositionX, headPositionY) = headPosition;

        if (direction == Up)
        {
            return (headPositionX - 1, headPositionY);
        }
        if (direction == Down)
        {
            return (headPositionX + 1, headPositionY);
        }
        if (direction == Left)
        {
            return (headPositionX, headPositionY + 1);
        }
        if (direction == Right)
        {
            return (headPositionX, headPositionY - 1);
        }

        throw new NotSupportedException();
    }
}