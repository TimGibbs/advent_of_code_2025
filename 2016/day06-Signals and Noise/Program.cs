// @algorithm FrequencyAnalysis
// @category String Processing / Error Correction
// @input
//   Equal-length lowercase strings
// @technique
//   Column-wise Character Frequency Counting
// @variant
//   Part1: Most-Frequent Character per Column
//   Part2: Least-Frequent Character per Column
// @data-structure
//   int[columns][26]
// @paradigm
//   Aggregation
//   Projection
// @complexity
//   Time: O(n * m)  // n = lines, m = string length
//   Space: O(m * 26)


await Part1();
await Part2();
return;

async Task Part1()
{
    var lines = await File.ReadAllLinesAsync("input.txt");
    var r = lines[0].Select(x => new int[26]).ToArray();
    foreach (var line in lines)
    {
        for (int i = 0; i < line.Length; i++)
        {
            var c = line[i];
            var v = c - 'a';
            r[i][v]++;
        }
    }

    var result = string.Join("",r
        .Select(x=> x.Select((n,i)=>(n,i)).MaxBy(y=>y.n).i)
        .Select(i=> (char)(i+'a')));
    Console.WriteLine(result);
}

async Task Part2()
{
    var lines = await File.ReadAllLinesAsync("input.txt");
    var r = lines[0].Select(x => new int[26]).ToArray();
    foreach (var line in lines)
    {
        for (int i = 0; i < line.Length; i++)
        {
            var c = line[i];
            var v = c - 'a';
            r[i][v]++;
        }
    }

    var result = string.Join("",r
        .Select(x=> x.Select((n,i)=>(n,i)).MinBy(y=>y.n).i)
        .Select(i=> (char)(i+'a')));
    Console.WriteLine(result);
}
