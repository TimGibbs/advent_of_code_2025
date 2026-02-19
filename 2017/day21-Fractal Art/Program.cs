// @algorithm Fractal Art Enhancement
// @category Grid Transformation / Cellular Expansion
// @input Enhancement rules mapping small patterns to larger patterns
// @representation Patterns as flattened square boolean arrays (# = on, . = off)
// @initial-state ".#./..#/###" (3×3 grid)
// @iterations 18
// @part1-output Pixels on after 5 iterations
// @part2-output Pixels on after 18 iterations
// @rule-matching
//   - Match using all rotations and horizontal flips of input patterns
//   - Precompute all symmetric permutations for each rule
// @symmetry-group Dihedral group D4 (4 rotations × 2 reflections)
// @grid-process-per-iteration
//   1. Split grid into blocks:
//        - If size divisible by 2 → 2×2 blocks → expand to 3×3
//        - Else if divisible by 3 → 3×3 blocks → expand to 4×4
//   2. Replace each block using rule dictionary
//   3. Recombine blocks into new grid
// @data-structures
//   - Dictionary<string, bool[]> for rule lookup
//   - Flattened boolean arrays for grids
// @pattern-encoding
//   Convert boolean arrays to strings of '#' and '.' for hashing
// @transformations
//   - Rotate 90° clockwise
//   - Horizontal flip
//   - Generate unique permutations only
// @split-combine
//   Split: Extract sub-blocks from large grid
//   Combine: Tile enhanced blocks into larger grid
// @complexity
//   Time: Exponential grid growth; dominated by pixel count after many iterations
//   Space: O(n²) for grid storage
// @notes
//   - Pre-expanding rule permutations ensures O(1) lookup per block
//   - Recursive split handles arbitrary grid sizes
//   - Flattened representation improves locality and simplifies indexing

await Part1();
return;

async Task Part1()
{
    var lines = await File.ReadAllLinesAsync("input.txt");
    var dict = new Dictionary<string, bool[]>();

    foreach (var line in lines)
    {
        var parts = line.Split(" => ");
        var input = ParsePattern(parts[0]);
        var output = ParsePattern(parts[1]);

        foreach (var permutation in RotationPermutations(input))
        {
            dict.Add(PatternToString(permutation), output);
        }
    }

    var initialPattern = ParsePattern(".#./..#/###");

    for (var i = 0; i < 18; i++)
    {
        initialPattern = ApplyRules(initialPattern, dict);
        if (i is 4 or 17)
        {
            Console.WriteLine(initialPattern.Count(x => x));
        }
    }

}

static bool[][] RotationPermutations(bool[] input)
{
    HashSet<string> seen = [];
    List<bool[]> result = [];

    var current = input;
    var flipped = current.Length == 4 ? Flip2X2(current) : Flip3X3(current);

    for (var i = 0; i < 4; i++)
    {
        // Add rotation
        var key = PatternToString(current);
        if (seen.Add(key))
            result.Add(current);

        key = PatternToString(flipped);
        if (seen.Add(key))
            result.Add(flipped);

        // Rotate for next iteration
        if (current.Length == 4)
        {
            current = Rotate2X2(current);
            flipped = Rotate2X2(flipped);
        }
        else
        {
            current = Rotate3X3(current);
            flipped = Rotate3X3(flipped);
        }
    }

    return result.ToArray();
}

static bool[] Rotate2X2(bool[] arr) => [arr[2], arr[0], arr[3], arr[1]];
static bool[] Flip2X2(bool[] arr) => [arr[1], arr[0], arr[3], arr[2]];

static bool[] Rotate3X3(bool[] arr) =>
    [arr[6], arr[3], arr[0], arr[7], arr[4], arr[1], arr[8], arr[5], arr[2]];

static bool[] Flip3X3(bool[] arr) =>
    [arr[2], arr[1], arr[0], arr[5], arr[4], arr[3], arr[8], arr[7], arr[6]];

static string PatternToString(bool[] pattern) =>
    string.Join("", pattern.Select(x => x ? '#' : '.'));

static bool[] ApplyRules(bool[] input, Dictionary<string, bool[]> rules)
{
    var splits = Split(input);

    var replacements = splits.Select(x =>
    {
        try
        {
            return rules[PatternToString(x)];
        }
        catch (Exception)
        {
            Console.WriteLine(PatternToString(x));
            throw;
        }
    }).ToArray();

    return Combine(replacements);
}

static bool[] Combine(IEnumerable<bool[]> input)
{
    var blocks = input.ToArray();
    var blockSize = (int)Math.Sqrt(blocks[0].Length);
    var blocksPerRow = (int)Math.Sqrt(blocks.Length);
    var n = blockSize * blocksPerRow;

    var result = new bool[n * n];

    for (var by = 0; by < blocksPerRow; by++)
    {
        for (var bx = 0; bx < blocksPerRow; bx++)
        {
            var block = blocks[by * blocksPerRow + bx];

            for (var y = 0; y < blockSize; y++)
            {
                for (var x = 0; x < blockSize; x++)
                {
                    var srcIdx = y * blockSize + x;
                    var dstIdx = (by * blockSize + y) * n + (bx * blockSize + x);
                    result[dstIdx] = block[srcIdx];
                }
            }
        }
    }

    return result;
}

static bool[][] Split(bool[] input)
{
    var n = (int)Math.Sqrt(input.Length);
    if (n is 2 or 3)
        return [input];

    var blockSize = (n % 2 == 0) ? 2 : 3;
    var blocksPerRow = n / blockSize;
    var result = new List<bool[]>();

    for (var by = 0; by < blocksPerRow; by++)
    {
        for (var bx = 0; bx < blocksPerRow; bx++)
        {
            var block = new bool[blockSize * blockSize];

            for (var y = 0; y < blockSize; y++)
            {
                for (var x = 0; x < blockSize; x++)
                {
                    var srcIdx = (by * blockSize + y) * n + (bx * blockSize + x);
                    var dstIdx = y * blockSize + x;
                    block[dstIdx] = input[srcIdx];
                }
            }

            var subBlocks = Split(block);
            result.AddRange(subBlocks);
        }
    }

    return result.ToArray();
}

static bool[] ParsePattern(ReadOnlySpan<char> str)
{
    var rows = str.ToString().Split('/');
    return rows.SelectMany(row => row.Select(c => c == '#')).ToArray();
}