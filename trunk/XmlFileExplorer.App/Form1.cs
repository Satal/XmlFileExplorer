using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace XmlFileExplorer.App
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            PopulateTreeView();
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
