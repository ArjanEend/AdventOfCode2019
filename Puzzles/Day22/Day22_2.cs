using System;
using System.Collections.Generic;
using System.Linq;

public class PuzzleDay22_2 : PuzzleBase
{
    private enum Instructions 
    {
        CUT = 0,
        DEAL = 1,
        INC = 2,
    }
    //private List<int> cards = new List<int>();
    private long target = 2020;

    private long cardCount = 119315717514047;

    private long repeat = 101741582076661;

    private List<(Instructions, int)> instructions = new List<(Instructions, int)>();

    protected override string GetPuzzleData()
    {
        return "/day22input.txt";
    }

    protected override void ParseLine(string line)
    {
        if (line.Contains("cut "))
        {
            line = line.Replace("cut ", "");
            int amount = int.Parse(line);
            instructions.Add((Instructions.CUT, amount));

            
            return;
        }
        if (line.Contains("deal into new stack"))
        {
            instructions.Add((Instructions.DEAL, 0));
        }
        if (line.Contains("deal with increment"))
        {
            line = line.Replace("deal with increment ", "");
            int amount = int.Parse(line);
            
            instructions.Add((Instructions.INC, amount));
        }
    }

    public override object CalculateSolutions()
    {
        Console.WriteLine(instructions.Count + " instructions");
        for(int i = 1; i < instructions.Count; i++)
        {
            if (instructions[i].Item1 == Instructions.CUT && instructions[i-1].Item1 == Instructions.CUT)
            {
                var tuple = instructions[i - 1];
                tuple.Item2 += instructions[i].Item2;
                instructions[i - 1] = tuple;
                instructions.RemoveAt(i);
            }
            if (instructions[i].Item1 == Instructions.INC && instructions[i-1].Item1 == Instructions.INC)
            {
                var tuple = instructions[i - 1];
                tuple.Item2 *= instructions[i].Item2;
                instructions[i - 1] = tuple;
                instructions.RemoveAt(i);
            }
            /*if (instructions[i].Item1 == Instructions.CUT && instructions[i-1].Item1 == Instructions.DEAL)
            {
                var tuple = instructions[i - 1];
                tuple.Item2 *= instructions[i].Item2;
                instructions[i - 1] = tuple;
                instructions.RemoveAt(i);
            }*/
        }
        
        Console.WriteLine(instructions.Count + " instructions");
        for(int i = 0; i < repeat; i++)
        {
            foreach(var instruction in instructions)
            {
                switch(instruction.Item1)
                {
                    case Instructions.CUT:
                    target = target - instruction.Item2;
                    if (target < 0)
                        target += cardCount;
                    break;
                    case Instructions.DEAL:
                        target = (cardCount - 1) - target;
                    break;
                    case Instructions.INC:
                        target = (target * instruction.Item2) % cardCount;
                    break;
                }
            }
        }
        

        return target;
    }
}