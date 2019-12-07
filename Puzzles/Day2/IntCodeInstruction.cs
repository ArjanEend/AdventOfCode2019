using System;
using System.Collections.Generic;

public abstract class IntCodeInstruction
{
    public IntCodeInstruction()
    {

    }
    
    public abstract int Identifier {get;}

    public abstract int Execute(List<int> memory, List<InstructionMode> modes, int input, int index);
}

public enum InstructionMode : int {
        Position = 0,
        Immediate = 1
    }

public class WriteInstruction : IntCodeInstruction
{
    public override int Identifier => 3;

    public override int Execute(List<int> memory, List<InstructionMode> modes, int input, int index)
    {
        int value = modes.Count > 0 && modes[0] == InstructionMode.Immediate ? memory[index + 1] : memory[memory[index + 1]];
        memory[memory[index + 1]] = input;
        
        return index + 2;
    }
}

public class LogInstruction : IntCodeInstruction
{
    public override int Identifier => 4;

    public int output;

    public override int Execute(List<int> memory, List<InstructionMode> modes, int input, int index)
    {
        int value = modes.Count > 0 && modes[0] == InstructionMode.Immediate ? memory[index + 1] : memory[memory[index + 1]];
        this.output = value;
        return index + 2;
    }
}

public abstract class CompareInstruction : IntCodeInstruction
{
    public override int Execute(List<int> memory, List<InstructionMode> modes, int input, int index)
    {
        int verb = modes.Count > 0 && modes[0] == InstructionMode.Immediate ? memory[index + 1] : memory[memory[index + 1]];
        int noun = modes.Count > 1 && modes[1] == InstructionMode.Immediate ? memory[index + 2] : memory[memory[index + 2]];
        bool compare = Compare(verb, noun);
        memory[memory[index + 3]] = compare ? 1 : 0;
        return index + 4;
    }

    protected abstract bool Compare(int verb, int noun);
}


public abstract class BoolInstruction : IntCodeInstruction
{
    public override int Execute(List<int> memory, List<InstructionMode> modes, int input, int index)
    {
        int verb = modes.Count > 0 && modes[0] == InstructionMode.Immediate ? memory[index + 1] : memory[memory[index + 1]];
        int noun = modes.Count > 1 && modes[1] == InstructionMode.Immediate ? memory[index + 2] : memory[memory[index + 2]];
        bool compare = Compare(verb);
        if (compare)
            return noun;
        return index + 3;
    }

    protected abstract bool Compare(int verb);
}

public class LessThan : CompareInstruction
{
    public override int Identifier => 7;

    protected override bool Compare(int verb, int noun)
    {
        return verb < noun;
    }
}

public class EqualsInstruction : CompareInstruction
{
    public override int Identifier => 8;

    protected override bool Compare(int verb, int noun)
    {
        return verb == noun;
    }
}

public class JumpTrue : BoolInstruction
{
    public override int Identifier => 5;

    protected override bool Compare(int verb)
    {
        return verb != 0;
    }
}

public class JumpFalse : BoolInstruction
{
    public override int Identifier => 6;

    protected override bool Compare(int verb)
    {
        return verb == 0;
    }
}

public abstract class MathInstruction : IntCodeInstruction
{

    public override int Execute(List<int> memory, List<InstructionMode> modes, int input, int index)
    {
        int verb = modes.Count > 0 && modes[0] == InstructionMode.Immediate ? memory[index + 1] : memory[memory[index + 1]];
        int noun = modes.Count > 1 && modes[1] == InstructionMode.Immediate ? memory[index + 2] : memory[memory[index + 2]];
        int sum = Calculate(verb, noun);
        memory[memory[index + 3]] = sum;
        return index + 4;
    }

    protected abstract int Calculate(int verb, int noun);
}

public class AddInstruction : MathInstruction
{
    public AddInstruction()
    {

    }
    public override int Identifier => 1;

    override protected int Calculate(int verb, int noun)
    {
        return verb + noun;
    }
}

public class MultiplyInstruction : MathInstruction
{
    public override int Identifier => 2;

    override protected int Calculate(int verb, int noun)
    {
        return verb * noun;
    }
}