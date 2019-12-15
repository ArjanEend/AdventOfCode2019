using System;
using System.Collections.Generic;
using System.Linq;

public enum Directions : int
{
    NORTH = 1,
    SOUTH = 2,
    WEST = 3,
    EAST = 4
}

public enum Status : int
{
    WALL = 0,
    MOVED = 1,
    OXYGENSTATION = 2
}

public class RepairBot
{
    private IntCodeComputer intCodeComputer;
    public IntVector2 Position;

    public bool Done => intCodeComputer.done;

    public RepairBot(List<long> program)
    {
        intCodeComputer = new IntCodeComputer(program, new List<int>());
        Position = new IntVector2();
    }

    public Status Move(Directions dir)
    {
        intCodeComputer.AddInput((int)dir);
        intCodeComputer.Execute();

        var status = (Status)intCodeComputer.output.LastOrDefault();
        if(status > 0)
        {
            switch(dir)
            {
                case Directions.NORTH:
                    Position.y++;
                    break;
                case Directions.EAST:
                    Position.x++;
                    break;
                    case Directions.SOUTH:
                    Position.y--;
                    break;
                    case Directions.WEST:
                    Position.x--;
                    break;
            }
        }


        return status;
    }

}