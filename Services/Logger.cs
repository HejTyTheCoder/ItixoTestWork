namespace ItixoTestWork.Services;

public class Logger
{
    private readonly string _filePath;

    public Logger(string logDir)
    {
        if (!Directory.Exists(logDir))
        {
            Directory.CreateDirectory(logDir);
        }

        _filePath = Path.Combine(logDir, $"{DateTime.Today:yyyy-MM-dd}.log");
    }

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
