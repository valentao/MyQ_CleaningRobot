using CleaningRobotLibrary.Logic;
using CleaningRobotLibrary.Models;
using CleaningRobotLibrary.Utils;
using System.Text.Json;

namespace CleaningRobotTests.Logic;

public class MapTests
{
    [Fact]
    public void PositionXIsOutOfMap()
    {
        //Arrange
        int x = -1;
        int y = 0;

        //Act
        bool result = Map.IsInMap(x, y);

        //Assert
        Assert.False(result, $"Position X:{x}, Y:{y} is out of map (X coordinate)");
    }

    [Fact]
    public void PositionYIsOutOfMap()
    {
        //Arrange
        int x = 0;
        int y = -1;

        //Act
        bool result = Map.IsInMap(x, y);

        //Assert
        Assert.False(result, $"Position X:{x}, Y:{y} is out of map (Y coordinate)");
    }

    [Fact]
    public void CellIsNotAccessible_CellIsNull()
    {
        //Arrange
        string path = @"..\..\..\..\..\doc\test\test1.json";
        FileInfo file = new FileInfo(path);

        Input? input = JsonSerializer.Deserialize<Input>(Document.ReadAllText(file));

        Map.GetMap(input.Map);

        int x = 1;
        int y = 3;

        //Act
        bool result = Map.IsCellAccessible(x, y);

        //Assert 
        Assert.False(result, $"Cell X:{x}, Y:{y} is null");
    }

    [Fact]
    public void CellIsNotAccessible_CellIsC()
    {
        //Arrange
        string path = @"..\..\..\..\..\doc\test\test1.json";
        FileInfo file = new FileInfo(path);

        Input? input = JsonSerializer.Deserialize<Input>(Document.ReadAllText(file));

        Map.GetMap(input.Map);

        int x = 2;
        int y = 1;

        //Act
        bool result = Map.IsCellAccessible(x, y);

        //Assert 
        Assert.False(result, $"Cell X:{x}, Y:{y} is C");
    }
}
