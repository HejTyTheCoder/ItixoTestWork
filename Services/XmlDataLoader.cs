using System.Threading.Tasks;
using System.Net.Http;

namespace ItixoTestWork;

public class XmlDataLoader
{
    private readonly HttpClient _client;
    private readonly string _url;

    public XmlDataLoader(string url)
    {
        _client = new HttpClient();
        _url = url;
    }

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