namespace XmlFileExplorer.Forms
{
    partial class ErrorsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ErrorsForm));
            this.olvValidationErrors = new BrightIdeasSoftware.ObjectListView();
            this.colSeverity = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.colError = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.imgLstIcons = new System.Windows.Forms.ImageList(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.olvValidationErrors)).BeginInit();
            this.SuspendLayout();
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
            this.olvValidationErrors.Size = new System.Drawing.Size(284, 261);
            this.olvValidationErrors.SmallImageList = this.imgLstIcons;
            this.olvValidationErrors.TabIndex = 5;
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
            // ErrorsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.olvValidationErrors);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "ErrorsForm";
            this.Text = "Errors/Warnings";
            ((System.ComponentModel.ISupportInitialize)(this.olvValidationErrors)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private BrightIdeasSoftware.ObjectListView olvValidationErrors;
        private BrightIdeasSoftware.OLVColumn colSeverity;
        private BrightIdeasSoftware.OLVColumn colError;
        private System.Windows.Forms.ImageList imgLstIcons;
    }
}