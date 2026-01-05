// @algorithm Modular Simulation / Brute Force Search
// @category Timing / Synchronization
// @input Disc descriptions (positions and starting offsets)
// @state Time counter, disc position arrays
// @execution-model Discrete time step simulation
// @technique
//   Encode valid alignment as boolean arrays
//   Test successive time values until all constraints align
// @features
//   Uses modular arithmetic for disc rotation
//   LINQ-based all-disc validation per timestep
// @variant
//   Part1: Use given discs only
//   Part2: Append extra disc with 11 positions
// @data-structures
//   bool[] per disc to mark valid alignment
//   bool[][] for all discs
// @complexity
//   Time: O(T * D) where T is time searched, D is number of discs
//   Space: O(sum of disc sizes)


await Part1();
await Part2();
return;

async Task Part1()
{
    var lines = await File.ReadAllLinesAsync("input.txt");
    var discs = lines.Select(x =>
    {
        var split = x.Split(' ');
        var length = int.Parse(split[3]);
        var disc = new bool[length];
        var startingPosition = int.Parse(split[11].TrimEnd('.'));
        disc[(length - startingPosition) % length] = true;
        return disc;
    }).ToArray();

    var i = 0;
    while (true)
    {
        i++;
        var success = discs.Index().All(x => x.Item[(x.Index + 1 + i) % x.Item.Length]);
        if (success)
        {
            Console.WriteLine(i);
            break;
        }
    }

}

async Task Part2()
{
    var lines = await File.ReadAllLinesAsync("input.txt");
    var discs = lines.Select(x =>
    {
        var split = x.Split(' ');
        var length = int.Parse(split[3]);
        var disc = new bool[length];
        var startingPosition = int.Parse(split[11].TrimEnd('.'));
        disc[(length - startingPosition) % length] = true;
        return disc;
    }).ToArray();

    var extra = new bool[11];
    extra[0] = true;
    discs = discs.Append(extra).ToArray();
    var i = 0;
    while (true)
    {
        i++;
        var success = discs.Index().All(x => x.Item[(x.Index + 1 + i) % x.Item.Length]);
        if (success)
        {
            Console.WriteLine(i);
            break;
        }
    }

}