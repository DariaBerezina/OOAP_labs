using Hwdtech;

namespace SpaceBattle.Lib;

public class MacroCommandStrategy : IStrategy
{
    public object Action(params object[] args)
    {
        var nameOperation = (string)args[0];
        var obj = (IUObject)args[1];

        var dependencies = IoC.Resolve<IList<string>>("Component" + nameOperation);
        IList<ICommand> commands = new List<ICommand>();

        dependencies.ToList().ForEach(dependence => commands.Add(IoC.Resolve<ICommand>(dependence, obj)));

        return new MacroCommand(commands);
    }
}