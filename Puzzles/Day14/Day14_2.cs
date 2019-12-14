using System;
using System.Collections.Generic;
using System.Linq;

public class PuzzleDay14_2 : PuzzleBase
{
    private Dictionary<Tuple<string, int>, Tuple<string, int>[]> conversions = new Dictionary<Tuple<string, int>, Tuple<string, int>[]>();
    //private Dictionary<string, long> amounts = new Dictionary<string, long>();

    public override object CalculateSolutions()
    {
        string production = "FUEL";
        Dictionary<string, long> amounts = new Dictionary<string, long>();
        amounts["ORE"] = 1000000000000;
        long factor = 1000000;
        //Output of part 1
        while(amounts["ORE"] > 158482)
        {
            Dictionary<string, long> testAmounts = new Dictionary<string, long>();
            Convert("FUEL", factor, testAmounts);
            while(factor > 1 && amounts["ORE"] + testAmounts["ORE"] < 0)
            {
                factor /= 10;
                if (factor <= 0)
                    factor = 1;
                testAmounts.Clear();
                Convert("FUEL", factor, testAmounts);
            }
            Convert("FUEL", factor, amounts);
            //factor *= 10;
        }
        return amounts["FUEL"];
    }

    private void Convert(string name, long amount, Dictionary<string, long> amounts)
    {
        var conversionArr = conversions.Where(kv => kv.Key.Item1 == name);
        if(conversionArr.Count() == 0)
        {
            return;
        }
        var conversion = conversionArr.FirstOrDefault();
        long multiplier = (long)MathF.Ceiling((float)amount / (float)conversion.Key.Item2);

        if (!amounts.ContainsKey(name))
            amounts[name] = 0;

        for (int i = 0; i < conversion.Value.Length; i++)
        {
            var input = conversion.Value[i];

            if (!amounts.ContainsKey(input.Item1))
                    amounts[input.Item1] = 0;
            
            long conversionAmount = multiplier * input.Item2;

            if (amounts[input.Item1] < conversionAmount)
            {
                long requiredAmount = conversionAmount - amounts[input.Item1];
                Convert(input.Item1, requiredAmount, amounts);
            }

            amounts[input.Item1] -= conversionAmount;
        }

        amounts[conversion.Key.Item1] += multiplier * conversion.Key.Item2;
    }

    protected override string GetPuzzleData()
    {
        return "/day14input.txt";
    }

    protected override void ParseLine(string line)
    {
        var inout = line.Split(" => ");
        var inputs = inout[0].Split(", ");
        
        Tuple<string, int>[] inputArr = new Tuple<string, int>[inputs.Length];
        for(int i = 0; i < inputs.Length; i++)
        {
            var item = ParseItem(inputs[i]);
            inputArr[i] = item;
        }

        conversions.Add(ParseItem(inout[1]), inputArr);
    }

    private Tuple<string, int> ParseItem(string item)
    {
        var values = item.Split(" ");

        return new Tuple<string, int>(values[1], int.Parse(values[0]));
    }
}