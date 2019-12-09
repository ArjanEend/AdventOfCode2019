
using System;
using System.Collections.Generic;
using System.Linq;

public class PuzzleDay9_1 : PuzzleBase
{
    private List<long> inputs = new List<long>();

    public override object CalculateSolutions()
    {
        IntCodeComputer computer = new IntCodeComputer(inputs, new List<int>(){2});
        while(!computer.done)
        {
            computer.Execute();
            //computer.AddInput(1);
        }

        foreach(var output in computer.output)
        {
            Console.WriteLine(output);
        }

        return computer.output.LastOrDefault();
        
        return "error";
    }

    protected override string GetPuzzleData()
    {
        return "/day9input.txt";
    }

    protected override void ParseLine(string line)
    {
        inputs.AddRange(line.Split(',')
            .Where(str => long.TryParse(str, out long res))
            .Select(str => long.Parse(str)));
    }
}