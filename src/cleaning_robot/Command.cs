namespace cleaning_robot;

/// <summary>
/// Class representing command
/// </summary>
public class Command
{
    /// <summary>
    /// Name
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Short name
    /// </summary>
    public string ShortName { get; }

    /// <summary>
    /// Battery cost
    /// </summary>
    public int Cost { get; }

    /// <summary>
    /// Description
    /// </summary>
    public string Description { get; }

    /// <summary>
    /// Turn direction 1=right, -1=left
    /// </summary>
    public int Turn { get; }

    // Create available commands
    public static readonly Command TurnLeft = new Command("Turn Left", "TL", 1, "Instructs the robot to turn 90 degrees to the left.", -1);
    public static readonly Command TurnRight = new Command("Turn Right", "TR", 1, "Instructs the robot to turn 90 degrees to the right.", 1);
    public static readonly Command Advance = new Command("Advance", "A", 2, "Instructs the robot to advance one cell forward into the next cell.", 0);
    public static readonly Command Back = new Command("Back", "B", 3, "Instructs the robot to move back one cell without changing direction.", 0);
    public static readonly Command Clean = new Command("Clean", "C", 5, "Instructs the robot to clean the current cell.", 0);

    /// <summary>
    /// Initialize new instance of <see cref="Command"/> class.
    /// </summary>
    /// <param name="name">Name of command</param>
    /// <param name="shortName">Short name of command</param>
    /// <param name="cost">Cost of command</param>
    /// <param name="description">Description of command</param>
    /// <param name="turn">Direction of command</param>
    protected Command(string name, string shortName, int cost, string description, int turn)
    {
        Name = name;
        ShortName = shortName;
        Cost = cost;
        Description = description;
        Turn = turn;
    }

    /// <summary>
    /// Initialize field of aviable commands
    /// </summary>
    private static Command[] commands = new Command[] { TurnLeft, TurnRight, Advance, Back, Clean };

    /// <summary>
    /// Return cost of command
    /// </summary>
    /// <param name="shortName">Short name of command</param>
    /// <returns>Cost of command</returns>
    public int GetCommandCost(string shortName)
    {
        var cost = commands.Where(cmd => cmd.ShortName == shortName)
        .Select(cmd => cmd.Cost)
        .FirstOrDefault();

        return cost;
    }

    /// <summary>
    /// Get command by short name
    /// </summary>
    /// <param name="shortName">Short name of command</param>
    /// <returns>Command</returns>
    public static Command? GetCommand(string shortName)
    {
        var cmd = commands.Where(cmd => cmd.ShortName == shortName)
        .FirstOrDefault();

        return cmd;
    }
}
