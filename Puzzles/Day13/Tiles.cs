public interface ITile
{
    string Display {get;}
}

public struct Wall : ITile
{
    public string Display => "#";
}

public struct Block : ITile
{
    public string Display => "■";
}

public struct Paddle : ITile
{
    public string Display => "_";
}

public struct Ball : ITile
{
    public string Display => "0";
}