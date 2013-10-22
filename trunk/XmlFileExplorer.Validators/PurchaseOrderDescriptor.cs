using KetoLibrary.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Globalization;
using System.IO;
using System.Linq;
using XmlFileExplorer.Domain;
using XmlFileExplorer.Domain.Describer;

namespace XmlFileExplorer.Validators
{
    [Export(typeof(IDescriptor))]
    public class PurchaseOrderDescriptor : IDescriptor
    {
        public Dictionary<string, string> GetAttributes(string fileLocation)
        {
            if (!File.Exists(fileLocation))
            {
                throw new ArgumentException(@"The specified file does not exist", "fileLocation");
            }

            var rtn = new Dictionary<string, string>();
            var po = Serializer.Deserialize<PurchaseOrderType>(File.ReadAllText(fileLocation));

            if (po != null)
            {
                rtn.Add("Bill to", ValueRetriever.RetrieveValue(po, poType => poType.billTo.name, "[Unspecified]"));
                rtn.Add("Ship to", ValueRetriever.RetrieveValue(po, poType => poType.shipTo.name, "[Unspecified]"));
                rtn.Add("Items", ValueRetriever.RetrieveValue(po, poType => poType.items.Count().ToString(CultureInfo.InvariantCulture), "0"));
            }

            return rtn;
        }
    }
}
