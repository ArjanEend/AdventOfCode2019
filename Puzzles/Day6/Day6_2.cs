
using System.Collections.Generic;
using System.Linq;

public class PuzzleDay6_2 : PuzzleBase
{
    private Dictionary<string, string> navigation = new Dictionary<string, string>();
    private HashSet<string> planets = new HashSet<string>();

    public override object CalculateSolutions()
    {
        int transfers = 0;

        string me = navigation["YOU"];
        string other = navigation["SAN"];

        List<string> navigated = new List<string>();
        List<string> navigatedOther = new List<string>();

        bool meTransferred = false;
        while(navigation.ContainsKey(me) || navigation.ContainsKey(other))
        {
            if (!meTransferred && navigation.ContainsKey(me))
            {
                navigated.Add(me);
                me = navigation[me];
            } else
            {
                navigatedOther.Add(other);
                other = navigation[other];
            }
            meTransferred = !meTransferred;
        }

        int commonIndex = navigated.Select(el => navigatedOther.IndexOf(el)).Where(index => index > 0).FirstOrDefault();
        string commonPlanet = navigatedOther[commonIndex];
        commonIndex += navigated.IndexOf(commonPlanet);
        
        return commonIndex;
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