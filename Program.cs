using Microsoft.Extensions.Configuration;
using ItixoTestWork;

namespace ItixoTestWork;

class Program
{
    static async Task Main()
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        string? environment = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");

        if (environment == "Development")
        {
            Console.WriteLine("Debug mode");
        }
        else
        {
            Console.WriteLine("Production mode");
        }

        var dbManager = new DatabaseManager(config["DatabasePath"]!);
        var loader = new XmlDataLoader(config["WeatherStationURL"]!);
        string? xml = await loader.Load();

        if (xml != null)
        {
            string json = XmlToJsonConverter.Convert(xml);
    
            dbManager.SaveJson(json);
        }
        else
        {
            Console.WriteLine("No Xml returned.");
            dbManager.SaveUnavailableMessage();
        }
    }
}
