
using System;
using System.Collections.Generic;
using System.Linq;

public class PuzzleDay7_2 : PuzzleBase
{
    private List<long> inputs = new List<long>();

    private List<IntCodeComputer> computers = new List<IntCodeComputer>();

    public override object CalculateSolutions()
    {
        int highestResult = 0;
        for(int a = 9; a >= 5; a--)
        {
            for(int b = 9; b >= 5; b --)
        {
            for(int c = 9; c >= 5; c --)
        {
            for(int d = 9; d >= 5; d --)
        {
            for(int e = 9; e >= 5; e --)
        {
            if(a == b || a == c || a == d || a == e)
            continue;
            if(b == c || b == d || b ==e)
            continue;
            if(c == d || c == e)
            continue;
            if(d == e)
            continue;
            var values = new List<int>{a,b,c,d,e};    
                   
            var computers = new List<IntCodeComputer>();
            var outputIndexes = new List<int>();
            var done = new List<bool>();
            for(int i = 0; i <values.Count; i++)
            {
                var computerInputs = new List<long>();
                computerInputs.Add(values[i]);
                if(i == 0)
                    computerInputs.Add(0);
                var program = new List<long>(inputs);
                IntCodeComputer computer = new IntCodeComputer(program, computerInputs);
                computers.Add(computer);
                outputIndexes.Add(0);
                done.Add(false);
            }

            int computerIndex = 0;
            int prevComputerIndex = 4;
            int loops = 0;
            while (!computers.LastOrDefault().done)
            {
                if (computers[prevComputerIndex].output.Count > outputIndexes[prevComputerIndex])
                {
                    computers[computerIndex].AddInput((int)computers[prevComputerIndex].output[outputIndexes[prevComputerIndex]]);
                    outputIndexes[prevComputerIndex]++;
                }

                computers[computerIndex].Execute();
                done[computerIndex] = computers[computerIndex].done;
                prevComputerIndex = computerIndex;
                computerIndex++;
                computerIndex %= computers.Count;
                loops++;
            }
            
            if(computers.LastOrDefault().output.LastOrDefault() > highestResult)
                highestResult = (int)computers.LastOrDefault().output.LastOrDefault();
        }
        }
        }
        }
        }
        
        
        return highestResult;
    }

    protected override string GetPuzzleData()
    {
        return "/day7input.txt";
    }

    protected override void ParseLine(string line)
    {
        inputs.AddRange(line.Split(',')
            .Where(str => long.TryParse(str, out long res))
            .Select(str => long.Parse(str)));
    }
}