using Newtonsoft.Json;

namespace cleaning_robot;

public class Cell
{
    [JsonProperty(Order = 1)]
    public int X { get; set; }

    [JsonProperty(Order = 1)]
    public int Y { get; set; }

    public Cell(int x, int y)
    {
        X = x;
        Y = y;
    }
}




