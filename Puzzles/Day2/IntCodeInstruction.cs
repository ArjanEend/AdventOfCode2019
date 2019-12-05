using System.Collections.Generic;

public abstract class IntCodeInstruction
{
    public IntCodeInstruction()
    {

    }
    
    public abstract int Identifier {get;}
    public abstract int AmountOfParameters {get;}

    public abstract void Execute(List<int> memory);
}

public enum InstructionMode : int {
        Position = 0,
        Immediate = 1
    }

public abstract class MathInstruction : IntCodeInstruction
{
    public override int AmountOfParameters => 3;

public MathInstruction()
{

}

    public override void Execute(List<int> memory)
    {
        int verb = memory[memory[a.Value]];
        int noun = memory[memory[b.Value]];
        int sum = Calculate(verb, noun);
        memory[memory[position.Value]] = sum;
    }

    protected abstract int Calculate(int verb, int noun);
}

public class AddInstruction : MathInstruction
{
    public AddInstruction()
    {

    }
    public AddInstruction(IntCodeParam a, IntCodeParam b, IntCodeParam position) : base(a, b, position)
    {
    }
    public override int Identifier => 1;

    override protected int Calculate(int verb, int noun)
    {
        return verb + noun;
    }
}