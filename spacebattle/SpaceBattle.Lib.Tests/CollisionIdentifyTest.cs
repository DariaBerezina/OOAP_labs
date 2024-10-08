using Hwdtech;
using Hwdtech.Ioc;
using Moq;

namespace SpaceBattle.Lib;
public class CollisionIndentifyCommandTest
{
    public CollisionIndentifyCommandTest()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();

        IoC.Resolve<ICommand>("Scopes.Current.Set",
            IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))
        ).Execute();
    }

    [Fact]
    public void TestCollisionIndentify_Successful()
    {
        var iCommand = new Mock<ICommand>();
        iCommand.Setup(x => x.Execute()).Verifiable();

        var commands = new Mock<IDictionary<int, object>>();
        commands.SetupGet(x => x[It.IsAny<int>()]).Returns(commands.Object);
        commands.SetupGet(x => x.Keys).Returns(new List<int> { 1 });

        IoC.Resolve<ICommand>("IoC.Register", "Game.UObject.GetProperty", (object[] args) => new List<int> { 1, 1 }).Execute();
        IoC.Resolve<ICommand>(
            "IoC.Register",
            "Game.Command.CheckCollision",
            (object[] args) => new CollisionIndentifyCommand((IUObject)args[0], (IUObject)args[1])
        ).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Game.CollisionTree", (object[] args) => commands.Object).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Game.Event.Collision", (object[] args) => iCommand.Object).Execute();

        var iUObject = new Mock<IUObject>();

        var indentifyCollisionCommand = IoC.Resolve<ICommand>("Game.Command.CheckCollision", iUObject.Object, iUObject.Object);

        indentifyCollisionCommand.Execute();

        iCommand.VerifyAll();
    }
}
