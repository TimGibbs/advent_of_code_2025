// @algorithm BotFactorySimulation
// @category Simulation / Rule Processing
// @input
//   Instruction list defining bots, values, and routing rules
// @entities
//   Bots with value queues
//   Output bins
// @technique
//   Discrete Event Simulation
//   State Propagation
// @operations
//   Instruction Parsing
//   Min/Max Value Routing
//   Conditional Target Dispatch (bot vs output)
// @data-structures
//   Dictionary<int, Bot>
//   Dictionary<int, List<int>>
// @paradigm
//   Imperative Simulation Loop
// @variant
//   Part1: Identify bot comparing specific values
//   Part2: Compute product of outputs 0,1,2
// @complexity
//   Time: O(n * steps)
//   Space: O(n)

await Part1();
await Part2();
return;

async Task Part1()
{
    var lines = await File.ReadAllLinesAsync("input.txt");
    var bots = new Dictionary<int, Bot>();
    var outputs = new Dictionary<int, List<int>>();
    foreach (var line in lines)
    {
        var split = line.Split(" ");
        if (split[0] == "bot")
        {
            var id = int.Parse(split[1]);
            var obLow = split[5] == "bot";
            var low = int.Parse(split[6]);
            var obHigh = split[10] == "bot";
            var high = int.Parse(split[11]);
            bots[id] = bots.TryGetValue(id,out var val) 
                ? val with { LowTarget = low, LowBot = obLow, HighTarget = high, HighBot = obHigh}
                : new Bot(id, [], low, obLow, high, obHigh);
            continue;
        }

        if (split[0] == "value")
        {
            var id = int.Parse(split[5]);
            if (bots.ContainsKey(id))
            {
                bots[id].Values.Add(int.Parse(split[1]));
                continue;
            }
            bots[id] = new Bot(id, [int.Parse(split[1])], null,null,null,null);
            continue;
        }
        throw new Exception("shouldnt get here");
    }

    while (true)
    {
        var activeBots = bots.Values.Where(b => b.Values.Count == 2).ToList();
        if (activeBots.Count == 0) break;
        foreach (var bot in activeBots)
        {
            if (bot.Values.Min() == 17 && bot.Values.Max() == 61)
            {
                Console.WriteLine(bot.Id);
                return;
            }
            
            if (bot.LowBot.HasValue && bot.LowBot.Value)
            {
                bots[bot.LowTarget.Value].Values.Add(bot.Values.Min());
            }

            if (bot.HighBot.HasValue && bot.HighBot.Value)
            {
                bots[bot.HighTarget.Value].Values.Add(bot.Values.Max());
            }
            
            if (bot.LowBot.HasValue && !bot.LowBot.Value)
            {
                if(!outputs.ContainsKey(bot.LowTarget.Value)) outputs.Add(bot.LowTarget.Value,[]); 
                outputs[bot.LowTarget.Value].Add(bot.Values.Min());
            }
            if (bot.HighBot.HasValue && !bot.HighBot.Value)
            {
                if(!outputs.ContainsKey(bot.HighTarget.Value)) outputs.Add(bot.HighTarget.Value,[]); 
                outputs[bot.HighTarget.Value].Add(bot.Values.Max());
            }
            bot.Values.Clear();
        }
    }
}

async Task Part2()
{
    var lines = await File.ReadAllLinesAsync("input.txt");
  var bots = new Dictionary<int, Bot>();
    var outputs = new Dictionary<int, List<int>>();
    foreach (var line in lines)
    {
        var split = line.Split(" ");
        if (split[0] == "bot")
        {
            var id = int.Parse(split[1]);
            var obLow = split[5] == "bot";
            var low = int.Parse(split[6]);
            var obHigh = split[10] == "bot";
            var high = int.Parse(split[11]);
            bots[id] = bots.TryGetValue(id,out var val) 
                ? val with { LowTarget = low, LowBot = obLow, HighTarget = high, HighBot = obHigh}
                : new Bot(id, [], low, obLow, high, obHigh);
            continue;
        }

        if (split[0] == "value")
        {
            var id = int.Parse(split[5]);
            if (bots.ContainsKey(id))
            {
                bots[id].Values.Add(int.Parse(split[1]));
                continue;
            }
            bots[id] = new Bot(id, [int.Parse(split[1])], null,null,null,null);
            continue;
        }
        throw new Exception("shouldnt get here");
    }

    while (true)
    {
        if (outputs.TryGetValue(0, out var v0) 
            && outputs.TryGetValue(1, out var v1) 
            && outputs.TryGetValue(2, out var v2))
        {
            Console.WriteLine(v0.First() * v1.First() * v2.First());
            return;
        }
        var activeBots = bots.Values.Where(b => b.Values.Count == 2).ToList();
        if (activeBots.Count == 0) break;
        foreach (var bot in activeBots)
        {
            if (bot.LowBot.HasValue && bot.LowBot.Value)
            {
                bots[bot.LowTarget.Value].Values.Add(bot.Values.Min());
            }

            if (bot.HighBot.HasValue && bot.HighBot.Value)
            {
                bots[bot.HighTarget.Value].Values.Add(bot.Values.Max());
            }
            
            if (bot.LowBot.HasValue && !bot.LowBot.Value)
            {
                if(!outputs.ContainsKey(bot.LowTarget.Value)) outputs.Add(bot.LowTarget.Value,[]); 
                outputs[bot.LowTarget.Value].Add(bot.Values.Min());
            }
            if (bot.HighBot.HasValue && !bot.HighBot.Value)
            {
                if(!outputs.ContainsKey(bot.HighTarget.Value)) outputs.Add(bot.HighTarget.Value,[]); 
                outputs[bot.HighTarget.Value].Add(bot.Values.Max());
            }
            bot.Values.Clear();
        }
    }
}

record Bot(int Id, List<int> Values, int? LowTarget, bool? LowBot, int? HighTarget, bool? HighBot);