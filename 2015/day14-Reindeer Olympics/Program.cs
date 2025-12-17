// @algorithm TimeStepSimulation
// @category Simulation / Scheduling
// @data-structure Array
// @complexity Time: O(n * t), Space: O(n)
// @variant Part1: MaxDistanceAfterFixedTime, Part2: PointScoringPerSecond

await Part1();
await Part2();
return;

async Task Part1()
{
    var lines = await File.ReadAllLinesAsync("input.txt");
    var reindeer = lines.Select(ParseLine).Select(x=>Project(x,2503)).Max();
    Console.WriteLine(reindeer);
}

async Task Part2()
{
    var lines = await File.ReadAllLinesAsync("input.txt");
    var reindeer = lines.Select(ParseLine).ToArray();
    var points = new int[reindeer.Length];
    for (int i = 1; i < 2503; i++)
    {
        var distances = reindeer.Select((r, index) =>
            (distance: Project(r, i), index)).ToArray();

        var max = distances.Max(x => x.distance);

        foreach (var d in distances.Where(x => x.distance == max))
        {
            points[d.index]++;
        }

    }
    Console.WriteLine(points.Max());
}

int Project(Reindeer reindeer, int seconds)
{
    var cycleTime = reindeer.FlySeconds + reindeer.RestSeconds;
    var cycles = seconds / cycleTime;
    var flightSeconds = cycles * reindeer.FlySeconds + Math.Min(seconds % cycleTime, reindeer.FlySeconds);
    return flightSeconds * reindeer.SpeedKmPerSecond;
}

static Reindeer ParseLine(string line)
{
    var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
    var name = parts[0];
    var speed = int.Parse(parts[3]);
    var flySeconds = int.Parse(parts[6]);
    var restSeconds = int.Parse(parts[13]);

    return new Reindeer(name, speed, flySeconds, restSeconds);
}


public record Reindeer(
    string Name,
    int SpeedKmPerSecond,
    int FlySeconds,
    int RestSeconds
);
