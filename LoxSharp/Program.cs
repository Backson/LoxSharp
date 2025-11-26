
namespace LoxSharp;

class Program
{
    static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            RunInteractively();
        }
        else if (args.Length == 1)
        {
            RunFile(args[0]);
        }
        else
        {
            Console.WriteLine($"Usage: LoxSharp [file]");
        }
    }

    private static void RunInteractively()
    {
        while (GetInput() is string line)
            Run(line);
    }

    private static string? GetInput()
    {
        Console.Write("> ");
        return Console.ReadLine();
    }

    private static void RunFile(string path)
    {
        string source = File.ReadAllText(path);
        Run(source);
    }

    private static void Run(string source)
    {
        Console.WriteLine(source);
    }
}
