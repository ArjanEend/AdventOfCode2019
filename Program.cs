using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2019
{
    class Program
    {
        static void Main(string[] args)
        {
            IEnumerable<PuzzleBase> puzzles = typeof(PuzzleBase)
                .Assembly.GetTypes()
                .Where(t => t.IsSubclassOf(typeof(PuzzleBase)) && !t.IsAbstract)
                .Select(t => (PuzzleBase)Activator.CreateInstance(t));
            
            foreach(var puzzle in puzzles)
            {
                puzzle.ReadFile();
                puzzle.LogPuzzleSolution();
            }
        }
    }
}
