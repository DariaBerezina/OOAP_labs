namespace SpaceBattle.Lib;
public class MoveCommand : ICommand
{
    private readonly IMovable _movable;
    public MoveCommand(IMovable movable)
    {
        this._movable = movable;
    }

    public void Execute()
    {
        _movable.Position += _movable.Velocity;
    }
}
