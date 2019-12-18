
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
        var keys = map.Where(m => m.Value != '#' && m.Value != '.' && m.Value != '@').Where(m => m.Value.ToString() == m.Value.ToString().ToLower()).ToDictionary(s => s.Key, s => s.Value);
        var doors = map.Where(m => m.Value != '#' && m.Value != '.' && m.Value != '@').Where(m => m.Value.ToString() == m.Value.ToString().ToUpper()).ToDictionary(s => s.Key, s => s.Value);
        var validPositions = map.Where(m => m.Value != '#').Select(_ => _.Key).ToList();
        var startPos = map.Where(m => m.Value == '@').FirstOrDefault().Key;

        Dictionary<char, char> lockedChars = new Dictionary<char, char>();
        
        foreach(var kv in keys)
        {
            var calcPath = PathFinder.FindPath(validPositions, startPos, kv.Key);
            if (calcPath.Count > 1 && calcPath.Last() == kv.Key)
            {
                while(calcPath.Count > 0 && !doors.ContainsKey(calcPath.Last()))
                    calcPath.RemoveAt(calcPath.Count - 1);
                if(calcPath.Count > 0)
                    lockedChars.Add(kv.Value, doors[calcPath.Last()]);
            }
        }

        List<char> priorityList = new List<char>();
        foreach(var kv in lockedChars)
        {
            var value = ToLower(kv.Value);
            while (lockedChars.ContainsKey(value))
            {
                value = ToLower(lockedChars[value]);
                if(priorityList.Contains(value) || !lockedChars.ContainsKey(value))
                    continue;
                priorityList.Insert(0, value);
            }
        }

        while(keys.Count > 0)
        {
            var walkableTiles = validPositions.Where(p => map[p] == '.' || map[p] == '@' || map[p].ToString().ToLower() == map[p].ToString() || collectedKeys.Contains(map[p].ToString().ToLower()[0])).ToList();

            int shortest = int.MaxValue;
            List<IntVector2> path = null;
            foreach(var kv in keys)
            {
                var calcPath = PathFinder.FindPath(walkableTiles, startPos, kv.Key);
                if (calcPath.Count > 1 && calcPath.Last() == kv.Key && calcPath.Count < shortest)
                {
                    path = calcPath;
                    shortest = path.Count;
                }
            }

            startPos = path.Last();
            steps += path.Count;
            keys.Remove(path.Last());
            collectedKeys.Add(map[path.Last()]);
        }
        

        DrawMap(map);

        return steps;
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