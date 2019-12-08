
using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Text;

public class PuzzleDay8_2 : PuzzleBase
{
    private List<short[,]> layers = new List<short[,]>();
    private List<List<short>> flatLayers = new List<List<short>>();

    private List<short> input = new List<short>();

    private const int width = 25;
    private const int height = 6;

    public override object CalculateSolutions()
    {
        int pixelsPerLayer = 25 * 6;
        int layerCount = input.Count / pixelsPerLayer;

        for(int i = 0; i < layerCount; i++)
        {
            short[,] layer = new short[width, height];
            layers.Add(layer);
            flatLayers.Add(new List<short>());
        }

        short[,] final = new short[width, height];
        for(int i = 0; i < layers.Count; i++)
        {
            for(int y = 0; y < height; y++)
            {
                for(int x = 0; x < width; x ++)
                {
                    short pixel = input[0]; 
                    input.RemoveAt(0);
                    layers[i][x,y] = pixel;
                    flatLayers[i].Add(pixel);

                    if(i != 0 && final[x,y] == 0)
                        continue;
                    if(i != 0 && final[x,y] == 1)
                        continue;
                    final[x,y] = pixel;
                }
            }
        }

        
        StringBuilder sb = new StringBuilder();
        sb.Append("\n");
        for(int y = 0; y < height; y++)
        {
            for(int x = 0; x < width; x ++)
            {
                sb.Append(final[x,y] == 0 ? " □ " : " ■ ");
            }
            sb.Append("\n");
        }


        return sb.ToString();
    }

    protected override string GetPuzzleData()
    {
        return "/day8input.txt";
    }

    protected override void ParseLine(string line)
    {
        input.AddRange(line.Select(c => short.Parse(c.ToString())));
    }
}