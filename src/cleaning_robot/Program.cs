using System.CommandLine;

namespace cleaning_robot;
class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine($"Number of arguments: {args.Length}");

        if (args.Length == 2)
        {
            string inputFileArg = args[0];
            string outputFileArg = args[1];

            FileInfo inputFile = new FileInfo(inputFileArg);
            FileInfo outputFile = new FileInfo(outputFileArg);
            if (inputFile.Exists && inputFile.Extension == ".json")
            {
                ReadFile(inputFile);
            }
            else
            {
                Console.WriteLine($"Argument {inputFileArg} is not valid .json file");
            }

            if (outputFile.Exists)
            {
                Console.WriteLine($"Output file {outputFileArg} already exists");
            }
            else
            {
                WriteFile(outputFile, inputFile);
            }
        }
        else
        {
            Console.WriteLine("the application requires exactly 2 arguments");
        }

        Console.ReadLine();
        Environment.Exit(0);
    }

    static void ReadFile(FileInfo file)
    {
        File.ReadLines(file.FullName).ToList()
            .ForEach(line => Console.WriteLine(line));
    }

    static void WriteFile(FileInfo file, FileInfo inputFile)
    {
        List<string> lines = new List<string>();

        File.ReadLines(inputFile.FullName).ToList()
            .ForEach(line => lines.Add(line));

        string[] linesArray = lines.ToArray();

        File.WriteAllLines(file.FullName, linesArray);
    }
}