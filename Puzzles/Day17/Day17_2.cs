
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class PuzzleDay17_2 : PuzzleBase
{
    private const float RAD2DEG = 360 / (MathF.PI * 2);

    private List<long> inputs = new List<long>();

    IntVector2 up = new IntVector2(0, 1);
    IntVector2 down = new IntVector2(0, -1);
    IntVector2 right = new IntVector2(1, 0);
    IntVector2 left = new IntVector2(-1, 0);

    private enum Instructions : int
    {
        FORWARD = 0,
        LEFT = 1,
        RIGHT = 2
    }

    public override object CalculateSolutions()
    {
        var comp = new IntCodeComputer(inputs);

        Dictionary<IntVector2, char> tiles = new Dictionary<IntVector2, char>();

        List<IntVector2> walkedTiles = new List<IntVector2>();
        List<Instructions> instructions = new List<Instructions>();

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

        IntVector2 start = tiles.Where(_ => _.Value == '^').FirstOrDefault().Key;

        IntVector2 startLine = start;
        IntVector2 current = start;
        
        float angle = MathF.Atan2(up.y, up.x) * RAD2DEG;
        IntVector2 forward = 
            new IntVector2((int)MathF.Round(MathF.Cos(MathF.PI * angle / 180f)), (int)MathF.Round(MathF.Sin(MathF.PI * angle / 180f)));

        while(!walkedTiles.Contains(current + forward))
        {
            if (!tiles.ContainsKey(current + forward) || tiles[current + forward] != '#')
            {
                float leftAngle = angle + 90f;
                float rightAngle = angle - 90f;

                IntVector2 right = 
                    new IntVector2((int)MathF.Round(MathF.Cos(MathF.PI * rightAngle / 180f)), (int)MathF.Round(MathF.Sin(MathF.PI * rightAngle / 180f)));

                IntVector2 left = 
                    new IntVector2((int)MathF.Round(MathF.Cos(MathF.PI * leftAngle / 180f)), (int)MathF.Round(MathF.Sin(MathF.PI * leftAngle / 180f)));

                if (tiles.ContainsKey(current + right) && tiles[current + right] == '#')
                {
                    angle -= 90f;
                    forward = right;
                    instructions.Add(Instructions.RIGHT);
                    continue;
                }
                if (tiles.ContainsKey(current + left) && tiles[current + left] == '#')
                {
                    angle += 90f;
                    forward = left;
                    instructions.Add(Instructions.LEFT);
                    continue;
                }
                break;
            }
            current += forward;
            walkedTiles.Add(current);
            instructions.Add(Instructions.FORWARD);
        }
        //lines.Add(startLine, current);
        /*int sum = 0;
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
                    IntVector2 top = current;
                    IntVector2 bottom = current;
                    while (tiles[top + up] == '#')
                        top.y += 1;
                    while (tiles[bottom])
                }
            }
        }*/


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

        return instructions.Count;
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