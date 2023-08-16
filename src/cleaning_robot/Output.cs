namespace cleaning_robot;

/// <summary>
/// Outup class for creating JSON file
/// </summary>
public class Output
{
    /// <summary>
    /// Array of visited cells
    /// </summary>
    public Cell[]? visited { get; set; }

    /// <summary>
    /// Array of cleaned cells
    /// </summary>
    public Cell[]? cleaned { get; set; }

    /// <summary>
    /// Final position of the robot
    /// </summary>
    public Position? final { get; set; }

    /// <summary>
    /// Final battery level
    /// </summary>
    public int battery { get; set; }
}