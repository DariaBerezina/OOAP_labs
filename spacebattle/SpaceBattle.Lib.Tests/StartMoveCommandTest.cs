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
        var target = new Mock<IUObject>();
        var iMoveCmdStartable = new Mock<IMoveCommandStartable>();

        iMoveCmdStartable.Setup(s => s.target).Returns(target.Object).Verifiable();
        iMoveCmdStartable.Setup(s => s.properties).Returns(new Dictionary<string, object>() { { "Velocity", new Vector(1, 1) } }).Verifiable();

        var startMoveCommand = new StartMoveCommand(iMoveCmdStartable.Object);

        startMoveCommand.Execute();

        iMoveCmdStartable.Verify(s => s.properties, Times.Once());
        queue.Verify(q => q.Enqueue(It.IsAny<ICommand>()), Times.Once());
    }
    [Fact]
    public void TestStartMoveCommand_ReadTargetProperties_Fail()
    {
        var target = new Mock<IUObject>();
        var iMoveCmdStartable = new Mock<IMoveCommandStartable>();
        var targetProperties = new Dictionary<string, object>();

        var properties = new Dictionary<string, object> { { "Position", "Vector" } };

        iMoveCmdStartable.SetupGet(s => s.properties).Callback(() => throw new Exception());
        iMoveCmdStartable.SetupGet(s => s.target).Callback(() => throw new Exception());
        target.Setup(o => o.SetProperty(It.IsAny<string>(), It.IsAny<object>())).Callback(() => throw new Exception()).Verifiable();

        var startCommand = new StartMoveCommand(iMoveCmdStartable.Object);

        Assert.Throws<Exception>(startCommand.Execute);
    }
}
