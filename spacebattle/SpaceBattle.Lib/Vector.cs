public class Vector
{
    private int[] _position;

    public int this[int i]
    {
        get { return _position[i]; }
        set { _position[i] = value; }
    }
    public Vector(int[] coordinates)
    {
        _position = coordinates;
    }
    private int Length()
    {
        return _position.Length;
    }
    public static Vector operator +(Vector x, Vector y)
    {
        Vector addition_result = new Vector(new int[x.Length()]);
        addition_result._position = x._position.Select((value, index) => value + y[index]).ToArray();
        return addition_result;
    }
    public override bool Equals(object? obj)
    {
        return obj != null && obj is Vector vector && _position.SequenceEqual(vector._position);
    }
    public override int GetHashCode()
    {
        return _position.GetHashCode();
    }
}
