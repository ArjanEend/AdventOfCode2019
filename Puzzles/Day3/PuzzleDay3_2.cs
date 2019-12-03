
using System;
using System.Collections.Generic;
using System.Linq;

public class PuzzleDay3_2 : PuzzleBase
{
    private Dictionary<IntVector2, int> path1 = new Dictionary<IntVector2, int>();
    private Dictionary<IntVector2, int> path2 = new Dictionary<IntVector2, int>();

    bool parsePath2;

    public override object CalculateSolutions()
    {
        int steps = int.MaxValue;

        var intersections = path1.Keys.Intersect(path2.Keys);

        foreach(var intersection in intersections)
        {
            if(intersection.x == 0 && intersection.y == 0)
                continue;
            int stepsCalc = path1[intersection] + path2[intersection];
            if(stepsCalc < steps)
            {
                steps = stepsCalc;
            }
        }

        return steps;
    }

    protected override string GetPuzzleData()
    {
        return "/day3input.txt";
    }

    protected override void ParseLine(string line)
    {
        var instructions = line.Split(',');

        var path = parsePath2 ? path2 : path1;

        int steps = 0;
        IntVector2 prev = new IntVector2(0, 0);
        foreach(var instruction in instructions)
        {
            int amount = int.Parse(instruction.Substring(1, instruction.Length -1));
            switch(instruction[0])
            {
                case 'R':
                for(int i = 0; i < amount; i++)
                {
            steps++;
                    prev += new IntVector2(1, 0);
                    if(path.ContainsKey(prev))
                        continue;
                    path.Add(prev, steps);
                }
                break;
                case 'U':for(int i = 0; i < amount; i++)
                {
            steps++;
                    prev += new IntVector2(0, 1);
                    if(path.ContainsKey(prev))
                        continue;
                    path.Add(prev, steps);
                }
                break;
                case 'D':
                for(int i = 0; i < amount; i++)
                {
            steps++;
                    prev += new IntVector2(0, -1);
                    if(path.ContainsKey(prev))
                        continue;
                    path.Add(prev, steps);
                }
                break;
                case 'L':
                for(int i = 0; i < amount; i++)
                {
            steps++;
                    prev += new IntVector2(-1, 0);
                    if(path.ContainsKey(prev))
                        continue;
                    path.Add(prev, steps);
                }
                break;
            }
        }

        parsePath2 = true;
    }
}