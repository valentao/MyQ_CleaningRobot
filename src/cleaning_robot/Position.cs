using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace cleaning_robot;

/// <summary>
/// Class representing robot possition. Class adds Facing parameter to <see cref="Cell"/> class. 
/// </summary>
public class Position : Cell
{
    /// <summary>
    /// Facing direction enum
    /// </summary>
    public enum FacingDirection
    {
        N,
        E,
        S,
        W
    }

    /// <summary>
    /// Array of facing directions
    /// </summary>
    public static readonly FacingDirection[] facingArray = new[] {
    FacingDirection.N,
    FacingDirection.E,
    FacingDirection.S,
    FacingDirection.W
    };


    /// <summary>
    /// Initialize new instance of <see cref="Position"/> class
    /// </summary>
    /// <param name="x">Collumn coordinate</param>
    /// <param name="y">Row coordinate</param>
    /// <param name="facing"><see cref="Facing"/> enum value</param>
    public Position(int x, int y, FacingDirection facing) : base(x, y)
    {
        base.X = x;
        base.Y = y;
        this.Facing = facing;
    }

    /// <summary>
    /// Facing direction
    /// </summary>
    [JsonProperty(Order = 3), JsonConverter(typeof(StringEnumConverter))]
    public FacingDirection Facing { get; set; }
}