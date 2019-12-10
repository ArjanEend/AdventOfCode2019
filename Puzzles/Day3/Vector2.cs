using System;

public struct IntVector2
{
    private const float Eps = 1e-7f;
    public int x;
    public int y;

    public IntVector2(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public static IntVector2 operator +(IntVector2 a, IntVector2 b)
    {
        return new IntVector2 ((dynamic)a.x + b.x, (dynamic)a.y + b.y);
    }

    public static IntVector2 operator -(IntVector2 a, IntVector2 b)
    {
        return new IntVector2((dynamic)a.x - b.x, (dynamic)a.y - b.y);
    }

    public static IntVector2 operator *(IntVector2 a, int b)
    {
        return new IntVector2((dynamic)a.x * b, (dynamic)a.y * b);
    }

    public static bool operator ==(IntVector2 a, IntVector2 b)
    {
        return a.x == b.x && a.y == b.y;
    }

    public static bool operator !=(IntVector2 a, IntVector2 b)
    {
        return !(a == b);
    }

    public float Magnitude()
    {
        return MathF.Sqrt(x * x + y * y);
    }

    public IntVector2 Intersection(IntVector2 dir, IntVector2 other, IntVector2 otherDir)
    {
        if(((dynamic)x == other.x && (dynamic)y == other.y) || (x == (-(dynamic)other.x) && y == (-(dynamic)other.y)))
            return new IntVector2(0,0);

        IntVector2 difference = (dynamic)other - this;

        int t = difference.Dot(otherDir) / dir.Perp(otherDir);

        return new IntVector2((int)(x + dir.x * t), (int)( y + (dynamic)dir.y * t));
    }

        public int Dot(IntVector2 other)
        {
            return (dynamic)x * other.x + (dynamic)y * other.y;
        }
    
        public int Perp(IntVector2 other)
        {
            return (dynamic)x * other.y - (dynamic)y * other.x;
        }

    public override bool Equals(object obj)
    {
        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        return x.GetHashCode() ^ y.GetHashCode();
    }

    public override string ToString()
    {
        return base.ToString();
    }
}