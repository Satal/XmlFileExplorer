namespace XmlFileExplorer.Forms
{
    partial class FolderExplorer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FolderExplorer));
            this.tvNavigation = new System.Windows.Forms.TreeView();
            this.imgLstIcons = new System.Windows.Forms.ImageList(this.components);
            this.SuspendLayout();
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
            this.tvNavigation.Size = new System.Drawing.Size(284, 261);
            this.tvNavigation.TabIndex = 2;
            this.tvNavigation.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.tvNavigation_BeforeExpand);
            this.tvNavigation.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvNavigation_AfterSelect);
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
            // FolderExplorer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.tvNavigation);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "FolderExplorer";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Folder Explorer";
            this.Shown += new System.EventHandler(this.FolderExplorer_Shown);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView tvNavigation;
        private System.Windows.Forms.ImageList imgLstIcons;
    }
}