
using System;
using System.Collections.Generic;
using System.Linq;

public class PuzzleDay16_1 : PuzzleBase
{
    private List<int> values = new List<int>();

    private List<int> basePattern = new List<int>{0, 1, 0, -1};

    public override object CalculateSolutions()
    {
        for(int a = 0; a < 100; a++)
        {
            List<int> newValues = new List<int>(values.Count);
            for (int i = 0; i < values.Count; i++)
            {
                int patternIndex = i;
                List<int> pattern = new List<int>();
                for(int j = 0; j < basePattern.Count; j++)
                {
                    for(int k = 0; k <= patternIndex; k++)
                    {
                        pattern.Add(basePattern[j]);
                    }
                }

                int value = 0;
                for(int j = 0; j < values.Count; j++)
                {
                    int loopedIndex = (j + 1) % pattern.Count;
                    value += (values[j] * pattern[loopedIndex]);
                }
                value %= 10;
                
                newValues.Add((int)MathF.Abs(value));
            }

            values = newValues;
            
            Console.WriteLine($"{values[0]}{values[1]}{values[2]}{values[3]}{values[4]}{values[5]}{values[6]}{values[7]}");
        }

        return $"{values[0]}{values[1]}{values[2]}{values[3]}{values[4]}{values[5]}{values[6]}{values[7]}";
    }

    protected override string GetPuzzleData()
    {
        return "/day16input.txt";
    }

    protected override void ParseLine(string line)
    {
        values.AddRange(line.Select(c => int.Parse(c.ToString())));
    }
}