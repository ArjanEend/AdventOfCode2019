
using System;
using System.Collections.Generic;
using System.Linq;

public class PuzzleDay11_1 : PuzzleBase
{
    private List<long> inputs = new List<long>();

    public override object CalculateSolutions()
    {
        var robot = new PaintingRobot(inputs);

        Dictionary<IntVector2, RobotColor> panels = new Dictionary<IntVector2, RobotColor>();

        while(!robot.Done)
        {
            var pos = robot.Position;
            if(!panels.ContainsKey(pos))
                panels.Add(pos, RobotColor.BLACK);
            panels[pos] = robot.Paint(panels[pos]);
        }

        return panels.Count;
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