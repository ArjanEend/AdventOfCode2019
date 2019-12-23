
using System;
using System.Collections.Generic;
using System.Linq;

public class PuzzleDay19_1 : PuzzleBase
{
    private List<long> inputs = new List<long>();
    Dictionary<IntVector2, int> tiles = new Dictionary<IntVector2, int>();

    public override object CalculateSolutions()
    {

        for(int y = 0; y < 50; y++)
        {
            for (int x = 0; x < 50; x++)
            {
                var computer = new IntCodeComputer(inputs.ToList(), new List<long>{x, y});
        
                computer.Execute();
                tiles.Add(new IntVector2(x, y), (int)computer.output.LastOrDefault());
            }
        }
        return tiles.Values.Where(v => v == 1).Count();

    }

    protected override string GetPuzzleData()
    {
        return "/day19input.txt";
    }

    protected override void ParseLine(string line)
    {
        inputs.AddRange(line.Split(',')
            .Where(str => long.TryParse(str, out long res))
            .Select(str => long.Parse(str)));
    }
}