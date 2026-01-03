// @algorithm PatternScanning
// @category String Processing / Network Protocols
// @input
//   IPv7-style strings with bracketed hypernet sequences
// @technique
//   Sliding Window
//   State Tracking (inside / outside brackets)
// @variant
//   Part1: ABBA Detection (TLS support)
//   Part2: ABA/BAB Correlation (SSL support)
// @data-structure
//   ReadOnlySpan<char>
//   HashSet<string>
// @paradigm
//   Iterative Scan
//   Set Membership
// @complexity
//   Time: O(n * m)
//   Space: O(k)


await Part1();
await Part2();
return;

async Task Part1()
{
    var lines = await File.ReadAllLinesAsync("input.txt");
    var count = 0;
    foreach (var line in lines)
    {
        var add = false;
        var poisoned = false;
        for (int i = 0; i < line.Length - 3; i++)
        {
            if (line[i] == '[')
            {
                poisoned = true;
                continue;
            }

            if (line[i] == ']')
            {
                poisoned = false;
                continue;
            }

            var sub = line.AsSpan().Slice(i, 4);
            if (Abba(sub))
            {
                if (poisoned)
                {
                    add = false;
                    break;
                }

                add = true;
            }
        }
        if (add) count++;
    }
    Console.WriteLine(count);
}

bool Abba(ReadOnlySpan<char> substring)
{
    if (substring.Length != 4) throw new Exception("should be 4");
    return substring[0] == substring[3] && substring[0] != substring[1] && substring[1] == substring[2];
}

bool Aba(ReadOnlySpan<char> substring)
{
    if (substring.Length != 3) throw new Exception("should be 3");
    return substring[0] == substring[2] && substring[0] != substring[1];
}

async Task Part2()
{
    var lines = await File.ReadAllLinesAsync("input.txt");
    var count = 0;
    foreach (var line in lines)
    {
        var abas = new HashSet<string>();
        var add = false;
        var babSpace = false;
        for (int i = 0; i < line.Length - 2; i++)
        {
            if (line[i] == '[')
            {
                babSpace = true;
                continue;
            }

            if (line[i] == ']')
            {
                babSpace = false;
                continue;
            }
            if(babSpace) continue;
            
            var sub = line.AsSpan().Slice(i, 3);
            if (Aba(sub))
            {
                abas.Add(sub.ToString());
            }
        }
        for (int i = 0; i < line.Length - 2; i++)
        {
            if (line[i] == '[')
            {
                babSpace = true;
                continue;
            }

            if (line[i] == ']')
            {
                babSpace = false;
                continue;
            }
            if(!babSpace) continue;
            
            var sub = line.AsSpan().Slice(i, 3);
            if (Aba(sub) && Add(abas, sub))
            {
                add = true;
                break;
            }
        }
        if (add) count++;
    }
    Console.WriteLine(count);
}

bool Add(HashSet<string> abas, ReadOnlySpan<char> readOnlySpan)
{
    {
        foreach (var aba in abas)
        {
            if (Bab(readOnlySpan, aba))
            {
                return true;
            }
        }

        return false;
    }

    bool Bab(ReadOnlySpan<char> substring, string aba)
    {
        if (substring.Length != 3) throw new Exception("should be 3");
        return substring[0] == substring[2] && substring[0] == aba[1] && substring[1] == aba[0] ;
    }
}