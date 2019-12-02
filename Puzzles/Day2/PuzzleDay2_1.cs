
using System.Collections.Generic;
using System.Linq;

public class PuzzleDay2_1 : PuzzleBase
{
    private List<int> inputs = new List<int>();

    public override object CalculateSolutions()
    {
        inputs[1] = 12;
        inputs[2] = 2;
        IntCodeComputer computer = new IntCodeComputer(inputs);
        computer.Execute();
        return inputs[0];
    }

    protected override string GetPuzzleData()
    {
        return "/day2input.txt";
    }

    protected override void ParseLine(string line)
    {
        inputs.AddRange(line.Split(',')
            .Where(str => int.TryParse(str, out int res))
            .Select(str => int.Parse(str)));
    }
}