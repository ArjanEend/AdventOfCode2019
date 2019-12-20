
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

public class PuzzleDay20_1 : PuzzleBase
{
    IntVector2 up = new IntVector2(0, 1);
    IntVector2 down = new IntVector2(0, -1);
    IntVector2 right = new IntVector2(1, 0);
    IntVector2 left = new IntVector2(-1, 0);

    Dictionary<IntVector2, char> map = new Dictionary<IntVector2, char>();
    int height = 0;

    public override object CalculateSolutions()
    {

        var letters = map.Where(m => char.IsLetter(m.Value)).Select(m => m.Key).ToList();
        //var origins = letters.Where(l => AdjacentTo());

        return 0;
    }

    private bool AdjacentTo(IntVector2 pos, char c)
    {
        return map[pos + up] == c; 
    }

    private void DrawMap(Dictionary<IntVector2, string> map)
    {
        int lowestX = map.Keys.OrderBy(_ => _.x).FirstOrDefault().x;    
        int lowestY = map.Keys.OrderBy(_ => _.y).FirstOrDefault().y;   
        int highestX = map.Keys.OrderBy(_ => _.x).LastOrDefault().x;
        int highestY = map.Keys.OrderBy(_ => _.y).LastOrDefault().y;

        StringBuilder sb = new StringBuilder();
        sb.Append("\n");
        for(int y = highestY; y >= lowestY ; y--)
        {
            for(int x = highestX; x >= lowestX; x--)
            {
                var pos = new IntVector2(x, y);
                sb.Append(map.ContainsKey(pos) ? map[pos] : " ");
            }
            sb.Append("\n");
        }

        Console.Write(sb.ToString()); 
    }

    protected override string GetPuzzleData()
    {
        return "/day15input.txt";
    }

    protected override void ParseLine(string line)
    {
        int x = 0;
        foreach(var str in line)
        {
            var pos = new IntVector2(x, height);
            map.Add(pos, str);
            x++;
        }
        height++;
    }
}