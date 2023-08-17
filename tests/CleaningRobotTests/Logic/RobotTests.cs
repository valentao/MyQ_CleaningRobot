using CleaningRobotLibrary.Logic;
using CleaningRobotLibrary.Models;
using CleaningRobotLibrary.Utils;
using System.Text.Json;

namespace CleaningRobotTests.Logic;

public class RobotTests
{
    [Fact]
    public void RobotHasNotEnoughBattery()
    {
        //Arrange
        Robot robot = Robot.GetRobot(); // default battery value is 0
        Command cmd = Command.Advance;

        //Act
        bool result = Robot.IsBatteryEnough(cmd.Cost);

        //Assert
        Assert.False(result, $"Command {cmd.Name} requires more battery ({cmd.Cost}) than is actual capacity {robot.Battery}.");
    }

    [Fact]
    public void RobotIsStucked()
    {
        //Arange
        string path = @"..\..\..\..\..\doc\test\testStucked.json";
        FileInfo file = new FileInfo(path);

        Robot robot = Robot.GetRobot();
        Robot.LoadJson(file);

        //Act
        Robot.Start();
        bool result = robot.IsStucked;
        
        //Assert
        Assert.True(result, "Robot is stucked");
    }
}
