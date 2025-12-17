// @algorithm CombinatorialOptimization
// @category BruteForce / ArrayManipulation
// @data-structure Array
// @complexity Time: O(n^4), Space: O(n)
// @variant Part1: MaxScoreAnyCalories, Part2: MaxScoreExactCalories
// @misc ExtensionMembers

await Part1();
await Part2();
return;

async Task Part1()
{
    var lines = await File.ReadAllLinesAsync("input.txt");
    var arrays = lines.Select(s => s.Split(' ').Where((x,i)=> i!= 0 && i%2 ==0 && i!=10).Select(s=>int.Parse(s[..^1])).ToArray()).ToArray();

    var max = 0;
    foreach (var (a,b,c,d) in SplitSpoons(100))
    {
        var val = arrays[0]*a + arrays[1]*b + arrays[2]*c + arrays[3]*d;
        if(val.Any(x=>x<=0)) continue;
        max = Math.Max(val.Aggregate((a, b) => a * b), max);
    }
    Console.WriteLine(max);


}

static IEnumerable<(int a, int b, int c, int d)> SplitSpoons(int total)
{
    for (int a = 0; a <= total; a++)
    {
        for (int b = 0; b <= total - a; b++)
        {
            for (int c = 0; c <= total - a - b; c++)
            {
                int d = total - a - b - c;
                yield return (a, b, c, d);
            }
        }
    }
}

async Task Part2()
{
    var lines = await File.ReadAllLinesAsync("input.txt");
    var arrays = lines.Select(s => s.Split(' ').Where((x,i)=> i!= 0 && i%2 ==0).Select(s=>int.Parse(s.TrimEnd(','))).ToArray()).ToArray();

    var max = 0;
    foreach (var (a,b,c,d) in SplitSpoons(100))
    {
        var val = arrays[0]*a + arrays[1]*b + arrays[2]*c + arrays[3]*d;
        if(val.Any(x=>x<=0) || val[4] != 500) continue;
        max = Math.Max(val[..^1].Aggregate((a, b) => a * b), max);
    }
    Console.WriteLine(max);


}

public static class ArrayExtensions
{
    extension(int[])
    {

    // Example: element-wise addition operator for int[]
    public static int[] operator +(int[] a, int[] b)
    {
        if (a.Length != b.Length) throw new ArgumentException();
        int[] result = new int[a.Length];
        for (int i = 0; i < result.Length; i++)
            result[i] = a[i] + b[i];
        return result;
    }

    // Element-wise multiplication by an integer
    public static int[] operator *(int[] a, int multiplier)
    {
        int[] result = new int[a.Length];
        for (int i = 0; i < result.Length; i++)
            result[i] = a[i] * multiplier;
        return result;
    }

    // Optional: multiplication with int on the left side
    public static int[] operator *(int multiplier, int[] a) => a * multiplier;
}

}
