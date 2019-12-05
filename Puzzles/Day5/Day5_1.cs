
using System.Collections.Generic;
using System.Linq;

public class PuzzleDay5_1 : PuzzleBase
{
    private const int desiredOutput = 19690720;
    private List<int> inputs = new List<int>();

    public override object CalculateSolutions()
    {
        IntCodeComputer computer = new IntCodeComputer(inputs);
        computer.Execute();
        
        return "error";
    }

    protected override string GetPuzzleData()
    {
        return "/day5input.txt";
    }

    protected override void ParseLine(string line)
    {
        inputs.AddRange(line.Split(',')
            .Where(str => int.TryParse(str, out int res))
            .Select(str => int.Parse(str)));
    }
}