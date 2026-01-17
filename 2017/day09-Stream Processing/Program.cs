// @algorithm Stream Parsing
// @category Parsing / State Machine
// @input Character stream with groups and garbage
// @problem
//   Parse a stream containing:
//     - Groups delimited by { }
//     - Garbage delimited by < >
//     - Cancelled characters via !
// @parts
//   Part1: Compute total group score based on nesting depth
//   Part2: Count non-cancelled characters inside garbage
// @model
//   - Single-pass character scan
//   - Boolean garbage state
//   - Integer nesting depth
// @technique
//   - Stateful parsing with explicit mode switches
//   - Skip cancelled characters by advancing index
// @data-structures
//   Primitive counters and flags only
// @complexity
//   Time: O(n)
//   Space: O(1)
// @notes
//   - '!' only has meaning while inside garbage
//   - Group delimiters are ignored inside garbage

await Part1();
await Part2();
return;

async Task Part1()
{
    var line = await File.ReadAllTextAsync("input.txt");

    var nestedness = 0;
    var sum = 0;
    var garbage = false;
    for (var i = 0; i < line.Length; i++)
    {
        var c = line[i];
        switch (c)
        {
            case '!' when garbage:
                i++;
                break;
            case '<':
                garbage = true;
                break;
            case '>':
                garbage = false;
                break;
            case '{' when !garbage:
                sum += ++nestedness;
                break;
            case '}' when !garbage:
                nestedness--;
                break;
        }
    }
    Console.WriteLine(sum);
}

async Task Part2()
{
    var line = await File.ReadAllTextAsync("input.txt");

    var count = 0;
    var garbage = false;
    for (var i = 0; i < line.Length; i++)
    {
        var c = line[i];
        switch (c)
        {
            case '!' when garbage:
                i++;
                continue;
            case '<' when !garbage:
                garbage = true;
                continue;
            case '>' when garbage:
                garbage = false;
                continue;
        }

        if (garbage)
        {
            count++;
        }
    }
    Console.WriteLine(count);
}