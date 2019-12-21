using System;
using System.Collections.Generic;
using System.Linq;

public class PathFinder {
    private static IntVector2 up = new IntVector2(0, -1);
    
    private static IntVector2 right = new IntVector2(1, 0);

    public delegate List<IntVector2> TilesDelegate(IntVector2 pos);

    public static List<IntVector2> FindPath(List<IntVector2> allowedTiles, IntVector2 start, IntVector2 end, TilesDelegate extraTilesCallback = null)
    {
        Dictionary<IntVector2, float> g_scores = new Dictionary<IntVector2, float>();
        Dictionary<IntVector2, float> f_scores = new Dictionary<IntVector2, float>();
        Dictionary<IntVector2, IntVector2> navigationMap = new Dictionary<IntVector2, IntVector2>();
         
        g_scores.Add(start, 0);
        f_scores.Add(start, g_scores[start] + RelativeScore(start, end));
 
        List<IntVector2> open = new List<IntVector2>();
        List<IntVector2> closed = new List<IntVector2>();
        open.Add(start);
 
        IntVector2 current = new IntVector2(-999999, -999999);
 
        while (open.Count > 0)
        {
            current = GetBestNode(open, f_scores);
 
            if (current == end)
                return ConstructPath(current, navigationMap);
 
            open.Remove(current);
            closed.Add(current);
 
            var neighBors = GetNeighbors(allowedTiles, current, extraTilesCallback);
            for(int i = 0; i < neighBors.Count; i++)
            {
               IntVector2 neighbor = neighBors[i];
                 
                float g_score = g_scores[current] + 1;//RelativeScore(current, neighbor);
                float f_score = g_score + RelativeScore(neighbor, end);
 
                if (closed.Contains(neighbor) && g_score >= g_scores[neighbor])
                   continue;
 
                if (!open.Contains(neighbor) || g_score < g_scores[neighbor])
                {
                    SetNavigation(navigationMap, neighbor, current);
                    SetScore(g_scores, f_scores, neighbor, g_score, f_score);
 
                    if (!open.Contains(neighbor))
                        open.Add(neighbor);
                }
            }
        }
 
        return ConstructPath(current, navigationMap);
    }

    private static List<IntVector2> GetNeighbors(List<IntVector2> allowedTiles, IntVector2 current, TilesDelegate extraTilesCallback)
    {
        List<IntVector2> returnValue = new List<IntVector2>();
        
        if (allowedTiles.Contains(current + up))
            returnValue.Add(current + up);
        if (allowedTiles.Contains(current - up))
            returnValue.Add(current - up);
        if (allowedTiles.Contains(current + right))
            returnValue.Add(current + right);
        if (allowedTiles.Contains(current - right))
            returnValue.Add(current - right);

        if (extraTilesCallback != null)
            returnValue.AddRange(extraTilesCallback(current));

        return returnValue;
    }

    public static float RelativeScore(IntVector2 start, IntVector2 other)
    {
        return MathF.Abs(start.x - other.x) + MathF.Abs(start.y - other.y);
    }
 
    private static void SetNavigation(Dictionary<IntVector2, IntVector2> navigationMap, IntVector2 to, IntVector2 from)
    {
        if (navigationMap.ContainsKey(to))
            navigationMap[to] = from;
        else
            navigationMap.Add(to, from);
    }
 
    private static void SetScore(Dictionary<IntVector2, float> g_scores, Dictionary<IntVector2, float> f_scores, IntVector2 node, float g_score, float f_score)
    {
        if (g_scores.ContainsKey(node))
        {
            g_scores[node] = MathF.Min(g_score, g_scores[node]);
            f_scores[node] = MathF.Min(f_score, f_scores[node]);
        } else
        {
            g_scores.Add(node, g_score);
            f_scores.Add(node, f_score);
        }
 
    }
 
    private static List<IntVector2> ConstructPath(IntVector2 current, Dictionary<IntVector2, IntVector2> navigationMap)
    {
        List<IntVector2> fullPath = new List<IntVector2>();
        fullPath.Add(current);
         
        while(navigationMap.ContainsKey(current))
        {
            current = navigationMap[current];
            fullPath.Insert(0, current);
        }
 
        return fullPath;
    }
 
    private static IntVector2 GetBestNode(List<IntVector2> nodeList, Dictionary<IntVector2, float> f_scores)
    {
        float lowest = float.MaxValue;
        IntVector2 returnValue = new IntVector2();
        for(int i = 0; i < nodeList.Count; i++)
        {
            if (f_scores.ContainsKey(nodeList[i]) && f_scores[nodeList[i]] < lowest)
            {
                lowest = f_scores[nodeList[i]];
                returnValue = nodeList[i];
            }
        }
 
        return returnValue;
    }
 
}