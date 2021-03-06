﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AdventOfCode2019
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Input Search string");
            while (true)
            {
                var input = Console.ReadLine();
                if(input == "q")
                    return;
                IEnumerable<PuzzleBase> puzzles = typeof(PuzzleBase)
                    .Assembly.GetTypes()
                    .Where(t => t.IsSubclassOf(typeof(PuzzleBase)) && !t.IsAbstract && (string.IsNullOrEmpty(input) || t.Name.Contains(input)))
                    .Select(t => (PuzzleBase)Activator.CreateInstance(t));

                foreach(var puzzle in puzzles)
                {
                    puzzle.ReadFile();
                    puzzle.LogPuzzleSolution();
                }
            }
        }
    }
}
