
using System;
using System.Collections.Generic;

public class IntCodeComputer
{
    public enum OpCodes 
    {
        ADD = 1,
        MULTIPLY = 2,
        QUIT = 99
    }

    private List<int> memory;

    public IntCodeComputer(List<int> opCodes)
    {
        this.memory = opCodes;
    }

    public void Execute()
    {
        int executionIndex = 0;
        while (executionIndex + 3 < memory.Count)
        {
            int opCode = memory[executionIndex];
            switch((OpCodes)opCode)
            {
                case OpCodes.ADD:
                    ExecuteAdd(executionIndex);
                break;
                case OpCodes.MULTIPLY:
                    ExecuteMultiply(executionIndex);
                break;
                case OpCodes.QUIT:
                    return;
                default:
                    Console.WriteLine("[IntCodeComputer] Something when wrong");
                break;
            }
            
            executionIndex += 4;
        }
    }

    private void ExecuteAdd(int executionIndex)
    {
        int verb = memory[memory[executionIndex + 1]];
        int noun = memory[memory[executionIndex + 2]];
        int sum = verb + noun;
        memory[memory[executionIndex + 3]] = sum;
    }

    private void ExecuteMultiply(int executionIndex)
    {
        int verb = memory[memory[executionIndex + 1]];
        int noun = memory[memory[executionIndex + 2]];
        int sum = verb * noun;
        memory[memory[executionIndex + 3]] = sum;
    }
}