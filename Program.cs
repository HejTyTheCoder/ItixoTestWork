using Microsoft.Extensions.Configuration;
using ItixoTestWork;

namespace ItixoTestWork;

class Program
{
    private static Logger _logger = new Logger("logs");
    private static System.Timers.Timer? _timer;
    private static DatabaseManager? _dbManager;
    private static XmlDataLoader? _loader;

    static async Task Main()
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        string? environment = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");

        if (environment == "Development")
        {
            _logger.Log("Running in debug mode...");
        }

        _timer = new System.Timers.Timer(TimeSpan.FromHours(1).TotalMilliseconds);
        _timer.Elapsed += async (s, e) => await WeatherUpdate();
        _timer.AutoReset = true;

        _dbManager = new DatabaseManager(config["DatabasePath"]!);
        _loader = new XmlDataLoader(config["WeatherStationURL"]!);

        await WeatherUpdate();

        _timer.Start();

        while (true)
        {
            var key = Console.ReadKey(true).Key;

            switch (key)
            {
                case ConsoleKey.Q:
                    _logger.Log("Manual shutdown requested.");
                    _timer.Stop();

                    await WeatherUpdate(isFinal: true);
                    _logger.Log("Shutting down...");
                    await Task.Delay(3000);
                    Environment.Exit(0);
                    break;

                case ConsoleKey.L:
                    _logger.Log("Showing last weather record in database...");
                    Console.WriteLine(_dbManager.GetLastJson());
                    break;

                case ConsoleKey.H:
                    Console.WriteLine("\nAvailable commands:");
                    Console.WriteLine("  [Q] - Quit and save one last weather record");
                    Console.WriteLine("  [L] - Show latest weather record (JSON)");
                    Console.WriteLine("  [H] - Show this help menu\n");
                    break;
            }
        }
    }

    private static async Task WeatherUpdate(bool isFinal = false)
    {
        _logger.Log("Starting weather data update...");

        try
        {
            string? xml = await _loader!.Load();

            if (xml != null)
            {
                string json = XmlToJsonConverter.Convert(xml);

                _dbManager!.SaveJson(json);

                _logger.Log("Data update completed.");
            }
            else
            {
                _logger.Log("No XML returned.");
                _dbManager!.SaveUnavailableMessage();
            }
        }
        catch (Exception ex)
        {
            _logger.Log($"ERROR: {ex.Message}");
        }

        if (isFinal)
        {
            _logger.Log("Final record saved before shutdown.");
        }
        else
        {
            _logger.Log("Next update in 1 hour.");
        }
    }
}
