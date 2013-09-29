using System;
using System.Collections.Generic;
using System.Configuration;
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
        }

        private void SetUpAspectToStringConverters()
        {
            // Specify the file size format
            colFilesize.AspectToStringConverter = delegate(object value)
                {
                    var size = (long) value;
                    var limits = new[] { 1024*1024*1024, 1024*1024, 1024};
                    var units = new[] {"GB", "MB", "KB"};

                    for (var i = 0; i < limits.Length; i++)
                    {
                        if (size >= limits[i])
                        {
                            return String.Format("{0:#,##0.##} {1}", ((double) size/limits[i]), units[i]);
                        }
                    }

                    return String.Format("{0} bytes", size);
                };
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
            var configFiles = directory.GetFiles("Folder.config", SearchOption.TopDirectoryOnly);

            CurrentFolderConfig = configFiles.Any() ? Serializer.Deserialize<FolderConfig>(File.ReadAllText(configFiles.First().FullName)) : null;

            LoadFiles(directory);
        }

        private void LoadFiles(DirectoryInfo directory)
        {
            olvFiles.Items.Clear();
            olvFiles.AddObjects(directory.GetFiles());

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
                            ImageKey = Resources.FolderResourceName
                        };

                    // Add a dummmy node to the node so that it is expandable
                    var dummyNode = new TreeNode("DummyNode");
                    aNode.Nodes.Add(dummyNode);

                    nodeToAddTo.Nodes.Add(aNode);
                }
                catch (UnauthorizedAccessException uaex)
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

        private void tvNavigation_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            var selectedDir = (DirectoryInfo) e.Node.Tag;
            ChangeDirectory(selectedDir);
        }

        #endregion

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (tvNavigation.SelectedNode == null) return;

            var selectedDir = (DirectoryInfo) tvNavigation.SelectedNode.Tag;
            ConfigurationManager.AppSettings["LastViewedDirectory"] = selectedDir.FullName;
        }

        private void olvFiles_FormatRow(object sender, BrightIdeasSoftware.FormatRowEventArgs e)
        {
            var file = e.Model as FileInfo;
            if (file == null) return;

            if (file.Extension == ".xml" && CurrentFolderConfig != null && CurrentFolderConfig.Schemas.Any())
            {
                e.Item.BackColor = IsXmlSchemaCompliant(file) ? _validColor : _invalidColor;
            }

        }
    }
}