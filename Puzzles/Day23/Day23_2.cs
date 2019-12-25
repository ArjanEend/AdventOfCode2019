
using System;
using System.Collections.Generic;
using System.Linq;

public class PuzzleDay23_2 : PuzzleBase
{
    private List<long> inputs = new List<long>();

    private List<IntCodeComputer> computers = new List<IntCodeComputer>();

    private Stack<(long, long)> natPackets = new Stack<(long, long)>();

    public override object CalculateSolutions()
    {   
        var outputIndexes = new List<int>();
        var done = new List<bool>();

        for(int i = 0; i < 50; i++)
        {
            computers.Add(new IntCodeComputer(inputs.ToList(), new List<long>{i}));
        }

        long lastY = 0;
        while(true)
        {
            bool idle = true;
            for(int i = 0; i < computers.Count; i++)
            {
                var comp = computers[i];
                if (!ExeCuteComputer(comp))
                    idle = false;

                if(!comp.InputExhausted)
                {
                    comp.AddInput(-1);
                }
            }

            if (!idle || natPackets.Count == 0)
                continue;
            
            var pack = natPackets.Pop();
            computers[0].AddInput(pack.Item1);
            computers[0].AddInput(pack.Item2);
            if(lastY == pack.Item2)
                return lastY;
            lastY = pack.Item2;
        }
        
        return 0;
    }

    private bool ExeCuteComputer(IntCodeComputer comp)
    {
        comp.Execute();

        bool exhausted = comp.InputExhausted;

        if (comp.InputExhausted)             
            comp.AddInput(-1);

        while(comp.output.Count >= 3)
        {
            exhausted = false;
            if (comp.output[0] == 255)
            {
                natPackets.Push((comp.output[1], comp.output[2]));
                comp.output.RemoveRange(0, 3);
                continue;
            }
            int address = (int)comp.output[0];
            computers[address].AddInput(comp.output[1]);        
            computers[address].AddInput(comp.output[2]);
            comp.output.RemoveRange(0, 3);
            //exhausted = ExeCuteComputer(computers[address]);
        }
        return exhausted;
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