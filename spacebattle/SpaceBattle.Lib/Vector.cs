public class Vector
{
    private int[] _position;
    private readonly int _positionLength;
    public Vector(params int[] coordinates)
    {
        _position = coordinates;
        _positionLength = _position.Length;
    }
    public static Vector operator +(Vector x, Vector y)
    {
        var addition_result = new Vector(new int[x._positionLength]);
        addition_result._position = x._position.Select((value, index) => value + y._position[index]).ToArray();
        return addition_result;
    }
    public override bool Equals(object? obj)
    {
        return obj != null && obj is Vector vector && _position.SequenceEqual(vector._position);
    }
    public override int GetHashCode()
    {
        var hash = 17;
        hash = _position.Aggregate((hash, value) => hash * 23 + value.GetHashCode());
        hash = hash * 23 + _positionLength.GetHashCode();

        return hash;
    }
}
