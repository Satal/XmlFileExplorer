using System;
using System.Collections.Generic;
using XmlFileExplorer.Domain;

namespace XmlFileExplorer.Event
{
    public class FileSelectionChangedEventArgs : EventArgs
    {
        public List<XfeFileInfo> Files { get; set; }
    }
}
