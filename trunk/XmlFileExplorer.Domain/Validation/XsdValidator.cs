﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Schema;

namespace XmlFileExplorer.Domain.Validation
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

        /// <summary>
        /// Add a schema to be used during the validation of the XML document
        /// </summary>
        /// <param name="schemaFileLocation">The file path for the XSD schema file to be added for validation</param>
        /// <returns>True if the schema file was successfully loaded, else false (if false, view Errors/Warnings for reason why)</returns>
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

        /// <summary>
        /// Perform the XSD validation against the specified XML document
        /// </summary>
        /// <param name="xmlLocation">The full file path of the file to be validated</param>
        /// <returns>True if the XML file conforms to the schemas, else false</returns>
        public bool IsValid(string xmlLocation)
        {
            if (!File.Exists(xmlLocation))
            {
                throw new FileNotFoundException("The specified XML file does not exist", xmlLocation);
            }

            using (var xmlStream = new FileStream(xmlLocation, FileMode.Open))
            {
                return IsValid(xmlStream);
            }
        }

        /// <summary>
        /// Perform the XSD validation against the supplied XML stream
        /// </summary>
        /// <param name="xmlStream">The XML stream to be validated</param>
        /// <returns>True is the XML stream conforms to the schemas, else false</returns>
        private bool IsValid(Stream xmlStream)
        {
            // Reset the Error/Warning collections
            Errors = new List<string>();
            Warnings = new List<string>();

            var settings = new XmlReaderSettings
            {
                ValidationType = ValidationType.Schema
            };
            settings.ValidationEventHandler += ValidationEventHandler;

            foreach (var xmlSchema in Schemas)
            {
                settings.Schemas.Add(xmlSchema);
            }

            var xmlFile = XmlReader.Create(xmlStream, settings);

            while (xmlFile.Read()) { }

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