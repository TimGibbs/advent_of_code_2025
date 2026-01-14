// @algorithm Tower Balance Resolution
// @category Tree Analysis / Recursive Aggregation
// @input List of discs with weights and child references
// @problem
//   Given a tower of discs where each disc may hold others:
//   - Identify the bottom (root) disc
//   - Find the incorrect disc weight causing imbalance
// @model
//   Directed tree (parent -> held children)
// @parts
//   Part1: Find the root disc (never held by another)
//   Part2: Detect imbalance and compute corrected weight
// @technique
//   - Name-based indexing (Dictionary lookup)
//   - Depth-first recursion
//   - Grouping child subtree weights to detect outliers
// @data-structures
//   Disc record (Name, Weight, Held[])
//   Dictionary<string, Disc>
// @complexity
//   Time: O(n)
//   Space: O(n)
// @notes
//   - First imbalance encountered (top-down) yields the correction
//   - Corrected weight = original weight + (expected - actual subtree weight)

await Part1();
await Part2();
return;

async Task Part1()
{
    var lines = await File.ReadAllLinesAsync("input.txt");
    var discs = lines.Select(x =>
    {
        var split = x.Split(' ');
        var name = split[0];
        var weight = int.Parse(split[1][1..^1]);
        var held = split.Length > 2 ? split[3..].Select(s => s.TrimEnd(',')).ToArray() : [];
        return new Disc()
        {
            Name = name,
            Weight = weight,
            Held = held
        };
    }).ToArray();

    var bottom = discs.Select(s => s.Name).Single(x => !discs.SelectMany(s => s.Held).Contains(x));
    Console.WriteLine(bottom);
}
async Task Part2()
{
    var lines = await File.ReadAllLinesAsync("input.txt");
    var discs = lines.Select(x =>
    {
        var split = x.Split(' ');
        var name = split[0];
        var weight = int.Parse(split[1][1..^1]);
        var held = split.Length > 2 ? split[3..].Select(s => s.TrimEnd(',')).ToArray() : [];
        return new Disc()
        {
            Name = name,
            Weight = weight,
            Held = held,
        };
    }).ToArray();
    var bottom = discs.Select(s => s.Name).Single(x => !discs.SelectMany(s => s.Held).Contains(x));
    var discByName = discs.ToDictionary(d => d.Name);

    int? correctedWeight = null;
    TotalWeight(bottom);
    Console.WriteLine(correctedWeight);
    return;

    int TotalWeight(string name)
    {
        var disc = discByName[name];
        
        if (disc.Held.Length == 0)
            return disc.Weight;
        
        var childWeights = disc.Held
            .Select(child => (child, weight: TotalWeight(child)))
            .ToArray();
        
        var groups = childWeights
            .GroupBy(c => c.weight)
            .ToArray();
        
        if (groups.Length > 1 && correctedWeight == null)
        {
            var correctGroup = groups.Single(g => g.Count() > 1);
            var wrongGroup = groups.Single(g => g.Count() == 1);

            var wrongChild = wrongGroup.Single();
            var diff = correctGroup.Key - wrongGroup.Key;

            correctedWeight = discByName[wrongChild.child].Weight + diff;
        }

        return disc.Weight + childWeights.Sum(c => c.weight);
    }

    
}


record struct Disc
{
    public string Name;
    public int Weight;
    public string[] Held;
}