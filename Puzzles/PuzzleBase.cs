using System;
using System.IO;

public abstract class PuzzleBase
{
    public PuzzleBase()
    {

    }

    public void ReadFile()
    {
        var lines = File.ReadAllLines(GetPuzzlesDataPath());
        foreach(var line in lines)
            ParseLine(line);
    }

    public abstract object CalculateSolutions();

    public virtual void LogPuzzleSolution()
    {
        Console.WriteLine("Solution for: "
            + GetPuzzleData()
            + " "
            + CalculateSolutions().ToString());
    }

    
    protected virtual string GetPuzzlesDataPath()
    {
        return Directory.GetCurrentDirectory() + "\\Data\\" + GetPuzzleData();
    }
    
    protected abstract string GetPuzzleData();

    protected abstract void ParseLine(string line);
}