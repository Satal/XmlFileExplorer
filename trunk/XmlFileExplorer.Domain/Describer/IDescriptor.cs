using System.Collections.Generic;

namespace XmlFileExplorer.Domain.Describer
{
    public interface IDescriptor
    {
        Dictionary<string, string> GetAttributes(string fileLocation);
    }
}
