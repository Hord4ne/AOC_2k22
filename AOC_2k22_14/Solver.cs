using AOC_2k22_Common;
using System.Collections;

namespace AOC_2k22_14;

internal class Solver : ChallangeSolver
{
    private (int, int) _sandDropStartPoint = (500, 0);
    protected override void SolvePart1(
        string[] input)
    {
        var occupiedPlaces = new HashSet<(int, int)>();

        foreach (var line in input)
        {
            var lineSplitted = line.Split(' ');
            var lineStart = lineSplitted[0].Split(",");

            foreach (var elem in lineSplitted.Skip(1))
            {
                if (elem == "->")
                {
                    continue;
                }

                var lineEnd = elem.Split(",");

                var (lineStartX, lineStartY) = (int.Parse(lineStart[0]), int.Parse(lineStart[1]));
                var (lineEndX, lineEndY) = (int.Parse(lineEnd[0]), int.Parse(lineEnd[1]));

                if (lineStartX == lineEndX)
                {
                    for (var i = Math.Min(lineStartY, lineEndY) ; i <= Math.Max(lineStartY, lineEndY); i++)
                    {
                        occupiedPlaces.Add((lineStartX, i));
                    }
                }
                else
                {
                    for (var i = Math.Min(lineStartX, lineEndX); i <= Math.Max(lineStartX, lineEndX); i++)
                    {
                        occupiedPlaces.Add((i, lineStartY));
                    }
                }

                lineStart = lineEnd;
            }
        }

        var maxRegisteredY = occupiedPlaces.Select(p => p.Item2).Max();
        var beforeFallingCount = occupiedPlaces.Count;
        var currentSandPoint = _sandDropStartPoint;
        
        while (true)
        {
            if (IsFallingOutOfTheGrid(currentSandPoint, maxRegisteredY))
            {
                break;
            }

            var (stayInPlace, nextPlace) = DefineNextMove(currentSandPoint, occupiedPlaces);

            if (stayInPlace)
            {
                occupiedPlaces.Add(currentSandPoint);
                currentSandPoint = _sandDropStartPoint;
                continue;
            }

            currentSandPoint = nextPlace;
        }

        Console.WriteLine(occupiedPlaces.Count - beforeFallingCount);
    }

    private (bool, (int,int)) DefineNextMove(
        (int, int) currentSandPoint,
        HashSet<(int, int)> occupiedPlaces)
    {
        var (currentX, currentY) = currentSandPoint;

        var oneDown = (currentX, currentY + 1);

        if (!occupiedPlaces.Contains(oneDown))
        {
            return (false, oneDown);
        }

        var oneDownToTheLeft = (currentX - 1, currentY + 1);
        if (!occupiedPlaces.Contains(oneDownToTheLeft))
        {
            return (false, oneDownToTheLeft);
        }

        var oneDownToTheRight = (currentX + 1, currentY + 1);
        if (!occupiedPlaces.Contains(oneDownToTheRight))
        {
            return (false, oneDownToTheRight);
        }

        return (true, currentSandPoint);
    }

    private bool IsFallingOutOfTheGrid(
        (int, int) currentSandPoint,
        int maxRegisteredY)
    {
        var (_, y) = currentSandPoint;
        return y >= maxRegisteredY;
    }

    protected override void SolvePart2(
        string[] input)
    {
        var occupiedPlaces = new HashSet<(int, int)>();

        foreach (var line in input)
        {
            var lineSplitted = line.Split(' ');
            var lineStart = lineSplitted[0].Split(",");

            foreach (var elem in lineSplitted.Skip(1))
            {
                if (elem == "->")
                {
                    continue;
                }

                var lineEnd = elem.Split(",");

                var (lineStartX, lineStartY) = (int.Parse(lineStart[0]), int.Parse(lineStart[1]));
                var (lineEndX, lineEndY) = (int.Parse(lineEnd[0]), int.Parse(lineEnd[1]));

                if (lineStartX == lineEndX)
                {
                    for (var i = Math.Min(lineStartY, lineEndY); i <= Math.Max(lineStartY, lineEndY); i++)
                    {
                        occupiedPlaces.Add((lineStartX, i));
                    }
                }
                else
                {
                    for (var i = Math.Min(lineStartX, lineEndX); i <= Math.Max(lineStartX, lineEndX); i++)
                    {
                        occupiedPlaces.Add((i, lineStartY));
                    }
                }

                lineStart = lineEnd;
            }
        }

        var maxRegisteredY = occupiedPlaces.Select(p => p.Item2).Max() + 2;
        var beforeFallingCount = occupiedPlaces.Count;
        var currentSandPoint = _sandDropStartPoint;

        while (true)
        {
            var (stayInPlace, nextPlace) = DefineNextMovePart2(currentSandPoint, occupiedPlaces, maxRegisteredY);

            if (stayInPlace)
            {
                if (currentSandPoint == _sandDropStartPoint)
                {
                    break;
                }

                occupiedPlaces.Add(currentSandPoint);
                currentSandPoint = _sandDropStartPoint;
                continue;
            }

            currentSandPoint = nextPlace;
        }

        Console.WriteLine(occupiedPlaces.Count - beforeFallingCount);
    }

    private (bool, (int, int)) DefineNextMovePart2(
        (int, int) currentSandPoint,
        HashSet<(int, int)> occupiedPlaces,
        int maxRegisteredY)
    {
        var (currentX, currentY) = currentSandPoint;

        if (currentY + 1 == maxRegisteredY)
        {
            return (true, currentSandPoint);
        }

        var oneDown = (currentX, currentY + 1);

        if (!occupiedPlaces.Contains(oneDown))
        {
            return (false, oneDown);
        }

        var oneDownToTheLeft = (currentX - 1, currentY + 1);
        if (!occupiedPlaces.Contains(oneDownToTheLeft))
        {
            return (false, oneDownToTheLeft);
        }

        var oneDownToTheRight = (currentX + 1, currentY + 1);
        if (!occupiedPlaces.Contains(oneDownToTheRight))
        {
            return (false, oneDownToTheRight);
        }

        return (true, currentSandPoint);
    }
}