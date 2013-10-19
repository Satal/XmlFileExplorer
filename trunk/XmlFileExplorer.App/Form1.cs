using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using XmlFileExplorer.Domain.Validation;
using XmlFileExplorer.Event;
using XmlFileExplorer.Forms;
using XmlFileExplorer.Properties;

namespace XmlFileExplorer
{
    public partial class Form1 : Form
    {
        private readonly ErrorsForm _errors;
        private readonly FolderExplorer _explorer;
        private readonly FilesForm _filesForm;
        private readonly Stack<DirectoryInfo> _history = new Stack<DirectoryInfo>();

        private bool _formFinishedLoading;

        public Form1()
        {
            InitializeComponent();

            _filesForm = new FilesForm();
            _filesForm.FileSelectionChanged += _filesForm_FileSelectionChanged;
            _filesForm.Show(dockPanel1);

            _explorer = new FolderExplorer();
            _explorer.FolderLocationChanged += explorer_FolderLocationChanged;
            _explorer.Show(dockPanel1, DockState.DockLeft);

            _errors = new ErrorsForm();
            _errors.Show(dockPanel1, DockState.DockBottom);

            OpenDirectory(Settings.Default.LastViewedDirectory);
        }

        private DirectoryInfo CurrentDirectory { get; set; }

        private void _filesForm_FileSelectionChanged(object sender, FileSelectionChangedEventArgs eventArgs)
        {
            IEnumerable<ValidationError> errors = eventArgs.Files.SelectMany(f => f.ValidationErrors);
            _errors.SetErrors(errors);
        }

        private void explorer_FolderLocationChanged(object sender, FolderLocationChangedEventArgs eventArgs)
        {
            ChangeDirectory(eventArgs.NewLocation);
            txtDirectory.Text = eventArgs.NewLocation.FullName;
        }

        private void ChangeDirectory(DirectoryInfo directory)
        {
            if (CurrentDirectory != null && _formFinishedLoading)
            {
                _history.Push(CurrentDirectory);
            }

            CurrentDirectory = directory;

            _filesForm.OpenDirectory(directory);
            txtDirectory.Text = directory.FullName;
            UpdateBackButtonImage();
        }

        private void OpenDirectory(string path)
        {
            _explorer.OpenDirectory(path);
        }

        private void HistoryGoBack()
        {
            if (!_history.Any()) return;

            DirectoryInfo dir = _history.Pop();
            OpenDirectory(dir.FullName);

            UpdateBackButtonImage();
        }

        #region Back button

        private void pctBack_Click(object sender, EventArgs e)
        {
            HistoryGoBack();
        }

        private void pctBack_MouseLeave(object sender, EventArgs e)
        {
            UpdateBackButtonImage();
        }

        private void pctBack_MouseEnter(object sender, EventArgs e)
        {
            if (!_history.Any()) return;

            pctBack.Image = Resources.BackHover;
            DirectoryInfo prevDir = _history.Peek();
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

        private bool _alreadyFocused;

        private void txtDirectory_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter && e.KeyCode != Keys.Return) return;
            e.Handled = true;
            e.SuppressKeyPress = true;
            ProcessAddressBar();
        }

        private void txtDirectory_Leave(object sender, EventArgs e)
        {
            _alreadyFocused = false;
        }

        private void txtDirectory_MouseUp(object sender, MouseEventArgs e)
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

        private void ProcessAddressBar()
        {
            var dir = new DirectoryInfo(txtDirectory.Text);

            if (dir.Exists)
            {
                OpenDirectory(dir.FullName);
            }
            else
            {
                var file = new FileInfo(txtDirectory.Text);

                if (file.Exists)
                {
                    Process.Start(file.FullName);
                }
                else
                {
                    MessageBox.Show(@"The directory you have specified does not exist", @"Error", MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                }
            }
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
            DirectoryInfo parentDir = CurrentDirectory.Parent;
            if (parentDir == null) return;

            toolTip1.Show(String.Format("Up a level to '{0}'",
                                        parentDir.Name),
                          pctUpALevel);
        }

        private void pctUpALevel_MouseClick(object sender, MouseEventArgs e)
        {
            DirectoryInfo parentDir = CurrentDirectory.Parent;

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

        #region Form events

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Settings.Default.LastViewedDirectory = _explorer.CurrentDirectory.FullName;
            Settings.Default.IsMaximised = WindowState == FormWindowState.Maximized;
            Settings.Default.Save();
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            _formFinishedLoading = true;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            bool pressHandled = false;

            switch (e.KeyCode)
            {
                case Keys.Back:
                    HistoryGoBack();
                    pressHandled = true;
                    break;
                case Keys.Next:
                    break;
                case Keys.F5:
                    _filesForm.RefreshFolder();
                    pressHandled = true;
                    break;
                case Keys.F6:
                    txtDirectory.Focus();
                    pressHandled = true;
                    break;
                case Keys.BrowserBack:
                    HistoryGoBack();
                    pressHandled = true;
                    break;
                case Keys.BrowserForward:
                    break;
                case Keys.BrowserRefresh:
                    _filesForm.RefreshFolder();
                    pressHandled = true;
                    break;
            }

            if (pressHandled)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
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
    }
}