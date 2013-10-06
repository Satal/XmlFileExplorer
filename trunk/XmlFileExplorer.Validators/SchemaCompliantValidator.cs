using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using XmlFileExplorer.Domain;
using XmlFileExplorer.Domain.Config;
using XmlFileExplorer.Domain.Validation;

namespace XmlFileExplorer.Validators
{
    [Export(typeof(IValidator))]
    public class SchemaCompliantValidator : IValidator
    {

        public IEnumerable<ValidationError> ValidateFile(string filePath)
        {
            var rtn = new List<ValidationError>();

            var file = new FileInfo(filePath);

            // Check that the file exists first, there's no point in continuing if it doesn't exist
            if (!file.Exists)
            {
                rtn.Add(new ValidationError
                    {
                        Description = "The file does not exist",
                        ErrorSeverity = ErrorSeverity.Error
                    });

                return rtn;
            }

            IEnumerable<FileInfo> configFiles = new FileInfo[0];

            try
            {
                if (file.Directory != null)
                    configFiles = file.Directory.GetFiles("Folder.config", SearchOption.TopDirectoryOnly);
            }
            catch (UnauthorizedAccessException)
            {
                // We may not have been allowed to read the directory, in which
                // case we will get an UnauthorizedAccessException thrown.
            }
            catch (IOException)
            {
                // We may not have been allowed to read the directory, in which
                // case we will get an UnauthorizedAccessException thrown.
            }

            var currentFolderConfig = configFiles.Any() ? Serializer.Deserialize<FolderConfig>(File.ReadAllText(configFiles.First().FullName)) : null;

            if (currentFolderConfig == null || !currentFolderConfig.Schemas.Any()) return rtn;

            var validator = new XsdValidator();
            foreach (var schema in currentFolderConfig.Schemas)
            {
                if (Path.IsPathRooted(schema))
                {
                    validator.AddSchema(schema);
                }
                else
                {
                    if (file.Directory != null)
                    {
                        validator.AddSchema(Path.Combine(file.Directory.FullName, schema));
                    }
                }
            }

            validator.IsValid(filePath);

            rtn.AddRange(validator.Errors.Select(e => new ValidationError { Description = e, ErrorSeverity = ErrorSeverity.Error }));
            rtn.AddRange(validator.Warnings.Select(e => new ValidationError { Description = e, ErrorSeverity = ErrorSeverity.Warning }));

            return rtn;
        }
    }
}
