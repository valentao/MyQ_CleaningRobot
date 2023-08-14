using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cleaning_robot;

public class Robot
{
    public enum Facing
    {
        N,
        E,
        S,
        W
    }

   public static readonly Facing[] facingArray = new[] {
    Facing.N,
    Facing.E,
    Facing.S,
    Facing.W
    };

    public enum Commands
    {

    }

    private static Robot? instance = null;

    private Robot()
    {

    }

    public List<List<string>>? Map { get; set; }

    //public Facing FacingDirection { get; set; }

    public int Battery { get; set; }

    public Position Position { get; set; }

    public Command[] CommandsArray { get; set; }

    public List<Cell> Visited { get; set; }

    public List<Cell> Cleaned { get; set; }


    //Lock Object
    private static object lockThis = new object();
    public static Robot GetRobot()
    {
        lock (lockThis)
        {
            if (Robot.instance == null)
                instance = new Robot();
        }
        return instance;
    }
}
