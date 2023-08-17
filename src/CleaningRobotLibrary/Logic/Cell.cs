using System.Text.Json.Serialization;

namespace CleaningRobotLibrary.Logic;

/// <summary>
/// Class representing a cell on map with X,Y coordinates
/// </summary>
public class Cell
{
    /// <summary>
    /// X (column) coordinate
    /// </summary>
    [JsonPropertyOrderAttribute(1)]
    public int X { get; set; }

    /// <summary>
    /// Y (row) coordinate
    /// </summary>
    [JsonPropertyOrderAttribute(2)]
    public int Y { get; set; }

    /// <summary>
    /// Initialize new instance of <see cref="Cell"/> class
    /// </summary>
    /// <param name="x">Column coordinate</param>
    /// <param name="y">Row coordinate</param>
    public Cell(int x, int y)
    {
        X = x;
        Y = y;
    }
}