
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

public class PuzzleDay18_2 : PuzzleBase
{
    Dictionary<IntVector2, char> map = new Dictionary<IntVector2, char>();
    int height = 0;
    private Dictionary<IntVector2, char> keys;
    private Dictionary<IntVector2, char> doors;
    private Dictionary<char, IntVector2> lookup;
    private Dictionary<(char, char), List<IntVector2>> cachedPaths = new Dictionary<(char, char), List<IntVector2>>();

    private List<char> robots = new List<char>{'1', '2', '3', '4'};

    public override object CalculateSolutions()
    {
        int steps = 0;
        List<char> collectedKeys = new List<char>();
        keys = map.Where(m => m.Value != '#' && m.Value != '.' && m.Value != '@').Where(m => m.Value.ToString() == m.Value.ToString().ToLower()).ToDictionary(s => s.Key, s => s.Value);
        var keyMap = keys.ToDictionary(kv => kv.Value, kv => kv.Key);
        doors = map.Where(m => m.Value != '#' && m.Value != '.' && m.Value != '@').Where(m => m.Value.ToString() == m.Value.ToString().ToUpper()).ToDictionary(s => s.Key, s => s.Value);

        var startPos = map.Where(m => m.Value == '@').FirstOrDefault().Key;
        var nextPos = startPos;
        nextPos.y = 0;
        while(map.ContainsKey(nextPos))
        {
            map[nextPos] = '#';
            nextPos.y ++;
        }
        nextPos = startPos;
        nextPos.x = 0;
        while(map.ContainsKey(nextPos))
        {
            map[nextPos] = '#';
            nextPos.x ++;
        }
        
        var robotPos = startPos;
        robotPos.x -= 1;
        robotPos.y -= 1;
        map[robotPos] = '1';
        robotPos.x += 2;
        map[robotPos] = '2';
        robotPos.y += 2;
        map[robotPos] = '3';
        robotPos.x -= 2;
        map[robotPos] = '4';

        lookup = map.Where(m => m.Value != '#' && m.Value != '.').ToDictionary(v => v.Value, v => v.Key);
        DrawMap(map);
        
        var validPositions = map.Where(m => m.Value != '#').Select(_ => _.Key).ToList();

        //Dictionary<char, char> lockedChars = new Dictionary<char, char>();
        for (int i = 0; i < robots.Count; i++)
        {
            foreach(var kv in keys)
            {
                var path = PathFinder.FindPath(validPositions, lookup[robots[i]], kv.Key);
                if(path.Last() != kv.Key)
                    continue;
                cachedPaths.Add((robots[i], kv.Value), path);
                cachedPaths.Add((kv.Value, robots[i]), path);
            }
        }
        
        foreach(var kv in keys)
        {
            foreach(var kv2 in keys)
            {
                    if (kv.Key == kv2.Key || cachedPaths.ContainsKey((kv.Value, kv2.Value)))
                        continue;
                    var path = PathFinder.FindPath(validPositions, kv.Key, kv2.Key);
                    if(path.Last() != kv2.Key)
                        continue;
                    cachedPaths.Add((kv2.Value, kv.Value), path);
                    cachedPaths.Add((kv.Value, kv2.Value), path);
            }
        }

        var state = new State("1234", "", 0);
        var visited = new Dictionary<(string, string), int>();
        PopulateEdges(validPositions, state, visited);
        
        
        var previous = new Dictionary<State, State>();
        /*while(queue.Count > 0)
        {
            var item = queue.Dequeue();
            foreach (var neighbour in graph.AdjacencyList[item])
            {
                if(previous.ContainsKey(neighbour))
                    continue;
                
                previous[neighbour] = item;
                queue.Enqueue(neighbour);
            }
        }*/

        var bestState = visited.Keys.Where(s => s.Item2.Length == keys.Count).OrderBy(s => visited[s]).FirstOrDefault();

        //paths = paths.OrderBy(p => pathCosts[p]).ToList();

        //DrawMap(map);

        return visited[bestState];
    }

    private void PopulateEdges(List<IntVector2> validPositions, State startState, Dictionary<(string, string), int> visited)
    {
        var queue = new Queue<State>();
        queue.Enqueue(startState);
        while(queue.Count > 0)
        {
            var state = queue.Dequeue();
            for (int i = 0; i < robots.Count; i++)
            {
                var startPos = lookup[robots[i]];
                foreach(var kv in keys)
                {
                    if (state.keys.Contains(kv.Value))
                        continue;
                    //var walkableTiles = validPositions.Where(m => state.keys.ToUpper().Contains(map[m]) || !doors.Keys.Contains(m)).ToList();
                    if (!cachedPaths.ContainsKey((state.current[i], kv.Value)))
                        continue;

                    var calcPath = cachedPaths[(state.current[i], kv.Value)];
                    var filterTiles = doors.Where(d => !state.keys.ToUpper().Contains(d.Value)).Select(d => d.Key).ToList();
                    if (calcPath.Count > 1 && calcPath.Intersect(filterTiles).Count() == 0)
                    {
                        if (state.keys.Length < 2)
                            Console.Write($"Retrieving key {kv.Value}");
                        var newState = state;
                        newState.keys = String.Concat((state.keys + kv.Value).OrderBy(c => c));
                        newState.steps += calcPath.Count - 1;
                        newState.current = newState.current.Remove(i, 1).Insert(i, kv.Value.ToString());

                        var tuple = (newState.current, newState.keys);
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
            }
        }
    }

    public class Graph<T> 
    {
        public Graph() {

        }

        public Dictionary<T, HashSet<T>> AdjacencyList { get; } = new Dictionary<T, HashSet<T>>();

        public void AddVertex(T vertex) {
            AdjacencyList[vertex] = new HashSet<T>();
        }

        public void AddEdge(T a, T b, bool bidir = false)
        {
            AddEdge(new Tuple<T,T>(a, b), bidir);
        }

        public void AddEdge(Tuple<T,T> edge, bool bidir = false) {
            if (AdjacencyList.ContainsKey(edge.Item1)) {
                AdjacencyList[edge.Item1].Add(edge.Item2);
            }
            if (bidir)
                AddEdge(edge.Item2, edge.Item1, false);
        }
    }

    private struct State
    {
        public string current;
        public string keys;
        public int steps;

        public State(string current, string keys, int steps)
        {
            this.current = current;
            this.keys = keys;
            this.steps = steps;
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