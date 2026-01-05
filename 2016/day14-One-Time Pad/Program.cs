// @algorithm Key Stretching / Hash Search
// @category Cryptography / Brute Force
// @input Salt string (puzzle input)
// @state Index counter, hash queue, key count
// @instruction-set MD5 hash, string comparison
// @execution-model Sequential generation with sliding window
// @technique
//   Part1: Standard MD5, check for 3-in-a-row and 5-in-a-row matches
//   Part2: Stretched MD5 (2017 total hashes), optimized caching
// @features
//   Sliding window of 1000 hashes
//   Triple detection and quintuple verification
//   Cache for repeated hash computations
// @variant
//   Part1: Single MD5 per index
//   Part2: MD5 stretched 2017 times per index
// @data-structures
//   Queue<(string, uint)> for future hash window
//   Dictionary<uint, HashInfo> for caching
//   HashSet<char> for quintuple detection
// @complexity
//   Time: O(N * hash cost), depends on number of hashes generated
//   Space: O(1000) for sliding window / cache


await Part1();
await Part2a();
await Part2();
return;

async Task Part1()
{
    var line = await File.ReadAllTextAsync("input.txt");
    var count = 0;
    var q = new Queue<(string,uint)>();

    for (uint j = 0u; j < 1000; j++)
    {
        q.Enqueue((Generate(line, j), j));
    }

    while (count < 64)
    {
        var (hash, index) = q.Dequeue();
        q.Enqueue((Generate(line, index + 1000), index + 1000));
        var c = 'z';
        var r = 0;
        var has3 = false;
        foreach (var t in hash)
        {
            if (t != c)
            {
                r = 0;
                c = t;
            }
            else
            {
                r++;
                if (r == 2)
                {
                    has3 = true;
                    break;
                }
            }
        }

        if (!has3) continue;
        var matches = q.Any(x => x.Item1.Contains(string.Concat(c, c, c, c, c)));
        if (matches) count++;
        if (count == 64)
        {
            Console.WriteLine(index);
            break;
            
        }
    }
}

string Generate(string salt, uint val)
{
    var step = System.Text.Encoding.ASCII.GetBytes($"{salt}{val}");
    var hash = System.Security.Cryptography.MD5.HashData(step);
    return Convert.ToHexString(hash).ToLowerInvariant();
}

async Task Part2()
{
    var line = await File.ReadAllTextAsync("input.txt");
    var count = 0;
    var q = new Queue<(string,uint)>();

    for (uint j = 0u; j < 1000; j++)
    {
        q.Enqueue((Stretch(Generate(line, j)), j));
    }

    while (count < 64)
    {
        var (hash, index) = q.Dequeue();
        q.Enqueue((Stretch(Generate(line, index + 1000)), index + 1000));
        var c = 'z';
        var r = 0;
        var has3 = false;
        foreach (var t in hash)
        {
            if (t != c)
            {
                r = 0;
                c = t;
            }
            else
            {
                r++;
                if (r == 2)
                {
                    has3 = true;
                    break;
                }
            }
        }

        if (!has3) continue;
        var matches = q.Any(x => x.Item1.Contains(string.Concat(c, c, c, c, c)));
        if (matches) count++;
        if (count == 64)
        {
            Console.WriteLine(index);
            break;
            
        }
    }
}

string Stretch(string val)
{
    for (int i = 0; i < 2016; i++)
    {
        var step = System.Text.Encoding.ASCII.GetBytes(val);
        var hash = System.Security.Cryptography.MD5.HashData(step);
        val =  Convert.ToHexString(hash).ToLowerInvariant();
    }

    return val;
}

async Task Part2a()
{
    var line = await File.ReadAllTextAsync("input.txt");

    var cache = new Dictionary<uint, HashInfo>();
    
    HashInfo GetHashInfo(uint index)
    {
        if (!cache.TryGetValue(index, out var info))
        {
            var h = Stretch(Generate(line, index));
            var quints = new HashSet<char>();
            char prev = '\0';
            int run = 0;
            foreach (var c in h)
            {
                if (c == prev)
                {
                    run++;
                    if (run == 5) quints.Add(c);
                }
                else
                {
                    prev = c;
                    run = 1;
                }
            }
            info = new HashInfo { Hash = h, Quintuples = quints };
            cache[index] = info;
        }
        return info;
    }
    
    for (uint i = 0; i < 1000; i++)
        GetHashInfo(i);

    int count = 0;
    uint index = 0;

    while (count < 64)
    {
        var hash = GetHashInfo(index);

        GetHashInfo(index + 1000);
        
        var c = 'z';
        var r = 0;
        var has3 = false;
        foreach (var t in hash.Hash)
        {
            if (t != c)
            {
                r = 0;
                c = t;
            }
            else
            {
                r++;
                if (r == 2)
                {
                    has3 = true;
                    break;
                }
            }
        }

        if (!has3)
        {
            index++;
            continue;
        }
        
        for (uint i = index + 1; i <= index + 1000; i++)
        {
            if (cache[i].Quintuples.Contains(c))
            {
                count++;
                break;
            }
        }
        
        index++;
    }
    Console.WriteLine(index - 1);
}

class HashInfo
{
    public string Hash { get; set; }
    public HashSet<char> Quintuples { get; set; } = new();
}


