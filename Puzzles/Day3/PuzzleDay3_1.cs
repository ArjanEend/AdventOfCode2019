
using System;
using System.Collections.Generic;
using System.Linq;

public class PuzzleDay3_1 : PuzzleBase
{
    private List<IntVector2> path1 = new List<IntVector2>();
    private List<IntVector2> path2 = new List<IntVector2>();

    bool parsePath2;

    public override object CalculateSolutions()
    {
        
        int manhattan = int.MaxValue;

        for(int i = 0; i < path1.Count; i++)
        {
            for(int j = 0; j < path2.Count; j++)
            {
                var pathA = path1[i];
                var pathB = path2[j];
                
                if(pathA.x != pathB.x && pathA.y != pathB.y)
                    break;

                if(pathA != pathB)
                    continue;

                if(pathA == new IntVector2(0,0) || pathB == new IntVector2(0,0))
                    continue;

                var intersection = pathA;
                
                Console.WriteLine(intersection);

                if (intersection != new IntVector2(0, 0))
                {
                    //Console.WriteLine("Intersection at: " + intersection.x + " : " + intersection.y);
                    if(Math.Abs(intersection.x) + Math.Abs(intersection.y) < manhattan)
                    {
                        manhattan = Math.Abs((int)intersection.x) + Math.Abs((int)intersection.y);
                    }
                }
            }
        }

        Console.WriteLine(path1.Count);

        return manhattan;
    }

    protected override string GetPuzzleData()
    {
        return "/day3input.txt";
    }

    protected override void ParseLine(string line)
    {
        var instructions = line.Split(',');

        var path = parsePath2 ? path2 : path1;

        IntVector2 prev = new IntVector2(0, 0);
        foreach(var instruction in instructions)
        {
            int amount = int.Parse(instruction.Substring(1, instruction.Length -1));
            switch(instruction[0])
            {
                case 'R':
                for(int i = 0; i < amount; i++)
                {
                    prev += new IntVector2(1, 0);
                    path.Add(prev);
                }
                break;
                case 'U':for(int i = 0; i < amount; i++)
                {
                    prev += new IntVector2(0, 1);
                    path.Add(prev);
                }
                break;
                case 'D':
                for(int i = 0; i < amount; i++)
                {
                    prev += new IntVector2(0, -1);
                    path.Add(prev);
                }
                break;
                case 'L':
                for(int i = 0; i < amount; i++)
                {
                    prev += new IntVector2(-1, 0);
                    path.Add(prev);
                }
                break;
            }
        }

        parsePath2 = true;
    }
}