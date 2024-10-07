using Hwdtech;
using Hwdtech.Ioc;
using Moq;
using Spacebattle.Lib;

namespace SpaceBattle.Lib.Tests;

public class StartMoveCommandTest
{
    private static readonly Mock<IQueue> queue;

    static StartMoveCommandTest()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();

        var movement = new Mock<ICommand>();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Operations.Movement", (object[] args) => movement.Object).Execute();

        var injectable = new Mock<ICommand>();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Commands.Injectable", (object[] args) => injectable.Object).Execute();

        var setPropertiesCommand = new Mock<ICommand>();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Properties.Set", (object[] args) => setPropertiesCommand.Object).Execute();

        queue = new Mock<IQueue>();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Queue", (object[] args) => queue.Object).Execute();
    }

    [Fact]
    public void TestStartMoveCommand_TargetAddToQueue_Successfuly()
    {
        var target = new Mock<IUObject>();
        var targetProperties = new Dictionary<string, object>();
        target.Setup(t => t.SetProperty(It.IsAny<string>(), It.IsAny<object>())).Callback<string, object>(targetProperties.Add).Verifiable();

        target.Object.SetProperty("key", "value");

        target.Setup(t => t.GetProperty(It.IsAny<string>())).Returns(targetProperties["key"]).Verifiable();

        var value = target.Object.GetProperty("key");

        Assert.Equal(targetProperties["key"], value);
    }
    [Fact]
    public void TestStartMoveCommand_PropertiesAddToQueue_Successfuly()
    {
        var uobject = new Mock<IUObject>();
        var startable = new Mock<IMoveCommandStartable>();

        startable.Setup(s => s.target).Returns(uobject.Object).Verifiable();
        startable.Setup(s => s.properties).Returns(new Dictionary<string, object>() { { "Velocity", new Vector(1, 1) } }).Verifiable();

        var startMoveCommand = new StartMoveCommand(startable.Object);

        startMoveCommand.Execute();

        startable.Verify(s => s.properties, Times.Once());
        queue.Verify(q => q.Enqueue(It.IsAny<ICommand>()), Times.Once());
    }


}
