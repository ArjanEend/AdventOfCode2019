
using System;
using System.Collections.Generic;
using System.Linq;

public class IntCodeComputer
{
    private List<long> memory;
    private List<int> inputs;
    private Dictionary<int, Type> instructions = new Dictionary<int, Type>();

    public List<long> output = new List<long>();

    private int executionIndex = 0;
    private int relativeBase = 0;
    private int inputIndex = 0;

    public bool done = false;

    public IntCodeComputer(List<int> opCodes, List<int> inputs = null)
    {
        this.memory = opCodes.Select(i => (long)i).ToList();
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

    public void ResetIndex()
    {
        executionIndex = 0;
        inputIndex = 0;
    }

    public IntCodeComputer(List<long> opCodes, List<int> inputs = null)
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
            long instructionCode = memory[executionIndex];
            List<InstructionMode> modes = new List<InstructionMode>();

            long opCode = instructionCode % 100;
            long num = instructionCode / 100;
            while (num != 0)
            {
                long opNum = num % 10;
                num /= 10;
                modes.Add((InstructionMode)opNum);
            }

            if(opCode == 99)
            {
                done = true;
                return;
            }

            IntCodeInstruction instruction = Activator.CreateInstance(instructions[(int)opCode]) as IntCodeInstruction;

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

            executionIndex = instruction.Process(memory, modes, input, relativeBase, executionIndex);

            if (instruction is LogInstruction log)
            {
                output.Add(log.output);
            }

            if(instruction is ModifyRelativeBase baseInstr)
            {
                relativeBase = baseInstr.modifiedBase;
            }
        }
        done = true;
    }
}