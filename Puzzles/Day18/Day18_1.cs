
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
        var keyMap = keys.ToDictionary(kv => kv.Value, kv => kv.Key);
        var doors = map.Where(m => m.Value != '#' && m.Value != '.' && m.Value != '@').Where(m => m.Value.ToString() == m.Value.ToString().ToUpper()).ToDictionary(s => s.Key, s => s.Value);
        var validPositions = map.Where(m => m.Value != '#').Select(_ => _.Key).ToList();
        var startPos = map.Where(m => m.Value == '@').FirstOrDefault().Key;

        //Dictionary<char, char> lockedChars = new Dictionary<char, char>();

        Dictionary<Tuple<char, char>, int> costs = new Dictionary<Tuple<char, char>, int>();

        var graph = new Graph<char>();
        graph.AddVertex(map[startPos]);
        foreach(var kv in keys)
            graph.AddVertex(kv.Value);
        
        foreach(var kv in keys)
        {
            var walkableTiles = validPositions.Where(m => !doors.Keys.Contains(m)).ToList();
            var calcPath = PathFinder.FindPath(walkableTiles, startPos, kv.Key);
            if (calcPath.Count > 1 && calcPath.Last() == kv.Key)
            {
                graph.AddEdge(map[startPos], kv.Value);
                costs.Add(new Tuple<char, char>('@', kv.Value), calcPath.Count);
            }
            foreach(var kv2 in keys)
            {
                calcPath = PathFinder.FindPath(validPositions, kv.Key, kv2.Key);
                if (calcPath.Count > 1 && calcPath.Last() == kv2.Key)
                {
                    graph.AddEdge(kv.Value, kv2.Value);
                    costs.Add(new Tuple<char, char>(kv.Value, kv2.Value), calcPath.Count);
                }
            }
        }

        var queue = new Queue<char>();
        queue.Enqueue('@');
        var previous = new Dictionary<char, char>();
        while(queue.Count > 0)
        {
            var item = queue.Dequeue();
            foreach (var neighbour in graph.AdjacencyList[item])
            {
                if(previous.ContainsKey(neighbour))
                    continue;
                
                previous[neighbour] = item;
                queue.Enqueue(neighbour);
            }
        }

        List<List<char>> paths = new List<List<char>>();
        Dictionary<List<char>, int> pathCosts = new Dictionary<List<char>, int>();
        foreach(var kv in keys)
        {
            var path = new List<char>();
            int cost = 0;

            var current = kv.Value;
            while (!current.Equals('@') && previous.ContainsKey(current)) 
            {
                path.Add(current);
                cost += costs[new Tuple<char, char>(previous[current], current)];
                current = previous[current];
            }

            //cost += costs[new Tuple<char, char>('@', current)];
            path.Add('@');
            path.Reverse();

            paths.Add(path);
            pathCosts.Add(path, cost);
        }

        paths = paths.OrderBy(p => pathCosts[p]).ToList();

        /*longestList.AddRange(lockedChars.Select(c => c.Key).Where(c => !longestList.Contains(c)).OrderBy(c => longestList.IndexOf(c)));

        while(keys.Count > 0)
        {
            var walkableTiles = validPositions.Where(p => map[p] == '.' || map[p] == '@' || map[p].ToString().ToLower() == map[p].ToString() || collectedKeys.Contains(map[p].ToString().ToLower()[0])).ToList();

            int shortest = int.MaxValue;
            List<IntVector2> path = null;
            foreach(var kv in lockedChars)
            {
                var calcPath = PathFinder.FindPath(walkableTiles, startPos, keyMap[kv.Key]);
                if (calcPath.Count > 1 && calcPath.Last() == keyMap[ToLower(kv.Key)] && calcPath.Count < shortest)
                {
                    path = calcPath;
                    shortest = path.Count;
                }
            }

            var foundKeys = path.Where(v => keys.ContainsKey(v)).ToList();
            for(int i = 0; i < foundKeys.Count; i++)
            {
                keys.Remove(foundKeys[i]);
                collectedKeys.Add(map[foundKeys[i]]);
            }
            startPos = path.Last();
            steps += path.Count;
        }
        /*for(int i = 0; i < longestList.Count; i++)
        {
            //var walkableTiles = validPositions.Where(p => map[p] == '.' || map[p] == '@' || map[p].ToString().ToLower() == map[p].ToString() || collectedKeys.Contains(map[p].ToString().ToLower()[0])).ToList();

            var calcPath = PathFinder.FindPath(validPositions, startPos, map.Where(m => m.Value == longestList[i]).FirstOrDefault().Key);
            /*if (calcPath.Count > 1 && calcPath.Last() == kv.Key && calcPath.Count < shortest)
            {
                path = calcPath;
                shortest = path.Count;
            }

            startPos = calcPath.Last();
            steps += calcPath.Count;
            keys.Remove(calcPath.Last());
            collectedKeys.Add(map[calcPath.Last()]);
        }*/

        /*
        
        */

        DrawMap(map);

        return steps;
    }

    public class Graph<T> 
    {
        public Graph() {

        }

        public Dictionary<T, HashSet<T>> AdjacencyList { get; } = new Dictionary<T, HashSet<T>>();

        public void AddVertex(T vertex) {
            AdjacencyList[vertex] = new HashSet<T>();
        }

        public void AddEdge(T a, T b)
        {
            AddEdge(new Tuple<T,T>(a, b));
        }

        public void AddEdge(Tuple<T,T> edge) {
            if (AdjacencyList.ContainsKey(edge.Item1) && AdjacencyList.ContainsKey(edge.Item2)) {
                AdjacencyList[edge.Item1].Add(edge.Item2);
                AdjacencyList[edge.Item2].Add(edge.Item1);
            }
        }
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