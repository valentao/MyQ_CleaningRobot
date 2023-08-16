﻿namespace cleaning_robot;

/// <summary>
/// Class representing map
/// </summary>
public class Map
{
    private static Map? map = null;

    /// <summary>
    /// Map matrix
    /// </summary>
    public static List<List<string>>? Matrix { get; private set; }

    /// <summary>
    /// Number of columns
    /// </summary>
    private static int ColumnsCount { get; set; }

    /// <summary>
    /// Number of rows
    /// </summary>
    private static int RowsCount { get; set; }

    /// <summary>
    /// Initialize new instance of <see cref="Map"/> class.
    /// </summary>
    /// <param name="map">Map matrix</param>
    public Map(List<List<string>> map)
    {
        Matrix = map;
        RowsCount = map.Count;
        if (RowsCount > 0)
        {
            ColumnsCount = map[0].Count;
        }
        else
        {
            ColumnsCount = 0;
        }
    }

    //Lock Object
    private static object lockThis = new object();
    /// <summary>
    /// Return instance of <see cref="Map"/> class.
    /// </summary>
    /// <param name="matrix"></param>
    /// <returns>Instance of Map object</returns>
    public static Map GetMap(List<List<string>> matrix)
    {
        lock (lockThis)
        {
            if (Map.map == null)
                map = new Map(matrix);
        }
        return map;
    }

    /// <summary>
    /// Check if X,Y coordinate is the part of the loaded map
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
            if (x > ColumnsCount)
            {
                Console.WriteLine($"Position X:{x} is out of map. Max X value is {ColumnsCount}.");
                return false;
            }
            else
            {
                // y is out of map
                if (y > RowsCount)
                {
                    Console.WriteLine($"Position Y:{y}  is out of map. Max Y value is  {RowsCount}.");
                    return false;
                }
            }
            return true;
        }
    }

    /// <summary>
    /// Check if X,Y coordinate is accecessible
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
            string? cellAccessibility = Matrix?[y][x]; // y=row index, x=column index
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
