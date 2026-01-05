// @algorithm Interpreter
// @category Virtual Machine / Instruction Simulation
// @input Assembly-like instruction list
// @instruction-set cpy inc dec jnz
// @state Registers (a, b, c, d)
// @execution-model Sequential with conditional jumps
// @technique Direct interpretation
// @features
//   Immediate vs register evaluation
//   Tuple-based immutable state updates
// @variant
//   Part1: c = 0
//   Part2: c = 1
// @data-structures
//   ValueTuple for registers
//   String[] for program
// @complexity
//   Time: O(number of executed instructions)
//   Space: O(1)

await Part1();
await Part2();
return;

async Task Part1()
{
    var lines = await File.ReadAllLinesAsync("input.txt");
    var state = (a: 0, b: 0, c: 0, d: 0);
    state = StateUpdate(lines, state);
    Console.WriteLine(state.a);
}

async Task Part2()
{
    var lines = await File.ReadAllLinesAsync("input.txt");
    var state = (a: 0, b: 0, c: 1, d: 0);
    state = StateUpdate(lines, state);
    Console.WriteLine(state.a);

}

int Eval(string s, (int a, int b, int c, int d) state)
{
    if(!int.TryParse(s, out var num))
    {
        num = s switch
        {
            "a" => state.a,
            "b" => state.b,
            "c" => state.c,
            "d" => state.d,
            _ => num
        };
    }

    return num;
}

(int a, int b, int c, int d) Cpy((int a, int b, int c, int d) state, string s)
{
    var split = s.Split(' ');
    var target = split[2];
    var num = Eval(split[1], state);
    
    return target switch
    {
        "a" => (num, state.b, state.c, state.d),
        "b" => (state.a, num, state.c, state.d),
        "c" => (state.a, state.b, num, state.d),
        "d" => (state.a, state.b, state.c, num),
        _ => throw new Exception("should not happen")
    };
}

(int a, int b, int c, int d) Inc((int a, int b, int c, int d) state, string s)
{
    var split = s.Split(' ');
    var target = split[1];

    return target switch
    {
        "a" => (++state.a, state.b, state.c, state.d),
        "b" => (state.a, ++state.b, state.c, state.d),
        "c" => (state.a, state.b, ++state.c, state.d),
        "d" => (state.a, state.b, state.c, ++state.d),
        _ => throw new Exception("should not happen")
    };
}

(int a, int b, int c, int d) Dec((int a, int b, int c, int d) state, string s)
{
    var split = s.Split(' ');
    var target = split[1];

    return target switch
    {
        "a" => (--state.a, state.b, state.c, state.d),
        "b" => (state.a, --state.b, state.c, state.d),
        "c" => (state.a, state.b, --state.c, state.d),
        "d" => (state.a, state.b, state.c, --state.d),
        _ => throw new Exception("should not happen")
    };
}

(int a, int b, int c, int d) StateUpdate(string[] strings, (int a, int b, int c, int d) valueTuple)
{
    for (var index = 0; index < strings.Length; index++)
    {
        var line = strings[index];
        var split = line.Split(' ');
        if (split[0] == "jnz")
        {
            if (Eval(split[1], valueTuple) != 0)
            {
                index += int.Parse(split[2]) - 1;
            }
            continue;
        }
        valueTuple = split[0] switch
        {
            "cpy" => Cpy(valueTuple, line),
            "inc" => Inc(valueTuple, line),
            "dec" => Dec(valueTuple, line),
            _ => throw new Exception("should not happen")
        };
    }

    return valueTuple;
}