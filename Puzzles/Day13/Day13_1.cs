
using System;
using System.Collections.Generic;
using System.Linq;

public class PuzzleDay13_1 : PuzzleBase
{
    private List<long> inputs = new List<long>();
    Dictionary<IntVector2, ITile> tiles = new Dictionary<IntVector2, ITile>();

    public override object CalculateSolutions()
    {
        var computer = new IntCodeComputer(inputs);

        computer.Execute();

        for(int i = 0; i < computer.output.Count; i += 3)
        {
            IntVector2 coordinates = new IntVector2((int)computer.output[i], (int)computer.output[i + 1]);

            switch(computer.output[i + 2])
            {
                case 0: continue;
                case 1:
                    tiles[coordinates] = new Wall();
                    break;
                case 2:
                    tiles[coordinates] = new Block();
                    break;
                case 3:
                    tiles[coordinates] = new Paddle();
                    break;
                case 4:
                    tiles[coordinates] = new Ball();
                    break;
            }

        }

        return tiles.Values.Where(t => t is Block).Count();
    }

    protected override string GetPuzzleData()
    {
        return "/day13input.txt";
    }

    protected override void ParseLine(string line)
    {
        inputs.AddRange(line.Split(',')
            .Where(str => long.TryParse(str, out long res))
            .Select(str => long.Parse(str)));
    }
}