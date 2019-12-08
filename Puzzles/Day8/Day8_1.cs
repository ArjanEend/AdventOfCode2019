
using System;
using System.Collections.Generic;
using System.Linq;

public class PuzzleDay8_1 : PuzzleBase
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

        for(int i = 0; i < layers.Count; i++)
        {
            for(int y = 0; y < height; y++)
            {
                for(int x = 0; x < width; x ++)
                {
                    layers[i][x,y] = input[0];
                    flatLayers[i].Add(input[0]);
                    input.RemoveAt(0);
                }
            }
        }

        var flatLayer = flatLayers.OrderBy(l => l.Count(s => s == 0)).FirstOrDefault();
        return flatLayer.Count(l => l == 1) * flatLayer.Count(l => l == 2);
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