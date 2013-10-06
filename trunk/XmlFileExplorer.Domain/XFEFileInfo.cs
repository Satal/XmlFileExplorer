using System.Collections.Generic;
using System.IO;
using XmlFileExplorer.Domain.Validation;

namespace XmlFileExplorer.Domain
{
    public class XfeFileInfo
    {
        public FileInfo FileInfo { get; set; }
        public List<ValidationError> ValidationErrors { get; set; }

        public XfeFileInfo(string filename)
        {
            FileInfo = new FileInfo(filename);
            ValidationErrors = new List<ValidationError>();
        }

        public XfeFileInfo(FileInfo fileInfo)
        {
            FileInfo = fileInfo;
            ValidationErrors = new List<ValidationError>();
        }
    }
}
