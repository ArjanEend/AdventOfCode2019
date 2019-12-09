
using System;
using System.Collections.Generic;
using System.Linq;

public class PuzzleDay7_1 : PuzzleBase
{
    private List<int> inputs = new List<int>();

    private List<IntCodeComputer> computers = new List<IntCodeComputer>();

    public override object CalculateSolutions()
    {
        int highestResult = 0;
        for(int a = 0; a < 5; a ++)
        {
            for(int b = 0; b < 5; b ++)
        {
            for(int c = 0; c < 5; c ++)
        {
            for(int d = 0; d < 5; d ++)
        {
            for(int e = 0; e < 5; e ++)
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
            int inputValue = 0;
            for(int i = 0; i <values.Count; i++)
            {
                var program = new List<int>(inputs);
                var computerInputs = new List<int>{values[i]};
                IntCodeComputer computer = new IntCodeComputer(program, computerInputs);
                while(!computer.done)
                {
                    computer.AddInput(inputValue);
                    computer.Execute();
                }
                inputValue = (int)computer.output.LastOrDefault();
            }
            if(inputValue > highestResult)
                highestResult = inputValue;
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
            .Where(str => int.TryParse(str, out int res))
            .Select(str => int.Parse(str)));
    }
}