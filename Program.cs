using Microsoft.Extensions.Configuration;

class Program
{
    static async Task Main()
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        string url = config["WeatherStationURL"];

        var environment = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");

        if (environment == "Development")
        {
            Console.WriteLine("Debug mode");
        }
        else
        {
            Console.WriteLine("Production mode");
        }

        Console.WriteLine($"URL: {url}");

        XmlDataLoader loader = new XmlDataLoader(url);

        string xml = await loader.Load();

        Console.WriteLine(xml);
    }
}
