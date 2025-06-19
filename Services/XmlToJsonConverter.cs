using Newtonsoft.Json;
using System.Xml;

namespace ItixoTestWork.Services;

/// <summary>
/// Functionality to convert XML data into JSON format.
/// </summary>
public static class XmlToJsonConverter
{
    /// <summary>
    /// Converts an XML string into a formatted JSON string.
    /// </summary>
    /// <param name="xml">The XML string to convert.</param>
    /// <returns>Formatted JSON string.</returns>
    public static string Convert(string xml)
    {
        XmlDocument doc = new XmlDocument();

        doc.LoadXml(xml);

        string json = JsonConvert.SerializeXmlNode(doc, Newtonsoft.Json.Formatting.Indented);

        return json;
    }
}
