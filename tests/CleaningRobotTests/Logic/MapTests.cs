using CleaningRobotLibrary.Logic;
using CleaningRobotLibrary.Models;
using CleaningRobotLibrary.Utils;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace CleaningRobotTests.Logic;

public class MapTests
{
    private ILogger _logger;

    public MapTests()
    {
        using var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder
                .AddFilter("Microsoft", LogLevel.Warning)
                .AddFilter("System", LogLevel.Warning)
                .AddFilter("LoggingConsoleApp.Program", LogLevel.Debug);
        });
        _logger = loggerFactory.CreateLogger<MapTests>();
    }


    [Fact]
    public void PositionXIsOutOfMap()
    {
        //Arrange
        var map = Map.GetMap(_logger, null);
        int x = -1;
        int y = 0;

        //Act
        bool result = map.IsInMap( x, y);

        //Assert
        Assert.False(result, $"Position X:{x}, Y:{y} is out of map (X coordinate)");
    }

    [Fact]
    public void PositionYIsOutOfMap()
    {
        //Arrange
        var map = Map.GetMap(_logger, null);
        int x = 0;
        int y = -1;

        //Act
        bool result = map.IsInMap(x, y);

        //Assert
        Assert.False(result, $"Position X:{x}, Y:{y} is out of map (Y coordinate)");
    }

    [Fact]
    public void CellIsNotAccessible_CellIsNull()
    {
        //Arrange
        string inputJson = @"{
  ""map"": [
    [""S"", ""S"", ""S"", ""S""],
    [""S"", ""S"", ""C"", ""S""],
    [""S"", ""S"", ""S"", ""S""],
    [""S"", ""null"", ""S"", ""S""]
  ],
  ""start"": {""X"": 3, ""Y"": 0, ""facing"": ""N""},
  ""commands"": [ ""TL"",""A"",""C"",""A"",""C"",""TR"",""A"",""C""],
  ""battery"": 80
}";
        Input? input = JsonSerializer.Deserialize<Input>(inputJson);

        var map = Map.GetMap(_logger, input.Map);

        int x = 1;
        int y = 3;

        //Act
        bool result = map.IsCellAccessible(x, y);

        //Assert 
        Assert.False(result, $"Cell X:{x}, Y:{y} is null");
    }

    [Fact]
    public void CellIsNotAccessible_CellIsC()
    {
        //Arrange
        string inputJson = @"{
  ""map"": [
    [""S"", ""S"", ""S"", ""S""],
    [""S"", ""S"", ""C"", ""S""],
    [""S"", ""S"", ""S"", ""S""],
    [""S"", ""null"", ""S"", ""S""]
  ],
  ""start"": {""X"": 3, ""Y"": 0, ""facing"": ""N""},
  ""commands"": [ ""TL"",""A"",""C"",""A"",""C"",""TR"",""A"",""C""],
  ""battery"": 80
}";
        Input? input = JsonSerializer.Deserialize<Input>(inputJson);
        var map = Map.GetMap(_logger, input.Map);

        int x = 2;
        int y = 1;

        //Act
        bool result = map.IsCellAccessible(x, y);

        //Assert 
        Assert.False(result, $"Cell X:{x}, Y:{y} is C");
    }
}
