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
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuItmAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.spltMain = new System.Windows.Forms.SplitContainer();
            this.tvNavigation = new System.Windows.Forms.TreeView();
            this.imgLstIcons = new System.Windows.Forms.ImageList(this.components);
            this.olvFiles = new BrightIdeasSoftware.ObjectListView();
            this.colFilename = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.colFilesize = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.colCreated = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.colModified = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.panel1 = new System.Windows.Forms.Panel();
            this.menuStrip1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spltMain)).BeginInit();
            this.spltMain.Panel1.SuspendLayout();
            this.spltMain.Panel2.SuspendLayout();
            this.spltMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.olvFiles)).BeginInit();
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
            this.menuStrip1.Size = new System.Drawing.Size(648, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(92, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
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
            this.mnuItmAbout.Size = new System.Drawing.Size(152, 22);
            this.mnuItmAbout.Text = "&About";
            this.mnuItmAbout.Click += new System.EventHandler(this.mnuItmAbout_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.spltMain, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 24);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5.945946F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 94.05405F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(648, 577);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // spltMain
            // 
            this.spltMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.spltMain.Location = new System.Drawing.Point(3, 37);
            this.spltMain.Name = "spltMain";
            // 
            // spltMain.Panel1
            // 
            this.spltMain.Panel1.Controls.Add(this.tvNavigation);
            // 
            // spltMain.Panel2
            // 
            this.spltMain.Panel2.Controls.Add(this.olvFiles);
            this.spltMain.Size = new System.Drawing.Size(642, 537);
            this.spltMain.SplitterDistance = 120;
            this.spltMain.TabIndex = 3;
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
            this.tvNavigation.Size = new System.Drawing.Size(120, 537);
            this.tvNavigation.TabIndex = 0;
            this.tvNavigation.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.tvNavigation_BeforeExpand);
            this.tvNavigation.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvNavigation_AfterSelect);
            // 
            // imgLstIcons
            // 
            this.imgLstIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgLstIcons.ImageStream")));
            this.imgLstIcons.TransparentColor = System.Drawing.Color.Transparent;
            this.imgLstIcons.Images.SetKeyName(0, "folder.ico");
            this.imgLstIcons.Images.SetKeyName(1, "document.ico");
            // 
            // olvFiles
            // 
            this.olvFiles.AllColumns.Add(this.colFilename);
            this.olvFiles.AllColumns.Add(this.colFilesize);
            this.olvFiles.AllColumns.Add(this.colCreated);
            this.olvFiles.AllColumns.Add(this.colModified);
            this.olvFiles.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colFilename,
            this.colFilesize,
            this.colCreated,
            this.colModified});
            this.olvFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.olvFiles.FullRowSelect = true;
            this.olvFiles.HasCollapsibleGroups = false;
            this.olvFiles.HideSelection = false;
            this.olvFiles.IsSimpleDragSource = true;
            this.olvFiles.Location = new System.Drawing.Point(0, 0);
            this.olvFiles.Name = "olvFiles";
            this.olvFiles.ShowGroups = false;
            this.olvFiles.Size = new System.Drawing.Size(518, 537);
            this.olvFiles.TabIndex = 1;
            this.olvFiles.UseCompatibleStateImageBehavior = false;
            this.olvFiles.View = System.Windows.Forms.View.Details;
            this.olvFiles.FormatRow += new System.EventHandler<BrightIdeasSoftware.FormatRowEventArgs>(this.olvFiles_FormatRow);
            this.olvFiles.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.olvFiles_ItemDrag);
            this.olvFiles.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.olvFiles_MouseDoubleClick);
            // 
            // colFilename
            // 
            this.colFilename.AspectName = "Name";
            this.colFilename.CellPadding = null;
            this.colFilename.Text = "File name";
            // 
            // colFilesize
            // 
            this.colFilesize.AspectName = "Length";
            this.colFilesize.CellPadding = null;
            this.colFilesize.Text = "File size";
            // 
            // colCreated
            // 
            this.colCreated.AspectName = "CreationTime";
            this.colCreated.CellPadding = null;
            this.colCreated.DisplayIndex = 3;
            this.colCreated.Text = "Created";
            // 
            // colModified
            // 
            this.colModified.AspectName = "LastWriteTime";
            this.colModified.CellPadding = null;
            this.colModified.DisplayIndex = 2;
            this.colModified.Text = "Modified";
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(642, 28);
            this.panel1.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(648, 601);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "XML File Explorer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.spltMain.Panel1.ResumeLayout(false);
            this.spltMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.spltMain)).EndInit();
            this.spltMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.olvFiles)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mnuItmAbout;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.SplitContainer spltMain;
        private System.Windows.Forms.ImageList imgLstIcons;
        private System.Windows.Forms.Panel panel1;
        private BrightIdeasSoftware.ObjectListView olvFiles;
        private BrightIdeasSoftware.OLVColumn colFilename;
        private BrightIdeasSoftware.OLVColumn colFilesize;
        private BrightIdeasSoftware.OLVColumn colCreated;
        private BrightIdeasSoftware.OLVColumn colModified;
        private System.Windows.Forms.TreeView tvNavigation;
    }
}

