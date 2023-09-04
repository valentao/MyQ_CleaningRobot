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
        bool result = robot.IsBatteryEnough(cmd.Cost);

        //Assert
        Assert.False(result, $"Command {cmd.Name} requires more battery ({cmd.Cost}) than is actual capacity {robot.Battery}.");
    }

    [Fact]
    public void RobotIsStucked()
    {
        //Arange
        string inputJson = @"{
  ""map"": [
    [""S"", ""C"", ""S"", ""S""],
    [""C"", ""S"", ""C"", ""S""],
    [""S"", ""S"", ""S"", ""S""],
    [""S"", ""null"", ""S"", ""S""]
  ],
  ""start"": {""X"": 0, ""Y"": 0, ""facing"": ""N""},
  ""commands"": [ ""TL"",""A"",""C"",""A"",""C"",""TR"",""A"",""C""],
  ""battery"": 80
}";

        Robot robot = Robot.GetRobot();
        robot.LoadJson(inputJson);

        //Act
        robot.Start();
        bool result = robot.IsStucked;
        
        //Assert
        Assert.True(result, "Robot is stucked");
    }
}
