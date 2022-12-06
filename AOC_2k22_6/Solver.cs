using AOC_2k22_Common;

namespace AOC_2k22_6;

internal class Solver : ChallangeSolver
{
    private const int BufferIdentifierLengthForPart1 = 4;
    private const int BufferIdentifierLengthForPart2 = 14;

    protected override void SolvePart1(
        string[] input)
    {
        var buffer = input[0];

        var initialItems = new char[BufferIdentifierLengthForPart1];

        for (var i = 0; i < BufferIdentifierLengthForPart1; i++)
        {
            initialItems[i] = buffer[i];
        }

        var bufferIdentifer = new ExactSizeCollection<char>(BufferIdentifierLengthForPart1, initialItems);

        for (var i = BufferIdentifierLengthForPart1; i < buffer.Length; i++)
        {
            if (bufferIdentifer.AreAllItemsUnique())
            {
                Console.WriteLine(i);
                return;
            }

            bufferIdentifer.AddItem(buffer[i]);
        }

        if (bufferIdentifer.AreAllItemsUnique())
        {
            Console.WriteLine(buffer.Length);
            return;
        }
    }

    protected override void SolvePart2(
        string[] input)
    {
        var buffer = input[0];

        var initialItems = new char[BufferIdentifierLengthForPart2];

        for (var i = 0; i < BufferIdentifierLengthForPart2; i++)
        {
            initialItems[i] = buffer[i];
        }

        var bufferIdentifer = new ExactSizeCollection<char>(BufferIdentifierLengthForPart2, initialItems);

        for (var i = BufferIdentifierLengthForPart2; i < buffer.Length; i++)
        {
            if (bufferIdentifer.AreAllItemsUnique())
            {
                Console.WriteLine(i);
                return;
            }

            bufferIdentifer.AddItem(buffer[i]);
        }

        if (bufferIdentifer.AreAllItemsUnique())
        {
            Console.WriteLine(buffer.Length);
            return;
        }
    }

    private class ExactSizeCollection<T>
    {
        private readonly int _size;
        private readonly T[] _items;
        private int _currentHead;

        public ExactSizeCollection(
            int size,
            ICollection<T> initialItems)
        {
            if (initialItems.Count != size)
            {
                throw new ArgumentException($"Item count: {initialItems.Count} doesn't match requested size: {size}");
            }

            _size = size;
            _items = initialItems.ToArray();
            _currentHead = 0;
        }

        public void AddItem(T item)
        {
            _items[_currentHead] = item;
            _currentHead = (_currentHead + 1) % _size;
        }

        public bool AreAllItemsUnique()
        {
            return new HashSet<T>(_items).Count == _size;
        }
    }
}
