
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

public class PuzzleDay24_1 : PuzzleBase
{
    IntVector2 up = new IntVector2(0, 1);
    IntVector2 down = new IntVector2(0, -1);
    IntVector2 right = new IntVector2(1, 0);
    IntVector2 left = new IntVector2(-1, 0);

    Dictionary<IntVector2, char> map = new Dictionary<IntVector2, char>();
    int height = 0;
    private Dictionary<IntVector2, char> keys;
    private Dictionary<IntVector2, char> doors;

    public override object CalculateSolutions()
    {
        int highestX = map.Keys.OrderBy(_ => _.x).LastOrDefault().x;
        int highestY = map.Keys.OrderBy(_ => _.y).LastOrDefault().y;

        Dictionary<IntVector2, char> newMap = new Dictionary<IntVector2, char>();

        List<ulong> ratings = new List<ulong>();

        ulong rating = 0;
        while(!ratings.Contains(rating))
        {
            newMap.Clear();
            ratings.Add(rating);
            rating = 0;
            DrawMap(map);
            int count = 0;
            for(int y = 0; y <= highestY ; y++)
            {
                for(int x = 0; x <= highestX; x++)
                {
                    IntVector2 pos = new IntVector2(x, y);
                    
                    if (map[pos] == '#')
                        rating |= (ulong)1 << count;
                    int adjacentBugs = AdjacentBugs(pos);
                    newMap[pos] = map[pos];
                    if (map[pos] == '#')
                    {
                        if(adjacentBugs != 1)
                            newMap[pos] = '.';
                    } else {
                        if (adjacentBugs == 1 || adjacentBugs == 2)
                            newMap[pos] = '#';
                    }   

                    count++;
                }
            }
            var oldMap = map;
            map = newMap;
            newMap = oldMap;
        }


        return rating;
    }

    private int AdjacentBugs(IntVector2 pos)
    {
        int count = 0;
        if (map.ContainsKey(pos + up) && map[pos + up] == '#')
            count++;
        if (map.ContainsKey(pos - up) && map[pos - up] == '#')
            count++;
        if (map.ContainsKey(pos + right) && map[pos + right] == '#')
            count++;
        if (map.ContainsKey(pos - right) && map[pos - right] == '#')
            count++;
        return count;
    }


    private char ToLower(char c)
    {
        return c.ToString().ToLower()[0];
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
        return "/day24input.txt";
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