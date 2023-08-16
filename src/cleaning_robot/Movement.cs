using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cleaning_robot;

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

    public static Position Advance(Position position)
    {
        Position newPosition = position;

        if (position.facing == Robot.Facing.N || position.facing == Robot.Facing.S)
        {
            newPosition.Y += position.facing == Robot.Facing.N ? 1 : -1;
        }
        if (position.facing == Robot.Facing.E || position.facing == Robot.Facing.W)
        {
            newPosition.X += position.facing == Robot.Facing.E ? 1 : -1;
        }

        return newPosition;
    }

    public static Position Back(Position position)
    {
        Position newPosition = position;

        if (position.facing == Robot.Facing.N || position.facing == Robot.Facing.S)
        {
            newPosition.Y += position.facing == Robot.Facing.N ? -1 : 1;
        }
        if (position.facing == Robot.Facing.E || position.facing == Robot.Facing.W)
        {
            newPosition.X += position.facing == Robot.Facing.E ? -1 : 1;
        }

        return newPosition;
    }

    public static readonly string[] backOffCommands0 = new string[] { "TR", "A", "TL" };
    public static readonly string[] backOffCommands1 = new string[] { "TR", "A", "TR" };
    public static readonly string[] backOffCommands2 = new string[] { "TR", "A", "TR" };
    public static readonly string[] backOffCommands3 = new string[] { "TR", "B", "TR", "A" };
    public static readonly string[] backOffCommands4 = new string[] { "TL", "TL", "A" };

    /// <summary>
    /// Back off sequence when robot hits an obstacle
    /// </summary>
    /// <param name="hitObstacleCount">Number of attempts to back off</param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static string[] BackOffStrategy(int hitObstacleCount)
    {
        Console.WriteLine($"Back off sequence. Hit obstacle count: {hitObstacleCount}.");

        string[] backOffCommands;
        //bool backOff = true;

        // all attempts failed
        //if (hitObstacleCount == 5)
        //{
        //    backOff = false;
        //    Console.WriteLine($"End of program.");
        //    return backOff;
        //}
        //else
        //{
        // back off sequence was successful
        //if (hitObstacleCount == 2)
        //{
        //    Console.WriteLine($"Back off sequence {hitObstacleCount} was successful");
        //    return true;
        //}

        backOffCommands = hitObstacleCount switch
        {
            0 => backOffCommands0,
            1 => backOffCommands1,
            2 => backOffCommands2,
            3 => backOffCommands3,
            4 => backOffCommands4,
            _ => throw new Exception($"Unknown back off sequence")
        };

        //Console.WriteLine($"Back off sequence {hitObstacleCount} commands {string.Join(", ", backOffCommands)}");

        //hitObstacleCount += 1;
        //return BackOff(hitObstacleCount);

        return backOffCommands;
        //}
    }
}

