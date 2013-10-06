using System.Collections.Generic;

namespace XmlFileExplorer.Domain.Validation
{
    public interface IValidator
    {
        IEnumerable<ValidationError> ValidateFile(string filePath);
    }
}
