

class Lox
{
    static bool hadError {get; set;} = false;

    static void Main(string[] args)
    {
        if (args.Length > 1)
        {
            Console.WriteLine("Usage: jlox [script]");
            Environment.Exit(64);
        }
        else if (args.Length == 1)
        {
            runFile(args[0]);
        }
        else
        {
            runPrompt();
        }
    }

    static void runPrompt()
    {
        for(;;)
        {
            Console.WriteLine("> ");
            
            string? line = Console.ReadLine();
            if (string.IsNullOrEmpty(line))
                break;

            run(line);
            hadError = false;
        }
    }

    static void runFile(string path)
    {
        string source = File.ReadAllText(path);
        run(source);

        if (hadError)
            Environment.Exit(65);
    }

    static void run(string source)
    {
        Scanner scanner = new (source);
        IEnumerable<Token> tokens = scanner.scanTokens();

        foreach(Token token in tokens)
        {
            Console.WriteLine(token);
        }
    }

    internal static void Error(int line, string message)
    {
        report(line, "", message);
    }

    static void report(int line, string where, string message)
    {
        Console.WriteLine($"[line {line}] Error {where} {message}");
    }
}