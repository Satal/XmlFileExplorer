using System.Collections.Generic;
using System.Linq;
using WeifenLuo.WinFormsUI.Docking;

namespace XmlFileExplorer.Forms
{
    public partial class DescriptionForm : DockContent
    {
        public DescriptionForm()
        {
            InitializeComponent();
        }

        public void SetDictionary(Dictionary<string, string> entries)
        {
            dataGridView1.Rows.Clear();
            foreach (var entry in entries.OrderBy(e => e.Key))
            {
                dataGridView1.Rows.Add(entry.Key, entry.Value);
            }
        }
    }
}
