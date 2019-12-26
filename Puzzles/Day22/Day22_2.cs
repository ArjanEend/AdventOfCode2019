using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

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

    private BigInteger cardCount = 119315717514047;

    private BigInteger repeat = 101741582076661;

    private BigInteger offset = 0;
    private BigInteger increment = 1;

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

        //for(int i = 0; i < repeat; i++)
        //{
            foreach(var instruction in instructions)
            {
                switch(instruction.Item1)
                {
                    case Instructions.CUT:
                    offset += instruction.Item2 * increment;
                    break;
                    case Instructions.DEAL:
                        increment *= -1;
                        offset += increment;
                    break;
                    case Instructions.INC:
                        increment *= BigInteger.ModPow(new BigInteger(instruction.Item2), cardCount - 2, cardCount);
                    break;
                }
                increment %= cardCount;
                offset %= cardCount;
            }
        //}
        var oldInc = increment;
        increment = BigInteger.ModPow(oldInc, repeat, cardCount);
        offset = offset * (1 - increment) * BigInteger.ModPow(((1 - oldInc) % cardCount), cardCount - 2, cardCount);

        offset %= cardCount;

        var card = (offset + target * increment) % cardCount;

        return card;
    }
}