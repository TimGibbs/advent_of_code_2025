// @algorithm Josephus Problem
// @category Circular Elimination / Queue Simulation
// @input Single integer N (number of participants)
// @state Remaining participants in a circle
// @execution-model Repeated elimination until one remains
// @variant
//   Part1: Eliminate next active participant (classic Josephus, k=2)
//   Part2: Eliminate participant directly opposite (two-queue optimization)
// @technique
//   Part1: Boolean array + circular scan
//   Part2: Two queues to model left/right halves
// @data-structures
//   bool[] for alive tracking (Part1)
//   Queue<int> for circular halves (Part2)
// @complexity
//   Part1: Time O(N²), Space O(N)
//   Part2: Time O(N), Space O(N)


await Part1();
await Part2();
return;

async Task Part1()
{
    var line = await File.ReadAllTextAsync("input.txt");
    var i = int.Parse(line);
    var f = new bool[i];
    Array.Fill(f,true);
    var p = 0;
    while (true)
    {
        if (!f[p]) p=Next(f,p);
        var next = Next(f, p);
        if (next == p)
        {
            Console.WriteLine(p+1);
            break;
        }
        f[next] = false;
        p=Next(f,p);
    }

}

int Next(bool[] all, int current)
{
    var i = 1;
    while (!all[(current + i) % all.Length])
    {
        i++;
    }
    return (current + i) % all.Length;
}

async Task Part2()
{
    var line = await File.ReadAllTextAsync("input.txt");
    var n = int.Parse(line);

    var left = new Queue<int>();
    var right = new Queue<int>();
    
    for (int i = 1; i <= n; i++)
    {
        if (i <= n / 2)
            left.Enqueue(i);
        else
            right.Enqueue(i);
    }

    while (left.Count + right.Count > 1)
    {
        right.Dequeue();
        
        right.Enqueue(left.Dequeue());
        
        if (right.Count > left.Count + 1)
            left.Enqueue(right.Dequeue());
    }

    Console.WriteLine(left.Count == 1 ? left.Dequeue() : right.Dequeue());
}

