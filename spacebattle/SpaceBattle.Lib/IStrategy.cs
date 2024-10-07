namespace SpaceBattle.Lib;

public interface IStrategy
{
    public object Action(params object[] args);
}