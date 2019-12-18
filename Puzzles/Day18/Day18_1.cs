
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

public class PuzzleDay18_1 : PuzzleBase
{
    Dictionary<IntVector2, char> map = new Dictionary<IntVector2, char>();
    int height = 0;

    public override object CalculateSolutions()
    {
        int steps = 0;
        List<char> collectedKeys = new List<char>();
        var keys = map.Where(m => m.Value != '#' && m.Value != '.' && m.Value != '@').Where(m => m.Value.ToString() == m.Value.ToString().ToLower()).OrderBy(k => k.Value).ToList();
        var doors = map.Where(m => m.Value != '#' && m.Value != '.' && m.Value != '@').Where(m => m.Value.ToString() == m.Value.ToString().ToUpper()).ToList();
        var validPositions = map.Where(m => m.Value != '#' || collectedKeys.Contains(m.Value.ToString().ToLower()[0])).Select(_ => _.Key).ToList();
        var startPos = map.Where(m => m.Value == '@').FirstOrDefault().Key;
        
        while(keys.Count > 0)
        {
            var walkableTiles = validPositions.Where(p => map[p] == '.' || map[p] == '@' || map[p].ToString().ToLower() == map[p].ToString() || collectedKeys.Contains(map[p].ToString().ToLower()[0])).ToList();

            int shortest = int.MaxValue;
            List<IntVector2> path = null;
            for(int i = 0; i < keys.Count; i++)
            {
                var calcPath = PathFinder.FindPath(walkableTiles, startPos, keys[i].Key);
                if (calcPath.Count > 1 && calcPath.Last() == keys[i].Key && calcPath.Count < shortest)
                {
                    path = calcPath;
                    shortest = path.Count;
                }
            }

            startPos = path.Last();
            steps += path.Count;
            keys.Remove(new KeyValuePair<IntVector2, char>(path.Last(), map[path.Last()]));
            collectedKeys.Add(map[path.Last()]);
        }
        

        DrawMap(map);

        return steps;
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
        return "/day18input.txt";
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