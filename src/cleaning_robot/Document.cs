namespace cleaning_robot;

/// <summary>
/// Class representing document
/// </summary>
public class Document
{
    /// <summary>
    /// Read file
    /// </summary>
    /// <param name="file">file</param>
    /// <returns>string content</returns>
    public static string ReadAllText(FileInfo file)
    {
        return File.ReadAllText(file.FullName);
    }

    /// <summary>
    /// Write text to file
    /// </summary>
    /// <param name="file">file to write</param>
    /// <param name="text">text to write</param>
    public static void WriteAllText(FileInfo file, string text)
    {
        File.WriteAllText(file.FullName, text);
    }

    /// <summary>
    /// Append text to file
    /// </summary>
    /// <param name="file">file to append</param>
    /// <param name="text">text to write</param>
    public static void AppendText(FileInfo file, string text)
    {
        if (file.Exists)
        {
            using (StreamWriter sw = File.AppendText(file.FullName))
            {
                sw.WriteLine(text);
            }
        }
        else
        {
            using (StreamWriter sw = File.CreateText(file.FullName))
            {
                sw.WriteLine(text);
            }
        }
    }

    /// <summary>
    /// Read file and write content to console
    /// </summary>
    /// <param name="file"></param>
    static void ReadFileToConsole(FileInfo file)
    {
        File.ReadLines(file.FullName).ToList()
            .ForEach(line => Console.WriteLine(line));
    }
}