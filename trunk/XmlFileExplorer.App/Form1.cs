using BrightIdeasSoftware;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using XmlFileExplorer.Domain;
using XmlFileExplorer.Domain.Config;
using XmlFileExplorer.Domain.Validation;
using XmlFileExplorer.Properties;

namespace XmlFileExplorer
{
    public partial class Form1 : Form
    {
        private readonly Color _validColor = Color.LightGreen;
        private readonly Color _invalidColor = Color.LightPink;
        private FolderConfig CurrentFolderConfig { get; set; }

        public Form1()
        {
            InitializeComponent();
            SetUpAspectToStringConverters();
            PopulateTreeView();
            OpenDirectory(@"C:\Users\satal_000\Documents\GitHub\XmlFileExplorer\trunk\Test Files\PurchaseOrder");
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
            IEnumerable<FileInfo> configFiles = new FileInfo[0];

            try
            {
                configFiles = directory.GetFiles("Folder.config", SearchOption.TopDirectoryOnly);
            }
            catch (UnauthorizedAccessException)
            {
                // We may not have been allowed to read the directory, in which
                // case we will get an UnauthorizedAccessException thrown.
            }

            CurrentFolderConfig = configFiles.Any() ? Serializer.Deserialize<FolderConfig>(File.ReadAllText(configFiles.First().FullName)) : null;

            LoadFiles(directory);
        }

        private void LoadFiles(DirectoryInfo directory)
        {
            olvFiles.Items.Clear();
            try
            {
                olvFiles.AddObjects(directory.GetFiles());
            }
            catch (UnauthorizedAccessException)
            {
                // We may not have been allowed to read the directory, in which
                // case we will get an UnauthorizedAccessException thrown.
            }

            foreach (ColumnHeader col in olvFiles.Columns)
            {
                col.Width = -2;
            }
        }

        private bool IsXmlSchemaCompliant(FileInfo file)
        {
            // If we are not 
            if (CurrentFolderConfig == null || !CurrentFolderConfig.Schemas.Any()) return true;

            var validator = new XsdValidator();
            foreach (var schema in CurrentFolderConfig.Schemas)
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

            return validator.IsValid(file.FullName);
        }

        #region Directory TreeView

        private static void GetDirectories(IEnumerable<DirectoryInfo> subDirs, TreeNode nodeToAddTo)
        {
            var displayHiddenDirectories = Convert.ToBoolean(ConfigurationManager.AppSettings["DisplayHiddenDirectories"]);

            nodeToAddTo.Nodes.Clear();

            foreach (var subDir in subDirs.Where(subDir => !subDir.Attributes.HasFlag(FileAttributes.Hidden) || displayHiddenDirectories))
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
            var treeNode = e.Node;
            var nodeDir = (DirectoryInfo) treeNode.Tag;

            GetDirectories(nodeDir.GetDirectories(), treeNode);
        }

        private void tvNavigation_AfterSelect(object sender, TreeViewEventArgs e)
        {
            var selectedDir = (DirectoryInfo)e.Node.Tag;
            ChangeDirectory(selectedDir);
        }

        #endregion

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (tvNavigation.SelectedNode == null) return;

            var selectedDir = (DirectoryInfo) tvNavigation.SelectedNode.Tag;
            ConfigurationManager.AppSettings["LastViewedDirectory"] = selectedDir.FullName;
        }

        #region ObjectListView

        private void olvFiles_FormatRow(object sender, FormatRowEventArgs e)
        {
            var file = e.Model as FileInfo;
            if (file == null) return;

            if (file.Extension == ".xml" && CurrentFolderConfig != null && CurrentFolderConfig.Schemas.Any())
            {
                e.Item.BackColor = IsXmlSchemaCompliant(file) ? _validColor : _invalidColor;
            }

        }

        private void olvFiles_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            OpenFile();
        }

        private void SetUpAspectToStringConverters()
        {
            // Specify the file size format
            colFilesize.AspectToStringConverter = delegate(object value)
            {
                var size = (long)value;
                var limits = new[] { 1024 * 1024 * 1024, 1024 * 1024, 1024 };
                var units = new[] { "GB", "MB", "KB" };

                for (var i = 0; i < limits.Length; i++)
                {
                    if (size >= limits[i])
                    {
                        return String.Format("{0:#,##0.##} {1}", ((double)size / limits[i]), units[i]);
                    }
                }

                return String.Format("{0} bytes", size);
            };
        }

        #endregion

        // This method is called when we have specified we want to open a file
        private void OpenFile()
        {
            foreach (var selectedObject in olvFiles.SelectedObjects.Cast<FileInfo>())
            {
                Process.Start(selectedObject.FullName);
            }
        }

        private void olvFiles_ItemDrag(object sender, ItemDragEventArgs e)
        {
            var item = e.Item as OLVListItem;
            
            if (item == null) return;
            
            var fileInfo = item.RowObject as FileInfo;

            if (fileInfo != null)
            {
                DoDragDrop(fileInfo.FullName, DragDropEffects.Copy);
            }
        }

        private void OpenDirectory(string path)
        {
            var dir = new DirectoryInfo(path);
            var dirList = new List<DirectoryInfo>();
            if (!dir.Exists) return;

            var curDir = dir;
            dirList.Add(curDir);
            while (curDir != null)
            {
                dirList.Add(curDir);
                curDir = curDir.Parent;
            }

            dirList.Reverse();

            var nodeCollection = tvNavigation.Nodes.Cast<TreeNode>().ToList();
            TreeNode node = null;

            foreach (var directoryInfo in dirList)
            {
                node = nodeCollection.FirstOrDefault(n => n.Text == directoryInfo.Name);

                if (node == null) break;

                node.Expand();
                
                nodeCollection = node.Nodes.Cast<TreeNode>().ToList();
            }

            if (node != null)
            {
                tvNavigation.SelectedNode = node; 
            }
        }

        private void mnuItmAbout_Click(object sender, EventArgs e)
        {
            var frm = new About();
            frm.ShowDialog();
        }
    }
}