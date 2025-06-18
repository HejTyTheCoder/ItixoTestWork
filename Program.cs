using Microsoft.Extensions.Configuration;
using ItixoTestWork;

namespace ItixoTestWork;

class Program
{
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
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Running in debug mode");
        }

        _timer = new System.Timers.Timer(TimeSpan.FromSeconds(10).TotalMilliseconds); // Testing time
        _timer.Elapsed += async (s, e) => await WeatherUpdate();
        _timer.AutoReset = true;

        _dbManager = new DatabaseManager(config["DatabasePath"]!);
        _loader = new XmlDataLoader(config["WeatherStationURL"]!);

        await WeatherUpdate();

        _timer.Start();

        while (true)
        {
            // Place for keyboard shortcuts (later)
        }
    }

    private static async Task WeatherUpdate()
    {
        Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Starting weather data update...");

        try
        {
            string? xml = await _loader!.Load();

            if (xml != null)
            {
                string json = XmlToJsonConverter.Convert(xml);

                _dbManager!.SaveJson(json);

                Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Data update completed.");
            }
            else
            {
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] No XML returned.");
                _dbManager!.SaveUnavailableMessage();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] ERROR: {ex.Message}");
        }

        Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Next update in 1 hour.");
    }
}
