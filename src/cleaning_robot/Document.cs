using Newtonsoft.Json;

namespace cleaning_robot;

public class Document
{
    /// <summary>
    /// Read file
    /// </summary>
    /// <param name="file">file</param>
    /// <returns>string content</returns>
    public static string Read(FileInfo file)
    {
        return File.ReadAllText(file.FullName);
    }
    /// <summary>
    /// Write text to file
    /// </summary>
    /// <param name="file">file to write</param>
    /// <param name="text">text to write</param>
    public static void Write(FileInfo file, string text)
    {
        File.WriteAllText(file.FullName, text);
    }

    /// <summary>
    /// Read file and write to console
    /// </summary>
    /// <param name="file"></param>
    static void ReadFileToConsole(FileInfo file)
    {
        File.ReadLines(file.FullName).ToList()
            .ForEach(line => Console.WriteLine(line));
    }

    //static void WriteFile(FileInfo file, FileInfo inputFile)
    //{
    //    List<string> lines = new List<string>();

    //    File.ReadLines(inputFile.FullName).ToList()
    //        .ForEach(line => lines.Add(line));

    //    string[] linesArray = lines.ToArray();

    //    File.WriteAllLines(file.FullName, linesArray);
    //}
}
