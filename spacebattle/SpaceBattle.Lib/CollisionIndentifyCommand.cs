using Hwdtech;

namespace SpaceBattle.Lib;
public class CollisionIndentifyCommand : ICommand
{
    private readonly IUObject _obj1;
    private readonly IUObject _obj2;

    public CollisionIndentifyCommand(IUObject obj1, IUObject obj2)
    {
        _obj1 = obj1;
        _obj2 = obj2;
    }

    public void Execute()
    {
        var position1 = IoC.Resolve<List<int>>("Game.UObject.GetProperty", _obj1, "Position");
        var velocity1 = IoC.Resolve<List<int>>("Game.UObject.GetProperty", _obj1, "Velocity");
        var position2 = IoC.Resolve<List<int>>("Game.UObject.GetProperty", _obj2, "Position");
        var velocity2 = IoC.Resolve<List<int>>("Game.UObject.GetProperty", _obj2, "Velocity");

        var position_attributes = position1.Zip(position2, (first, second) => first - second).ToList();
        var velocity_attributes = velocity1.Zip(velocity2, (first, second) => first - second).ToList();

        var attributes = position_attributes.Concat(velocity_attributes).ToList();

        var collisionTree = IoC.Resolve<IDictionary<int, object>>("Game.CollisionTree");

        attributes.ForEach(attribute => collisionTree = (IDictionary<int, object>)collisionTree[attribute]);

        IoC.Resolve<ICommand>("Game.Event.Collision", _obj1, _obj2).Execute();
    }
}
