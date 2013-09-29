using System.Xml.Linq;

namespace XmlFileExplorer.Domain
{
    public static class XmlFormatter
    {
        public static string FormatXml(string xml)
        {
            var doc = XDocument.Parse(xml);
            return doc.ToString();
        }
    }
}
