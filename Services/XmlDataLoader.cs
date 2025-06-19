using System.Threading.Tasks;
using System.Net.Http;

namespace ItixoTestWork.Services;

/// <summary>
/// Handles HTTP request to load weather data in XML format.
/// </summary>
public class XmlDataLoader
{
    private readonly HttpClient _client;
    private readonly string _url;
    
    /// <summary>
    /// Initializes the loader with a target URL.
    /// </summary>
    /// <param name="url">The URL to load the XML data from.</param>
    public XmlDataLoader(string url)
    {
        _client = new HttpClient();
        _url = url;
    }
    
    /// <summary>
    /// Runs an async HTTP GET request to load the XML data.
    /// </summary>
    /// <returns>Raw XML as string or null if request fails.</returns>
    public async Task<string?> Load()
    {
        HttpResponseMessage response = await _client.GetAsync(_url);

        if (response.IsSuccessStatusCode)
        {
            string content = await response.Content.ReadAsStringAsync();
            return content;
        }
        else
        {
            return null;
        }
    }
}