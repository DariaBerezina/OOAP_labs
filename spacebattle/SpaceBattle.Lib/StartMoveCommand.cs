using Hwdtech;
using Spacebattle.Lib;

namespace SpaceBattle.Lib;
public class StartMoveCommand : ICommand
{
    private readonly IMoveCommandStartable _iMoveCmdStartable;

    public StartMoveCommand(IMoveCommandStartable iMoveCmdStartable)
    {
        _iMoveCmdStartable = iMoveCmdStartable;
    }

    public void Execute()
    {

        _iMoveCmdStartable.properties.ToList().ForEach(a => IoC.Resolve<ICommand>("Properties.Set", _iMoveCmdStartable.target, a.Key, a.Value).Execute());
        var cmd = IoC.Resolve<ICommand>("Operations.Movement", _iMoveCmdStartable.target);
        var injectable = IoC.Resolve<ICommand>("Commands.Injectable", cmd);
        IoC.Resolve<ICommand>("Properties.Set", _iMoveCmdStartable.target, "Operations.Movement", cmd).Execute();
        IoC.Resolve<IQueue>("Game.Queue").Enqueue(injectable);
    }
}
