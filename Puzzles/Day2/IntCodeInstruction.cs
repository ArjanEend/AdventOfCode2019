using System;
using System.Collections.Generic;

public abstract class IntCodeInstruction
{
    public IntCodeInstruction()
    {

    }

    protected long this[int index] 
    {  
         get {
             while(index >= memory.Count)
                memory.Add(0);
             return memory[index];
         }
         private set {
            while(index >= memory.Count)
                memory.Add(0);
             memory[index] = value;
         }
    }  
    
    public abstract int Identifier {get;}

    private List<long> memory;
    private List<InstructionMode> modes;
    protected long input;
    protected int index;
    protected long relativeBase;

    public int Process(List<long> memory, List<InstructionMode> modes, long input, long relativeBase, int index)
    {
        this.memory = memory;
        this.modes = modes;
        this.input = input;
        this.index = index;
        this.relativeBase = relativeBase;
        return Execute();
    }

    protected abstract int Execute();

    protected long GetParameter(int paramIndex)
    {
        if(modes.Count > paramIndex)
        {
            if (modes[paramIndex] == InstructionMode.Immediate)
                return this[index + paramIndex + 1];
            if (modes[paramIndex] == InstructionMode.Relative)
                return this[(int)relativeBase + (int)this[index + paramIndex + 1]];

        }
        return this[(int)this[index + paramIndex + 1]];
    }

    protected void WriteParameter(int paramIndex, long value)
    {
        if(modes.Count > paramIndex)
        {
            if (modes[paramIndex] == InstructionMode.Relative)
            {
                this[(int)relativeBase + (int)this[index + paramIndex + 1]] = value;
                return;
            }
            if (modes[paramIndex] == InstructionMode.Immediate)
            {
                this[index + paramIndex + 1] = value;
                return;
            }

        }
        
        this[(int)this[index + paramIndex + 1]] = value;
    }
}

public enum InstructionMode : int {
        Position = 0,
        Immediate = 1,
        Relative = 2
    }

public class WriteInstruction : IntCodeInstruction
{
    public override int Identifier => 3;

    protected override int Execute()
    {
        long value = GetParameter(0);
        WriteParameter(0, input);
        //this[(int)this[index + 1]] = input;
        
        return index + 2;
    }
}

public class LogInstruction : IntCodeInstruction
{
    public override int Identifier => 4;

    public long output;

    protected override int Execute()
    {
        long value = GetParameter(0);
        this.output = value;
        return index + 2;
    }
}

public abstract class CompareInstruction : IntCodeInstruction
{
    protected override int Execute()
    {
        long verb = GetParameter(0);
        long noun = GetParameter(1);
        bool compare = Compare(verb, noun);
        WriteParameter(2, compare ? 1 : 0);
        return index + 4;
    }

    protected abstract bool Compare(long verb, long noun);
}


public abstract class BoolInstruction : IntCodeInstruction
{
    protected override int Execute()
    {
        long verb = GetParameter(0);
        long noun = GetParameter(1);
        bool compare = Compare(verb);
        if (compare)
            return (int)noun;
        return index + 3;
    }

    protected abstract bool Compare(long verb);
}

public class LessThan : CompareInstruction
{
    public override int Identifier => 7;

    protected override bool Compare(long verb, long noun)
    {
        return verb < noun;
    }
}

public class EqualsInstruction : CompareInstruction
{
    public override int Identifier => 8;

    protected override bool Compare(long verb, long noun)
    {
        return verb == noun;
    }
}

public class JumpTrue : BoolInstruction
{
    public override int Identifier => 5;

    protected override bool Compare(long verb)
    {
        return verb != 0;
    }
}

public class JumpFalse : BoolInstruction
{
    public override int Identifier => 6;

    protected override bool Compare(long verb)
    {
        return verb == 0;
    }
}

public abstract class MathInstruction : IntCodeInstruction
{

    protected override int Execute()
    {
        long verb = GetParameter(0);
        long noun = GetParameter(1);
        long sum = Calculate(verb, noun);
        WriteParameter(2, sum);
        return index + 4;
    }

    protected abstract long Calculate(long verb, long noun);
}

public class AddInstruction : MathInstruction
{
    public AddInstruction()
    {

    }
    public override int Identifier => 1;

    override protected long Calculate(long verb, long noun)
    {
        return verb + noun;
    }
}

public class MultiplyInstruction : MathInstruction
{
    public override int Identifier => 2;

    override protected long Calculate(long verb, long noun)
    {
        return verb * noun;
    }
}

public class ModifyRelativeBase : IntCodeInstruction
{
    public override int Identifier => 9;

    public long modifiedBase;

    protected override int Execute()
    {
        long value = GetParameter(0);
        relativeBase += (int)value;
        modifiedBase = relativeBase;
        
        return index + 2;
    }
}