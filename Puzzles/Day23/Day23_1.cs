
using System;
using System.Collections.Generic;
using System.Linq;

public class PuzzleDay23_1 : PuzzleBase
{
    private List<long> inputs = new List<long>();

    private List<IntCodeComputer> computers = new List<IntCodeComputer>();

    public override object CalculateSolutions()
    {   
        var outputIndexes = new List<int>();
        var done = new List<bool>();

        for(int i = 0; i < 50; i++)
        {
            computers.Add(new IntCodeComputer(inputs.ToList(), new List<long>{i}));
        }

        while(true)
        {
            for(int i = 0; i < computers.Count; i++)
            {
                var comp = computers[i];
                long output = ExeCuteComputer(comp);
                if (output != 0)
                    return output;   
            }
        }
        
        return 0;
    }

    private long ExeCuteComputer(IntCodeComputer comp)
    {
        comp.Execute();

        if (comp.InputExhausted)                 
            comp.AddInput(-1);

        while(comp.output.Count >= 3)
        {
            if (comp.output[0] == 255)
                return comp.output[2];
            int address = (int)comp.output[0];
            computers[address].AddInput(comp.output[1]);        
            computers[address].AddInput(comp.output[2]);
            comp.output.RemoveRange(0, 3);
            
            return ExeCuteComputer(computers[address]);
        }
        return 0;
    }

    protected override string GetPuzzleData()
    {
        return "/day23input.txt";
    }

    protected override void ParseLine(string line)
    {
        inputs.AddRange(line.Split(',')
            .Where(str => long.TryParse(str, out long res))
            .Select(str => long.Parse(str)));
    }
}