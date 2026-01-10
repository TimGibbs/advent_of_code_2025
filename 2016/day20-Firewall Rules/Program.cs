// @algorithm Range Merging / Gap Counting
// @category Interval Processing / Forbidden IPs
// @input List of ranges in "low-high" format
// @state Highest allowed IP seen, total allowed count
// @execution-model Sequential scan after sorting
// @technique
//   Part1: Find first gap by keeping track of max endpoint
//   Part2: Sum sizes of all gaps between merged ranges
// @data-structures
//   (uint low, uint high)[] for ranges
//   ulong for counting allowed IPs
// @complexity
//   Time: O(N log N) for sorting ranges
//   Space: O(N)


await Part1();
await Part2();
return;

async Task Part1()
{
    var lines = await File.ReadAllLinesAsync("input.txt");
    var highest = 0u;
    var ranges = lines.Select(line =>
    {
        var split = line.Split('-');
        var low = uint.Parse(split[0]);
        var high = uint.Parse(split[1]);
        return (low, high);
    }).OrderBy(x=>x.low).ToArray();

    foreach (var (low, high) in ranges)
    {
        if (low > highest)
        {
            Console.WriteLine(highest+1);
            return;
        }

        highest = Math.Max(highest, high);
    }
}

async Task Part2()
{
    var lines = await File.ReadAllLinesAsync("input.txt");

    var ranges = lines.Select(line =>
        {
            var split = line.Split('-');
            var low = uint.Parse(split[0]);
            var high = uint.Parse(split[1]);
            return (low, high);
        })
        .OrderBy(r => r.low)
        .ToArray();

    ulong allowed = 0;
    uint highest = 0;
    bool first = true;

    foreach (var (low, high) in ranges)
    {
        if (first)
        {
            if (low > 0)
                allowed += low;
            highest = high;
            first = false;
            continue;
        }
        
        if (low > (ulong)highest + 1)
        {
            allowed += low - ((ulong)highest + 1);
        }

        if (high > highest)
            highest = high;
    }
    if (highest < uint.MaxValue)
        allowed += uint.MaxValue - (ulong)highest;
    

    Console.WriteLine(allowed);
}



