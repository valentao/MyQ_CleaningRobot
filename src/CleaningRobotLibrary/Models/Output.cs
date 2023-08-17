using CleaningRobotLibrary.Logic;
using System.Text.Json.Serialization;

namespace CleaningRobotLibrary.Models;

/// <summary>
/// Outup class for creating JSON file
/// </summary>
public class Output
{
    /// <summary>
    /// Array of visited cells
    /// </summary>
    [JsonPropertyNameAttribute("visited")]
    public Cell[]? Visited { get; set; }

    /// <summary>
    /// Array of cleaned cells
    /// </summary>
    [JsonPropertyNameAttribute("cleaned")]
    public Cell[]? Cleaned { get; set; }

    /// <summary>
    /// Final position of the robot
    /// </summary>
    [JsonPropertyNameAttribute("final")]
    public Position? Final { get; set; }

    /// <summary>
    /// Final battery level
    /// </summary>
    [JsonPropertyNameAttribute("battery")]
    public int Battery { get; set; }
}