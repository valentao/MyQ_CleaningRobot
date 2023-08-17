using CleaningRobotLibrary.Utils;

namespace CleaningRobotTests.Utils;

public class DocumentTests
{
    [Fact]
    public void InputFileExists()
    {
        //Arrange
        string path = @"..\..\..\..\..\doc\test\test1.json";
        FileInfo file = new FileInfo(path);

        //Act
        bool result = Document.Exists(file);

        //Assert
        Assert.True(result, $"Input file {file.FullName} should exists");
    }

    //[Fact]
    //public void InputFileNotExists()
    //{
    //    //Arrange
    //    string path = @"..\..\..\..\..\doc\test\test.json";
    //    FileInfo file = new FileInfo(path);

    //    //Act
    //    bool result = file.Exists;

    //    //Assert
    //    Assert.False(result, $"Input file {file.FullName} should not exists");
    //}

    [Fact]
    public void InputFileIsJson()
    {
        //Arrange
        string path = @"..\..\..\..\..\doc\test\test1.json";
        FileInfo file = new FileInfo(path);

        //Act
        bool result = Document.IsJson(file);

        //Assert
        Assert.True(result, $"Input file {file.FullName} should be json");
    }

}