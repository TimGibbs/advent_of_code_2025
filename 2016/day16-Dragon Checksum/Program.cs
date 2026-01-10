// @algorithm Dragon Curve Expansion + Checksum Reduction
// @category Data Expansion / Bitwise Processing
// @input Binary seed string
// @state Boolean array representing bits
// @execution-model Iterative expansion followed by iterative reduction
// @technique
//   Dragon curve generation (A + 0 + reverse(not A))
//   Pairwise checksum reduction
// @features
//   Boolean array for efficient bit operations
//   Symmetric reverse/invert during expansion
// @variant
//   Part1: Target length = 272
//   Part2: Target length = 35651584
// @data-structures
//   bool[] for bit storage
// @complexity
//   Time: O(N) expansion + O(N) reduction
//   Space: O(N)

await Part1();
await Part2();
return;

async Task Part1()
{
    var line = await File.ReadAllTextAsync("input.txt");
    var x = line.Select(c => c == '1').ToArray();
    while (x.Length < 272)
    {
        x = Dragon(x);
    }

    x = x.Take(272).ToArray();
    while (x.Length % 2 == 0)
    {
        x = CheckReduce(x);
    }

    var str = string.Join("", x.Select(c => c ? "1" : "0"));
    Console.WriteLine(str);
}   

bool[] Dragon(bool[] arr)
{
    var l = new bool[arr.Length * 2 + 1];
    for (int i = 0; i < arr.Length; i++)
    {
        l[i] = arr[i];
        l[^(i+1)] = !arr[i];
    }

    return l;
}

bool[] CheckReduce(bool[] arr)
{
    var l = new bool[arr.Length / 2];
    for (int i = 0; i < l.Length; i++)
    {
        l[i] = arr[i*2] == arr[i*2 + 1];
    }

    return l;
}

async Task Part2()
{
    var line = await File.ReadAllTextAsync("input.txt");
    var x = line.Select(c => c == '1').ToArray();
    while (x.Length < 35651584)
    {
        x = Dragon(x);
    }

    x = x.Take(35651584).ToArray();
    while (x.Length % 2 == 0)
    {
        x = CheckReduce(x);
    }

    var str = string.Join("", x.Select(c => c ? "1" : "0"));
    Console.WriteLine(str);
}
