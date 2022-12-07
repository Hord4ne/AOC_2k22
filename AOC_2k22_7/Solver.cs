using AOC_2k22_Common;

namespace AOC_2k22_7;

internal class Solver : ChallangeSolver
{
    private const int ThresholdSize = 100000;
    
    private const int DiskSize = 70000000;
    private const int UpdateSize = 30000000;

    protected override void SolvePart1(
        string[] input)
    {
        var root = new DirectoryNode("/", null);

        ParseInput(input, root);

        Console.WriteLine(GetSizesOfDirectories(root).Where(s => s <= ThresholdSize).Sum());
    }

    protected override void SolvePart2(
        string[] input)
    {
        var root = new DirectoryNode("/", null);

        ParseInput(input, root);
        
        Console.WriteLine(FindPerfectDirectoryToDeletete(root, GetSizesOfDirectories(root)));
    }

    private void ParseInput(
        string[] input,
        DirectoryNode root)
    {
        var current = root;

        foreach (var line in input.Skip(1))
        {
            if (CdCommand.IsCdCommand(line))
            {
                var cdCommand = InterpretAsCdCommand(line);
                current = DetermineCurrentNode(current, cdCommand);
                continue;
            }

            if (LsCommand.IsLsCommand(line))
            {
                continue;
            }

            var splittedLine = line.Split(' ');

            if (splittedLine[0] == "dir")
            {
                current.AddChildDirectoryIfNotExist(splittedLine[1]);
                continue;
            }

            // otherwise it has to be file line
            current.AddChildFile(splittedLine[1], int.Parse(splittedLine[0]));
        }
    }

    private ICollection<int> GetSizesOfDirectories(
        DirectoryNode root)
    {
        var nodesSizes = new List<int>();

        TraverseThroughDirectoriesAndCalculateSizes(root, nodesSizes);

        return nodesSizes;
    }

    private int FindPerfectDirectoryToDeletete(
        DirectoryNode root,
        ICollection<int> directoriesSizes)
    {
        var freeSpaceOnDisk = DiskSize - root.CalculateSize();
        var neededSpace = UpdateSize - freeSpaceOnDisk;

        if (neededSpace < 0)
        {
            return 0;
        }
        else
        {
            var closestDifference = DiskSize;
            var directorySizeClosestToPerfectSize = DiskSize;

            foreach (var directorySize in directoriesSizes)
            {
                if (directorySize >= neededSpace
                    && closestDifference > directorySize - neededSpace)
                {
                    closestDifference = directorySize - neededSpace;
                    directorySizeClosestToPerfectSize = directorySize;
                }
            }

            return directorySizeClosestToPerfectSize;
        }
    }

    private void TraverseThroughDirectoriesAndCalculateSizes(
        DirectoryNode node,
        List<int> nodesSizes)
    {
        nodesSizes.Add(node.CalculateSize());

        foreach (var child in node.Children)
        {
            if (child.Value is DirectoryNode childDirectoryNode)
            {
                TraverseThroughDirectoriesAndCalculateSizes(childDirectoryNode, nodesSizes);
            }
        }
    }

    private DirectoryNode DetermineCurrentNode(
        DirectoryNode current,
        CdCommand cdCommand)
    {
        if (cdCommand.IsGoingUp)
        {
            return current.Parent ?? current;
        }
        else
        {
            return (DirectoryNode)current.Children[cdCommand.Path];
        }
    }

    private CdCommand InterpretAsCdCommand(
        string line)
    {
        return new CdCommand(line.Split(' ')[2]);
    }

    public abstract class Node
    {
        public string Id { get; }

        protected Node(
            string id)
        {
            ArgumentNullException.ThrowIfNull(id);

            Id = id;
        }

        public abstract int CalculateSize();
    }

    private class DirectoryNode : Node
    {
        private int? _lastSizeRequestResult;

        public Dictionary<string, Node> Children { get; } = new Dictionary<string, Node>();
        public DirectoryNode? Parent { get; }

        public DirectoryNode(
            string id,
            DirectoryNode? parentNode) : base(id)
        {
            Parent = parentNode;
        }

        private void AddChild(string id, Node node)
        {
            _lastSizeRequestResult = null;
            Children.Add(id, node);
        }

        public void AddChildDirectoryIfNotExist(
            string directoryId)
        {
            if (!Children.ContainsKey(directoryId))
            {
                var child = new DirectoryNode(directoryId, this);
                AddChild(directoryId, child);
            }
        }

        public void AddChildFile(
            string fileId,
            int size)
        {
            AddChild(fileId, new FileNode(fileId, size));
        }

        public override int CalculateSize()
        {
            if (_lastSizeRequestResult is not null)
            {
                return _lastSizeRequestResult.Value;
            }

            var size = 0;
            
            foreach (var child in Children.Values)
            {
                size += child.CalculateSize();
            }

            _lastSizeRequestResult = size;
            return _lastSizeRequestResult.Value;
        }
    }

    private class FileNode : Node
    {
        public int Size { get; }

        public FileNode(
            string id,
            int size) : base(id)
        {
            Size = size;
        }

        public override int CalculateSize() => Size;
    }

    private class CdCommand
    {
        private const string GoingUpPath = "..";

        public string Path { get; }
        public bool IsGoingUp => Path == GoingUpPath;

        public CdCommand(
            string path)
        {
            Path = path;
        }

        public static bool IsCdCommand(
            string command) => command.StartsWith("$ cd");
    }

    private class LsCommand
    {
        public static bool IsLsCommand(
            string command) => command.StartsWith("$ ls");
    }
}
