using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace cleaning_robot;

public class Command
{
    /// <summary>
    /// Name
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// ShortName
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
    /// Turn dirrection 1=right, -1=left
    /// </summary>
    public int Turn { get; }

    // Create available commands
    public static readonly Command TurnLeft = new Command("Turn Left", "TL", 1, "Instructs the robot to turn 90 degrees to the left.", -1);
    public static readonly Command TurnRight = new Command("Turn Right", "TR", 1, "Instructs the robot to turn 90 degrees to the right.", 1);
    public static readonly Command Advance = new Command("Advance", "A", 2, "Instructs the robot to advance one cell forward into the next cell.", 0);
    public static readonly Command Back = new Command("Back", "B", 3, "Instructs the robot to move back one cell without changing direction.", 0);
    public static readonly Command Clean = new Command("Clean", "C", 5, "Instructs the robot to clean the current cell.", 0);

    protected Command(string name, string shortName, int cost, string description, int turn)
    {
        Name = name;
        ShortName = shortName;
        Cost = cost;
        Description = description;
        Turn = turn;
    }

    // 
    private static Command[] commands = new Command[] { TurnLeft, TurnRight, Advance, Back, Clean };

    public int GetCommandCost(string shortName)
    {
        var cost = commands.Where(cmd => cmd.ShortName == shortName)
        .Select(cmd => cmd.Cost)
        .FirstOrDefault();

        return cost;
    }

    public static Command? GetCommand(string shortName)
    {
        var cmd = commands.Where(cmd => cmd.ShortName == shortName)
        .FirstOrDefault();

        return cmd;
    }
}
