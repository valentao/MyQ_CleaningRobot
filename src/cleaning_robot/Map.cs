using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cleaning_robot;

public class Map
{
    private static Map? instance = null;


    public static List<List<string>>? MapMatrix { get; private set; }

    private static int XLength { get; set; }

    private static int YLength { get; set; }

    public Map(List<List<string>> map) 
    {
        MapMatrix = map;
        XLength = map.Count;
        if (XLength > 0)
        {
            YLength = map[0].Count;
        }
        else
        {
            YLength = 0;
        }
    }

    //Lock Object
    private static object lockThis = new object();
    public static Map GetMap(List<List<string>> map)
    {
        lock (lockThis)
        {
            if (Map.instance == null)
                instance = new Map(map);
        }
        return instance;
    }

    /// <summary>
    /// Check if X,Y coordinate is part of loaded map
    /// </summary>
    /// <param name="x">X coordinate</param>
    /// <param name="y">Y coordinate</param>
    /// <returns>X,Y coordinate is part of map</returns>
    public static bool IsInMap(int x, int y)
    {
        // one of coordinates is negative
        if (x < 0 || y < 0)
        {
            Console.WriteLine($"Position X:{x}, Y:{y} is out of map. None of the coordinates can be negative.");
            return false; 
        }
        else
        {
            // x is out of map
            if (x > XLength)
            {
                Console.WriteLine($"Position X:{x} is out of map. Max X value is {XLength}.");
                return false;
            }
            else
            {
                // y is out of map
                if (y > YLength)
                {
                    Console.WriteLine($"Position Y:{y}  is out of map. Max Y value is  {y}.");
                    return false;
                }
            }
            return true;
        }
    }

    /// <summary>
    /// Check if X,Y coordinate accecessible
    /// </summary>
    /// <param name="x">X coordinate</param>
    /// <param name="y">Y coordinate</param>
    /// <returns>X,Y cell is accessible for robot</returns>
    public static bool IsCellAccessible(int x, int y)
    {
        bool isInMap = IsInMap(x, y);
        bool isAccessible = false;

        if (isInMap)
        {
            string? cellAccessibility = MapMatrix?[x][y];
            // S - can be occupied and cleaned
            // C -  can’t be occupied or cleaned
            // null - empty cell
            if (cellAccessibility != null && cellAccessibility == "S")
            {
                Console.WriteLine($"Position X:{x},Y:{y} is {cellAccessibility}.");
                isAccessible = true;
            }
            else
            {
                Console.WriteLine($"Position X:{x},Y:{y} is not accessible.");
                isAccessible = false;
            }
        }

        return isAccessible;
    }
}
