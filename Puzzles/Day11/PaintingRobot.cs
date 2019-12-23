using System;
using System.Collections.Generic;

public enum RobotColor : int
{
    BLACK = 0,
    WHITE = 1,
}

public class PaintingRobot
{
    private IntCodeComputer intCodeComputer;
    private IntVector2 position;
    public IntVector2 Position => position;

    private float angle = 90f;

    public bool Done => intCodeComputer.done;

    public PaintingRobot(List<long> program)
    {
        intCodeComputer = new IntCodeComputer(program);
        position = new IntVector2();
    }

    public RobotColor Paint(RobotColor color)
    {
        intCodeComputer.AddInput((int)color);
        intCodeComputer.Execute();

        RobotColor panelColor = (RobotColor)intCodeComputer.output[intCodeComputer.output.Count - 2];
        bool turnRight = intCodeComputer.output[intCodeComputer.output.Count - 1] == 1;

        if (turnRight)
            angle += 90f;
        else
            angle -= 90f;

        position.x += (int)MathF.Round(MathF.Cos(MathF.PI * angle / 180f));  
        position.y += (int)MathF.Round(MathF.Sin(MathF.PI * angle / 180f));

        return panelColor;
    }

}