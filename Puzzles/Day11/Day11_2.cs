
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class PuzzleDay11_2 : PuzzleBase
{
    private List<long> inputs = new List<long>();

    public override object CalculateSolutions()
    {
        var robot = new PaintingRobot(inputs);

        Dictionary<IntVector2, RobotColor> panels = new Dictionary<IntVector2, RobotColor>();

        panels.Add(robot.Position, RobotColor.WHITE);
        while(!robot.Done)
        {
            var pos = robot.Position;
            if(!panels.ContainsKey(pos))
                panels.Add(pos, RobotColor.BLACK);
            panels[pos] = robot.Paint(panels[pos]);
        }

        int lowestX = panels.Keys.OrderBy(_ => _.x).FirstOrDefault().x;    
        int lowestY = panels.Keys.OrderBy(_ => _.y).FirstOrDefault().y;   
        int highestX = panels.Keys.OrderBy(_ => _.x).LastOrDefault().x;
        int highestY = panels.Keys.OrderBy(_ => _.y).LastOrDefault().y;

        StringBuilder sb = new StringBuilder();
        sb.Append("\n");
        for(int y = highestY; y >= lowestY ; y--)
        {
            for(int x = highestX; x >= lowestX; x--)
            {
                var pos = new IntVector2(x, y);
                sb.Append(panels.ContainsKey(pos) && panels[pos] == RobotColor.WHITE ? " ■ " : " □ ");
            }
            sb.Append("\n");
        }

        return sb.ToString();
    }

    protected override string GetPuzzleData()
    {
        return "/day11input.txt";
    }

    protected override void ParseLine(string line)
    {
        inputs.AddRange(line.Split(',')
            .Where(str => long.TryParse(str, out long res))
            .Select(str => long.Parse(str)));
    }
}