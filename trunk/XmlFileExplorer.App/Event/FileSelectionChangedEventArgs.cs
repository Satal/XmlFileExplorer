using System.Collections.Generic;
using XmlFileExplorer.Domain;

namespace XmlFileExplorer.Event
{
    public class FileSelectionChangedEventArgs
    {
        public List<XfeFileInfo> Files { get; set; }
    }
}
