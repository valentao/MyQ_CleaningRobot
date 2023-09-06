using CleaningRobotLibrary.Logic;
using CleaningRobotLibrary.Models;
using CleaningRobotLibrary.Utils;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Text.Json;

namespace CleaningRobotTests.Logic;

public class RobotTests
{
    private ILogger _logger;

    public RobotTests()
    {
        using var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder
                .AddFilter("Microsoft", LogLevel.Warning)
                .AddFilter("System", LogLevel.Warning)
                .AddFilter("LoggingConsoleApp.Program", LogLevel.Debug);
        });
        _logger = loggerFactory.CreateLogger<RobotTests>();
    }
    //using var loggerFactory = LoggerFactory.Create(builder =>
    //    {
    //        builder
    //            .AddFilter("Microsoft", LogLevel.Warning)
    //            .AddFilter("System", LogLevel.Warning)
    //            .AddFilter("LoggingConsoleApp.Program", LogLevel.Debug)
    //            .AddConsole();
    //    });

    //using var loggerFactory = LoggerFactory.Create(builder => {
    //    bui
    //});


    [Fact]
    public void RobotHasNotEnoughBattery()
    {
        //Arrange
        Robot robot = Robot.GetRobot(_logger); // default battery value is 0
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

        Robot robot = Robot.GetRobot(_logger);
        robot.LoadJson(inputJson);

        //Act
        robot.Start();
        bool result = robot.IsStucked;
        
        //Assert
        Assert.True(result, "Robot is stucked");
    }
}
