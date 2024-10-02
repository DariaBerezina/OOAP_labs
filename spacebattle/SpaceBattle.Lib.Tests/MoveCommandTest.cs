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
}
