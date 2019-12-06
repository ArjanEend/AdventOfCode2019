
using System.Collections.Generic;
using System.Linq;

public class PuzzleDay6_1 : PuzzleBase
{
    private Dictionary<string, string> navigation = new Dictionary<string, string>();
    private HashSet<string> planets = new HashSet<string>();

    public override object CalculateSolutions()
    {
        int orbits = 0;
        for (int i = 0; i < planets.Count; i++)
        {
            var planet = planets.ElementAt(i);
            while (navigation.ContainsKey(planet))
            {
                planet = navigation[planet];
                orbits++;
            }
        }
        
        return orbits;
    }

    protected override string GetPuzzleData()
    {
        return "/day6input.txt";
    }

    protected override void ParseLine(string line)
    {
        var inputs = line.Split(')');
        navigation.Add(inputs[1], inputs[0]);
        planets.Add(inputs[1]);
        planets.Add(inputs[0]);
    }
}