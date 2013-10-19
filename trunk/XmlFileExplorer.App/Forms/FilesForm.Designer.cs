namespace XmlFileExplorer.Forms
{
    partial class FilesForm
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
            this.olvFiles = new BrightIdeasSoftware.ObjectListView();
            this.colFilename = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.colFilesize = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.colCreated = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.colModified = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.colFileType = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            ((System.ComponentModel.ISupportInitialize)(this.olvFiles)).BeginInit();
            this.SuspendLayout();
            // 
            // olvFiles
            // 
            this.olvFiles.AllColumns.Add(this.colFilename);
            this.olvFiles.AllColumns.Add(this.colFilesize);
            this.olvFiles.AllColumns.Add(this.colCreated);
            this.olvFiles.AllColumns.Add(this.colModified);
            this.olvFiles.AllColumns.Add(this.colFileType);
            this.olvFiles.AllowDrop = true;
            this.olvFiles.CellEditActivation = BrightIdeasSoftware.ObjectListView.CellEditActivateMode.F2Only;
            this.olvFiles.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colFilename,
            this.colFilesize,
            this.colCreated,
            this.colModified,
            this.colFileType});
            this.olvFiles.CopySelectionOnControlC = false;
            this.olvFiles.CopySelectionOnControlCUsesDragSource = false;
            this.olvFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.olvFiles.FullRowSelect = true;
            this.olvFiles.HasCollapsibleGroups = false;
            this.olvFiles.HideSelection = false;
            this.olvFiles.IsSimpleDragSource = true;
            this.olvFiles.IsSimpleDropSink = true;
            this.olvFiles.LabelEdit = true;
            this.olvFiles.Location = new System.Drawing.Point(0, 0);
            this.olvFiles.Name = "olvFiles";
            this.olvFiles.ShowGroups = false;
            this.olvFiles.Size = new System.Drawing.Size(284, 261);
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
            // colFileType
            // 
            this.colFileType.CellPadding = null;
            this.colFileType.Text = "File Type";
            // 
            // FilesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.olvFiles);
            this.DockAreas = WeifenLuo.WinFormsUI.Docking.DockAreas.Document;
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FilesForm";
            this.Text = "FilesForm";
            ((System.ComponentModel.ISupportInitialize)(this.olvFiles)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private BrightIdeasSoftware.ObjectListView olvFiles;
        private BrightIdeasSoftware.OLVColumn colFilename;
        private BrightIdeasSoftware.OLVColumn colFilesize;
        private BrightIdeasSoftware.OLVColumn colCreated;
        private BrightIdeasSoftware.OLVColumn colModified;
        private BrightIdeasSoftware.OLVColumn colFileType;
    }
}