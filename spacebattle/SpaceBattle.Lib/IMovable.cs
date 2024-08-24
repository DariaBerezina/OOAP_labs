using System.Numerics;

namespace SpaceBattle.Lib;

public interface IMovable
{
    public Vector<float> Position { get; set; }
    public Vector<float> Velocity { get; }
}