
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

public class PuzzleDay25_1 : PuzzleBase
{
    private List<long> inputs = new List<long>();

    IntVector2 up = new IntVector2(0, 1);
    IntVector2 down = new IntVector2(0, -1);
    IntVector2 right = new IntVector2(1, 0);
    IntVector2 left = new IntVector2(-1, 0);

    public override object CalculateSolutions()
    {
        var comp = new IntCodeComputer(inputs);

        comp.Execute();

        StringBuilder sb = new StringBuilder();
        sb.Append("\n");
        for (int i = 0; i < comp.output.Count; i++)
            sb.Append((char)comp.output[i]);

        Console.WriteLine(sb.ToString());

        while(true)
        {
            var str = Console.ReadLine();

            foreach(var c in str)
                comp.AddInput(c);
            comp.AddInput(10);

            comp.Execute();

            sb = new StringBuilder();
            sb.Append("\n");
            for (int i = 0; i < comp.output.Count; i++)
                sb.Append((char)comp.output[i]);

            Console.WriteLine(sb.ToString());

            comp.output.Clear();
        }

        return comp.output.LastOrDefault();
    }

    protected override string GetPuzzleData()
    {
        return "/day25input.txt";
    }

    protected override void ParseLine(string line)
    {
        inputs.AddRange(line.Split(',')
            .Where(str => long.TryParse(str, out long res))
            .Select(str => long.Parse(str)));
    }
}