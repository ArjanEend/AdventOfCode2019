
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

    private List<int> opCodes;


    public IntCodeComputer(List<int> opCodes)
    {
        this.opCodes = opCodes;
    }

    public void Execute()
    {
                    Console.WriteLine("[IntCodeComputer] exeucting " + opCodes.Count + " elements");
        int executionIndex = 0;
        while (executionIndex + 3 < opCodes.Count)
        {
            int opCode = opCodes[executionIndex];
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
        int a = opCodes[opCodes[executionIndex + 1]];
        int b = opCodes[opCodes[executionIndex + 2]];
        int sum = a + b;
        opCodes[opCodes[executionIndex + 3]] = sum;
    }

    private void ExecuteMultiply(int executionIndex)
    {
        int a = opCodes[opCodes[executionIndex + 1]];
        int b = opCodes[opCodes[executionIndex + 2]];
        int sum = a * b;
        opCodes[opCodes[executionIndex + 3]] = sum;
    }
}