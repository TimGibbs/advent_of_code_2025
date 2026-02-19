// @algorithm Sporifica Virus Simulation
// @category Grid Traversal / Cellular Automaton
// @input Square grid of '.' (clean) and '#' (infected)
// @coordinate-system Integer grid with origin at top-left of input
// @initial-state Carrier starts at grid center facing North
// @representation
//   - Part1: HashSet<Point> storing infected nodes only
//   - Part2: Dictionary<Point, State> storing full node state
// @direction-model Four cardinal directions (N, E, S, W)
// @movement Move forward one step after state transition
// @part1-rules (2-state system)
//   If current node is Infected:
//     - Turn right
//     - Clean node (remove from set)
//   Else (Clean):
//     - Turn left
//     - Infect node (add to set)
//     - Increment infection counter
// @part1-iterations 10,000 bursts
// @part1-output Number of bursts that cause a node to become infected
// @part2-rules (4-state system)
//   State transitions cycle:
//     Clean → Weakened → Infected → Flagged → Clean
//   Actions per state:
//     Clean:
//       - Turn left
//       - Become Weakened
//     Weakened:
//       - No turn
//       - Become Infected
//       - Increment infection counter
//     Infected:
//       - Turn right
//       - Become Flagged
//     Flagged:
//       - Reverse direction
//       - Become Clean
// @part2-iterations 10,000,000 bursts
// @part2-output Number of bursts that cause infection (Weakened → Infected)
// @turn-operations
//   - TurnLeft: N→W→S→E→N
//   - TurnRight: N→E→S→W→N
//   - Reverse: N↔S, E↔W
// @data-structures
//   - HashSet for sparse binary grid (efficient membership checks)
//   - Dictionary for sparse multi-state grid with default Clean
//   - Immutable Point record struct for coordinates
// @grid-properties
//   - Conceptually infinite grid
//   - Nodes created lazily as visited
// @complexity
//   Time: O(iterations)
//   Space: O(number of visited nodes)
// @notes
//   - Sparse representation avoids allocating a large infinite grid
//   - Direction changes depend solely on current node state
//   - Position updates occur after state transition each burst

await Part1();
await Part2();
return;

async Task Part1()
{
    var lines = await File.ReadAllLinesAsync("input.txt");
    var infected = new HashSet<Point>();
    for (var y = 0; y < lines.Length; y++)
    {
        var line = lines[y];
        for (var x = 0; x < line.Length; x++)
        {
            var c = line[x];
            if (c is '#') infected.Add(new Point(x, y));
        }
    }
    var position = new Point(lines[0].Length / 2 , lines.Length / 2);
    var facing = Facing.North;
    var count = 0;
    for (var i = 0; i < 10000; i++)
    {
        
        if (infected.Contains(position))
        {
            facing = TurnRight(facing);
            infected.Remove(position);
        }
        else
        {
            count++;
            infected.Add(position);
            facing = TurnLeft(facing);
        }

        position = Move(position, facing);
    }
    Console.WriteLine(count);
    
}
async Task Part2()
{
    var lines = await File.ReadAllLinesAsync("input.txt");
    var infected = new Dictionary<Point, State>();
    for (var y = 0; y < lines.Length; y++)
    {
        var line = lines[y];
        for (var x = 0; x < line.Length; x++)
        {
            var c = line[x];
            infected[new Point(x, y)] = c is '#' ? State.Infected : State.Clean;
        }
    }
    var position = new Point(lines[0].Length / 2 , lines.Length / 2);
    var facing = Facing.North;
    var count = 0;
    for (var i = 0; i < 10000000; i++)
    {
        if (!infected.TryGetValue(position, out var state))
        {
            infected[position] = State.Clean;
            state = State.Clean;
        }
        
        switch (state)
        {
            case State.Clean:
                facing = TurnLeft(facing);
                infected[position] = State.Weakened;
                break;
            case State.Weakened:
                count++;
                infected[position] = State.Infected;
                break;
            case State.Infected:
                facing = TurnRight(facing);
                infected[position] = State.Flagged;
                break;
            case State.Flagged:
                facing = Reverse(facing);
                infected[position] = State.Clean;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        position = Move(position, facing);
    }
    Console.WriteLine(count);
}
Facing TurnRight(Facing current) => current switch
{
    Facing.North => Facing.East,
    Facing.East => Facing.South,
    Facing.South => Facing.West,
    Facing.West => Facing.North,
    _ => throw new ArgumentOutOfRangeException(nameof(current), current, null)
};
Facing TurnLeft(Facing current) => current switch
{
    Facing.North => Facing.West,
    Facing.East => Facing.North,
    Facing.South => Facing.East,
    Facing.West => Facing.South,
    _ => throw new ArgumentOutOfRangeException(nameof(current), current, null)
};
Facing Reverse(Facing current) => current switch
{
    Facing.North => Facing.South,
    Facing.East => Facing.West,
    Facing.South => Facing.North,
    Facing.West => Facing.East,
    _ => throw new ArgumentOutOfRangeException(nameof(current), current, null)
};

Point Move(Point point, Facing facing) => facing switch
{
    Facing.North => point with { Y = point.Y - 1 },
    Facing.East  => point with { X = point.X + 1 },
    Facing.South => point with { Y = point.Y + 1 },
    Facing.West  => point with { X = point.X - 1 },
    _ => throw new ArgumentOutOfRangeException(nameof(facing), facing, null)
};
enum Facing
{
    North,
    East,
    South,
    West,
}

internal readonly record struct Point(int X, int Y);
enum State
{
    Clean,
    Weakened,
    Infected,
    Flagged
}
