
using System;
using System.Collections.Generic;
using System.Linq;

public class PuzzleDay12_2 : PuzzleBase
{
    private List<Moon> moons = new List<Moon>();

    public override object CalculateSolutions()
    {
        Vector3 loopValues = new Vector3(-1, -1, -1);
        for (int i = 1; ; i++)
        {
            for (int j = 0; j < moons.Count; j++)
            {
                var moonA = moons[j];
                for (int k = 0; k < moons.Count; k++)
                {
                    if (j == k)
                        continue;
                    var moonB = moons[k];
                    
                    float xDiff = moonA.position.x - moonB.position.x;
                    if (xDiff != 0)
                    {
                        moonA.velocity.x -= MathF.Sign(xDiff);
                    }
                    float yDiff = moonA.position.y - moonB.position.y;
                    if (yDiff != 0)
                    {
                        moonA.velocity.y -= MathF.Sign(yDiff);
                    }
                    float zDiff = moonA.position.z - moonB.position.z;
                    if (zDiff != 0)
                    {
                        moonA.velocity.z -= MathF.Sign(zDiff);
                    }
                }
            }

            int xLoop = 0;
            int yLoop = 0;
            int zLoop = 0;
            for (int j = 0; j < moons.Count; j++)
            {
                var moonA = moons[j];
                moonA.position += moonA.velocity;

                if(moonA.velocity.x == 0f && moonA.position.x == moonA.originalPos.x)
                {
                    xLoop++;
                }
                if(moonA.velocity.y == 0f && moonA.position.y == moonA.originalPos.y)
                {
                    yLoop++;
                }
                if(moonA.velocity.z == 0f && moonA.position.z == moonA.originalPos.z)
                {
                    zLoop++;
                }
            }

            if (loopValues.x == -1 && xLoop == moons.Count)
                loopValues.x = i;
                
            if (loopValues.y == -1 && yLoop == moons.Count)
                loopValues.y = i;
                
            if (loopValues.z == -1 && zLoop == moons.Count)
                loopValues.z = i;

            if(loopValues.x != -1 && loopValues.y != -1 && loopValues.z != -1)
                break;
        }

        return lcm(lcm((int)loopValues.x, (int)loopValues.y), (int)loopValues.z);
    }

    private int gcd(int a, int b) 
    { 
        if (b == 0) 
            return a;  
        return gcd(b, a % b);  
    } 
      
    private int lcm(int a, int b) 
    { 
        return (a * b) / gcd(a, b); 
    }

    protected override string GetPuzzleData()
    {
        return "/day12input.txt";
    }

    protected override void ParseLine(string line)
    {
        var moon = new Moon();
        int xIndex = line.IndexOf("x=") + 2;
        int nextComma = line.IndexOf(",", xIndex);
        moon.position.x = int.Parse(line.Substring(xIndex, nextComma - xIndex));

        int yIndex = line.IndexOf("y=") + 2;
        nextComma = line.IndexOf(",", yIndex);
        moon.position.y = int.Parse(line.Substring(yIndex, nextComma - yIndex));

        int zIndex = line.IndexOf("z=") + 2;
        nextComma = line.IndexOf(">", zIndex);
        moon.position.z = int.Parse(line.Substring(zIndex, nextComma - zIndex));

        moon.originalPos = moon.position;

        moons.Add(moon);
    }
}