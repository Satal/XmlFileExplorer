namespace XmlFileExplorer
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuItmExit = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuItmAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.imgLstIcons = new System.Windows.Forms.ImageList(this.components);
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.panel2 = new System.Windows.Forms.Panel();
            this.pctGo = new System.Windows.Forms.PictureBox();
            this.pctUpALevel = new System.Windows.Forms.PictureBox();
            this.txtDirectory = new System.Windows.Forms.TextBox();
            this.pctBack = new System.Windows.Forms.PictureBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.spltMain = new System.Windows.Forms.SplitContainer();
            this.tvNavigation = new System.Windows.Forms.TreeView();
            this.olvFiles = new BrightIdeasSoftware.ObjectListView();
            this.colFilename = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.colFilesize = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.colCreated = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.colModified = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvValidationErrors = new BrightIdeasSoftware.ObjectListView();
            this.colSeverity = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.colError = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.menuStrip1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pctGo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pctUpALevel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pctBack)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spltMain)).BeginInit();
            this.spltMain.Panel1.SuspendLayout();
            this.spltMain.Panel2.SuspendLayout();
            this.spltMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.olvFiles)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.olvValidationErrors)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1368, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuItmExit});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // mnuItmExit
            // 
            this.mnuItmExit.Name = "mnuItmExit";
            this.mnuItmExit.Size = new System.Drawing.Size(92, 22);
            this.mnuItmExit.Text = "E&xit";
            this.mnuItmExit.Click += new System.EventHandler(this.mnuItmExit_Click);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.optionsToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.toolsToolStripMenuItem.Text = "Tools";
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.optionsToolStripMenuItem.Text = "&Options";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuItmAbout});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // mnuItmAbout
            // 
            this.mnuItmAbout.Name = "mnuItmAbout";
            this.mnuItmAbout.Size = new System.Drawing.Size(107, 22);
            this.mnuItmAbout.Text = "&About";
            this.mnuItmAbout.Click += new System.EventHandler(this.mnuItmAbout_Click);
            // 
            // imgLstIcons
            // 
            this.imgLstIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgLstIcons.ImageStream")));
            this.imgLstIcons.TransparentColor = System.Drawing.Color.Transparent;
            this.imgLstIcons.Images.SetKeyName(0, "folder.ico");
            this.imgLstIcons.Images.SetKeyName(1, "document.ico");
            this.imgLstIcons.Images.SetKeyName(2, "Error");
            this.imgLstIcons.Images.SetKeyName(3, "Information");
            this.imgLstIcons.Images.SetKeyName(4, "Warning");
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.pctGo);
            this.panel2.Controls.Add(this.pctUpALevel);
            this.panel2.Controls.Add(this.txtDirectory);
            this.panel2.Controls.Add(this.pctBack);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 24);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1368, 24);
            this.panel2.TabIndex = 3;
            // 
            // pctGo
            // 
            this.pctGo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pctGo.Image = global::XmlFileExplorer.Properties.Resources.Go;
            this.pctGo.Location = new System.Drawing.Point(1341, 2);
            this.pctGo.Name = "pctGo";
            this.pctGo.Size = new System.Drawing.Size(20, 20);
            this.pctGo.TabIndex = 4;
            this.pctGo.TabStop = false;
            this.pctGo.Click += new System.EventHandler(this.pctGo_Click);
            this.pctGo.MouseEnter += new System.EventHandler(this.pctGo_MouseEnter);
            this.pctGo.MouseLeave += new System.EventHandler(this.pctGo_MouseLeave);
            // 
            // pctUpALevel
            // 
            this.pctUpALevel.Image = global::XmlFileExplorer.Properties.Resources.UpALevel;
            this.pctUpALevel.Location = new System.Drawing.Point(82, 0);
            this.pctUpALevel.Name = "pctUpALevel";
            this.pctUpALevel.Size = new System.Drawing.Size(24, 24);
            this.pctUpALevel.TabIndex = 3;
            this.pctUpALevel.TabStop = false;
            this.pctUpALevel.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pctUpALevel_MouseClick);
            this.pctUpALevel.MouseEnter += new System.EventHandler(this.pctUpALevel_MouseEnter);
            this.pctUpALevel.MouseLeave += new System.EventHandler(this.pctUpALevel_MouseLeave);
            // 
            // txtDirectory
            // 
            this.txtDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDirectory.Location = new System.Drawing.Point(112, 2);
            this.txtDirectory.Name = "txtDirectory";
            this.txtDirectory.Size = new System.Drawing.Size(1230, 20);
            this.txtDirectory.TabIndex = 2;
            this.txtDirectory.Enter += new System.EventHandler(this.txtDirectory_Enter);
            this.txtDirectory.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtDirectory_KeyDown);
            this.txtDirectory.Leave += new System.EventHandler(this.txtDirectory_Leave);
            this.txtDirectory.MouseUp += new System.Windows.Forms.MouseEventHandler(this.txtDirectory_MouseUp);
            // 
            // pctBack
            // 
            this.pctBack.Image = global::XmlFileExplorer.Properties.Resources.BackUnavailable;
            this.pctBack.Location = new System.Drawing.Point(2, 2);
            this.pctBack.Name = "pctBack";
            this.pctBack.Size = new System.Drawing.Size(21, 21);
            this.pctBack.TabIndex = 0;
            this.pctBack.TabStop = false;
            this.pctBack.Click += new System.EventHandler(this.pctBack_Click);
            this.pctBack.MouseEnter += new System.EventHandler(this.pctBack_MouseEnter);
            this.pctBack.MouseLeave += new System.EventHandler(this.pctBack_MouseLeave);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 48);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.spltMain);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.olvValidationErrors);
            this.splitContainer1.Size = new System.Drawing.Size(1368, 553);
            this.splitContainer1.SplitterDistance = 300;
            this.splitContainer1.TabIndex = 4;
            // 
            // spltMain
            // 
            this.spltMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.spltMain.Location = new System.Drawing.Point(0, 0);
            this.spltMain.Name = "spltMain";
            // 
            // spltMain.Panel1
            // 
            this.spltMain.Panel1.Controls.Add(this.tvNavigation);
            // 
            // spltMain.Panel2
            // 
            this.spltMain.Panel2.Controls.Add(this.olvFiles);
            this.spltMain.Size = new System.Drawing.Size(1368, 300);
            this.spltMain.SplitterDistance = 246;
            this.spltMain.TabIndex = 5;
            // 
            // tvNavigation
            // 
            this.tvNavigation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvNavigation.HideSelection = false;
            this.tvNavigation.ImageIndex = 0;
            this.tvNavigation.ImageList = this.imgLstIcons;
            this.tvNavigation.Location = new System.Drawing.Point(0, 0);
            this.tvNavigation.Name = "tvNavigation";
            this.tvNavigation.SelectedImageIndex = 0;
            this.tvNavigation.Size = new System.Drawing.Size(246, 300);
            this.tvNavigation.TabIndex = 0;
            this.tvNavigation.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.tvNavigation_BeforeExpand);
            this.tvNavigation.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvNavigation_AfterSelect);
            // 
            // olvFiles
            // 
            this.olvFiles.AllColumns.Add(this.colFilename);
            this.olvFiles.AllColumns.Add(this.colFilesize);
            this.olvFiles.AllColumns.Add(this.colCreated);
            this.olvFiles.AllColumns.Add(this.colModified);
            this.olvFiles.AllowDrop = true;
            this.olvFiles.CellEditActivation = BrightIdeasSoftware.ObjectListView.CellEditActivateMode.DoubleClick;
            this.olvFiles.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colFilename,
            this.colFilesize,
            this.colCreated,
            this.colModified});
            this.olvFiles.CopySelectionOnControlC = false;
            this.olvFiles.CopySelectionOnControlCUsesDragSource = false;
            this.olvFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.olvFiles.FullRowSelect = true;
            this.olvFiles.HasCollapsibleGroups = false;
            this.olvFiles.HideSelection = false;
            this.olvFiles.IsSimpleDragSource = true;
            this.olvFiles.IsSimpleDropSink = true;
            this.olvFiles.Location = new System.Drawing.Point(0, 0);
            this.olvFiles.Name = "olvFiles";
            this.olvFiles.ShowGroups = false;
            this.olvFiles.Size = new System.Drawing.Size(1118, 300);
            this.olvFiles.TabIndex = 1;
            this.olvFiles.UseCompatibleStateImageBehavior = false;
            this.olvFiles.View = System.Windows.Forms.View.Details;
            this.olvFiles.CanDrop += new System.EventHandler<BrightIdeasSoftware.OlvDropEventArgs>(this.olvFiles_CanDrop);
            this.olvFiles.CellEditFinishing += new BrightIdeasSoftware.CellEditEventHandler(this.olvFiles_CellEditFinishing);
            this.olvFiles.FormatRow += new System.EventHandler<BrightIdeasSoftware.FormatRowEventArgs>(this.olvFiles_FormatRow);
            this.olvFiles.SelectionChanged += new System.EventHandler(this.olvFiles_SelectionChanged);
            this.olvFiles.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.olvFiles_ItemDrag);
            this.olvFiles.DragDrop += new System.Windows.Forms.DragEventHandler(this.olvFiles_DragDrop);
            this.olvFiles.KeyDown += new System.Windows.Forms.KeyEventHandler(this.olvFiles_KeyDown);
            this.olvFiles.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.olvFiles_MouseDoubleClick);
            this.olvFiles.MouseUp += new System.Windows.Forms.MouseEventHandler(this.olvFiles_MouseUp);
            // 
            // colFilename
            // 
            this.colFilename.AspectName = "FileInfo.Name";
            this.colFilename.CellPadding = null;
            this.colFilename.Text = "File name";
            // 
            // colFilesize
            // 
            this.colFilesize.AspectName = "FileInfo.Length";
            this.colFilesize.CellPadding = null;
            this.colFilesize.IsEditable = false;
            this.colFilesize.Text = "File size";
            // 
            // colCreated
            // 
            this.colCreated.AspectName = "FileInfo.CreationTime";
            this.colCreated.CellPadding = null;
            this.colCreated.DisplayIndex = 3;
            this.colCreated.IsEditable = false;
            this.colCreated.Text = "Created";
            // 
            // colModified
            // 
            this.colModified.AspectName = "FileInfo.LastWriteTime";
            this.colModified.CellPadding = null;
            this.colModified.DisplayIndex = 2;
            this.colModified.IsEditable = false;
            this.colModified.Text = "Modified";
            // 
            // olvValidationErrors
            // 
            this.olvValidationErrors.AllColumns.Add(this.colSeverity);
            this.olvValidationErrors.AllColumns.Add(this.colError);
            this.olvValidationErrors.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colSeverity,
            this.colError});
            this.olvValidationErrors.Dock = System.Windows.Forms.DockStyle.Fill;
            this.olvValidationErrors.FullRowSelect = true;
            this.olvValidationErrors.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.olvValidationErrors.Location = new System.Drawing.Point(0, 0);
            this.olvValidationErrors.Name = "olvValidationErrors";
            this.olvValidationErrors.Size = new System.Drawing.Size(1368, 249);
            this.olvValidationErrors.SmallImageList = this.imgLstIcons;
            this.olvValidationErrors.TabIndex = 4;
            this.olvValidationErrors.UseCompatibleStateImageBehavior = false;
            this.olvValidationErrors.View = System.Windows.Forms.View.Details;
            // 
            // colSeverity
            // 
            this.colSeverity.AspectName = "ErrorSeverity";
            this.colSeverity.CellPadding = null;
            this.colSeverity.MaximumWidth = 20;
            this.colSeverity.MinimumWidth = 20;
            this.colSeverity.Text = "";
            this.colSeverity.Width = 20;
            // 
            // colError
            // 
            this.colError.AspectName = "Description";
            this.colError.CellPadding = null;
            this.colError.FillsFreeSpace = true;
            this.colError.Text = "Description";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1368, 601);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(664, 640);
            this.Name = "Form1";
            this.Text = "XML File Explorer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Shown += new System.EventHandler(this.Form1_Shown);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pctGo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pctUpALevel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pctBack)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.spltMain.Panel1.ResumeLayout(false);
            this.spltMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.spltMain)).EndInit();
            this.spltMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.olvFiles)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.olvValidationErrors)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mnuItmExit;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mnuItmAbout;
        private System.Windows.Forms.ImageList imgLstIcons;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.PictureBox pctGo;
        private System.Windows.Forms.PictureBox pctUpALevel;
        private System.Windows.Forms.TextBox txtDirectory;
        private System.Windows.Forms.PictureBox pctBack;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer spltMain;
        private System.Windows.Forms.TreeView tvNavigation;
        private BrightIdeasSoftware.ObjectListView olvFiles;
        private BrightIdeasSoftware.OLVColumn colFilename;
        private BrightIdeasSoftware.OLVColumn colFilesize;
        private BrightIdeasSoftware.OLVColumn colCreated;
        private BrightIdeasSoftware.OLVColumn colModified;
        private BrightIdeasSoftware.ObjectListView olvValidationErrors;
        private BrightIdeasSoftware.OLVColumn colSeverity;
        private BrightIdeasSoftware.OLVColumn colError;
    }
}

