
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

public class PuzzleDay20_2 : PuzzleBase
{
    IntVector2 up = new IntVector2(0, 1);
    IntVector2 down = new IntVector2(0, -1);
    IntVector2 right = new IntVector2(1, 0);
    IntVector2 left = new IntVector2(-1, 0);

    Dictionary<IntVector2, char> map = new Dictionary<IntVector2, char>();
    int height = 0;

    string nonmap = "# .";
    string tileTypes = ".";
    private Dictionary<IntVector2, IntVector2> connections;
    private List<IntVector2> tiles;

    private struct State
    {
        public IntVector2 pos;
        public int steps;
        public int level;

        public State(IntVector2 pos, int steps, int level)
        {
            this.pos = pos;
            this.steps = steps;
            this.level = level;
        }
    }

    private List<IntVector2> GetNeighbours(IntVector2 v)
    {
        var returnValue = new List<IntVector2>();

        if (tiles.Contains(v + up))
            returnValue.Add(v + up);
        if (tiles.Contains(v - up))
            returnValue.Add(v - up);
        if (tiles.Contains(v + right))
            returnValue.Add(v + right);
        if (tiles.Contains(v - right))
            returnValue.Add(v - right);

        if (!connections.ContainsKey(v))
            return returnValue;
        returnValue.Add(connections[v]);
        returnValue.Remove(v);

        return returnValue;
    }

    public override object CalculateSolutions()
    {
        var letters = map.Where(m => char.IsLetter(m.Value)).Select(m => m.Key).ToList();
        tiles = map.Where(m => m.Value == '.').Select(v => v.Key).ToList();
        var origins = letters.Where(l => AdjacentTo(l, '.')).ToList();
        connections = new Dictionary<IntVector2, IntVector2>();
        
        var portalToTile = origins.ToDictionary(l => l, l => AdjacentTile(l));
        for(int i = 0; i < origins.Count; i++)
        {
            var letter = map[origins[i]];
            var adjacent = AdjacentLetter(origins[i]);
            if (letter == adjacent)
                continue;
            var portalPositions = origins.Where(o => o != origins[i] && 
                ((map[o] == letter && AdjacentLetter(o) == adjacent) || (map[o] == adjacent && AdjacentLetter(o) == letter)));

            if (portalPositions.Count() >= 2)
            {
                Console.WriteLine(letter + " " + adjacent);
            }

            var portalPos = portalPositions.FirstOrDefault();

            connections.Add(AdjacentTile(origins[i]), AdjacentTile(portalPos));
        }

        var startState = new State(AdjacentTile(origins.Where(o => map[o] == 'A' && AdjacentLetter(o) == 'A').FirstOrDefault()), 0, 0);
        
        var visited = new Dictionary<(IntVector2, int), int>();
        var queue = new Queue<State>();
        queue.Enqueue(startState);

        var target = (AdjacentTile(origins.Where(o => map[o] == 'Z' && AdjacentLetter(o) == 'Z').FirstOrDefault()), 0);

        while(!visited.ContainsKey(target))
        {   
            queue = new Queue<State>(queue.OrderBy(q => q.level));
            var state = queue.Dequeue();
            var startPos = state.pos;
            var neighbours = GetNeighbours(startPos);
            foreach(var neighbour in neighbours)
            {
                var newState = state;
                newState.steps += 1;
                newState.pos = neighbour;
                if ((startPos - newState.pos).Magnitude() > 1)
                {
                    if (neighbour.x < 3 || neighbour.x > 119 || neighbour.y < 4 || neighbour.y > 118)
                    {
                        newState.level += 1;
                    } else {
                        newState.level -= 1;
                    }
                }
                if (newState.level < 0 || newState.level > 50)
                    continue;
                
                var tuple = (neighbour, newState.level);
                if (visited.ContainsKey(tuple))
                {
                    if (visited[tuple] <= newState.steps)
                        continue;
                    visited[tuple] = newState.steps;
                } else{
                    visited.Add(tuple, newState.steps);
                }

                queue.Enqueue(newState);
            }
        }
        
        return visited[target];
    }

    private bool AdjacentTo(IntVector2 pos, char c)
    {
        return map[pos + up] == c || map[pos + left] == c || map[pos - up] == c || map[pos - left] == c; 
    }

    private IntVector2 AdjacentTile(IntVector2 pos)
    {
        if (tileTypes.Contains(map[pos + up]))
            return pos + up;
        if (tileTypes.Contains(map[pos - up]))
            return pos - up;
        if (tileTypes.Contains(map[pos + right]))
            return pos + right;
        if (tileTypes.Contains(map[pos - right]))
            return pos - right;
        return new IntVector2();
    }

    private char AdjacentLetter(IntVector2 pos)
    {
        if (!nonmap.Contains(map[pos + up]))
            return map[pos + up];
        if (!nonmap.Contains(map[pos - up]))
            return map[pos - up];
        if (!nonmap.Contains(map[pos + right]))
            return map[pos + right];
        if (!nonmap.Contains(map[pos - right]))
            return map[pos - right];
        return ' ';
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
        return "/day20input.txt";
    }

    protected override void ParseLine(string line)
    {
        map.Add(new IntVector2(-1, height), ' ');
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