using AOC_2k22_Common;

namespace AOC_2k22_5;

internal class Solver : ChallangeSolver
{
    private static readonly List<Stack<string>> _crates = new List<Stack<string>>()
    {
        new Stack<string>("D H N Q T W V B".Split(' ')),
        new Stack<string>("D W B".Split(' ')),
        new Stack<string>("T S Q W J C".Split(' ')),
        new Stack<string>("F J R N Z T P".Split(' ')),
        new Stack<string>("G P V J M S T".Split(' ')),
        new Stack<string>("B W F T N".Split(' ')),
        new Stack<string>("B L D Q F H V N".Split(' ')),
        new Stack<string>("H P F R".Split(' ')),
        new Stack<string>("Z S M B L N P H".Split(' ')),
    };

    protected override void SolvePart1(
        string[] input)
    {
        foreach (var instruction in input)
        {
            var moveInstruction = ConvertToMoveInstruction(instruction);

            ExecuteInstruction(moveInstruction);
        }

        foreach (var crateStack in _crates)
        {
            Console.Write(crateStack.Peek());
        }

        Console.WriteLine();
    }    

    protected override void SolvePart2(
        string[] input)
    {
        foreach (var instruction in input)
        {
            var moveInstruction = ConvertToMoveInstruction(instruction);

            ExecuteInstructionBatchedWay(moveInstruction);
        }

        foreach (var crateStack in _crates)
        {
            Console.Write(crateStack.Peek());
        }

        Console.WriteLine();
    }

    private void ExecuteInstruction(
        MoveInstruction moveInstruction)
    {
        for (var i = 0; i < moveInstruction.Quantity; i++)
        {
            var elementToMove = _crates[moveInstruction.Source].Pop();

            _crates[moveInstruction.Target].Push(elementToMove);
        }
    }

    private void ExecuteInstructionBatchedWay(
    MoveInstruction moveInstruction)
    {
        var intermediaryStack = new Stack<string>();
        
        for (var i = 0; i < moveInstruction.Quantity; i++)
        {
            var elementToMove = _crates[moveInstruction.Source].Pop();

            intermediaryStack.Push(elementToMove);
        }

        while(intermediaryStack.Count > 0)
        {
            var elementToMove = intermediaryStack.Pop();

            _crates[moveInstruction.Target].Push(elementToMove);
        }
    }

    private MoveInstruction ConvertToMoveInstruction(
        string instruction)
    {
        var instructionSplitted = instruction.Split(' ');

        return new MoveInstruction(
            int.Parse(instructionSplitted[3]) - 1,
            int.Parse(instructionSplitted[5]) - 1,
            int.Parse(instructionSplitted[1]));
    }

    private record MoveInstruction(
        int Source,
        int Target,
        int Quantity);
}
