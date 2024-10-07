namespace SpaceBattle.Lib;

public interface IMoveCommandStartable
{
    IUObject target { get; }
    Dictionary<string, object> properties { get; }
}