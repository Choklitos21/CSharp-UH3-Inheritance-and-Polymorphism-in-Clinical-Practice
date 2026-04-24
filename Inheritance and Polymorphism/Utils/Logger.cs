namespace Inheritance_and_Polymorphism.Utils;

// Simple logger that writes to both the console and a local text file.
//
// PRODUCTION SCALING NOTE:
// In a real system you would replace this with a structured logging library
// such as Serilog or Microsoft.Extensions.Logging. Those give you:
//   - Log levels (Verbose, Debug, Information, Warning, Error, Fatal)
//   - Sinks (file, database, Seq, Application Insights, etc.)
//   - Structured/JSON output for log aggregation tools (Datadog, Loki, etc.)
//   - Async file writes to avoid blocking the main thread
// This class intentionally mirrors the same API shape (Info / Warning / Error)
// so that swapping it out for a real library later requires minimal changes.
public static class Logger
{
    private static readonly string LogFilePath = "vet_system.log";

    // ISO-8601 timestamp prefix makes logs sortable and machine-readable.
    private static string Timestamp => DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

    public static void Info(string message)
    {
        WriteLog("INFO", message, ConsoleColor.Cyan);
    }

    public static void Warning(string message)
    {
        WriteLog("WARN", message, ConsoleColor.Yellow);
    }

    public static void Error(string message, Exception? ex = null)
    {
        string full = ex != null ? $"{message} | Exception: {ex.Message}" : message;
        WriteLog("ERROR", full, ConsoleColor.Red);
    }

    private static void WriteLog(string level, string message, ConsoleColor color)
    {
        string line = $"[{Timestamp}] [{level}] {message}";

        // Console output with color coding for quick visual scanning during development.
        Console.ForegroundColor = color;
        Console.WriteLine(line);
        Console.ResetColor();

        // Append to file — each run accumulates entries; rotate in production.
        try
        {
            File.AppendAllText(LogFilePath, line + Environment.NewLine);
        }
        catch
        {
            // Never let logging failures crash the application — swallow silently here only.
        }
    }
}