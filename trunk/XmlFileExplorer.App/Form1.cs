using BrightIdeasSoftware;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using XmlFileExplorer.Domain;
using XmlFileExplorer.Domain.Config;
using XmlFileExplorer.Domain.Validation;
using XmlFileExplorer.Properties;

namespace XmlFileExplorer
{
    public partial class Form1 : Form
    {
        private Color _validBackgroundColor;
        private Color _validForegroundColor;
        private Color _invalidBackgroundColor;
        private Color _invalidForegroundColor;
        private DirectoryInfo CurrentDirectory { get; set; }
        private FolderConfig CurrentFolderConfig { get; set; }
        private readonly Stack<DirectoryInfo> _history = new Stack<DirectoryInfo>(); 

        private bool _formFinishedLoading = false;
        private bool _openingDirectory = false;

        public Form1()
        {
            InitializeComponent();
            SetUpAspectToStringConverters();
            PopulateTreeView();
            LoadAppearanceConfiguration();

            OpenDirectory(Settings.Default.LastViewedDirectory);
        }

        private void LoadAppearanceConfiguration()
        {
            _validBackgroundColor = Settings.Default.ValidXmlBackgroundColor;
            _validForegroundColor = Settings.Default.ValidXmlForegroundColor;
            _invalidBackgroundColor = Settings.Default.InvalidXmlBackgroundColor;
            _invalidForegroundColor = Settings.Default.InvalidXmlForegroundColor;
        }

        private void PopulateTreeView()
        {
            foreach (var drive in Directory.GetLogicalDrives().Select(d => new DirectoryInfo(d)))
            {
                var rootNode = new TreeNode(drive.Name) { Tag = drive };
                tvNavigation.Nodes.Add(rootNode);
            }
        }

        private void ChangeDirectory(DirectoryInfo directory)
        {
            if (CurrentDirectory != null && _formFinishedLoading)
            {
                _history.Push(CurrentDirectory);
            }

            CurrentDirectory = directory;
            
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
            catch (IOException)
            {
                // We may not have been allowed to read the directory, in which
                // case we will get an UnauthorizedAccessException thrown.
            }

            CurrentFolderConfig = configFiles.Any() ? Serializer.Deserialize<FolderConfig>(File.ReadAllText(configFiles.First().FullName)) : null;

            LoadFiles(directory);
            txtDirectory.Text = directory.FullName;
            UpdateBackButtonImage();
        }

        private void LoadFiles(DirectoryInfo directory)
        {
            olvFiles.BeginUpdate();

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

            olvFiles.EndUpdate();
        }

        private void RefreshFolder()
        {
            LoadFiles(CurrentDirectory);
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

        private static void PopulateNode(TreeNode node)
        {
            var dir = node.Tag as DirectoryInfo;

            if (dir == null) return;

            GetDirectories(dir.GetDirectories(), node);
        }

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
            if (_formFinishedLoading && !_openingDirectory)
            {
                var treeNode = e.Node;
                var nodeDir = (DirectoryInfo)treeNode.Tag;

                GetDirectories(nodeDir.GetDirectories(), treeNode);
            }
        }

        private void tvNavigation_AfterSelect(object sender, TreeViewEventArgs e)
        {
            var selectedDir = (DirectoryInfo)e.Node.Tag;
            ChangeDirectory(selectedDir);
        }

        #endregion

        #region ObjectListView

        private void olvFiles_FormatRow(object sender, FormatRowEventArgs e)
        {
            var file = e.Model as FileInfo;
            if (file == null) return;

            if (file.Extension != ".xml" || CurrentFolderConfig == null || !CurrentFolderConfig.Schemas.Any()) return;

            var isSchemaCompliant = IsXmlSchemaCompliant(file);
            e.Item.BackColor = isSchemaCompliant ? _validBackgroundColor : _invalidBackgroundColor;
            e.Item.ForeColor = isSchemaCompliant ? _validForegroundColor : _invalidForegroundColor;
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

        #endregion

        #region Form events

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (tvNavigation.SelectedNode == null) return;

            var selectedDir = (DirectoryInfo)tvNavigation.SelectedNode.Tag;
            Settings.Default.LastViewedDirectory = selectedDir.FullName;
            Settings.Default.Save();
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            _formFinishedLoading = true;
        }

        #endregion

        #region Menu items

        private void mnuItmAbout_Click(object sender, EventArgs e)
        {
            var frm = new About();
            frm.ShowDialog();
        }

        private void mnuItmExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        #endregion

        private void OpenFile()
        {
            foreach (var selectedObject in olvFiles.SelectedObjects.Cast<FileInfo>())
            {
                OpenFile(selectedObject.FullName);
            }
        }

        private static void OpenFile(string path)
        {
            Process.Start(path);
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

            tvNavigation.BeginUpdate();

            foreach (var directoryInfo in dirList)
            {
                var info = directoryInfo;
                foreach (var treeNode in nodeCollection.Where(n => n.Text == info.Name))
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
        }

        #region Right click
        
        private void olvFiles_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (olvFiles.FocusedItem.Bounds.Contains(e.Location))
                {
                    ctxRightClick.Show(Cursor.Position);
                }
            }
        }

        private void ctxProperties_Click(object sender, EventArgs e)
        {
            var file = olvFiles.SelectedObject as FileInfo;
            if (file == null) return;
            ShowFileProperties(file.FullName);
        }

        private void ctxRename_Click(object sender, EventArgs e)
        {

        }

        private void ctxDelete_Click(object sender, EventArgs e)
        {
            var files = olvFiles.SelectedObjects.Cast<FileInfo>().ToList();
            var message = "Are you sure you wish to delete these files?";

            if (!files.Any()) return;
            
            if (files.Count() == 1)
            {
                message = String.Format("Are you sure you wish to delete '{0}'", files.First().Name);
            }

            if (MessageBox.Show(message, @"Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                                MessageBoxDefaultButton.Button1) != DialogResult.Yes) return;

            foreach (var fileInfo in files)
            {
                fileInfo.Delete();
            }

            RefreshFolder();
        }

        private void ctxCopy_Click(object sender, EventArgs e)
        {
            var paths = GetSelectedFilesStringCollection();

            Clipboard.Clear();
            Clipboard.SetFileDropList(paths);
        }

        private StringCollection GetSelectedFilesStringCollection()
        {
            var paths = new StringCollection();

            foreach (var selectedObject in olvFiles.SelectedObjects.Cast<FileInfo>())
            {
                paths.Add(selectedObject.FullName);
            }
            return paths;
        }

        private void ctxCut_Click(object sender, EventArgs e)
        {
            var moveEffect = new byte[] { 2, 0, 0, 0 };
            using (var dropEffect = new MemoryStream())
            {
                dropEffect.Write(moveEffect, 0, moveEffect.Length);

                var data = new DataObject();
                data.SetFileDropList(GetSelectedFilesStringCollection());
                data.SetData("Preferred DropEffect", dropEffect);

                Clipboard.Clear();
                Clipboard.SetDataObject(data, true);
            }
        }

        private void ctxOpen_Click(object sender, EventArgs e)
        {
            OpenFile();
        }

        #region Properties Dialog box
        [DllImport("shell32.dll", CharSet = CharSet.Auto)]
        static extern bool ShellExecuteEx(ref ShellExecuteInfo lpExecInfo);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct ShellExecuteInfo
        {
            public int cbSize;
            public uint fMask;
            public IntPtr hwnd;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpVerb;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpFile;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpParameters;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpDirectory;
            public int nShow;
            public IntPtr hInstApp;
            public IntPtr lpIDList;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpClass;
            public IntPtr hkeyClass;
            public uint dwHotKey;
            public IntPtr hIcon;
            public IntPtr hProcess;
        }

        private const int SwShow = 5;
        private const uint SeeMaskInvokeidlist = 12;
        public static bool ShowFileProperties(string filename)
        {
            var info = new ShellExecuteInfo();
            info.cbSize = Marshal.SizeOf(info);
            info.lpVerb = "properties";
            info.lpFile = filename;
            info.nShow = SwShow;
            info.fMask = SeeMaskInvokeidlist;
            return ShellExecuteEx(ref info);
        }
        #endregion

        #endregion

        #region Back button
        private void pctBack_Click(object sender, EventArgs e)
        {
            if (!_history.Any()) return;

            var dir = _history.Pop();
            _openingDirectory = true;
            OpenDirectory(dir.FullName);
            _openingDirectory = false;

            UpdateBackButtonImage();
        }

        private void pctBack_MouseLeave(object sender, EventArgs e)
        {
            UpdateBackButtonImage();
        }

        private void pctBack_MouseEnter(object sender, EventArgs e)
        {
            if (!_history.Any()) return;

            pctBack.Image = Resources.BackHover;
            var prevDir = _history.Peek();
            toolTip1.Show(
                String.Format("Go back to '{0}",
                              prevDir.Name),
                pctBack);
        }

        private void UpdateBackButtonImage()
        {
            pctBack.Image = _history.Any() ? Resources.BackAvailable : Resources.BackUnavailable;
        }
        #endregion

        #region Address bar
        private void txtDirectory_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter && e.KeyCode != Keys.Return) return;
            e.Handled = true;
            e.SuppressKeyPress = true;
            ProcessAddressBar();
        }

        private bool _alreadyFocused;

        void txtDirectory_Leave(object sender, EventArgs e)
        {
            _alreadyFocused = false;
        }

        void txtDirectory_MouseUp(object sender, MouseEventArgs e)
        {
            if (_alreadyFocused || txtDirectory.SelectionLength != 0) return;

            _alreadyFocused = true;
            txtDirectory.SelectAll();
        }

        private void txtDirectory_Enter(object sender, EventArgs e)
        {
            if (MouseButtons != MouseButtons.None) return;
            txtDirectory.SelectAll();
            _alreadyFocused = true;
        }

        #endregion

        #region Up a level

        private void pctUpALevel_MouseLeave(object sender, EventArgs e)
        {
            pctUpALevel.Image = Resources.UpALevel;
        }

        private void pctUpALevel_MouseEnter(object sender, EventArgs e)
        {
            pctUpALevel.Image = Resources.UpALevelHover;
            var parentDir = CurrentDirectory.Parent;
            if (parentDir == null) return;

            toolTip1.Show(String.Format("Up a level to '{0}'",
                parentDir.Name),
                pctUpALevel);
        }

        private void pctUpALevel_MouseClick(object sender, MouseEventArgs e)
        {
            var parentDir = CurrentDirectory.Parent;

            if (parentDir != null)
            {
                ChangeDirectory(parentDir);
            }
        }

        #endregion

        #region Go button

        private void pctGo_Click(object sender, EventArgs e)
        {
            ProcessAddressBar();
        }

        private void pctGo_MouseEnter(object sender, EventArgs e)
        {
            pctGo.Image = Resources.GoHover;
        }

        private void pctGo_MouseLeave(object sender, EventArgs e)
        {
            pctGo.Image = Resources.Go;
        }

        #endregion

        private void ProcessAddressBar()
        {
            var dir = new DirectoryInfo(txtDirectory.Text);

            if (dir.Exists)
            {
                OpenDirectory(dir.FullName);
                olvFiles.Focus();
            }
            else
            {
                var file = new FileInfo(txtDirectory.Text);

                if (file.Exists)
                {
                    OpenFile(file.FullName);
                }
                else
                {
                    MessageBox.Show(@"The directory you have specified does not exist", @"Error", MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                }
            }
        }
    }
}