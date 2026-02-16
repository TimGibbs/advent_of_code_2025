using System.Text.RegularExpressions;
Regex NumberRegex = new(@"-?\d+", RegexOptions.Compiled);

await Part1();
await Part2();
return;

async Task Part1()
{
    var lines = await File.ReadAllLinesAsync("input.txt");
    var particle = lines.Select(ParseParticle)
        .OrderBy(x => x.Acceleration.ManhattanDistance)
        .ThenBy(x => x.Velocity.ManhattanDistance)
        .ThenBy(x => x.Position.ManhattanDistance)
        .First();
    Console.WriteLine(particle.Index);
}
async Task Part2()
{
    var lines = await File.ReadAllLinesAsync("input.txt");
    var particles = lines.Select(ParseParticle).ToArray();
    for (var i = 0; i < 1_000; i++)
    {
        particles = particles.Select(x => x.Update())
            .GroupBy(x => x.Position)
            .Where(x => x.Count() == 1)
            .SelectMany(x => x)
            .ToArray();
    }
    Console.WriteLine(particles.Length);
}


Particle ParseParticle(string line, int index)
{
    var m = NumberRegex.Matches(line);

    return new Particle(
        index,
        new Vect3D(int.Parse(m[0].Value), int.Parse(m[1].Value), int.Parse(m[2].Value)),
        new Vect3D(int.Parse(m[3].Value), int.Parse(m[4].Value), int.Parse(m[5].Value)),
        new Vect3D(int.Parse(m[6].Value), int.Parse(m[7].Value), int.Parse(m[8].Value))
    );
}

record struct Particle(int Index, Vect3D Position, Vect3D Velocity, Vect3D Acceleration)
{
    public Particle Update()
    {
        var velocity = new Vect3D(Velocity.X + Acceleration.X, Velocity.Y + Acceleration.Y,
            Velocity.Z + Acceleration.Z);
        var position = new Vect3D(Position.X + velocity.X, Position.Y + velocity.Y, Position.Z + velocity.Z);
        return this with { Velocity = velocity, Position = position };
    }
};
record struct Vect3D(int X, int Y, int Z)
{
    public int ManhattanDistance => Math.Abs(X) + Math.Abs(Y) + Math.Abs(Z);
}