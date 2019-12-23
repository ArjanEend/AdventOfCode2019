
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class PuzzleDay19_2 : PuzzleBase
{
    private List<long> inputs = new List<long>();
    Dictionary<IntVector2, char> tiles = new Dictionary<IntVector2, char>();

    private int TestPos(int x, int y)
    {
        IntVector2 pos = new IntVector2(x, y);
        var computer = new IntCodeComputer(inputs.ToList(), new List<long>{x, y});

        computer.Execute();

        return (int)computer.output.LastOrDefault();
    }

    public override object CalculateSolutions()
    {
        int x = 500;
        int y = 0;

        while(true) 
        {
			if (TestPos(x, y) == 1) 
            {
				if (TestPos(x-99, y+99) == 1)
                    return 10000 * (x - 99) + y;

                x++;
                continue;
			}
            y++;
		}

        /*int maxX = 9999;
        int maxY = 9999;
        bool didX = false;
        while(x <= maxX || y <= maxY)
        {
            didX = !didX;
            int midX = (x + maxX) / 2;
            int midY = (y + maxY) / 2;
            if (TestPos(midX, midY) == 1)
            {
                if (!didX && TestPos(midX + 100, midY) == 1)
                {
                    maxX = midX - 1;
                    continue;
                }
                if (didX && TestPos(midX, midY + 100) == 1)
                {
                   maxY = midY - 1;
                    continue;
                }
            }
            if (didX)
                x = midX + 1;
            else
                y = midY + 1;
        }*/

    }

    private void DrawMap(Dictionary<IntVector2, char> map)
    {
        int highestX = map.Keys.OrderBy(_ => _.x).LastOrDefault().x;
        int highestY = map.Keys.OrderBy(_ => _.y).LastOrDefault().y;

        StringBuilder sb = new StringBuilder();
        sb.Append("\n");
        for(int y = 0; y <= highestY ; y++)
        {
            for(int x = 0; x <= highestX; x++)
            {
                var pos = new IntVector2(x, y);
                sb.Append(map.ContainsKey(pos) ? map[pos] : ' ');
            }
            sb.Append("\n");
        }

        Console.Write(sb.ToString()); 
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