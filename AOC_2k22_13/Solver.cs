using AOC_2k22_Common;
using System.Collections;

namespace AOC_2k22_13;

internal class Solver : ChallangeSolver
{
    protected override void SolvePart1(
        string[] input)
    {
        var goodPackages = new List<int>();

        for (int i = 0; i < input.Length; i += 3)
        {
            string leftPacket = input[i];
            string rightPacket = input[i+1];

            var leftPacketAsList = FillPacketList(leftPacket);
            var rightPacketAsList = FillPacketList(rightPacket);

            if (IsInCorrectOrder(leftPacketAsList, rightPacketAsList) == Result.GoodOrder)
            {
                var currentPairIndex = i / 3 + 1;
                if (currentPairIndex == 18)
                {
                    Console.WriteLine("fd");
                }
                Console.WriteLine(currentPairIndex);

                goodPackages.Add(currentPairIndex);
            }
        }

        Console.WriteLine(goodPackages.Sum());
    }

    private static Result IsInCorrectOrder(
        object left,
        object right)
    {
        if (left is List<object> leftCollection
            && right is List<object> rightCollection)
        {            
            for (int i = 0; i < leftCollection.Count; i++)
            {
                object item = leftCollection[i];

                if (rightCollection.Count <= i)
                {
                    return Result.WrongOrder;
                }

                var orderResult = IsInCorrectOrder(item, rightCollection[i]);

                if (orderResult == Result.GoodOrder
                    || orderResult == Result.WrongOrder)
                {
                    return orderResult;
                }             
            }
            
            if (leftCollection.Count == rightCollection.Count)
            {
                return Result.Dunno;
            }

            return Result.GoodOrder;
        }
        else if (left is int leftInt 
            && right is int rightInt)
        {
            if (leftInt == rightInt)
            {
                return Result.Dunno;
            }
            if (leftInt > rightInt)
            {
                return Result.WrongOrder;
            }
            else
            {
                return Result.GoodOrder;
            }
        }
        else if (left is List<object>)
        {   
            return IsInCorrectOrder(left, new List<object> { right });
        }
        else if (right is List<object>)
        {
            return IsInCorrectOrder(new List<object> { left }, right);
        }
        else
        {
            throw new NotSupportedException("I wasn't expecting this");
        }
    }

    private enum Result
    {
        GoodOrder = 1,
        WrongOrder = 2,
        Dunno = 3
    }

    private List<object> FillPacketList(
        string packet)
    {
        var stack = new Stack<object>();
        for (int i = 0; i < packet.Length; i++)
        {
            var character = packet[i].ToString();
            if (character == ",")
            {
                continue;
            }

            if (character == "]")
            {
                var currList = new List<object>();
                object topFromTheStack;

                while (!string.Equals(topFromTheStack = stack.Pop(), "["))
                {
                    currList.Insert(0, topFromTheStack);
                }

                stack.Push(currList);
            }
            else if (character == "[")
            {
                stack.Push(character);
            }
            else
            {
                if (char.IsDigit(packet[i]))
                {
                    int j = 1;
                    while (true)
                    {
                        if (!char.IsDigit(packet[i + j]))
                        {
                            break;
                        }

                        j++;
                    }
                    stack.Push(int.Parse(packet.Substring(i, j)));
                }
            }
        }

        return (List<object>)stack.Pop();

    }

    protected override void SolvePart2(
        string[] input)
    {
        List<object> allPackets = new List<object>();
        List<object> packetToFind2 = null;
        List<object> packetToFind6 = null;
        for (int i = 0; i < input.Length; i += 3)
        {
            string leftPacket = input[i];
            string rightPacket = input[i + 1];

            var leftPacketAsList = FillPacketList(leftPacket);
            var rightPacketAsList = FillPacketList(rightPacket);

            if (i + 3 >= input.Length)
            {
                packetToFind2 = leftPacketAsList;
                packetToFind6 = rightPacketAsList;
            }

            allPackets.Add(leftPacketAsList);
            allPackets.Add(rightPacketAsList);
        }
        var comparer = new PacketComparer();
        
        var ordered = allPackets
            .OrderByDescending(x => x, comparer)
            .ToList();

        var indexOfTPacket2 = ordered.IndexOf(packetToFind2!) + 1;
        var indexOfTPacket6 = ordered.IndexOf(packetToFind6!) + 1;

        Console.WriteLine(indexOfTPacket2 * indexOfTPacket6);
    }

    public class PacketComparer : IComparer<object>
    {
        public int Compare(
            object? x,
            object? y)
        {
            var compareResult = IsInCorrectOrder(x!, y!);
            
            if (compareResult == Result.GoodOrder)
            {
                return 1;
            }
            else if (compareResult == Result.Dunno)
            {
                return 0;
            }

            return -1;
        }
    }
}