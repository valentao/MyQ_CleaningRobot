namespace cleaning_robot;

/// <summary>
/// Class representing posible movements of the robot
/// </summary>
public class Movement
{
    /// <summary>
    /// Change facing direction
    /// </summary>
    /// <param name="facing">Current facing</param>
    /// <param name="move">Right = 1, Left = -1</param>
    /// <returns></returns>
    public static Robot.Facing Turn(Robot.Facing facing, int move)
    {
        var facingArray = Robot.facingArray;
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
    public static Position Advance(Position position)
    {
        Position newPosition = new Position(position.X, position.Y, position.Facing);

        if (position.Facing == Robot.Facing.N || position.Facing == Robot.Facing.S)
        {
            newPosition.Y += position.Facing == Robot.Facing.N ? -1 : 1;
        }
        if (position.Facing == Robot.Facing.E || position.Facing == Robot.Facing.W)
        {
            newPosition.X += position.Facing == Robot.Facing.E ? 1 : -1;
        }

        return newPosition;
    }

    /// <summary>
    /// Move back one cell
    /// </summary>
    /// <param name="position">Current position</param>
    /// <returns>Target position for backward movement</returns>
    public static Position Back(Position position)
    {
        Position newPosition = new Position(position.X, position.Y, position.Facing);

        if (position.Facing == Robot.Facing.N || position.Facing == Robot.Facing.S)
        {
            newPosition.Y += position.Facing == Robot.Facing.N ? 1 : -1;
        }
        if (position.Facing == Robot.Facing.E || position.Facing == Robot.Facing.W)
        {
            newPosition.X += position.Facing == Robot.Facing.E ? -1 : 1;
        }

        return newPosition;
    }

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
}

