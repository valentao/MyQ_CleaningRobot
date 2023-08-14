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
            newPosition.X = position.facing == Robot.Facing.N ? 1 : -1;
        }
        if (position.facing == Robot.Facing.E || position.facing == Robot.Facing.W)
        {
            newPosition.Y = position.facing == Robot.Facing.E ? 1 : -1;
        }

        return newPosition;
    }

    public static Position Back(Position position)
    {
        Position newPosition = position;

        if (position.facing == Robot.Facing.N || position.facing == Robot.Facing.S)
        {
            newPosition.X = position.facing == Robot.Facing.N ? -1 : 1;
        }
        if (position.facing == Robot.Facing.E || position.facing == Robot.Facing.W)
        {
            newPosition.Y = position.facing == Robot.Facing.E ? -1 : 1;
        }

        return newPosition;
    }
}
