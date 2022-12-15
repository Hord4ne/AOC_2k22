using AOC_2k22_Common;
using System.Collections;
using System.Diagnostics;

namespace AOC_2k22_15;

internal class Solver : ChallangeSolver
{
    protected override void SolvePart1(
        string[] input)
    {
        checked
        {
            var rowToInvestigate = 2000000;
            var sensorToDistance = new Dictionary<(int, int), int>();
            var beaconPositions = new HashSet<(int, int)>();
            
            foreach (var line in input)
            {
                var (sensor, closestBeacon) = ParseLine(line);

                var distance = CalculateDistance(sensor, closestBeacon);

                sensorToDistance[sensor] = distance;
                beaconPositions.Add(closestBeacon);
            }

            var result = FindOccupieOnRowNumber(sensorToDistance, rowToInvestigate).Count;
            int relevantBeacons = beaconPositions.Where(x => x.Item2 == rowToInvestigate).Count();

            Console.WriteLine(result - relevantBeacons);
        }
    }

    private HashSet<(int, int)> FindOccupieOnRowNumber(
        Dictionary<(int, int), int> sensorToDistance,
        int rowToInvestigate)
    {
        var occupiedPlaces = new HashSet<(int, int)>();

        foreach (var (sensorPosition, distanceToClosestBeacon) in sensorToDistance)
        {
            var (senosorX, sensorY) = sensorPosition;

            var verticalDistanceFromSensor = Math.Abs(sensorY - rowToInvestigate);
            if (verticalDistanceFromSensor <= distanceToClosestBeacon)
            {
                var distanceFromSensor = distanceToClosestBeacon - verticalDistanceFromSensor + 1;
                var numberOfOccupiedPlaces = 2 * distanceFromSensor - 1;

                for (int i = 0; i <= numberOfOccupiedPlaces / 2; i++)
                {
                    occupiedPlaces.Add((senosorX - i, rowToInvestigate));
                }

                occupiedPlaces.Add((senosorX, rowToInvestigate));

                for (int i = 0; i <= numberOfOccupiedPlaces / 2; i++)
                {
                    occupiedPlaces.Add((senosorX + i, rowToInvestigate));
                }
            }
        }

        return occupiedPlaces;
    }

    private ((int, int) sensor, (int, int) beacon) ParseLine(
        string line)
    {
        var lineSplitted = line.Split(" ");

        var sensorXPart = lineSplitted[2];
        var sensorYPart = lineSplitted[3];

        var beaconXPart = lineSplitted[8];
        var beaconYPart = lineSplitted[9];

        return (ParseNumbersFromInputParts(sensorXPart, sensorYPart), ParseNumbersFromInputParts(beaconXPart, beaconYPart));
    }

    private (int, int) ParseNumbersFromInputParts(
        string xPart,
        string yPart)
    {
        return (ParseNumberFromInputPart(xPart), ParseNumberFromInputPart(yPart));
    }

    private int ParseNumberFromInputPart(
        string part)
    {
        var number = part.Split("=")[1].Replace(",", "").Replace(":", "");

        return int.Parse(number);
    }

    private int CalculateDistance(
        (int, int) point,
        (int, int) otherPoint)
    {
        var (pointX, pointY) = point;
        var (otherPointX, otherPointY) = otherPoint;

        return Math.Abs(pointX - otherPointX) + Math.Abs(pointY - otherPointY);
    }

    public readonly record struct Line(int A, int B) { }

    protected override void SolvePart2(
        string[] input)
    {
        checked
        {
            var maxCoordinateValue = 4000000L;
            var sensorToDistance = new Dictionary<(int, int), int>();
            var beaconPositions = new HashSet<(int, int)>();
            var sw = Stopwatch.StartNew();
            foreach (var line in input)
            {
                var (sensor, closestBeacon) = ParseLine(line);

                var distance = CalculateDistance(sensor, closestBeacon);

                sensorToDistance[sensor] = distance;
                beaconPositions.Add(closestBeacon);
            }

            var lines = new List<Line>();

            foreach (var (sensorPosition, distanceToClosestBeacon) in sensorToDistance)
            {
                var (senosorX, sensorY) = sensorPosition;
                var topMostPoint = (senosorX, sensorY - distanceToClosestBeacon);
                var bottomMostPoint = (senosorX, sensorY + distanceToClosestBeacon);
                var leftMostPoint = (senosorX - distanceToClosestBeacon, sensorY);
                var rightMostPoint = (senosorX + distanceToClosestBeacon, sensorY);

                var line1 = GetLine(topMostPoint, rightMostPoint);
                var line2 = GetLine(topMostPoint, leftMostPoint);
                var line3 = GetLine(bottomMostPoint, rightMostPoint);
                var line4 = GetLine(bottomMostPoint, leftMostPoint);

                lines.Add(line1);
                lines.Add(line2);
                lines.Add(line3);
                lines.Add(line4);
            }

            var linesIntersectionPoints = GetIntersectPoints(lines)
                .Where(x => x.Item1>= -1 && x.Item1 <=  maxCoordinateValue+1 && x.Item2 >= -1 && x.Item2 <= maxCoordinateValue+1)
                .OrderByDescending(x => x.Item1)
                .ThenByDescending(x=>x.Item2)
                .ToList();
            var notOccupied = new List<(int, int)>();
            for (int i = 0; i < linesIntersectionPoints.Count; i++)
            {
                (int, int) intersecPoint = linesIntersectionPoints[i];
                var (x, y) = intersecPoint;
                var intersetingPoints = new[] { (-1, 0), (1, 0), (0, -1), (0, 1) };
                foreach (var (iX, iY) in intersetingPoints)
                {
                    var pointX = x + iX;
                    var pointY = y + iY;
                    if (pointX >= 0 
                        && pointY >= 0 
                        && pointX <= maxCoordinateValue 
                        && pointY <= maxCoordinateValue 
                        && !IsOccupied(pointY, pointX, sensorToDistance))
                    {
                        Console.WriteLine((pointX, pointY));
                        notOccupied.Add((pointX, pointY));
                        
                        Console.WriteLine(sw.ElapsedMilliseconds);
                        Console.WriteLine((long)pointX * 4000000L + (long)pointY);
                        return;
                    }
                }
            }
        }
    }

    private List<(int,int)> GetIntersectPoints(
        List<Line> lines)
    {
        var result = new List<(int, int)>();

        for (int i = 0; i < lines.Count; i++)
        {
            var lineLeft = lines[i];
            for (int j = i+1; j < lines.Count; j++)
            {
                var lineRight = lines[j];
                if (lineLeft.A != lineRight.A)
                {
                    var x = (lineRight.B - lineLeft.B) / (lineLeft.A - lineRight.A);
                    var y = lineLeft.A * x + lineLeft.B;
    
                    result.Add((x, y));
                }
            }
        }

        return result;
    }

    private Line GetLine(
        (int, int) firstPointOnLine,
        (int, int) secondPointOnLine)
    {
        var (firstPointX, firstPointY) = firstPointOnLine;
        var (secondPointX, secondPointY) = secondPointOnLine;

        var a = (firstPointY - secondPointY) / (firstPointX - secondPointX);
        var b = secondPointY - a * secondPointX;

        return new Line(a, b);
    }

    private static bool IsOccupied(
        int row,
        long col,
        Dictionary<(int, int), int> sensorToDistance)
    {
        foreach (var (sensorPosition, distanceToClosestBeacon) in sensorToDistance)
        {
            var (senosorX, sensorY) = sensorPosition;

            var horizontalDistanceFromSensor = Math.Abs(senosorX - col);
            if (horizontalDistanceFromSensor <= distanceToClosestBeacon)
            {
                var distanceFromSensor = distanceToClosestBeacon - horizontalDistanceFromSensor + 1;
                var numberOfOccupiedPlaces = 2 * distanceFromSensor - 1;

                if (Math.Abs(sensorY - row) <= numberOfOccupiedPlaces / 2)
                {
                    return true;
                }
            }
        }
        return false;
    }
}