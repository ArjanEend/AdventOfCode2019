
using System;
using System.Collections.Generic;
using System.Linq;

public class PuzzleDay16_2 : PuzzleBase
{
    private List<int> values = new List<int>();

    private List<int> basePattern = new List<int>{0, 1, 0, -1};

    public override object CalculateSolutions()
    {
        List<int> copy = values.ToList();

        var range = int.Parse($"{copy[0]}{copy[1]}{copy[2]}{copy[3]}{copy[4]}{copy[5]}{copy[6]}");
        int length = values.Count * 10000;

        values.Clear();
        for(int i = range; i < length; i++)
        {
            values.Add(copy[i % copy.Count]);
        }
        
        List<int> newValues = new List<int>(values);
        for(int a = 0; a < 100; a++)
        {
            for (int i = values.Count - 2; i >= 0; i--)
            {
                //value[i] + newValue[i + 1] % 10;
                int value = 0;
                value = values[i] + newValues[i + 1];
                value %= 10;
                
                newValues[i] = ((int)MathF.Abs(value));
            }

            values = newValues;
            
            Console.WriteLine($"{values[0]}{values[1]}{values[2]}{values[3]}{values[4]}{values[5]}{values[6]}{values[7]}");
        }


        //values.RemoveRange(0, range);

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