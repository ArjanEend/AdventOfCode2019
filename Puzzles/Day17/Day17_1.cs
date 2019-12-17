
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class PuzzleDay17_1 : PuzzleBase
{
    private List<long> inputs = new List<long>();

    IntVector2 up = new IntVector2(0, 1);
    IntVector2 down = new IntVector2(0, -1);
    IntVector2 right = new IntVector2(1, 0);
    IntVector2 left = new IntVector2(-1, 0);

    public override object CalculateSolutions()
    {
        var comp = new IntCodeComputer(inputs);

        Dictionary<IntVector2, char> tiles = new Dictionary<IntVector2, char>();

        Dictionary<IntVector2, IntVector2> lines = new Dictionary<IntVector2, IntVector2>();

        comp.Execute();

        int x = 0;
        int y = 0;
        for(int i = 0; i < comp.output.Count; i++)
        {
            IntVector2 coord = new IntVector2(x, y);
            char tile = (char)comp.output[i];
            tiles.Add(coord, tile);

            x++;
            if(tile == '\n')
            {
                x = 0;
                y++;
            }
        }

        
        int highestX = tiles.Keys.OrderBy(_ => _.x).LastOrDefault().x;
        int highestY = tiles.Keys.OrderBy(_ => _.y).LastOrDefault().y;

        int sum = 0;
        for(y = 0; y <= highestY; y++)
        {
            for(x = 0; x <= highestX; x++)
            {
                IntVector2 current = new IntVector2(x, y);
                if(tiles.ContainsKey(current) && tiles[current] == '#' && tiles.ContainsKey(current + up) && tiles.ContainsKey(current + down) && tiles.ContainsKey(current + left) && tiles.ContainsKey(current + right)
                    && tiles[current + up] == '#' && tiles[current + down] == '#' && tiles[current + left] == '#' && tiles[current + right] == '#')
                {
                    sum += x * y;
                    tiles[current] = 'O';
                    //IntVector2 top = current;
                    //while (tiles[top + up] == '#')
                    //    top.y += 1;
                }
            }
        }


        StringBuilder sb = new StringBuilder();
        sb.Append("\n");
        for(y = 0; y <= highestY; y++)
        {
            for(x = 0; x <= highestX; x++)
            {
                var pos = new IntVector2(x, y);
                sb.Append(tiles.ContainsKey(pos) ? tiles[pos] : ' ');
            }
            sb.Append("\n");
        }

        Console.WriteLine(sb.ToString());

        return sum;
    }

    protected override string GetPuzzleData()
    {
        return "/day17input.txt";
    }

    protected override void ParseLine(string line)
    {
        inputs.AddRange(line.Split(',')
            .Where(str => long.TryParse(str, out long res))
            .Select(str => long.Parse(str)));
    }
}