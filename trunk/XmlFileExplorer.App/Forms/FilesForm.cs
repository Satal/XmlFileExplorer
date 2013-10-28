using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using BrightIdeasSoftware;
using Microsoft.VisualBasic.FileIO;
using WeifenLuo.WinFormsUI.Docking;
using XmlFileExplorer.Domain;
using XmlFileExplorer.Domain.Validation;
using XmlFileExplorer.Event;
using XmlFileExplorer.Properties;

namespace XmlFileExplorer.Forms
{
    public delegate void FileSelectionChangedHandler(object sender, FileSelectionChangedEventArgs e);

    public partial class FilesForm : DockContent
    {
        private Color _invalidBackgroundColor;
        private Color _invalidForegroundColor;
        private bool _showExtension;
        private Color _validBackgroundColor;
        private Color _validForegroundColor;

        public FilesForm()
        {
            InitializeComponent();
            Validators = new List<IValidator>();
            olvFiles.AfterLabelEdit += olvFiles_AfterLabelEdit;

            CloseButton = false;
            CloseButtonVisible = false;

            LoadValidators();
            SetUpOlvDelegates();
            LoadAppearanceConfiguration();

            Load += (sender, args) => MdiParent.Resize += MdiParent_Resize;
        }

        [ImportMany(typeof (IValidator))]
        public List<IValidator> Validators { get; set; }

        public DirectoryInfo CurrentDirectory { get; set; }
        public event FileSelectionChangedHandler FileSelectionChanged;

        private void LoadAppearanceConfiguration()
        {
            _validBackgroundColor = Settings.Default.ValidXmlBackgroundColor;
            _validForegroundColor = Settings.Default.ValidXmlForegroundColor;
            _invalidBackgroundColor = Settings.Default.InvalidXmlBackgroundColor;
            _invalidForegroundColor = Settings.Default.InvalidXmlForegroundColor;

            _showExtension = Settings.Default.ShowFileExtension;
        }

        private void SetUpOlvDelegates()
        {
            colFilename.AspectToStringConverter = delegate(object value)
                {
                    var file = (string) value;
                    return _showExtension ? file : Path.GetFileNameWithoutExtension(file);
                };

            // Specify the file size format
            colFilesize.AspectToStringConverter = delegate(object value)
                {
                    var size = (long) value;
                    var limits = new[] {1024*1024*1024, 1024*1024, 1024};
                    var units = new[] {"GB", "MB", "KB"};

                    for (int i = 0; i < limits.Length; i++)
                    {
                        if (size >= limits[i])
                        {
                            return String.Format("{0:#,##0.##} {1}", ((double) size/limits[i]), units[i]);
                        }
                    }

                    return String.Format("{0} bytes", size);
                };

            try
            {
                colFileType.AspectGetter = x => NativeMethods.GetFileType(((XfeFileInfo)x).FileInfo.FullName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void LoadValidators()
        {
            var catalog = new AggregateCatalog();
            foreach (string dll in Directory.GetFiles(Application.StartupPath, "*.dll"))
            {
                catalog.Catalogs.Add(new AssemblyCatalog(Assembly.LoadFrom(dll)));
            }
            var container = new CompositionContainer(catalog);
            container.ComposeParts(this);
        }

        private void olvFiles_FormatRow(object sender, FormatRowEventArgs e)
        {
            if (Validators.Any())
            {
                var file = e.Model as XfeFileInfo;
                if (file == null) return;

                if (file.FileInfo.Extension != ".xml") return;

                file.ValidationErrors.Clear();
                foreach (IValidator validator in Validators)
                {
                    file.ValidationErrors.AddRange(validator.ValidateFile(file.FileInfo.FullName));
                }

                bool passedAll = !file.ValidationErrors.Any();

                e.Item.BackColor = passedAll ? _validBackgroundColor : _invalidBackgroundColor;
                e.Item.ForeColor = passedAll ? _validForegroundColor : _invalidForegroundColor;
            }
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
            IEnumerable<XfeFileInfo> files = olvFiles.SelectedObjects.Cast<XfeFileInfo>();
            var args = new FileSelectionChangedEventArgs
                {
                    Files = files.ToList()
                };

            if (FileSelectionChanged != null)
            {
                FileSelectionChanged(this, args);
            }
        }

        private void olvFiles_KeyDown(object sender, KeyEventArgs e)
        {
            bool pressHandled = false;

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
                        bool deletePerm = e.Modifiers == Keys.Shift;
                        DeleteSelectedFiles(!deletePerm);
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
            if ((string) e.NewValue == (string) e.Value) return;

            // We have finished editing the cell
            var file = e.RowObject as XfeFileInfo;

            if (file == null || file.FileInfo.Directory == null) return;

            var newFile = new FileInfo(Path.Combine(file.FileInfo.Directory.FullName, (string) e.NewValue));

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
            // If Label is null then the user cancelled the rename
            if (e.Label == null) return;
            var item = olvFiles.Items[e.Item] as OLVListItem;
            if (item == null) return;
            var file = item.RowObject as XfeFileInfo;

            // Check that the file isn't null and that the file name has changed
            if (file == null || file.FileInfo.Directory == null || e.Label == file.FileInfo.Name) return;

            var newFileName = e.Label;
            var newFile = new FileInfo(Path.Combine(file.FileInfo.Directory.FullName, newFileName));

            if (newFile.Exists)
            {
                MessageBox.Show(@"A file with that name already exists", @"Error", MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                e.CancelEdit = true;
                return;
            }

            file.FileInfo.MoveTo(newFile.FullName);
        }

        private void OpenFile()
        {
            foreach (XfeFileInfo selectedObject in olvFiles.SelectedObjects.Cast<XfeFileInfo>())
            {
                OpenFile(selectedObject.FileInfo.FullName);
            }
        }

        private static void OpenFile(string path)
        {
            Process.Start(path);
        }

        private StringCollection GetSelectedFilesStringCollection()
        {
            var paths = new StringCollection();

            foreach (XfeFileInfo selectedObject in olvFiles.SelectedObjects.Cast<XfeFileInfo>())
            {
                paths.Add(selectedObject.FileInfo.FullName);
            }

            return paths;
        }

        private void PutSelectedFilesInClipboard()
        {
            StringCollection paths = GetSelectedFilesStringCollection();

            Clipboard.Clear();
            Clipboard.SetFileDropList(paths);
        }

        private void Paste()
        {
            IDataObject d = Clipboard.GetDataObject();
            if (d == null || !d.GetDataPresent(DataFormats.FileDrop)) return;

            var files = d.GetData(DataFormats.FileDrop) as string[];

            PasteFiles(files, false);
        }

        private void PasteFiles(IEnumerable<string> files, bool moveIfSameDrive)
        {
            if (files == null) return;
            foreach (FileInfo file in files.Select(f => new FileInfo(f)))
            {
                // Check that the source file still exists
                if (!file.Exists) continue;

                var destFile = new FileInfo(Path.Combine(CurrentDirectory.FullName, file.Name));

                // If a file with that name already exists work out what to do.
                if (destFile.Exists && (file.Directory == null || file.Directory.FullName == CurrentDirectory.FullName))
                {
                    int counter = 1;
                    string ext = Path.GetExtension(file.Name);

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

                if (moveIfSameDrive && file.Directory != null && destFile.Directory != null &&
                    file.Directory.Root.Name == destFile.Directory.Root.Name)
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

        public void RefreshFolder()
        {
            OpenDirectory(CurrentDirectory);
        }

        public void DeleteSelectedFiles(bool sendToRecycleBin = true)
        {
            StringCollection files = GetSelectedFilesStringCollection();
            int fileCount = files.Count;
            RecycleOption recycleOption = sendToRecycleBin
                                              ? RecycleOption.SendToRecycleBin
                                              : RecycleOption.DeletePermanently;
            UIOption uiOption = fileCount == 1 ? UIOption.AllDialogs : UIOption.OnlyErrorDialogs;

            if (!sendToRecycleBin && fileCount > 1)
            {
                if (MessageBox.Show(
                    String.Format(@"Are you sure that you want to permanently delete these {0} items?", fileCount),
                    @"Delete Multiple Items", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) !=
                    DialogResult.Yes)
                {
                    return;
                }
            }

            foreach (string fileInfo in files)
            {
                FileSystem.DeleteFile(fileInfo, uiOption, recycleOption);
            }

            RefreshFolder();
        }

        public void OpenDirectory(DirectoryInfo directory)
        {
            CurrentDirectory = directory;

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

        private void MdiParent_Resize(object sender, EventArgs e)
        {
            if (CurrentDirectory == null) return;

            RefreshFolder();
        }

        private static bool PasteFilesAvailable()
        {
            IDataObject d = Clipboard.GetDataObject();
            return d != null && d.GetDataPresent(DataFormats.FileDrop);
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
                    ToolStripItem pasteOption = ctx.Items.Add("Paste");
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
                    ToolStripItem pasteOption = ctx.Items.Add("Paste");
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

        private void pasteOption_Click(object sender, EventArgs e)
        {
            Paste();
        }

        private void ctxProperties_Click(object sender, EventArgs e)
        {
            var file = olvFiles.SelectedObject as XfeFileInfo;
            string propLocation = (file == null) ? CurrentDirectory.FullName : file.FileInfo.FullName;
            NativeMethods.ShowProperties(propLocation);
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
            var moveEffect = new byte[] {2, 0, 0, 0};
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

        #endregion
    }
}