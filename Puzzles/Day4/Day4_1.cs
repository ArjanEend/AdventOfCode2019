using System;
using System.Collections.Generic;

public class PuzzleDay4_1 : PuzzleBase
{
    private int startRange;
    private int endRange;

    protected override string GetPuzzleData()
    {
        return "/day4input.txt";
    }

    protected override void ParseLine(string line)
    {
        var inputs = line.Split('-');
        startRange = int.Parse(inputs[0]);
        endRange = int.Parse(inputs[1]);
    }

    public override object CalculateSolutions()
    {
        int validSolutions = 0;
        for(int i = startRange; i <= endRange; i++)
        {
            int num = i;
            int prevDigit = 0;
            int digits = 0;

            bool hasDouble = false;
            bool isAscending = true;

            while (num != 0)
            {
                prevDigit = num % 10;
                num /= 10;
                var digit = num % 10;

                if (digit > prevDigit)
                {
                    isAscending = false;
                    digits++;
                    break;
                }

                if (prevDigit == digit)
                {
                    hasDouble = true;
                }

                digits++;
            }

            if(hasDouble && isAscending)
                validSolutions++;
        }

        return validSolutions; 
    }
}