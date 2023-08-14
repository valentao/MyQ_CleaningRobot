namespace cleaning_robot;

public class Output
{
    public Cell[]? visited { get; set; }

    public Cell[]? cleaned { get; set; }

    public Position? final { get; set; }

    public int battery { get; set; }
}