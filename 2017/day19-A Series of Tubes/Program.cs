// @algorithm ASCII Path Following
// @category Grid Traversal / Simulation
// @input 2D character grid representing a tube network
// @problem
//   Traverse path starting at top entry point
// @output
//   Collected letters along the path
//   Total number of steps taken
// @grid-elements
//   '|' vertical path
//   '-' horizontal path
//   '+' intersection (turn)
//   'A'–'Z' letters to collect
//   ' ' empty space (termination)
// @movement
//   - Start at first '|' in top row
//   - Initial direction: South
//   - Move one cell per step
//   - Continue until moving into empty space
// @turning
//   - At '+', change direction perpendicular to current movement
//   - Choose the only non-empty adjacent cell
// @technique
//   - Direction-based stepping
//   - Bounds-safe grid access helper
//   - Intersection handling via neighbor checks
// @state
//   - Current position (x, y)
//   - Current direction
//   - Collected letters (StringBuilder)
//   - Step counter
// @data-structures
//   - char[][] grid
//   - Enum for direction
//   - StringBuilder for output
// @complexity
//   Time: O(path length)
//   Space: O(1) auxiliary (excluding input)
// @notes
//   - Letters are appended in traversal order
//   - Path is guaranteed to be non-branching except at turns

using System.Text;

await Part1();
return;

async Task Part1()
{
    var lines = await File.ReadAllLinesAsync("input.txt");
    var grid = lines.Select(l => l.ToCharArray()).ToArray();

    var x = Array.IndexOf(grid[0], '|');
    var y = 0;

    var dir = Direction.South;

    var letters = new StringBuilder();
    var step = 0;

    while (true)
    {
        
        var c = GetChar(grid, x, y);
        if (c == ' ')
            break;
        step++;

        if (char.IsUpper(c))
            letters.Append(c);

        if (c == '+')
            dir = Turn(grid, x, y, dir);

        (x, y) = Step(x, y, dir);
    }

    Console.WriteLine(letters.ToString());
    Console.WriteLine(step);
}



char GetChar(char[][] g, int x, int y)
{
    if (y < 0 || y >= g.Length) return ' ';
    if (x < 0 || x >= g[y].Length) return ' ';
    return g[y][x];
}


(int x, int y) Step(int x, int y, Direction d) => d switch
{
    Direction.North => (x, y - 1),
    Direction.South => (x, y + 1),
    Direction.East  => (x + 1, y),
    Direction.West  => (x - 1, y),
    _ => (x, y)
};

Direction Turn(char[][] g, int x, int y, Direction d)
{
    if (d is Direction.North or Direction.South)
    {
        if (GetChar(g, x - 1, y) != ' ') return Direction.West;
        if (GetChar(g, x + 1, y) != ' ') return Direction.East;
    }
    else
    {
        if (GetChar(g, x, y - 1) != ' ') return Direction.North;
        if (GetChar(g, x, y + 1) != ' ') return Direction.South;
    }

    throw new Exception("No valid turn");
}


internal enum Direction
{
    North,
    East,
    South,
    West,
}