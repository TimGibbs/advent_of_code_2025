// @algorithm InstructionInterpreter / Simulation
// @category VirtualMachine / Parsing
// @data-structure Array
// @complexity
//   Time: O(N) where N = number of instructions executed
//   Space: O(1)
// @technique SequentialExecution, ConditionalJump, RegisterMachine
// @variant
//   Part1: InitialRegisters a=0,b=0
//   Part2: InitialRegisters a=1,b=0

await Part1();
await Part2();
return;

async Task Part1()
{
    var lines = await File.ReadAllLinesAsync("input.txt");
    
    var a = 0u;
    var b = 0u;
    Console.WriteLine($"a:{a}, b:{b}");
    for (var i = 0; i < lines.Length; i++)
    {
        var instruction = lines[i];
        switch(instruction[..3])
        {
            case "hlf":
            {
                var target = instruction[4];
                if (target == 'a') a /= 2;
                if (target == 'b') b /= 2;
                break;
            }
            case "tpl":
            {
                var target = instruction[4];
                if (target == 'a') a *= 3;
                if (target == 'b') b *= 3;
                break;
            }
            case "inc":
            {
                var target = instruction[4];
                if (target == 'a') a += 1;
                if (target == 'b') b += 1;
                break;
            }
            case "jmp":
            {
                var jump = int.Parse(instruction.Split(' ')[1]);
                i += jump - 1;
                break;
            }
            case "jie":
            {
                var target = instruction[4];
                if ((target == 'a' && a % 2 == 0) || (target == 'b' && b % 2 == 0))
                {
                    var jump = int.Parse(instruction.Split(' ')[2]);
                    i += jump - 1;
                }
                break;
            }
            case "jio":
            {
                var target = instruction[4];
                if ((target == 'a' && a == 1) || (target == 'b' && b == 1))
                {
                    var jump = int.Parse(instruction.Split(' ')[2]);
                    i += jump - 1;
                }
                break;
            }
        }
        Console.WriteLine($"a:{a}, b:{b}");
        
    }
}

async Task Part2()
{
    var lines = await File.ReadAllLinesAsync("input.txt");
    
    var a = 1u;
    var b = 0u;
    Console.WriteLine($"a:{a}, b:{b}");
    for (var i = 0; i < lines.Length; i++)
    {
        var instruction = lines[i];
        switch(instruction[..3])
        {
            case "hlf":
            {
                var target = instruction[4];
                if (target == 'a') a /= 2;
                if (target == 'b') b /= 2;
                break;
            }
            case "tpl":
            {
                var target = instruction[4];
                if (target == 'a') a *= 3;
                if (target == 'b') b *= 3;
                break;
            }
            case "inc":
            {
                var target = instruction[4];
                if (target == 'a') a += 1;
                if (target == 'b') b += 1;
                break;
            }
            case "jmp":
            {
                var jump = int.Parse(instruction.Split(' ')[1]);
                i += jump - 1;
                break;
            }
            case "jie":
            {
                var target = instruction[4];
                if ((target == 'a' && a % 2 == 0) || (target == 'b' && b % 2 == 0))
                {
                    var jump = int.Parse(instruction.Split(' ')[2]);
                    i += jump - 1;
                }
                break;
            }
            case "jio":
            {
                var target = instruction[4];
                if ((target == 'a' && a == 1) || (target == 'b' && b == 1))
                {
                    var jump = int.Parse(instruction.Split(' ')[2]);
                    i += jump - 1;
                }
                break;
            }
        }
        Console.WriteLine($"a:{a}, b:{b}");
        
    }
}
