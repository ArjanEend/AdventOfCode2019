
using System;

public class Moon
{
    public Vector3 position = new Vector3();
    public Vector3 velocity = new Vector3();

    public Vector3 originalPos;

    public float GetEnergy()
    {
        return (MathF.Abs(position.x) + MathF.Abs(position.y) + MathF.Abs(position.z)) 
            * (MathF.Abs(velocity.x) + MathF.Abs(velocity.y) + MathF.Abs(velocity.z));
    }
}