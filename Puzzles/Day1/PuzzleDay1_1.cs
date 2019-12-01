using System;
using System.Collections.Generic;

public class PuzzleDay1_1 : PuzzleBase
{
    private List<int> inputs = new List<int>();

    protected override string GetPuzzleData()
    {
        return "/day1input.txt";
    }

    protected override void ParseLine(string line)
    {
        inputs.Add(int.Parse(line));
    }

    public override object CalculateSolutions()
    {
        int mass = 0;

        for (int i = 0; i < inputs.Count; i++)
            mass += Day1Methods.GetFuelRequiredForMass(inputs[i]);

        return mass; 
    }
}