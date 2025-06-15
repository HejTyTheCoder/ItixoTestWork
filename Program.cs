using Microsoft.Extension.Configuration;

var config = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

string url = config["WeatherStationURL"];

Console.WriteLine($"URL: {url}");
