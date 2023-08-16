using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace cleaning_robot;

public class Position : Cell
{
    public Position(int x, int y, Robot.Facing facing) : base(x, y)
    {
        base.X = x;
        base.Y = y;
        this.facing = facing;
    }

    [JsonProperty(Order = 3), JsonConverter(typeof(StringEnumConverter))]
    public Robot.Facing facing { get; set; }
}




