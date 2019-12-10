using System;
using System.Diagnostics;
using System.IO;

public abstract class PuzzleBase
{
    private Stopwatch watch = new Stopwatch();
    public PuzzleBase()
    {
    }

    public void ReadFile()
    {
        watch.Start();
        var lines = File.ReadAllLines(GetPuzzlesDataPath());
        foreach(var line in lines)
            ParseLine(line);
    }

    public abstract object CalculateSolutions();

    public virtual void LogPuzzleSolution()
    {
        Console.WriteLine("Solution for: "
            + GetType().Name
            + " "
            + CalculateSolutions().ToString() + " in " + watch.ElapsedMilliseconds.ToString() + "ms");
        watch.Stop();
    }

    
    protected virtual string GetPuzzlesDataPath()
    {
        return Directory.GetCurrentDirectory() + "\\Data\\" + GetPuzzleData();
    }
    
    protected abstract string GetPuzzleData();

    protected abstract void ParseLine(string line);
}