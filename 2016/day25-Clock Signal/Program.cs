// @algorithm Clock Signal Detection (Assembunny VM)
// @category Virtual Machine / Instruction Simulation
// @input Assembly-like instruction list with output (`out`)
// @instruction-set cpy inc dec jnz tgl out
// @state Registers (a, b, c, d)
// @execution-model Sequential with jumps, self-modifying code
// @technique
//   - Direct interpretation
//   - Early termination on invalid output
//   - Incremental search over initial register values
// @features
//   - Output stream validation (alternating 0/1 clock signal)
//   - Peephole optimization for tight increment/decrement loops
//   - Iterator-based output (`yield return`)
// @variant
//   Part1: Find smallest `a` producing stable alternating signal
// @data-structures
//   int[] for registers
//   Instr[] for parsed program
// @complexity
//   Time: O(K * executed instructions), K = tested initial values
//   Space: O(program size)


await Part1();

async Task Part1()
{
    var lines = await File.ReadAllLinesAsync("input.txt");
    var program = Parse(lines);
    var x = 0;
    var cont = true;
    while (cont)
    {
        var state = new int[4];
        state[0] = ++x;
        var i = 0;
        var one = false;
        foreach (var val in RunProgram(program, state))
        {
            if (one && val != 1) break; 
            if(!one && val != 0) break;
            one = !one;
            if (++i > 1000)
            {
                cont = false;
                break;
            }
        }
    }
    Console.WriteLine(x);
}

IEnumerable<int> RunProgram(Instr[] instrs, int[] state)
{
    for (var ip = 0; ip < instrs.Length; ip++)
    {
        var instr = instrs[ip];
        var x = instr.XIsReg ? state[instr.X] : instr.X;
        var y = instr.YIsReg ? state[instr.Y] : instr.Y;

        // --- Peephole optimization for multiply loop ---
        // pattern: inc a / dec b / jnz b -2
        if (instr.Op == Op.Inc && ip + 2 < instrs.Length)
        {
            var next1 = instrs[ip + 1];
            var next2 = instrs[ip + 2];

            if (next1.Op == Op.Dec && next1.XIsReg &&
                next2.Op == Op.Jnz && next2.XIsReg &&
                next2.X == next1.X && next2.Y == -2 && !next2.YIsReg)
            {
                // Apply a += b; b = 0
                state[instr.X] += state[next1.X];
                state[next1.X] = 0;
                ip += 2; // skip next 2 instructions
                continue;
            }
        }

        switch (instr.Op)
        {
            case Op.Cpy:
                if (instr.YIsReg)
                    state[instr.Y] = x;
                break;

            case Op.Inc:
                if (instr.XIsReg) state[instr.X]++;
                break;

            case Op.Dec:
                if (instr.XIsReg) state[instr.X]--;
                break;

            case Op.Jnz:
                if (x != 0)
                    ip += y - 1; // jump relative
                break;

            case Op.Tgl:
                var target = ip + x;
                if (target >= 0 && target < instrs.Length)
                {
                    var t = instrs[target];
                    t.Op = t.Op switch
                    {
                        Op.Inc => Op.Dec,
                        Op.Dec => Op.Inc,
                        Op.Tgl => Op.Inc,
                        Op.Jnz => Op.Cpy,
                        Op.Cpy => Op.Jnz,
                        _ => t.Op
                    };
                    instrs[target] = t; // write back
                }
                break;

            case Op.Out:
                yield return x;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}

Instr[] Parse(string[] lines)
{
    var arr = new Instr[lines.Length];
    for (var i = 0; i < lines.Length; i++)
        arr[i] = ParseLine(lines[i]);
    return arr;

    Instr ParseLine(string line)
    {
        var split = line.Split(' ');
        var op = split[0] switch
        {
            "cpy" => Op.Cpy,
            "inc" => Op.Inc,
            "dec" => Op.Dec,
            "jnz" => Op.Jnz,
            "tgl" => Op.Tgl,
            "out" => Op.Out,
            _ => throw new Exception($"Unknown op {split[0]}")
        };

        return new Instr
        {
            Op = op,
            X = ParseX(out var xreg),
            XIsReg = xreg,
            Y = ParseY(out var yreg),
            YIsReg = yreg
        };

        bool TryParseReg(string s, out int idx)
        {
            idx = RegIndex(s);
            return idx != -1;
        }

        int ParseX(out bool isReg)
        {
            if (TryParseReg(split[1], out var r)) { isReg = true; return r; }
            isReg = false; return int.Parse(split[1]);
        }

        int ParseY(out bool isReg)
        {
            if (split.Length < 3) { isReg = false; return 0; }
            if (TryParseReg(split[2], out var r)) { isReg = true; return r; }
            isReg = false; return int.Parse(split[2]);
        }

        int RegIndex(string s) => s switch
        {
            "a" => 0,
            "b" => 1,
            "c" => 2,
            "d" => 3,
            _ => -1
        };
    }
}

internal enum Op { Cpy, Inc, Dec, Jnz, Tgl, Out }

internal struct Instr
{
    public Op Op;
    public int X;      // register index or literal
    public bool XIsReg;
    public int Y;      // register index or literal
    public bool YIsReg;
}
