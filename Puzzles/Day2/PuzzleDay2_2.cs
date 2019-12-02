
using System.Collections.Generic;
using System.Linq;

public class PuzzleDay2_2 : PuzzleBase
{
    private const int desiredOutput = 19690720;
    private List<int> inputs = new List<int>();

    public override object CalculateSolutions()
    {
        for(int noun = 0; noun < 100; noun++)
        {
            for (int verb = 0; verb < 100; verb++)
            {
                var computerInputs = inputs.Select(input => input).ToList();
                computerInputs[1] = noun;
                computerInputs[2] = verb;
                IntCodeComputer computer = new IntCodeComputer(computerInputs);
                computer.Execute();
                if(computerInputs[0] == desiredOutput)
                    return (100 * noun) + verb;
            }
        }
        
        return "error";
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