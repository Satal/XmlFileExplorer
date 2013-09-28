using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace XmlFileExplorer.Domain.Config
{
    [Serializable]
    [XmlRoot("FolderConfig")]
    public class FolderConfig
    {
        [XmlArray("Schemas")]
        [XmlArrayItem("Schema")]
        public List<string> Schemas { get; set; }

        public FolderConfig()
        {
            Schemas = new List<string>();
        }
    }
}
