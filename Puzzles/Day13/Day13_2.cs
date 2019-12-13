
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

public class PuzzleDay13_2 : PuzzleBase
{
    private List<long> inputs = new List<long>();
    Dictionary<IntVector2, ITile> tiles = new Dictionary<IntVector2, ITile>();

    public override object CalculateSolutions()
    {
        inputs[0] = 2;
        var computer = new IntCodeComputer(inputs, new List<int>());

        computer.AddInput(0);
        //var CursorLeft = Console.CursorLeft;
        //var cursorTop = Console.CursorTop;

        while(true)
        {
            int score = 0;
            IntVector2 ballPos = new IntVector2();
            IntVector2 paddlePos = new IntVector2();

            computer.Execute();

            IntVector2 maxSize = new IntVector2();
            for(int i = 0; i < computer.output.Count; i += 3)
            {
                IntVector2 coordinates = new IntVector2((int)computer.output[i], (int)computer.output[i + 1]);

                if(coordinates.x > maxSize.x)
                    maxSize.x = coordinates.x;
                if(coordinates.y > maxSize.y)
                    maxSize.y = coordinates.y;

                if(coordinates.x == -1)
                {
                    score = (int)computer.output[i + 2];
                    continue;
                }

                switch(computer.output[i + 2])
                {
                    case 0: 
                        if(tiles.ContainsKey(coordinates))
                            tiles.Remove(coordinates);
                        break;
                    case 1:
                        tiles[coordinates] = new Wall();
                        break;
                    case 2:
                        tiles[coordinates] = new Block();
                        break;
                    case 3:
                        tiles[coordinates] = new Paddle();
                        paddlePos = coordinates;
                        break;
                    case 4:
                        tiles[coordinates] = new Ball();
                        ballPos = coordinates;
                        break;
                }
            }

            //Console.Clear();
            //Console.SetCursorPosition(cursorTop, CursorLeft);
            for(int y = 0; y < maxSize.y; y++)
            {
                for(int x = 0; x < maxSize.x; x ++)
                {
                    IntVector2 coordinates = new IntVector2(x, y);
                    if (!tiles.ContainsKey(coordinates))
                    {
                        Console.Write(" ");
                        continue;
                    }

                    Console.Write(tiles[coordinates].Display);
                }
                Console.Write("\n");
            }

            if (ballPos.x < paddlePos.x)
                computer.AddInput(-1);
            else if (ballPos.x > paddlePos.x)
                computer.AddInput(1);
            else
                computer.AddInput(0);

            if (tiles.Values.Where(t => t is Block).Count() == 0)
                return score;

            Thread.Sleep(1);
        }
    }

    protected override string GetPuzzleData()
    {
        return "/day13input.txt";
    }

    protected override void ParseLine(string line)
    {
        inputs.AddRange(line.Split(',')
            .Where(str => long.TryParse(str, out long res))
            .Select(str => long.Parse(str)));
    }
}