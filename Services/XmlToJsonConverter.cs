using Newtonsoft.Json;
using System.Xml;

namespace ItixoTestWork.Services;

public static class XmlToJsonConverter
{
    public static string Convert(string xml)
    {
        XmlDocument doc = new XmlDocument();

        doc.LoadXml(xml);

        string json = JsonConvert.SerializeXmlNode(doc, Newtonsoft.Json.Formatting.Indented);

        return json;
    }
}
