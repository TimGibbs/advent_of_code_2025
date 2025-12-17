// @algorithm PermutationOptimization
// @category Combinatorics / Graph
// @data-structure Dictionary, Array
// @complexity Time: O(n!), Space: O(n)
// @variant Part1: MaxHappinessSeating, Part2: AddNeutralGuest

await Part1();
return;

async Task Part1()
{
    var lines = await File.ReadAllLinesAsync("input.txt");
    var dict = new Dictionary<(char,char), int>();
    foreach (var line in lines)
    {
        var split = line.Split(" ");
        dict[(split[0][0], split[10][0])] = int.Parse(split[3]) * (split[2] == "lose" ? -1 : 1);
    }
    
    var names = dict.Keys.Select(x => x.Item1).Distinct().ToArray();
    
    var orders = Permute(names, 0);
    var max = 0;
    foreach (var order in orders)
    {
        max = Math.Max(max, Value(order, dict));
    }
    Console.WriteLine(max);

    foreach (var name in names)
    {
        dict[(name, 'Z')] = 0;
        dict[('Z', name)] = 0;
    }

    names = names.Append('Z').ToArray();
    
    orders = Permute(names, 0);
    max = 0;
    foreach (var order in orders)
    {
        max = Math.Max(max, Value(order, dict));
    }
    Console.WriteLine(max);
}

int Value(char[] positions, Dictionary<(char, char), int> values)
{
    var result = 0;
    for (int i = 0; i < positions.Length - 1; i++)
    {
        result += values[(positions[i], positions[i + 1])];
        result += values[(positions[i + 1], positions[i])];
    }
    result += values[(positions[^1], positions[0])];
    result += values[(positions[0], positions[^1])];
    return result;
    
}

static IEnumerable<T[]> Permute<T>(T[] array, int start)
{
    if (start == array.Length - 1)
    {
        yield return (T[])array.Clone();
        yield break;
    }

    for (int i = start; i < array.Length; i++)
    {
        Swap(array, start, i);

        foreach (var perm in Permute(array, start + 1))
            yield return perm;

        Swap(array, start, i); // backtrack
    }
}

static void Swap<T>(T[] array, int i, int j)
{
    (array[i], array[j]) = (array[j], array[i]);
}