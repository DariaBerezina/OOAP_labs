using Moq;

namespace SpaceBattle.Lib.Tests;

public class MoveCommandTest
{
    [Fact]
    public void TestExecute_ObjectMoveStraight_Successful()
    {
        var movable = new Mock<IMovable>();
        movable.SetupGet(obj => obj.Position).Returns(new Vector(12, 5)).Verifiable();
        movable.SetupGet(obj => obj.Velocity).Returns(new Vector(-7, 3)).Verifiable();
        var move_command_object = new MoveCommand(movable.Object);

        move_command_object.Execute();

        movable.VerifySet(obj => obj.Position = new Vector(5, 8), Times.Once);
        movable.VerifyAll();
    }
    [Fact]
    public void TestTryExecute_GetObjectPosition_Fail()
    {
        var movable = new Mock<IMovable>();
        movable.Setup(obj => obj.Position).Callback(() => throw new Exception());
        movable.SetupGet(obj => obj.Velocity).Returns(new Vector(-7, 3)).Verifiable();
        var move_command_object = new MoveCommand(movable.Object);

        Assert.Throws<Exception>(() => move_command_object.Execute());
    }
    [Fact]
    public void TestTryExecute_GetObjectVelocity_Fail()
    {
        var movable = new Mock<IMovable>();
        movable.SetupGet(obj => obj.Position).Returns(new Vector(12, 5)).Verifiable();
        movable.Setup(obj => obj.Velocity).Callback(() => throw new Exception());
        var move_command_object = new MoveCommand(movable.Object);

        Assert.Throws<Exception>(() => move_command_object.Execute());
    }
    [Fact]
    public void TestTryExecute_MotionlessObjectChangePostion_Fail()
    {
        var movable = new Mock<IMovable>();
        movable.SetupGet(obj => obj.Position).Returns(new Vector(12, 5)).Verifiable();
        movable.SetupGet(obj => obj.Velocity).Returns(new Vector(-7, 3)).Verifiable();
        var move_command_object = new MoveCommand(movable.Object);

        move_command_object.Execute();

        movable.SetupSet(obj => obj.Position = It.IsAny<Vector>()).Callback(() => throw new Exception());
        Assert.Throws<Exception>(() => move_command_object.Execute());
    }
}
