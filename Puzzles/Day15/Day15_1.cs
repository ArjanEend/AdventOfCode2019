
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

public class PuzzleDay15_1 : PuzzleBase
{
    private List<long> inputs = new List<long>();

    public override object CalculateSolutions()
    {
        var robot = new RepairBot(inputs);

        IntVector2 up = new IntVector2(0, 1);
        IntVector2 down = new IntVector2(0, -1);
        IntVector2 right = new IntVector2(1, 0);
        IntVector2 left = new IntVector2(-1, 0);

        Dictionary<IntVector2, string> map = new Dictionary<IntVector2, string>();
        List<IntVector2> undiscovered = new List<IntVector2>();
        undiscovered.Add(down);
        bool tankFound = false;
        while(undiscovered.Count > 0 && !tankFound)
        {
            var pos = robot.Position;
            if(!map.ContainsKey(pos + up))
                undiscovered.Add(pos + up);
            if(!map.ContainsKey(pos + down))
                undiscovered.Add(pos + down);
            if(!map.ContainsKey(pos + left))
                undiscovered.Add(pos + left);
            if(!map.ContainsKey(pos + right))
                undiscovered.Add(pos + right);

            while(undiscovered.Count > 0 && map.ContainsKey(undiscovered[undiscovered.Count - 1]))
                undiscovered.RemoveAt(undiscovered.Count - 1);

            var undiscoveredTile = undiscovered[undiscovered.Count - 1];
            var validPositions = map.Where(m => m.Value == "." || m.Value == "D" || m.Value == "O").Select(_ => _.Key).ToList();
            validPositions.Add(undiscoveredTile);
            if(validPositions.Contains(robot.Position))
                validPositions.Add(robot.Position);
            var pathToUndiscovered = PathFinder.FindPath(validPositions, robot.Position, undiscoveredTile);

            for(int i = 0; i < pathToUndiscovered.Count; i++)
            {
                pos = robot.Position;
                map[robot.Position] = ".";

                var direction = Directions.NORTH;
                if (pathToUndiscovered[i] == pos)
                    continue;
                if (pathToUndiscovered[i].x > pos.x)
                    direction = Directions.EAST;
                if (pathToUndiscovered[i].x < pos.x)
                    direction = Directions.WEST;
                if (pathToUndiscovered[i].y < pos.y)
                    direction = Directions.SOUTH;

                pos = pathToUndiscovered[i];

                var status = robot.Move(direction);
                if(!map.ContainsKey(pos))
                    map.Add(pos, "D");

                map[pos] = status == 0 ? "#" : ".";
                if ((int)status == 2) 
                {
                    map[pos] = "O";
                    tankFound = true;
                    break;
                }
                map[robot.Position] = "D";

                if(!map.ContainsKey(robot.Position + up))
                    undiscovered.Add(robot.Position + up);
                if(!map.ContainsKey(robot.Position + down))
                    undiscovered.Add(robot.Position + down);
                if(!map.ContainsKey(robot.Position + left))
                    undiscovered.Add(robot.Position + left);
                if(!map.ContainsKey(robot.Position + right))
                    undiscovered.Add(robot.Position + right);

                DrawMap(map);
            }
        }

        var path = PathFinder.FindPath(map.Where(m => m.Value == "." || m.Value == "D" || m.Value == "O").Select(m => m.Key).ToList(),
        new IntVector2(), map.Where(_ => _.Value == "O").LastOrDefault().Key);

        DrawMap(map);

        return path.Count;
    }

    private void DrawMap(Dictionary<IntVector2, string> map)
    {
        int lowestX = map.Keys.OrderBy(_ => _.x).FirstOrDefault().x;    
        int lowestY = map.Keys.OrderBy(_ => _.y).FirstOrDefault().y;   
        int highestX = map.Keys.OrderBy(_ => _.x).LastOrDefault().x;
        int highestY = map.Keys.OrderBy(_ => _.y).LastOrDefault().y;

        StringBuilder sb = new StringBuilder();
        sb.Append("\n");
        for(int y = highestY; y >= lowestY ; y--)
        {
            for(int x = highestX; x >= lowestX; x--)
            {
                var pos = new IntVector2(x, y);
                sb.Append(map.ContainsKey(pos) ? map[pos] : " ");
            }
            sb.Append("\n");
        }

        Console.Write(sb.ToString()); 
    }

    protected override string GetPuzzleData()
    {
        return "/day15input.txt";
    }

    protected override void ParseLine(string line)
    {
        inputs.AddRange(line.Split(',')
            .Where(str => long.TryParse(str, out long res))
            .Select(str => long.Parse(str)));
    }
}