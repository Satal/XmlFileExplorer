using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Schema;

namespace XmlFileExplorer
{
    public partial class Form1 : Form
    {
        private readonly Color _validColor = Color.LightGreen;
        private readonly Color _invalidColor = Color.LightPink;
        private List<String> _schemaFiles;

        public Form1()
        {
            InitializeComponent();
            //PopulateTreeView();

            var lvi1 = new ListViewItem("File1");
            var lvi2 = new ListViewItem("File2");
            lvi1.BackColor = Color.Green;
            lvi2.BackColor = Color.Red;

            listView1.Items.Add(lvi1);
            listView1.Items.Add(lvi2);
        }

        private void PopulateTreeView()
        {
            var info = new DirectoryInfo("C:\\");

            if (info.Exists)
            {
                var rootNode = new TreeNode(info.Name) {Tag = info};
                GetDirectories(info.GetDirectories(), rootNode);
                tvNavigation.Nodes.Add(rootNode);
            }
        }

        private void ChangeDirectory(DirectoryInfo directory)
        {
            LoadSchemas(directory);
            LoadFiles(directory);
        }

        private void LoadSchemas(DirectoryInfo directory)
        {
            _schemaFiles = new List<string>();

        }

        private void LoadFiles(DirectoryInfo directory)
        {
            foreach (var file in directory.GetFiles())
            {
                var lvi = new ListViewItem(file.Name);

                if (file.Extension == ".xml")
                {
                    lvi.BackColor = IsXmlSchemaCompliant(file) ? _validColor : _invalidColor;
                }
            }
        }

        private bool IsXmlSchemaCompliant(FileInfo file)
        {
            return true;
        }

        private static void GetDirectories(IEnumerable<DirectoryInfo> subDirs, TreeNode nodeToAddTo)
        {
            foreach (var subDir in subDirs)
            {
                var aNode = new TreeNode(subDir.Name, 0, 0)
                    {
                        Tag = subDir,
                        ImageKey = "folder"
                    };

                var subSubDirs = subDir.GetDirectories();

                if (subSubDirs.Any())
                {
                    GetDirectories(subSubDirs, aNode);
                }

                nodeToAddTo.Nodes.Add(aNode);
            }
        }
    }
}
