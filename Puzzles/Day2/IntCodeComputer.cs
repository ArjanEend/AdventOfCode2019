
using System;
using System.Collections.Generic;
using System.Linq;

public class IntCodeComputer
{
    private List<int> memory;
    private List<int> inputs;
    private Dictionary<int, Type> instructions = new Dictionary<int, Type>();

    public List<int> output = new List<int>();

    private int executionIndex = 0;
    private int inputIndex = 0;

    public bool done = false;

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

    public void AddInput(int input)
    {
        inputs.Add(input);
    }

    public void Execute()
    {
        while (executionIndex < memory.Count)
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
            {
                done = true;
                return;
            }

            IntCodeInstruction instruction = Activator.CreateInstance(instructions[opCode]) as IntCodeInstruction;

            int input = 0;
            if(instruction is WriteInstruction && inputIndex >= inputs.Count)
            {
                return;
            } else if (inputIndex >= inputs.Count) {
                input = inputs[0];
            } else {
                input = inputs[inputIndex];
                if(instruction is WriteInstruction) 
                inputIndex++;
            }

            executionIndex = instruction.Execute(memory, modes, input, executionIndex);

            if (instruction is LogInstruction log)
            {
                output.Add(log.output);
            }
        }
        done = true;
    }
}