
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class PuzzleDay24_2 : PuzzleBase
{
    IntVector2 up = new IntVector2(0, 1);
    IntVector2 down = new IntVector2(0, -1);
    IntVector2 right = new IntVector2(1, 0);
    IntVector2 left = new IntVector2(-1, 0);

    private Dictionary<int, Dictionary<IntVector2, char>> mapPerLevel =new Dictionary<int, Dictionary<IntVector2, char>>();
    private Dictionary<int, Dictionary<IntVector2, char>> newMapPerLevel = new Dictionary<int, Dictionary<IntVector2, char>>();
    int height = 0;

    int lowestDepth = -5;
    int highestDepth = 5;

    public override object CalculateSolutions()
    {
        int highestX = mapPerLevel[0].Keys.OrderBy(_ => _.x).LastOrDefault().x;
        int highestY = mapPerLevel[0].Keys.OrderBy(_ => _.y).LastOrDefault().y;


        newMapPerLevel.Add(0, new Dictionary<IntVector2, char>());
        for(int i = lowestDepth; i <= highestDepth; i++)
        {
            if (i == 0)
                continue;
            AddLevel(i);
        }

        //QuadTree<char> quadTree = new QuadTree<char>(0, 0, 5 * 10, 5 * 10);
        

        for (int r = 0; r < 200; r++)
        {
            
            for(int i = lowestDepth; i <= highestDepth; i++)
            {
                var map = mapPerLevel[i];
                var newMap = newMapPerLevel[i];
                newMap.Clear();
                //DrawMap(i);
                int count = 0;
                for(int y = 0; y < 5 ; y++)
                {
                    for(int x = 0; x < 5; x++)
                    {
                        IntVector2 pos = new IntVector2(x, y);

                        if (x == 2 && y == 2)
                        {
                            newMap[pos] = '?';
                            continue;
                        }
                        
                        int adjacentBugs = AdjacentBugs(pos, i);
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
            }

            var oldMap = mapPerLevel;
            mapPerLevel = newMapPerLevel;
            newMapPerLevel = oldMap;
            
            if (mapPerLevel[lowestDepth].ContainsValue('#'))
            {
                lowestDepth--;
                AddLevel(lowestDepth);
            }
            if (mapPerLevel[highestDepth].ContainsValue('#'))
            {
                highestDepth++;;
                AddLevel(highestDepth);
            }
        }

        return mapPerLevel.Sum(v => v.Value.Count(m => m.Value == '#'));
    }

    private void AddLevel(int level)
    {
        if (mapPerLevel.ContainsKey(level))
            throw new InvalidOperationException();
        mapPerLevel[level] = new Dictionary<IntVector2, char>();
        newMapPerLevel[level] = new Dictionary<IntVector2, char>();
        var map = mapPerLevel[level];
        for(int y = 0; y < 5 ; y++)
        {
            for(int x = 0; x < 5; x++)
            {
                map[new IntVector2(x, y)] = '.';
            }
        }
    }

    private int AdjacentBugs(IntVector2 pos, int level)
    {
        int count = 0;
        count += GetCount(pos, up, level);      
        count += GetCount(pos, down, level); 
        count += GetCount(pos, right, level);
        count += GetCount(pos, left, level);
        return count;
    }

    private int GetCount(IntVector2 pos, IntVector2 diff, int level)
    {
        int count = 0;
        if (pos.x + diff.x == 2 && pos.y + diff.y == 2)
        {
            if (mapPerLevel[level][pos] == '#' && level + 1 > highestDepth)
            {
                highestDepth++;
                AddLevel(highestDepth);
            }
            if (!mapPerLevel.ContainsKey(level + 1))
                return count;
            for(int i = 0; i < 5; i++)
            {
                if (diff.y == -1)
                    count += mapPerLevel[level + 1][new IntVector2(i, 4)] == '#' ? 1 : 0;
                    
                if (diff.y == 1)
                    count += mapPerLevel[level + 1][new IntVector2(i, 0)] == '#' ? 1 : 0;
                    
                if (diff.x == -1)
                    count += mapPerLevel[level + 1][new IntVector2(4, i)] == '#' ? 1 : 0;
                    
                if (diff.x == 1)
                    count += mapPerLevel[level + 1][new IntVector2(0, i)] == '#' ? 1 : 0;

            }
            return count;
        }
        if (!mapPerLevel[level].ContainsKey(pos + diff))
        {
            if (mapPerLevel[level][pos] == '#' && level - 1 < lowestDepth)
            {
                lowestDepth--;
                AddLevel(lowestDepth);
            }
            if (mapPerLevel.ContainsKey(level - 1))
                count += mapPerLevel[level -1][new IntVector2(2, 2) + diff] == '#' ? 1 : 0;
            return count;
        }
        if (mapPerLevel[level].ContainsKey(pos + diff) && mapPerLevel[level][pos + diff] == '#')
            return 1;
        return count;
    }


    private char ToLower(char c)
    {
        return c.ToString().ToLower()[0];
    }

    private void DrawMap(int level)
    { 
        var map = mapPerLevel[level];
        int highestX = map.Keys.OrderBy(_ => _.x).LastOrDefault().x;
        int highestY = map.Keys.OrderBy(_ => _.y).LastOrDefault().y;

        StringBuilder sb = new StringBuilder();
        sb.Append($"DEPTH {level}\n");
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
        if(!mapPerLevel.ContainsKey(0))
            mapPerLevel[0] = new Dictionary<IntVector2, char>();
        int x = 0;
        foreach(var str in line)
        {
            var pos = new IntVector2(x, height);
            mapPerLevel[0].Add(pos, str);
            x++;
        }
        height++;
    }
}