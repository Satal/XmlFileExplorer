using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Schema;

namespace XmlFileExplorer
{
    public class XsdValidator
    {
        public List<XmlSchema> Schemas { get; set; }
        public List<String> Errors { get; set; }
        public List<String> Warnings { get; set; }

        public XsdValidator()
        {
            Schemas = new List<XmlSchema>();
        }

        public bool AddSchema(string schemaFileLocation)
        {
            // Reset the Error/Warning collections
            Errors = new List<string>();
            Warnings = new List<string>();

            XmlSchema schema;

            using (var fs = new FileStream(schemaFileLocation, FileMode.Open))
            {
                schema = XmlSchema.Read(fs, ValidationEventHandler);
            }

            var isValid = !Errors.Any() && !Warnings.Any();

            if (isValid)
            {
                Schemas.Add(schema);
            }

            return isValid;
        }

        public bool IsValid(string xmlLocation)
        {
            using (var xmlStream = new FileStream(xmlLocation, FileMode.Open))
            {
                return IsValid(xmlStream);
            }
        }

        private bool IsValid(Stream xmlStream)
        {
            // Reset the Error/Warning collections
            Errors = new List<string>();
            Warnings = new List<string>();

            var settings = new XmlReaderSettings()
                {
                    ValidationType = ValidationType.Schema
                };
            settings.ValidationEventHandler += ValidationEventHandler;

            foreach (var xmlSchema in Schemas)
            {
                settings.Schemas.Add(xmlSchema);
            }

            var xmlFile = XmlReader.Create(xmlStream, settings);

            while (xmlFile.Read())
            {
            }

            return !Errors.Any() && !Warnings.Any();
        }

        private void ValidationEventHandler(object sender, ValidationEventArgs e)
        {
            switch (e.Severity)
            {
                case XmlSeverityType.Error:
                    Errors.Add(e.Message);
                    break;
                case XmlSeverityType.Warning:
                    Warnings.Add(e.Message);
                    break;
            }
        }
    }
}