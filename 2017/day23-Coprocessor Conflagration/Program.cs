await Part1();
await Part2();
return;

async Task Part1()
{
    var lines = await File.ReadAllLinesAsync("input.txt");
    var instructions = lines.Select(ParseLine).ToArray();
    var registers = new long[8];
    var count = 0;
    for (int i = 0; i >= 0 && i < instructions.Length;)
    {
        var instruction = instructions[i];
        var v1 = instruction.IsRegister1 ? registers[instruction.Value1] : instruction.Value1;
        long? v2 = null;
        if (instruction.Value2.HasValue)
        {
            v2 = instruction.IsRegister2
                ? registers[instruction.Value2.Value]
                : instruction.Value2.Value;
        }
        switch (instruction.InstructionCode)
        {
            case InstructionCode.set:
                registers[instruction.Value1] = v2!.Value;
                break;
            case InstructionCode.sub:
                registers[instruction.Value1] -= v2!.Value;
                break;
            case InstructionCode.mul:
                count++;
                registers[instruction.Value1] *= v2!.Value;
                break;
            case InstructionCode.jnz:
                if (v1 != 0)
                {
                    i += (int)v2!.Value;
                    continue;
                }
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        i++;
    }
    Console.WriteLine(count);
}
async Task Part2()
{
    var b = 93 * 100 + 100000;
    var c = b + 17000;
    
    var count = 0;
    for (var i = b ; i <= c; i+=17)
    {
        if (!IsPrime(i)) count++;
    }
    Console.WriteLine(count);
}

bool IsPrime(long n)
{
    if (n < 2)
        return false;
    for (int i = 2; i < Math.Sqrt(n); i++)
    {
        if (n % i == 0) return false;
    }
    return true;
}


Instruction ParseLine(string line)
{
    var split = line.Split(' ');
    var isnt = split[0] switch
    {
        "set" => InstructionCode.set,
        "sub" => InstructionCode.sub,
        "mul" => InstructionCode.mul,
        "jnz" => InstructionCode.jnz,
        _ => throw new ArgumentOutOfRangeException()
    };
    var isRegister1 = true;
    if (long.TryParse(split[1], out var value1))
    {
        isRegister1 = false;
    }
    else
    {
        value1 = split[1][0] - 'a';
    }
    long? value2 = null;
    var isRegister2 = true;
    if (split.Length > 2)
    {
        if (long.TryParse(split[2], out var val))
        {
            value2 = val;
            isRegister2 = false;
        }
        else
        {
            value2 = split[2][0] - 'a';
        }
    }

    return new Instruction(isnt, value1, isRegister1, value2, isRegister2);
}

enum InstructionCode
{
    set,
    mul,
    sub,
    jnz,
}

record struct Instruction(InstructionCode InstructionCode, long Value1, bool IsRegister1, long? Value2, bool IsRegister2);
