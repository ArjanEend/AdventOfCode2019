using System;
using System.Collections.Generic;
using System.Linq;

public class PuzzleDay22_1 : PuzzleBase
{
    private List<int> cards = new List<int>();

    public PuzzleDay22_1()
    {
    }

    protected override string GetPuzzleData()
    {
        return "/day22input.txt";
    }

    protected override void ParseLine(string line)
    {
        if (cards.Count == 0)
        {
        for (int i = 0; i < 10007 ; i++)
            cards.Add(i);
        }
        if (line.Contains("cut "))
        {
            line = line.Replace("cut ", "");
            int amount = int.Parse(line);

            if (amount > 0)
            {
                for (int i = 0; i < amount; i++)
                {
                    cards.Add(cards[0]);
                    cards.RemoveAt(0);                    
                }
            } else {
                for (int i = amount; i < 0; i++)
                {
                    cards.Insert(0, cards[cards.Count -1]);
                    cards.RemoveAt(cards.Count - 1);
                }
            }
            return;
        }
        if (line.Contains("deal into new stack"))
        {
            cards.Reverse();
        }
        if (line.Contains("deal with increment"))
        {
            line = line.Replace("deal with increment ", "");
            int amount = int.Parse(line);
            var newCards = cards.ToList();
            for (int i = 0; i < cards.Count; i++)
                newCards[(i * amount) % cards.Count] = cards[i];
            cards = newCards; 
        }
    }

    public override object CalculateSolutions()
    {
        ReadFile();
        return cards.IndexOf(2019);
    }
}