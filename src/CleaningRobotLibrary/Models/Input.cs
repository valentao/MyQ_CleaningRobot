using CleaningRobotLibrary.Logic;
using System.Text.Json.Serialization;

namespace CleaningRobotLibrary.Models;

/// <summary>
/// Input class for creating object from JSON
/// </summary>
public class Input
{
    /// <summary>
    /// Map of room
    /// </summary>
    [JsonPropertyNameAttribute("map")]
    public List<List<string>>? Map { get; set; }

    /// <summary>
    /// Array of commands
    /// </summary>
    [JsonPropertyNameAttribute("commands")]
    public string[]? Commands { get; set; }

    /// <summary>
    /// Starting position of the robot
    /// </summary>
    [JsonPropertyNameAttribute("start")]
    public Position? Start { get; set; }

    /// <summary>
    /// Initial battery level
    /// </summary>
    [JsonPropertyNameAttribute("battery")]
    public int Battery { get; set; }

}
