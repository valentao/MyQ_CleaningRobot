using static cleaning_robot.Position;

namespace cleaning_robot;

/// <summary>
/// Class representing command
/// </summary>
public class Command
{
    #region Properties

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
    public int TurnDirection { get; }

    #endregion

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
    protected Command(string name, string shortName, int cost, string description, int turnDirection)
    {
        Name = name;
        ShortName = shortName;
        Cost = cost;
        Description = description;
        TurnDirection = turnDirection;
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

    #region Movements

    /// <summary>
    /// Change facing direction
    /// </summary>
    /// <param name="facing">Current facing</param>
    /// <param name="move">Right = 1, Left = -1</param>
    /// <returns></returns>
    public static FacingDirection MoveTurn(FacingDirection facing, int move)
    {
        var facingArray = Position.facingArray;
        int currentFacing = Array.IndexOf(facingArray, facing);
        int turnedFacing = currentFacing + move;

        // array 0-3, Enum 1-4
        if (turnedFacing == -1) //overflow
        {
            turnedFacing = 3;
        }
        if (turnedFacing == 4) // overflow
        {
            turnedFacing = 0;
        }

        return facingArray[turnedFacing];
    }

    /// <summary>
    /// Advance one cell forward
    /// </summary>
    /// <param name="position">Current position</param>
    /// <returns>Target position for advance movement</returns>
    public static Position MoveAdvance(Position position)
    {
        Position newPosition = new Position(position.X, position.Y, position.Facing);

        if (position.Facing == FacingDirection.N || position.Facing == FacingDirection.S)
        {
            newPosition.Y += position.Facing == FacingDirection.N ? -1 : 1;
        }
        if (position.Facing == FacingDirection.E || position.Facing == FacingDirection.W)
        {
            newPosition.X += position.Facing == FacingDirection.E ? 1 : -1;
        }

        return newPosition;
    }

    /// <summary>
    /// Move back one cell
    /// </summary>
    /// <param name="position">Current position</param>
    /// <returns>Target position for backward movement</returns>
    public static Position MoveBack(Position position)
    {
        Position newPosition = new Position(position.X, position.Y, position.Facing);

        if (position.Facing == FacingDirection.N || position.Facing == FacingDirection.S)
        {
            newPosition.Y += position.Facing == FacingDirection.N ? 1 : -1;
        }
        if (position.Facing == FacingDirection.E || position.Facing == FacingDirection.W)
        {
            newPosition.X += position.Facing == FacingDirection.E ? -1 : 1;
        }

        return newPosition;
    }

    #region Back off strategy

    /// <summary>
    /// Arrays of back off strategies commands
    /// </summary>
    public static readonly string[] backOffCommands1 = new string[] { "TR", "A", "TL" };
    public static readonly string[] backOffCommands2 = new string[] { "TR", "A", "TR" };
    public static readonly string[] backOffCommands3 = new string[] { "TR", "A", "TR" };
    public static readonly string[] backOffCommands4 = new string[] { "TR", "B", "TR", "A" };
    public static readonly string[] backOffCommands5 = new string[] { "TL", "TL", "A" };

    /// <summary>
    /// Back off strategy when robot hits an obstacle
    /// </summary>
    /// <param name="hitObstacleCount">Number of attempts to back off</param>
    /// <returns>Array of current back off strategy commands</returns>
    /// <exception cref="Exception">Thrown when unknow back off strategy index</exception>
    public static string[] BackOffStrategy(int hitObstacleCount)
    {
        string[] backOffCommands;

        backOffCommands = hitObstacleCount switch
        {
            1 => backOffCommands1,
            2 => backOffCommands2,
            3 => backOffCommands3,
            4 => backOffCommands4,
            5 => backOffCommands5,
            _ => throw new Exception($"Unknown back off strategy")
        };

        return backOffCommands;
    }

    #endregion

    #endregion
}