﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Xml.Schema;

namespace XmlFileExplorer.Tests
{
    [TestClass]
    public class XsdValidatorTests
    {
        [TestMethod]
        public void AddValidSchema()
        {
            var validator = new XmlFileExplorer.XsdValidator();
            var actual = validator.AddSchema(@"C:\Users\satal_000\Documents\GitHub\XmlFileExplorer\trunk\Test Files\PurchaseOrder\PurchaseOrder.xsd");
            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void IsValidReturnsTrueForValidXml()
        {
            var validator = new XsdValidator();
            validator.AddSchema(@"C:\Users\satal_000\Documents\GitHub\XmlFileExplorer\trunk\Test Files\PurchaseOrder\PurchaseOrder.xsd");
            var isValid = validator.IsValid(@"C:\Users\satal_000\Documents\GitHub\XmlFileExplorer\trunk\Test Files\PurchaseOrder\Valid PurchaseOrder1.xml");
            Assert.IsTrue(isValid);
        }

        [TestMethod]
        public void IsValidReturnsFalseForInvalidXml()
        {
            var validator = new XsdValidator();
            validator.AddSchema(@"C:\Users\satal_000\Documents\GitHub\XmlFileExplorer\trunk\Test Files\PurchaseOrder\PurchaseOrder.xsd");
            var isValid = validator.IsValid(@"C:\Users\satal_000\Documents\GitHub\XmlFileExplorer\trunk\Test Files\PurchaseOrder\Invalid PurchaseOrder1.xml");
            Assert.IsFalse(isValid);
        }

        [TestMethod]
        public void MultipleSchemas()
        {
            var validator = new XsdValidator();
            validator.AddSchema(@"C:\Users\satal_000\Documents\GitHub\XmlFileExplorer\trunk\Test Files\Products\SchemaDoc1.xsd");
            validator.AddSchema(@"C:\Users\satal_000\Documents\GitHub\XmlFileExplorer\trunk\Test Files\Products\SchemaDoc2.xsd");
            var isValid = validator.IsValid(@"C:\Users\satal_000\Documents\GitHub\XmlFileExplorer\trunk\Test Files\Products\ValidXmlDoc1.xml");
            Assert.IsTrue(isValid);
        }
    }
}
