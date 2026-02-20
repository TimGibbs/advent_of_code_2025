// @algorithm Exhaustive Bridge Construction (Magnetic Components)
// @category Backtracking / Combinatorial Search / Graph Path Enumeration
// @input List of components "A/B" representing undirected ports
// @goal Build bridges starting from port 0 without reusing components
// @representation
//   - Component as (A, B) with strength A + B
//   - Bridge as HashSet<Component> (set of used pieces)
// @connection-rule
//   A component can attach if either port matches the current open port
//   After attachment, the open port becomes the component’s other side
// @start-condition Open port = 0
// @no-reuse Each component may be used at most once per bridge
// @search-strategy Depth-first recursive backtracking
// @branching For each compatible unused component:
//   1. Add component to current bridge
//   2. Recurse with updated open port
//   3. Collect all resulting bridges
// @termination
//   If no compatible unused components remain → current bridge is complete
// @part1-output Maximum total strength among all bridges
//   Strength = sum of (A + B) over components in the bridge
// @part2-output Strength of the strongest bridge among the longest bridges
//   - First maximize bridge length (component count)
//   - Then maximize strength among those
// @data-structures
//   - Component[] for all pieces
//   - HashSet<Component> to track used pieces (fast membership)
//   - Array of HashSet<Component> to return all terminal bridges
// @component-properties
//   - Components are undirected (A/B same as B/A)
//   - Double-port components (A == B) keep same open port
// @complexity
//   Time: Exponential in number of components (worst-case explores all subsets)
//   Space: O(depth) recursion stack + storage of all terminal bridges
// @notes
//   - Produces every valid bridge, not just optimal ones
//   - Suitable because input size is small
//   - Copying HashSet ensures branch isolation during recursion
//   - Final selection performed via LINQ aggregation

await Part1();
return;

async Task Part1()
{
    var lines = await File.ReadAllLinesAsync("input.txt");
    var components = lines.Select(l =>
    {
        var split = l.Split('/');
        return new Component(int.Parse(split[0]), int.Parse(split[1]));
    }).ToArray();

    var used = new HashSet<Component>();
    var bridges = Step(0, used, components);
    var pt1 = bridges.Select(x => x.Sum(c => c.Strength)).Max();
    var pt2 = bridges.GroupBy(x=>x.Count).MaxBy(k=>k.Key)!
        .Select(x => x.Sum(c => c.Strength)).Max();
    Console.WriteLine(pt1);
    Console.WriteLine(pt2);
}


HashSet<Component>[] Step(int end, HashSet<Component> existing, Component[] components)
{
    var possibles = components
        .Where(x => !existing.Contains(x) && (x.A == end || x.B == end))
        .ToArray();
    if (!possibles.Any()) return [existing];
    return possibles.Select(x =>
        {
            var newEnd = x.A == x.B ? x.A
                : x.A == end ? x.B
                : x.A;
            var newExisting = new HashSet<Component>(existing) { x };
            return Step(newEnd, newExisting, components);
        }).SelectMany(x => x)
        .ToArray();
}


internal readonly record struct Component(int A, int B)
{
    public int Strength => A + B;
};