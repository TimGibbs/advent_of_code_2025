// @algorithm NumberTheory / Sieve
// @category Math
// @data-structure Array
// @complexity 
//   Part1: Time ~O(n√n), Space O(1)
//   Part2: Time O(n log n), Space O(n)
// @technique PrimeFactorization, DivisorSum, BoundedSieve
// @variant 
//   Part1: InfiniteElvesUnbounded
//   Part2: InfiniteElvesBounded50

await Part1();
await Part2();
return;

async Task Part1()
{
    var line = await File.ReadAllTextAsync("input.txt");
    var val = long.Parse(line);
    var target = val / 10;
    var stop = false;
    long i = 0;
    while (!stop)
    {
        var sum = SumOfFactors(++i);
        if (sum >= target) stop = true;
    }
    
    Console.WriteLine(i);
}

async Task Part2()
{
    var line = await File.ReadAllTextAsync("input.txt");
    var target = long.Parse(line);
    var houses = new long[1_000_000];

    for (var i = 1; i < houses.Length; i++)
    {
        for (var d = 1; d <= 50 && i*d<houses.Length; d++)
        {
            houses[i * d] += i * 11;
        }
    }
    
    for (var index = 0; index < houses.Length; index++)
    {
        var house = houses[index];
        if (house < target) continue;
        Console.WriteLine(index);
        break;
    }
    
}

static long SumOfFactors(long n)
{
    long result = 1;
    var temp = n;

    for (var p = 2; p * p <= temp; p++)
    {
        if (temp % p == 0)
        {
            var exponent = 0;
            while (temp % p == 0)
            {
                temp /= p;
                exponent++;
            }
            
            var term = (Pow(p, exponent + 1) - 1) / (p - 1);
            result *= term;
        }
    }
    
    if (temp > 1)
    {
        result *= (temp + 1);
    }

    return result;
}

static long Pow(long baseVal, int exp)
{
    long result = 1;
    while (exp-- > 0)
        result *= baseVal;
    return result;
}