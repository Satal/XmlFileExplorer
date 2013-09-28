using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XmlFileExplorer.Domain;
using XmlFileExplorer.Domain.Config;

namespace XmlFileExplorer.Tests
{
    [TestClass]
    public class SerializerTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            var folderConfig = new FolderConfig();
            folderConfig.Schemas.Add("Schema1.xsd");
            var serialised = Serializer.Serialize<FolderConfig>(folderConfig);
            Console.WriteLine(serialised);
        }
    }
}
