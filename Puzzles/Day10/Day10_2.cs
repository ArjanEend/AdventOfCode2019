
using System;
using System.Collections.Generic;
using System.Linq;

public class PuzzleDay10_2 : PuzzleBase
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
        IntVector2 satellitePos = new IntVector2();
        foreach (var posA in asteroidPositions)
        {
            Dictionary<float, float> blockedAngles = new Dictionary<float, float>();
            foreach (var posB in asteroidPositions)
            {
                if(posA == posB)
                    continue;
                
                float angle = MathF.Atan2(posB.y - posA.y, posB.x - posA.x);
                
                if (blockedAngles.ContainsKey(angle))
                {
                    continue;
                }
                blockedAngles.Add(angle, (posA - posB).Magnitude());
            }
            if (highest < blockedAngles.Count)
            {
                highest = blockedAngles.Count;
                satellitePos = posA;
            }
        }

        List<IntVector2> vaporized = new List<IntVector2>();
        while(vaporized.Count < 200)
        {
            Dictionary<float, IntVector2> blockedAngles = new Dictionary<float, IntVector2>();
            foreach (var posB in asteroidPositions)
            {
                if(satellitePos == posB)
                    continue;
                if (vaporized.Contains(posB))
                    continue;
                
                float angle = MathF.Atan2(satellitePos.x - posB.x, satellitePos.y - posB.y);
                
                float dist = (satellitePos - posB).Magnitude();
                if (blockedAngles.ContainsKey(angle))
                {
                    if ((blockedAngles[angle] - satellitePos).Magnitude() < dist)
                        continue;
                }
                blockedAngles[angle] = posB;
            }

            var blockedOrder = blockedAngles.Keys.OrderBy(x => (180 - x) % 180).ToList();

            for (int i = 0; i < blockedOrder.Count; i++)
            {
                vaporized.Add(blockedAngles[blockedOrder[i]]);
            }
        }

        return vaporized[199].x * 100 + vaporized[199].y;
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