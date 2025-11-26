
namespace LoxSharp;

static class Program
{
    // Exit codes that the program returns and their ordinal values.
    enum ExitCode
    {
        Ok = 0,
        InvalidCommandLine = 1,
        EndOfStream = 2,
        SyntaxError = 3,
        FileNotFound = 4,
    }

    // main entry point
    static int Main(string[] args)
    {
        // call the the implementation that returns an ExitCode and cast it to int
        // We have to do this, because the runtime expects the entry point to return int
        return (int)LoxMain(args);
    }

    private static ExitCode LoxMain(string[] args)
    {
        if (args.Length == 0)
        {
            return RunInteractively();
        }
        else if (args.Length == 1)
        {
            return RunFile(args[0]);
        }
        else
        {
            Console.WriteLine($"Usage: LoxSharp [file]");
            return ExitCode.InvalidCommandLine;
        }
    }

    private static bool hadError = false;

    private static ExitCode RunInteractively()
    {
        while (true)
        {
            Console.Write("> ");
            string? line = Console.ReadLine();
            if (line == null)
                return ExitCode.EndOfStream;

            ExitCode error = Run(line);
            if (error != ExitCode.Ok)
                return error;
        }
    }

    private static ExitCode RunFile(string path)
    {
        try
        {
            string source = File.ReadAllText(path);
            return Run(source);
        }
        catch (Exception ex) when (ex is FileNotFoundException or DirectoryNotFoundException)
        {
            Console.WriteLine($"File {path} not found!");
            return ExitCode.FileNotFound;
        }
    }

    private static ExitCode Run(string source)
    {
        Console.WriteLine(source);
        return ExitCode.Ok;
    }

    private static void OnError(int line, string message)
    {
        PrintError(line, "", message);
    }

    private static void PrintError(int line, string where, string message)
    {
        Console.WriteLine($"[line {line}] Error{where}: {message}");
        hadError = true;
    }
}
