// @algorithm Spiral Memory
// @category Grid Geometry / Spatial Simulation
// @input Single integer N
// @problem
//   Work with values laid out in an outward square spiral
// @rules
//   Part1: Compute Manhattan distance from N to center
//   Part2: Fill spiral with neighbor-sums until value exceeds N
// @technique
//   - Ring math for direct distance calculation
//   - Incremental spiral walk with neighbor accumulation
// @features
//   - Closed-form solution for Part1 (no grid)
//   - Dynamic sparse grid using dictionary for Part2
// @variant
//   Part1: Analytical spiral ring distance
//   Part2: Constructive spiral sum
// @data-structures
//   Dictionary<(int x, int y), int>
// @complexity
//   Part1 Time: O(√N)
//   Part2 Time: O(K)
//   Space: O(K)
//   (K = number of spiral cells generated)


await Part1();
await Part2();
return;

async Task Part1()
{
    var line = await File.ReadAllTextAsync("input.txt");
    var val = int.Parse(line);
    var ring = 0;
    var size = 0;
    var max = 0;
    
    for (var i = 1; ; i++)
    {
        size = i * 2 + 1;
        max = size * size;

        if (max >= val)
        {
            ring = i;
            break;
        }
    }

    var sideLength = size - 1;
    var offset = (max - val) % sideLength;
    var distanceToMiddle = Math.Abs(offset - ring);

    Console.WriteLine(ring + distanceToMiddle);
}
async Task Part2()
{
    var line = await File.ReadAllTextAsync("input.txt");
    var target = int.Parse(line);

    var grid = new Dictionary<(int x, int y), int>
    {
        [(0, 0)] = 1
    };

    int x = 0, y = 0;
    var stepSize = 1;

    while (true)
    {
        // Right, Up
        for (var i = 0; i < stepSize; i++)
        {
            x++;
            if (WriteCell(x, y, grid, target)) return;
        }
        for (var i = 0; i < stepSize; i++)
        {
            y++;
            if (WriteCell(x, y, grid, target)) return;
        }

        stepSize++;

        // Left, Down
        for (var i = 0; i < stepSize; i++)
        {
            x--;
            if (WriteCell(x, y, grid, target)) return;
        }
        for (var i = 0; i < stepSize; i++)
        {
            y--;
            if (WriteCell(x, y, grid, target)) return;
        }

        stepSize++;
    }
}

bool WriteCell(
    int x,
    int y,
    Dictionary<(int x, int y), int> grid,
    int target)
{
    int sum = 0;

    for (int dx = -1; dx <= 1; dx++)
    for (int dy = -1; dy <= 1; dy++)
    {
        if (dx == 0 && dy == 0) continue;

        if (grid.TryGetValue((x + dx, y + dy), out var v))
            sum += v;
    }

    grid[(x, y)] = sum;

    if (sum > target)
    {
        Console.WriteLine(sum);
        return true;
    }

    return false;

}