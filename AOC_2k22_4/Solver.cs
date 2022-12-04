using AOC_2k22_0;

namespace AOC_2k22_4;

internal class Solver : ChallangeSolver
{
    protected override void SolvePart1(
        string[] input)
    {
        var assignmentsForReconsideration = 0;

        foreach (var assignmentsRangePair in input)
        {
            var (firstAssignmentRange, secondAssignmentRange) = DetermineAssignmentRanges(assignmentsRangePair);

            var firstRange = new HashSet<int>(Enumerable.Range(firstAssignmentRange.Start, firstAssignmentRange.End - firstAssignmentRange.Start + 1));
            var secondRange = new HashSet<int>(Enumerable.Range(secondAssignmentRange.Start, secondAssignmentRange.End - secondAssignmentRange.Start + 1));

            var countNeededToContainOneAnother = Math.Min(firstRange.Count, secondRange.Count);

            var intersectCount = firstRange.Intersect(secondRange).Count();

            if (intersectCount == countNeededToContainOneAnother)
            {
                assignmentsForReconsideration += 1;
            }
        }

        Console.WriteLine(assignmentsForReconsideration);
    }

    protected override void SolvePart2(
        string[] input)
    {
        var assignmentsForReconsideration = 0;

        foreach (var assignmentsRangePair in input)
        {
            var (firstAssignmentRange, secondAssignmentRange) = DetermineAssignmentRanges(assignmentsRangePair);

            var firstRange = new HashSet<int>(Enumerable.Range(firstAssignmentRange.Start, firstAssignmentRange.End - firstAssignmentRange.Start + 1));
            var secondRange = new HashSet<int>(Enumerable.Range(secondAssignmentRange.Start, secondAssignmentRange.End - secondAssignmentRange.Start + 1));

            var intersectCount = firstRange.Intersect(secondRange).Count();

            if (intersectCount > 0)
            {
                assignmentsForReconsideration += 1;
            }
        }

        Console.WriteLine(assignmentsForReconsideration);
    }

    private (AssignmentRange firstAssignmentRange, AssignmentRange secondAssignmentRange) DetermineAssignmentRanges(
        string assignmentsRangePair)
    {
        var splittedAssignmentsRangePair = assignmentsRangePair.Split(',');
        return (
            CreateAssignmentRange(splittedAssignmentsRangePair[0]),
            CreateAssignmentRange(splittedAssignmentsRangePair[1]));
    }

    private AssignmentRange CreateAssignmentRange(
        string spilttedAssignmentPart)
    {
        var rangesSeparated = spilttedAssignmentPart.Split("-");
        return new AssignmentRange(
            int.Parse(rangesSeparated[0]),
            int.Parse(rangesSeparated[1]));
    }

    private record AssignmentRange(int Start, int End);
}
