using BrightIdeasSoftware;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using XmlFileExplorer.Domain;
using XmlFileExplorer.Domain.Validation;
using XmlFileExplorer.Properties;

namespace XmlFileExplorer
{
    public partial class Form1 : Form
    {
        [ImportMany(typeof(IValidator))]
        private IEnumerable<IValidator> _validators; 

        private Color _validBackgroundColor;
        private Color _validForegroundColor;
        private Color _invalidBackgroundColor;
        private Color _invalidForegroundColor;
        private bool _showExtension;
        private DirectoryInfo CurrentDirectory { get; set; }
        private readonly Stack<DirectoryInfo> _history = new Stack<DirectoryInfo>();

        private bool _formFinishedLoading = false;
        private bool _openingDirectory = false;

        public Form1()
        {
            InitializeComponent();

            // This gets removed when the designer regenerates the code, so we have to put this here
            olvFiles.AfterLabelEdit += olvFiles_AfterLabelEdit;

            SetUpOlvDelegates();
            PopulateTreeView();
            LoadAppearanceConfiguration();
            LoadValidators();
            OpenDirectory(Settings.Default.LastViewedDirectory);
        }

        private void LoadValidators()
        {
            var catalog = new AggregateCatalog();
            foreach (var dll in Directory.GetFiles(Application.StartupPath, "*.dll"))
            {
                catalog.Catalogs.Add(new AssemblyCatalog(Assembly.LoadFrom(dll)));
            }
            var container = new CompositionContainer(catalog);
            container.ComposeParts(this);
        }

        private void LoadAppearanceConfiguration()
        {
            _validBackgroundColor = Settings.Default.ValidXmlBackgroundColor;
            _validForegroundColor = Settings.Default.ValidXmlForegroundColor;
            _invalidBackgroundColor = Settings.Default.InvalidXmlBackgroundColor;
            _invalidForegroundColor = Settings.Default.InvalidXmlForegroundColor;

            _showExtension = Settings.Default.ShowFileExtension;

            var errorsPanelPercentage = Settings.Default.ErrorsPanelPercentage;
            splitContainer1.SplitterDistance = ((splitContainer1.Height / 100) * errorsPanelPercentage) + panel2.Height;
            WindowState = Settings.Default.IsMaximised ? FormWindowState.Maximized : FormWindowState.Normal;
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
                olvFiles.AddObjects(directory.GetFiles().Select(f => new XfeFileInfo(f)).ToList());
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

        private void SetUpOlvDelegates()
        {
            colFilename.AspectToStringConverter = delegate(object value)
                {
                    var file = (string)value;
                    return _showExtension ? file : Path.GetFileNameWithoutExtension(file);
                };

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
            
            colSeverity.AspectGetter = rowObject => ((ValidationError) rowObject).ErrorSeverity;
            colSeverity.AspectToStringConverter = value => String.Empty;
            colSeverity.GroupKeyGetter = rowObject => ((ValidationError)rowObject).ErrorSeverity;
            colSeverity.GroupKeyToTitleConverter = key => ((ErrorSeverity)key).ToString();
            colSeverity.ImageGetter = delegate(object rowObject)
                {
                    switch (((ValidationError)rowObject).ErrorSeverity)
                    {
                        case ErrorSeverity.Error:
                            return "Error";
                        case ErrorSeverity.Warning:
                            return "Warning";
                        default:
                            return "Error";
                    }
                };
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

        #region olvFiles events

        private void olvFiles_FormatRow(object sender, FormatRowEventArgs e)
        {
            var file = e.Model as XfeFileInfo;
            if (file == null) return;

            if (file.FileInfo.Extension != ".xml") return;

            file.ValidationErrors.Clear();
            foreach (var validator in _validators)
            {
                file.ValidationErrors.AddRange(validator.ValidateFile(file.FileInfo.FullName));
            }

            var passedAll = !file.ValidationErrors.Any();

            e.Item.BackColor = passedAll ? _validBackgroundColor : _invalidBackgroundColor;
            e.Item.ForeColor = passedAll ? _validForegroundColor : _invalidForegroundColor;
        }

        private void olvFiles_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            OpenFile();
        }

        private void olvFiles_ItemDrag(object sender, ItemDragEventArgs e)
        {
            var item = e.Item as OLVListItem;

            if (item == null) return;

            var fileInfo = item.RowObject as XfeFileInfo;

            if (fileInfo != null)
            {
                DoDragDrop(fileInfo.FileInfo.FullName, DragDropEffects.Copy);
            }
        }

        private void olvFiles_SelectionChanged(object sender, EventArgs e)
        {
            var files = olvFiles.SelectedObjects.Cast<XfeFileInfo>();
            var errors = files.SelectMany(v => v.ValidationErrors).ToList();

            olvValidationErrors.Items.Clear();
            olvValidationErrors.AddObjects(errors);
        }

        private void olvFiles_KeyDown(object sender, KeyEventArgs e)
        {
            var pressHandled = false;

            if (e.Modifiers == Keys.Control)
            {
                switch (e.KeyCode)
                {
                    case Keys.C:
                        PutSelectedFilesInClipboard();
                        pressHandled = true;
                        break;
                    case Keys.V:
                        Paste();
                        pressHandled = true;
                        break;
                }
            }
            else
            {
                switch (e.KeyCode)
                {
                    case Keys.Delete:
                        var deletePerm = e.Modifiers == Keys.Shift;
                        DeleteSelectedFiles(!deletePerm);
                        pressHandled = true;
                        break;
                    case Keys.F5:
                        RefreshFolder();
                        pressHandled = true;
                        break;
                    case Keys.F6:
                        txtDirectory.Focus();
                        pressHandled = true;
                        break;
                }
            }

            if (pressHandled)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void olvFiles_DragDrop(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop)) return;

            var fileLocations = e.Data.GetData(DataFormats.FileDrop) as string[];
            if (fileLocations == null || !fileLocations.Any()) return;
            
            PasteFiles(fileLocations, true);
        }

        private void olvFiles_CanDrop(object sender, OlvDropEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        private void olvFiles_CellEditFinishing(object sender, CellEditEventArgs e)
        {
            // Check the file name has changed
            if ((string)e.NewValue == (string)e.Value) return;

            // We have finished editing the cell
            var file = e.RowObject as XfeFileInfo;

            if (file == null || file.FileInfo.Directory == null) return;

            var newFile = new FileInfo(Path.Combine(file.FileInfo.Directory.FullName, (string)e.NewValue));

            if (newFile.Exists)
            {
                MessageBox.Show(@"A file with that name already exists", @"Error", MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                e.Cancel = true;
                return;
            }

            file.FileInfo.MoveTo(newFile.FullName);
        }

        private void olvFiles_AfterLabelEdit(object sender, LabelEditEventArgs e)
        {
            var item = olvFiles.Items[e.Item] as OLVListItem;
            if (item == null) return;
            var file = item.RowObject as XfeFileInfo;

            // Check that the file isn't null and that the file name has changed
            if (file == null || file.FileInfo.Directory == null || e.Label == file.FileInfo.Name) return;

            var newFile = new FileInfo(Path.Combine(file.FileInfo.Directory.FullName, e.Label));

            if (newFile.Exists)
            {
                MessageBox.Show(@"A file with that name already exists", @"Error", MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                e.CancelEdit = true;
                return;
            }

            file.FileInfo.MoveTo(newFile.FullName);
        }

        #endregion
        
        #region Form events

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (tvNavigation.SelectedNode == null) return;

            var selectedDir = (DirectoryInfo)tvNavigation.SelectedNode.Tag;
            Settings.Default.LastViewedDirectory = selectedDir.FullName;
            Settings.Default.IsMaximised = WindowState == FormWindowState.Maximized;
            Settings.Default.ErrorsPanelPercentage = Convert.ToInt32(((double)splitContainer1.SplitterDistance / (splitContainer1.Height - panel2.Height)) * 100);
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
            foreach (var selectedObject in olvFiles.SelectedObjects.Cast<XfeFileInfo>())
            {
                OpenFile(selectedObject.FileInfo.FullName);
            }
        }

        private static void OpenFile(string path)
        {
            Process.Start(path);
        }

        private void OpenDirectory(string path)
        {
            // Make sure that the path is not null or empty
            if (String.IsNullOrEmpty(path)) return;
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
            if (e.Button != MouseButtons.Right) return;
            var ctx = new ContextMenuStrip();

            if (olvFiles.FocusedItem != null && olvFiles.FocusedItem.Bounds.Contains(e.Location))
            {
                ctx.Items.Add("Open", null, ctxOpen_Click);
                ctx.Items.Add(new ToolStripSeparator());
                ctx.Items.Add("Copy", null, ctxCopy_Click);
                ctx.Items.Add("Cut", null, ctxCut_Click);
                
                if (PasteFilesAvailable())
                {
                    var pasteOption = ctx.Items.Add("Paste");
                    pasteOption.Click += pasteOption_Click;
                }

                ctx.Items.Add(new ToolStripSeparator());
                ctx.Items.Add("Delete", null, ctxDelete_Click);

                if (olvFiles.SelectedObjects.Count == 1)
                {
                    ctx.Items.Add("Rename", null, ctxRename_Click);
                }
            }
            else
            {
                // The user has right clicked on an empty area
                if (PasteFilesAvailable())
                {
                    var pasteOption = ctx.Items.Add("Paste");
                    pasteOption.Click += pasteOption_Click;
                    ctx.Items.Add(new ToolStripSeparator());
                }

                ctx.Items.Add("Refresh", null, ctxRefresh_Click);
            }

            ctx.Items.Add(new ToolStripSeparator());
            ctx.Items.Add("Properties", null, ctxProperties_Click);
            ctx.Show(Cursor.Position);
        }

        private void ctxRefresh_Click(object sender, EventArgs eventArgs)
        {
            RefreshFolder();
        }

        void pasteOption_Click(object sender, EventArgs e)
        {
            Paste();
        }

        private void ctxProperties_Click(object sender, EventArgs e)
        {

            var file = olvFiles.SelectedObject as XfeFileInfo;
            var propLocation = (file == null) ? CurrentDirectory.FullName : file.FileInfo.FullName;
            ShowProperties(propLocation);
        }

        private void ctxRename_Click(object sender, EventArgs e)
        {
            if (olvFiles.SelectedObjects.Count == 0) return;
            
            olvFiles.EditModel(olvFiles.SelectedObject);
        }

        private void ctxDelete_Click(object sender, EventArgs e)
        {
            DeleteSelectedFiles();
        }

        private void ctxCopy_Click(object sender, EventArgs e)
        {
            PutSelectedFilesInClipboard();
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
        public static bool ShowProperties(string filename)
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

        private void Paste()
        {
            var d = Clipboard.GetDataObject();
            if (d == null || !d.GetDataPresent(DataFormats.FileDrop)) return;

            var files = d.GetData(DataFormats.FileDrop) as string[];

            PasteFiles(files, false);
        }

        private void PasteFiles(IEnumerable<string> files, bool moveIfSameDrive)
        {
            if (files == null) return;
            foreach (var file in files.Select(f => new FileInfo(f)))
            {
                // Check that the source file still exists
                if (!file.Exists) continue;

                var destFile = new FileInfo(Path.Combine(CurrentDirectory.FullName, file.Name));

                // If a file with that name already exists work out what to do.
                if (destFile.Exists && (file.Directory == null || file.Directory.FullName == CurrentDirectory.FullName))
                {
                    var counter = 1;
                    var ext = Path.GetExtension(file.Name);

                    do
                    {
                        destFile = new FileInfo(
                            Path.Combine(
                                CurrentDirectory.FullName,
                                String.Format("{0} - copy {1}.{2}",
                                              Path.GetFileNameWithoutExtension(file.Name),
                                              counter,
                                              ext)
                                ));

                        counter++;
                    } while (destFile.Exists);
                }

                if (moveIfSameDrive && file.Directory != null && destFile.Directory != null && file.Directory.Root.Name == destFile.Directory.Root.Name)
                {
                    FileSystem.MoveFile(file.FullName, destFile.FullName, UIOption.AllDialogs, UICancelOption.DoNothing);
                }
                else
                {
                    FileSystem.CopyFile(file.FullName, destFile.FullName, UIOption.AllDialogs, UICancelOption.DoNothing);
                }
            }

            RefreshFolder();
        }

        private static bool PasteFilesAvailable()
        {
            var d = Clipboard.GetDataObject();
            return d != null && d.GetDataPresent(DataFormats.FileDrop);
        }

        private StringCollection GetSelectedFilesStringCollection()
        {
            var paths = new StringCollection();

            foreach (var selectedObject in olvFiles.SelectedObjects.Cast<XfeFileInfo>())
            {
                paths.Add(selectedObject.FileInfo.FullName);
            }

            return paths;
        }

        private void DeleteSelectedFiles(bool sendToRecycleBin = true)
        {
            var files = GetSelectedFilesStringCollection();
            var fileCount = files.Count;
            var recycleOption = sendToRecycleBin ? RecycleOption.SendToRecycleBin : RecycleOption.DeletePermanently;
            var uiOption = fileCount == 1 ? UIOption.AllDialogs : UIOption.OnlyErrorDialogs;

            if (!sendToRecycleBin && fileCount > 1)
            {
                if (MessageBox.Show(String.Format(@"Are you sure that you want to permanently delete these {0} items?", fileCount),
                                    @"Delete Multiple Items", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) !=
                    DialogResult.Yes)
                {
                    return;
                }
            }

            foreach (var fileInfo in files)
            {
                FileSystem.DeleteFile(fileInfo, uiOption, recycleOption);
            }

            RefreshFolder();
        }
        
        private void PutSelectedFilesInClipboard()
        {
            var paths = GetSelectedFilesStringCollection();

            Clipboard.Clear();
            Clipboard.SetFileDropList(paths);
        }
    }
}