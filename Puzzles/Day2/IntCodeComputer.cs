
using System;
using System.Collections.Generic;
using System.Linq;

public class IntCodeComputer
{
    private List<int> memory;
    private List<int> inputs;
    private Dictionary<int, Type> instructions = new Dictionary<int, Type>();

    public int output = 0;

    public IntCodeComputer(List<int> opCodes, List<int> inputs = null)
    {
        this.memory = opCodes;
        this.inputs = inputs ?? new List<int>{1};
        
        IEnumerable<IntCodeInstruction> types = typeof(IntCodeInstruction)
                .Assembly.GetTypes()
                .Where(t => t.IsSubclassOf(typeof(IntCodeInstruction)) && !t.IsAbstract)
                .Select(t => (IntCodeInstruction)Activator.CreateInstance(t));

        foreach(var type in types)
        {
            instructions.Add(type.Identifier, type.GetType());
        }
    }

    public void Execute()
    {
        int executionIndex = 0;
        int inputIndex = 0;
        while (executionIndex + 3 < memory.Count)
        {
            int instructionCode = memory[executionIndex];
            List<InstructionMode> modes = new List<InstructionMode>();

            int opCode = instructionCode % 100;
            int num = instructionCode / 100;
            while (num != 0)
            {
                int opNum = num % 10;
                num /= 10;
                modes.Add((InstructionMode)opNum);
            }

            if(opCode == 99)
                return;

            IntCodeInstruction instruction = Activator.CreateInstance(instructions[opCode]) as IntCodeInstruction;
            executionIndex = instruction.Execute(memory, modes, inputs[inputIndex], executionIndex);
            
            if (instruction is WriteInstruction && inputIndex + 1 < inputs.Count)
                inputIndex++;

            if (instruction is LogInstruction log)
                output = log.output;
        }
    }
}