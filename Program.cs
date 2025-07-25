﻿using Microsoft.Extensions.Configuration;
using ItixoTestWork.Services;
using ItixoTestWork.Data;

namespace ItixoTestWork;

/// <summary>
/// Main application entry point. Handles configuration loading,
/// timer-based updates, and keyboard shortcut interactions.
/// </summary>
class Program
{
    private static Logger _logger = new Logger("logs");
    private static System.Timers.Timer? _timer;
    private static DatabaseManager? _dbManager;
    private static XmlDataLoader? _loader;

    /// <summary>
    /// Starts the application and enters the input listening loop.
    /// </summary>
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
                    await Task.Delay(3000); // Additional time to read the final logs
                    Environment.Exit(0);
                    break;

                case ConsoleKey.L:
                    _logger.Log("Showing last weather record in database...");
                    Console.WriteLine(_dbManager.GetLastJson());
                    break;

                case ConsoleKey.H:
                    ShowHelp();
                    break;
            }
        }
    }

    /// <summary>
    /// Downloads, converts, and saves the weather data.
    /// </summary>
    /// <param name="isFinal">Set to true to log final update message.</param>
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

    /// <summary>
    /// Prints help menu to the console.
    /// </summary>
    private static void ShowHelp()
    {
        Console.WriteLine();
        Console.WriteLine("Available commands:");
        Console.WriteLine("  [Q] - Quit and save one last weather record");
        Console.WriteLine("  [L] - Show latest weather record (JSON)");
        Console.WriteLine("  [H] - Show this help menu");
        Console.WriteLine();
    }
}
