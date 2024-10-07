using Hwdtech;
using Hwdtech.Ioc;
using Moq;

namespace SpaceBattle.Lib.Tests;

public class MacroCommandTest
{
    public MacroCommandTest()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Strategy.MacroCommand", (object[] args) =>
        {
            var operationName = (string)args[0];
            var obj = (IUObject)args[1];
            return new MacroCommandStrategy().Action(operationName, obj);
        }).Execute();
    }

    [Fact]
    public void TestExecuteCommands_Successfuly()
    {
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Current"))).Execute();

        var nameOperation = "MovementAndRotationOperation";
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Component" + nameOperation, (object[] args) =>
        new string[] { "Game.Operation.Move" }).Execute();

        var obj = new Mock<IUObject>();

        var moveCommand = new Mock<ICommand>();
        moveCommand.Setup(x => x.Execute()).Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Operation.Move", (object[] args) => moveCommand.Object).Execute();

        var macroCommand = IoC.Resolve<ICommand>("Game.Strategy.MacroCommand", nameOperation, obj.Object);

        macroCommand.Execute();

        moveCommand.Verify(x => x.Execute(), Times.Once);
    }

    [Fact]
    public void TestExecuteCommands_Fail()
    {
        var cmd1 = new Mock<ICommand>();
        cmd1.Setup(x => x.Execute()).Throws(new Exception());

        var cmd2 = new Mock<ICommand>();
        cmd2.Setup(x => x.Execute()).Verifiable();

        var commands = new List<ICommand> { cmd1.Object, cmd2.Object };

        Assert.Throws<Exception>(() => new MacroCommand(commands).Execute());
        cmd2.Verify(x => x.Execute(), Times.Never);
    }
}