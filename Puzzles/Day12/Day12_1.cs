
using System;
using System.Collections.Generic;
using System.Linq;

public class PuzzleDay12_1 : PuzzleBase
{
    private List<Moon> moons = new List<Moon>();

    public override object CalculateSolutions()
    {
        for (int i = 0; i < 1000; i++)
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

            for (int j = 0; j < moons.Count; j++)
            {
                var moonA = moons[j];
                moonA.position += moonA.velocity;

                Console.WriteLine($"Step {i} Moon {j}: {moonA.position} ::: {moonA.velocity}");
            }
        }

        int energy = 0;
        for (int j = 0; j < moons.Count; j++)
        {
            var moonA = moons[j];
            energy += (int)MathF.Round(moonA.GetEnergy());
        }

        return energy;
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

        moons.Add(moon);
    }
}