// @algorithm ConstraintFiltering
// @category Matching / RuleEvaluation
// @data-structure Record / Dictionary-like Properties
// @complexity Time: O(n · p), Space: O(n)
// @variant Part1: ExactMatchWithUnknowns, Part2: RangeBasedMatch

using System.Reflection;

await Part1();
await Part2();
return;

async Task Part1()
{
    var lines = await File.ReadAllLinesAsync("input.txt");
    var sues = lines.Select(ParseSue).ToArray();
    var target = new Sue { Children = 3, Cats = 7, Samoyeds = 2, Pomeranians = 3, Akitas = 0, Vizslas = 0, Goldfish = 5, Trees = 3, Cars = 2, Perfumes = 1 };
    var result = sues.Single(sue => 
        (sue.Children == target.Children || sue.Children == null) 
        && (sue.Cats == target.Cats || sue.Cats == null) 
        && (sue.Samoyeds == target.Samoyeds || sue.Samoyeds == null)
        && (sue.Pomeranians == target.Pomeranians || sue.Pomeranians == null) 
        && (sue.Akitas == target.Akitas || sue.Akitas == null) 
        && (sue.Vizslas == target.Vizslas || sue.Vizslas == null) 
        && (sue.Goldfish == target.Goldfish || sue.Goldfish == null) 
        && (sue.Trees == target.Trees || sue.Trees == null) 
        && (sue.Cars == target.Cars || sue.Cars == null) 
        && (sue.Perfumes == target.Perfumes || sue.Perfumes == null)).No;
    Console.WriteLine(result);
}

async Task Part2()
{
    var lines = await File.ReadAllLinesAsync("input.txt");
    var sues = lines.Select(ParseSue).ToArray();
    var target = new Sue { Children = 3, Cats = 7, Samoyeds = 2, Pomeranians = 3, Akitas = 0, Vizslas = 0, Goldfish = 5, Trees = 3, Cars = 2, Perfumes = 1 };
    var result = sues.Single(sue => 
        (sue.Children == target.Children || sue.Children == null) 
        && (sue.Cats > target.Cats || sue.Cats == null) 
        && (sue.Samoyeds == target.Samoyeds || sue.Samoyeds == null)
        && (sue.Pomeranians < target.Pomeranians || sue.Pomeranians == null) 
        && (sue.Akitas == target.Akitas || sue.Akitas == null) 
        && (sue.Vizslas == target.Vizslas || sue.Vizslas == null) 
        && (sue.Goldfish < target.Goldfish || sue.Goldfish == null) 
        && (sue.Trees > target.Trees || sue.Trees == null) 
        && (sue.Cars == target.Cars || sue.Cars == null) 
        && (sue.Perfumes == target.Perfumes || sue.Perfumes == null)).No;
    Console.WriteLine(result);
}

Sue ParseSue(string line)
{
    var split = line.Split(' ');
    var no = int.Parse(split[1][..^1]);
    var sue = new Sue { No = no };
    for (int i = 2; i < split.Length; i+=2)
    {
        var prop = split[i].TrimEnd(':');
        var val = split[i + 1].TrimEnd(',');
        var property = sue.GetType().GetProperty(
            prop,
            BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase
        );

        property?.SetValue(sue, int.Parse(val));
    }
    return sue;
}

record Sue
{
    public int No { get; set; }
    public int? Children { get; set; }
    public int? Cats { get; set; }
    public int? Samoyeds { get; set; }
    public int? Pomeranians { get; set; }
    public int? Akitas { get; set; }
    public int? Vizslas { get; set; }
    public int? Goldfish { get; set; }
    public int? Trees { get; set; }
    public int? Cars { get; set; }
    public int? Perfumes { get; set; }
}