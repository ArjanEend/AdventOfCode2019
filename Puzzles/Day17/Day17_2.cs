
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class PuzzleDay17_2 : PuzzleBase
{
    private const float RAD2DEG = 360 / (MathF.PI * 2);

    private List<long> inputs = new List<long>();

    IntVector2 up = new IntVector2(0, -1);
    IntVector2 down = new IntVector2(0, -1);
    IntVector2 right = new IntVector2(1, 0);
    IntVector2 left = new IntVector2(-1, 0);

    private enum Instructions : int
    {
        FORWARD = 0,
        LEFT = 1,
        RIGHT = 2
    }

    public override object CalculateSolutions()
    {
        var comp = new IntCodeComputer(inputs.ToList());

        Dictionary<IntVector2, char> tiles = new Dictionary<IntVector2, char>();

        List<IntVector2> walkedTiles = new List<IntVector2>();
        List<string> instructions = new List<string>();

        comp.Execute();

        int x = 0;
        int y = 0;
        for(int i = 0; i < comp.output.Count; i++)
        {
            IntVector2 coord = new IntVector2(x, y);
            char tile = (char)comp.output[i];
            tiles.Add(coord, tile);

            x++;
            if(tile == '\n')
            {
                x = 0;
                y++;
            }
        }

        
        int highestX = tiles.Keys.OrderBy(_ => _.x).LastOrDefault().x;
        int highestY = tiles.Keys.OrderBy(_ => _.y).LastOrDefault().y;

        IntVector2 start = tiles.Where(_ => _.Value == '^').FirstOrDefault().Key;

        IntVector2 startLine = start;
        IntVector2 current = start;
        
        float angle = MathF.Atan2(up.y, up.x) * RAD2DEG;
        IntVector2 forward = 
            new IntVector2((int)MathF.Round(MathF.Cos(MathF.PI * angle / 180f)), (int)MathF.Round(MathF.Sin(MathF.PI * angle / 180f)));

        int steps = 0;
        while(true)
        {
            if (!tiles.ContainsKey(current + forward) || tiles[current + forward] != '#')
            {
                if (steps > 0)
                {
                    instructions.Add(steps.ToString());
                    steps = 0;
                }
                float leftAngle = angle - 90f;
                float rightAngle = angle + 90f;

                IntVector2 right = 
                    new IntVector2((int)MathF.Round(MathF.Cos(MathF.PI * rightAngle / 180f)), (int)MathF.Round(MathF.Sin(MathF.PI * rightAngle / 180f)));

                IntVector2 left = 
                    new IntVector2((int)MathF.Round(MathF.Cos(MathF.PI * leftAngle / 180f)), (int)MathF.Round(MathF.Sin(MathF.PI * leftAngle / 180f)));

                if (tiles.ContainsKey(current + right) && tiles[current + right] == '#')
                {
                    angle += 90f;
                    forward = right;
                    instructions.Add("R");
                    continue;
                }
                if (tiles.ContainsKey(current + left) && tiles[current + left] == '#')
                {
                    angle -= 90f;
                    forward = left;
                    instructions.Add("L");
                    continue;
                }
                break;
            }
            current += forward;
            walkedTiles.Add(current);
            steps++;
        }

        List<string> patterns = new List<string>();
        List<int> segmentList = new List<int>();
        for(int i = 0; i < instructions.Count; i += 2)
        {
            string pattern = instructions[i];
            pattern += ",";
            pattern += instructions[i + 1];
            if(!patterns.Contains(pattern))
                patterns.Add(pattern);
            segmentList.Add(patterns.IndexOf(pattern));
        }

        List<int[]> uniqueSegments = new List<int[]>();
        for(int i = 0; i < segmentList.Count; i++)
        {
            List<int> segment = new List<int>{segmentList[i]};
            for(int j = i + 1; j < i + 8 && j < segmentList.Count; j++)
            {
                segment.Add(segmentList[j]);
                if (j - i > 5)
                    uniqueSegments.Add(segment.ToArray());
            }
        }



        string mainPattern = instructions[0];
        for(int i = 1; i < instructions.Count; i++)
        {
            mainPattern += ",";
            mainPattern += instructions[i];
        }

/*
0,1,0,2,3,3,0,0,1,0,2,3,4,4,0,3,3,0,3,4,4,0,0,1,0,2,3,3,0,0,1,0,2,3,4,4,0

0,1,0,2
3,3,0
3,4,4,0

4,4,0

0,1,0,2,3,3,0
0,1,0,2,3,4,4,0
3,3,0,3,4,4,0
*/
        //var newPatterns = uniqueSegments.Where(s => s.Length > 5).GroupBy(p => p).OrderByDescending(pp => pp.Count()).Take(9).Select(gp => gp.Key).ToList();

        var newPatterns = new List<int[]>{new int[]{0,1,0,2}, new int[]{3,3,0}, new int[]{3,4,4,0}};

        List<string> functions = new List<string>();
        for(int i = 0; i < newPatterns.Count; i++)
        {
            string patternstr = "";
            foreach(var index in newPatterns[i])
            {
                patternstr += patterns[index];
                patternstr += ",";
            }
            patternstr = patternstr.Substring(0, patternstr.Length - 1);
            if(patternstr.Length >= 20)
                throw new Exception("INstruction too long");
            string replace = "A";
            if (i == 1)
                replace = "B";
            if (i == 2)
                replace = "C";
            functions.Add(patternstr);
            mainPattern = mainPattern.Replace(patternstr, replace);
        }

        inputs[0] = 2;
        List<int> computerInput = new List<int>();
        computerInput.AddRange(mainPattern.Select(c => (int)c));
        computerInput.Add(10);
        for(int i = 0; i < functions.Count; i++)
        {
            computerInput.AddRange(functions[i].Select(c => (int)c));
            computerInput.Add(10);
        }
        computerInput.Add((int)'y');
        computerInput.Add(10);

        comp = new IntCodeComputer(inputs, computerInput);
        comp.Execute();

        StringBuilder sb = new StringBuilder();
        foreach(var output in comp.output)
            sb.Append((char)output);

        Console.WriteLine(sb.ToString());

        return comp.output.LastOrDefault();
    }

    protected override string GetPuzzleData()
    {
        return "/day17input.txt";
    }

    protected override void ParseLine(string line)
    {
        inputs.AddRange(line.Split(',')
            .Where(str => long.TryParse(str, out long res))
            .Select(str => long.Parse(str)));
    }
}