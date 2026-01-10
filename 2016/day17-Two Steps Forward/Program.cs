// @algorithm State-Space Search with Hash-Gated Moves
// @category Pathfinding / Graph Search
// @input Passcode string
// @state Current path string (implicit position derived from path)
// @execution-model Incremental path expansion
// @technique
//   Breadth-first search over paths
//   MD5-based dynamic edge availability
// @features
//   Position derived by replaying moves
//   Door states determined by hash prefix
// @variant
//   Part1: Shortest path to target (early exit BFS)
//   Part2: Longest path reaching target (exhaustive BFS)
// @data-structures
//   PriorityQueue<string,int> for BFS by path length
//   Strings to represent paths
// @constraints
//   Grid size fixed at 4x4
// @complexity
//   Time: O(number of reachable paths)
//   Space: O(number of queued paths)

await Part1();
await Part2();
return;

async Task Part1()
{
    var line = await File.ReadAllTextAsync("input.txt");
    var target = (3u, 3u);
    var q = new PriorityQueue<string, int>();
    char[] directions = ['U', 'D', 'L', 'R'];
    q.Enqueue("",0);
    while (true)
    {
        var path = q.Dequeue();
        var position = Position(path);
        if(position.Item1 >3 || position.Item2 >3) continue;
        if (position == target)
        {
            Console.WriteLine(path);
            return;
        }
        var doors = Doors(line, path);
        var dirs = doors
            .Zip(directions)
            .Where(c => !IsClosed(c.First))
            .Select(c => c.Second)
            .ToArray();
        if(!dirs.Any()) continue;
        foreach (var dir in dirs)
        {
            q.Enqueue(path+dir, path.Length+1);
        }
    }
}

string Doors(ReadOnlySpan<char> input, ReadOnlySpan<char> path)
{
    var step = System.Text.Encoding.ASCII.GetBytes($"{input}{path}");
    var hash = System.Security.Cryptography.MD5.HashData(step);
    return Convert.ToHexString(hash)[..4];
}

bool IsClosed(char c) => char.IsNumber(c) || c == 'A';

(uint, uint) Position(string path)
{
    var i = (0u, 0u);
    foreach (var c in path)
    {
        i = c switch
        {
            'U' => (i.Item1, i.Item2 - 1),
            'D' => (i.Item1, i.Item2 + 1),
            'L' => (i.Item1 - 1, i.Item2),
            'R' => (i.Item1 + 1, i.Item2),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
    return i;
}

async Task Part2()
{
    var line = await File.ReadAllTextAsync("input.txt");
    var target = (3u, 3u);
    var q = new PriorityQueue<string, int>();
    char[] directions = ['U', 'D', 'L', 'R'];
    q.Enqueue("",0);
    var longest = 0;
    while (true)
    {
        if (!q.TryDequeue(out var path, out var _)) break;
        var position = Position(path);
        if(position.Item1 >3 || position.Item2 >3) continue;
        if (position == target)
        {
            longest = Math.Max(longest, path.Length);
            continue;
        }
        var doors = Doors(line, path);
        var dirs = doors
            .Zip(directions)
            .Where(c => !IsClosed(c.First))
            .Select(c => c.Second)
            .ToArray();
        if(!dirs.Any()) continue;
        foreach (var dir in dirs)
        {
            q.Enqueue(path+dir, path.Length+1);
        }
    }
    Console.WriteLine(longest);
}