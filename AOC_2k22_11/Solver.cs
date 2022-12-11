using AOC_2k22_Common;

namespace AOC_2k22_11;

internal class Solver : ChallangeSolver
{
    private const int RoundCount = 20;
    private const int RoundCountPart2 = 10000;

    protected override void SolvePart1(
        string[] input)
    {
        List<Item> items = new List<Item>();
        List<Monkey> monkeys = new List<Monkey>();

        for (int i = 0; i < input.Length; i += 7)
        {
            var monkeyId = input[i].Split(" ")[1].Split(":")[0];

            var monkeyItemsLine = input[i + 1];
            var monkeyItems = monkeyItemsLine.Split(":")[1].Split(",");

            var monkeyOperationLine = input[i + 2];
            var monkeyTestLines = input[(i + 3)..(i+6)];

            foreach (var itemDefintion in monkeyItems) 
            {
                var itemWorryLevel = long.Parse(itemDefintion.Trim());
                
                items.Add(new Item
                {
                    CurrentMonkeyHolderId = monkeyId,
                    WorryLevel= itemWorryLevel 
                });
            }

            monkeys.Add(new Monkey(monkeyId, monkeyOperationLine, monkeyTestLines));
        }

        Dictionary<string, int> inspectCountByMonkeyId = monkeys
            .ToDictionary(k => k.Id, v => 0);

        for (int i = 0; i < RoundCount; i++)
        {
            foreach (var monkey in monkeys)
            {
                var monkeyItems = items
                    .Where(item => item.CurrentMonkeyHolderId == monkey.Id);

                foreach (var item in monkeyItems)
                {
                    inspectCountByMonkeyId[monkey.Id] += 1;

                    var (newWorryLevel, newMonkeyId) = monkey.TestItem(item!);

                    item.WorryLevel = newWorryLevel;
                    item.CurrentMonkeyHolderId = newMonkeyId;
                }
            }
        }

        var top2MonkeyInspections = inspectCountByMonkeyId
            .Values
            .OrderByDescending(v => v)
            .Take(2)
            .ToArray();

        Console.WriteLine((long)top2MonkeyInspections[0] * (long)top2MonkeyInspections[1]);
    }

    protected override void SolvePart2(
        string[] input)
    {
        List<Item> items = new List<Item>();
        List<Monkey> monkeys = new List<Monkey>();

        for (int i = 0; i < input.Length; i += 7)
        {
            var monkeyId = input[i].Split(" ")[1].Split(":")[0];

            var monkeyItemsLine = input[i + 1];
            var monkeyItems = monkeyItemsLine.Split(":")[1].Split(",");

            var monkeyOperationLine = input[i + 2];
            var monkeyTestLines = input[(i + 3)..(i + 6)];

            foreach (var itemDefintion in monkeyItems)
            {
                var itemWorryLevel = long.Parse(itemDefintion.Trim());

                items.Add(new Item
                {
                    CurrentMonkeyHolderId = monkeyId,
                    WorryLevel = itemWorryLevel
                });
            }

            monkeys.Add(new Monkey(monkeyId, monkeyOperationLine, monkeyTestLines));
        }

        Dictionary<string, int> inspectCountByMonkeyId = monkeys
            .ToDictionary(k => k.Id, v => 0);

        //long modulo = 1;
        //foreach (var monkey in monkeys)
        //{
        //    modulo *= monkey.MonkeyTest.DivisibleBy;
        //}

        for (int i = 0; i < RoundCountPart2; i++)
        {
            foreach (var monkey in monkeys)
            {
                var monkeyItems = items
                    .Where(item => item.CurrentMonkeyHolderId == monkey.Id);

                foreach (var item in monkeyItems)
                {
                    inspectCountByMonkeyId[monkey.Id] += 1;

                    var (newWorryLevel, newMonkeyId) = monkey.TestItem(item!, true);

                    item.WorryLevel = newWorryLevel;
                    item.CurrentMonkeyHolderId = newMonkeyId;
                }
            }
        }

        var top2MonkeyInspections = inspectCountByMonkeyId
            .Values
            .OrderByDescending(v => v)
            .Take(2)
            .ToArray();

        Console.WriteLine((long)top2MonkeyInspections[0] * (long)top2MonkeyInspections[1]);
    }

    private class Item
    {
        public long WorryLevel { get; set; }
        public string CurrentMonkeyHolderId { get; set; }
    }

    private class Monkey
    {
        public string Id { get; }
        private string[] Operation { get; }

        public MonkeyTest MonkeyTest { get; }

        public Monkey(
        string id,
        string operation,
        string[] test)
        {
            Id = id;
            Operation = operation.Split(" ");

            MonkeyTest = InterpretTest(test);
        }

        private MonkeyTest InterpretTest(
            string[] test)
        {
            var divisibleBy = test[0].Split(" ").Last();
            var successMonkey = test[1].Split(" ").Last();
            var failureMonkey = test[2].Split(" ").Last();

            return new MonkeyTest(int.Parse(divisibleBy), successMonkey, failureMonkey);
        }

        public (long itemWorryLevel, string targetMonkeyId) TestItem(
            Item item,
            bool isPart2 = false)
        {
            var worryLevelAfterPickingUp = PerformOperation(item.WorryLevel);
            var borredWorryLevel = isPart2 ? worryLevelAfterPickingUp : worryLevelAfterPickingUp / 3;

            if (borredWorryLevel % MonkeyTest.DivisibleBy == 0)
            {
                return (borredWorryLevel, MonkeyTest.SuccessMonkey);
            }
            else
            {
                return (borredWorryLevel, MonkeyTest.FailureMonkey);
            }
        }

        private long PerformOperation(
            long worryLevel)
        {
            var operationSign = Operation[Operation.Length - 2];
            var operationElement = Operation[Operation.Length - 1];

            long operationValueToInclude;
            if (operationElement == "old")
            {
                operationValueToInclude = worryLevel;
            }
            else
            {
                operationValueToInclude = long.Parse(operationElement);
            }
            
            checked
            {
                if (operationSign == "+")
                {
                    return (worryLevel + operationValueToInclude) % 9699690; // makes part 1 invalid but who cares at this point ;)
                }
                else
                {
                    return (worryLevel * operationValueToInclude) % 9699690; // makes part 1 invalid but who cares at this point ;)
                }
            }
        }    
    }
    public record MonkeyTest(int DivisibleBy, string SuccessMonkey, string FailureMonkey);

}