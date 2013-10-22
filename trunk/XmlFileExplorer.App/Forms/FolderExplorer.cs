using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using XmlFileExplorer.Event;
using XmlFileExplorer.Properties;

namespace XmlFileExplorer.Forms
{
    public delegate void FolderLocationChangedHandler(object sender, FolderLocationChangedEventArgs e);

    public partial class FolderExplorer : DockContent
    {
        private bool _formFinishedLoading;
        private bool _openingDirectory;

        public FolderExplorer()
        {
            InitializeComponent();
            PopulateTreeView();

            string startDirectory = Settings.Default.LastViewedDirectory ??
                                    Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            OpenDirectory(startDirectory);
        }

        public DirectoryInfo CurrentDirectory { get; private set; }
        public event FolderLocationChangedHandler FolderLocationChanged;

        public void OpenDirectory(string path)
        {
            _openingDirectory = true;

            // Make sure that the path is not null or empty
            if (String.IsNullOrEmpty(path)) return;
            var dir = new DirectoryInfo(path);
            var dirList = new List<DirectoryInfo>();
            if (!dir.Exists) return;

            CurrentDirectory = dir;
            DirectoryInfo curDir = dir;
            dirList.Add(curDir);
            while (curDir != null)
            {
                dirList.Add(curDir);
                curDir = curDir.Parent;
            }

            dirList.Reverse();

            List<TreeNode> nodeCollection = tvNavigation.Nodes.Cast<TreeNode>().ToList();
            TreeNode node = null;

            tvNavigation.BeginUpdate();

            foreach (DirectoryInfo directoryInfo in dirList)
            {
                DirectoryInfo info = directoryInfo;
                foreach (TreeNode treeNode in nodeCollection.Where(n => n.Text == info.Name))
                {
                    node = treeNode;
                    PopulateNode(node);
                    node.Expand();
                    nodeCollection = node.Nodes.Cast<TreeNode>().ToList();
                    break;
                }
            }

            if (node != null)
            {
                tvNavigation.SelectedNode = node;
            }

            tvNavigation.EndUpdate();

            _openingDirectory = false;
        }

        private void PopulateTreeView()
        {
            foreach (DirectoryInfo drive in Directory.GetLogicalDrives().Select(d => new DirectoryInfo(d)))
            {
                var rootNode = new TreeNode(drive.Name) {Tag = drive};
                tvNavigation.Nodes.Add(rootNode);
            }
        }

        private static void PopulateNode(TreeNode node)
        {
            var dir = node.Tag as DirectoryInfo;

            if (dir == null) return;

            GetDirectories(dir.GetDirectories(), node);
        }

        private static void GetDirectories(IEnumerable<DirectoryInfo> subDirs, TreeNode nodeToAddTo)
        {
            bool displayHiddenDirectories =
                Convert.ToBoolean(ConfigurationManager.AppSettings["DisplayHiddenDirectories"]);

            nodeToAddTo.Nodes.Clear();

            foreach (
                DirectoryInfo subDir in
                    subDirs.Where(
                        subDir => !subDir.Attributes.HasFlag(FileAttributes.Hidden) || displayHiddenDirectories))
            {
                try
                {
                    var aNode = new TreeNode(subDir.Name, 0, 0)
                        {
                            Tag = subDir,
                            ImageKey = Resources.FolderResourceName,
                        };

                    // Add a dummmy node to the node so that it is expandable
                    var dummyNode = new TreeNode("DummyNode");
                    aNode.Nodes.Add(dummyNode);

                    nodeToAddTo.Nodes.Add(aNode);
                }
                catch (UnauthorizedAccessException)
                {
                    // If we don't have permission to access this directory then don't display it.
                    // Eventually this should show the directory but with an icon that lets the user
                    // know that they wont be able to view it.
                }
            }
        }

        private void tvNavigation_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            if (_formFinishedLoading && !_openingDirectory)
            {
                TreeNode treeNode = e.Node;
                var nodeDir = (DirectoryInfo) treeNode.Tag;

                GetDirectories(nodeDir.GetDirectories(), treeNode);
            }
        }

        private void tvNavigation_AfterSelect(object sender, TreeViewEventArgs e)
        {
            var selectedDir = (DirectoryInfo) e.Node.Tag;

            if (FolderLocationChanged != null)
            {
                FolderLocationChanged(this,
                                      new FolderLocationChangedEventArgs(selectedDir.FullName));
            }
        }

        private void FolderExplorer_Shown(object sender, EventArgs e)
        {
            _formFinishedLoading = true;
        }
    }
}