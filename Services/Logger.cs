namespace ItixoTestWork.Services;

/// <summary>
/// Handles writing logs both to console and to daily log files.
/// </summary>
public class Logger
{
    private readonly string _filePath;

    /// <summary>
    /// Initializes the logger and ensures log directory exists.
    /// </summary>
    /// <param name="logDir">Target directory for log files.</param>
    public Logger(string logDir)
    {
        if (!Directory.Exists(logDir))
        {
            Directory.CreateDirectory(logDir);
        }

        _filePath = Path.Combine(logDir, $"{DateTime.Today:yyyy-MM-dd}.log");
    }

    /// <summary>
    /// Logs message with current timestamp to console and log file.
    /// </summary>
    /// <param name="message">Message to be logged.</param>
    public void Log(string message)
    {
        string line = $"[{DateTime.Now:HH:mm:ss}] {message}";

        Console.WriteLine(line);

        try
        {
            File.AppendAllText(_filePath, line + Environment.NewLine);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Failed to write to log: {ex.Message}");
        }
    }
}
