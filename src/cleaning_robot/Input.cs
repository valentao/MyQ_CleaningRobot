namespace cleaning_robot;

/// <summary>
/// Input class for creating object from JSON
/// </summary>
public class Input
{
    /// <summary>
    /// Map of room
    /// </summary>
    public List<List<string>>? map { get; set; }

    /// <summary>
    /// Array of commands
    /// </summary>
    public string[]? commands { get; set; }

    /// <summary>
    /// Starting position of the robot
    /// </summary>
    public Position? start { get; set; }

    /// <summary>
    /// Initial battery level
    /// </summary>
    public int battery { get; set; }

}
