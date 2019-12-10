
using System;
using System.Collections.Generic;
using System.Linq;

public class PuzzleDay10_1 : PuzzleBase
{
    private const float RAD2DEG = 360 / (MathF.PI * 2);

    private List<List<bool>> asteroids = new List<List<bool>>();
    private List<IntVector2> asteroidPositions = new List<IntVector2>();

    public override object CalculateSolutions()
    {
        for(int y = 0; y < asteroids.Count; y++)
        {
            for(int x = 0; x < asteroids[y].Count; x ++)
            {
                bool isAsteroid = asteroids[y][x];
                if (!isAsteroid)
                    continue;
                asteroidPositions.Add(new IntVector2(x, y));
            }
        }

        int highest = int.MinValue;
        foreach (var posA in asteroidPositions)
        {
            Dictionary<float, float> blockedAngles = new Dictionary<float, float>();
            foreach (var posB in asteroidPositions)
            {
                if(posA == posB)
                    continue;
                
                float angle = MathF.Atan2(posB.y - posA.y, posB.x - posA.x) * RAD2DEG;
                
                if (blockedAngles.ContainsKey(angle))
                {
                    continue;
                }
                blockedAngles.Add(angle, (posA - posB).Magnitude());
            }
            if (highest < blockedAngles.Count)
                highest = blockedAngles.Count;
        }

        return highest;
    }

    protected override string GetPuzzleData()
    {
        return "/day10input.txt";
    }

    protected override void ParseLine(string line)
    {
        asteroids.Add(line.Select(c => c == '#').ToList());
    }
}