using System;
using System.IO;

namespace XmlFileExplorer.Event
{
    public class FolderLocationChangedEventArgs : EventArgs
    {
        public DirectoryInfo NewLocation { get; set; }

        public FolderLocationChangedEventArgs(string newLocation)
        {
            NewLocation = new DirectoryInfo(newLocation);
        }

        public FolderLocationChangedEventArgs(DirectoryInfo newLocation)
        {
            NewLocation = newLocation;
        }
    }
}
