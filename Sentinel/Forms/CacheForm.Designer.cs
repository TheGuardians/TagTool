namespace Sentinel.Forms
{
    partial class CacheForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CacheForm));
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.tagTreeView = new System.Windows.Forms.TreeView();
            this.tagEditorPanel = new System.Windows.Forms.Panel();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.cacheToolsDropDownButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.saveTagNamesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editorToolsDropDownButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.saveCurrentTagToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAllTagsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeCurrentTagToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeAllTagsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.currentTagsComboBox = new System.Windows.Forms.ToolStripComboBox();
            this.currentTagsLabel = new System.Windows.Forms.ToolStripLabel();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.progressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.tagNodeContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.extractToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.renameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.folderContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.renameToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.toolStrip.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.tagNodeContextMenuStrip.SuspendLayout();
            this.folderContextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer.Location = new System.Drawing.Point(0, 25);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.tagTreeView);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.tagEditorPanel);
            this.splitContainer.Size = new System.Drawing.Size(1191, 535);
            this.splitContainer.SplitterDistance = 300;
            this.splitContainer.TabIndex = 0;
            // 
            // tagTreeView
            // 
            this.tagTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tagTreeView.Location = new System.Drawing.Point(0, 0);
            this.tagTreeView.Name = "tagTreeView";
            this.tagTreeView.Size = new System.Drawing.Size(300, 535);
            this.tagTreeView.TabIndex = 1;
            this.tagTreeView.DoubleClick += new System.EventHandler(this.tagTreeView_DoubleClick);
            // 
            // tagEditorPanel
            // 
            this.tagEditorPanel.AutoScroll = true;
            this.tagEditorPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tagEditorPanel.Location = new System.Drawing.Point(0, 0);
            this.tagEditorPanel.Name = "tagEditorPanel";
            this.tagEditorPanel.Size = new System.Drawing.Size(887, 535);
            this.tagEditorPanel.TabIndex = 0;
            this.tagEditorPanel.SizeChanged += new System.EventHandler(this.tagEditorPanel_SizeChanged);
            // 
            // toolStrip
            // 
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cacheToolsDropDownButton,
            this.editorToolsDropDownButton,
            this.currentTagsComboBox,
            this.currentTagsLabel});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(1191, 25);
            this.toolStrip.TabIndex = 0;
            this.toolStrip.Text = "toolStrip1";
            // 
            // cacheToolsDropDownButton
            // 
            this.cacheToolsDropDownButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.cacheToolsDropDownButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveTagNamesToolStripMenuItem});
            this.cacheToolsDropDownButton.Image = ((System.Drawing.Image)(resources.GetObject("cacheToolsDropDownButton.Image")));
            this.cacheToolsDropDownButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.cacheToolsDropDownButton.Name = "cacheToolsDropDownButton";
            this.cacheToolsDropDownButton.Size = new System.Drawing.Size(84, 22);
            this.cacheToolsDropDownButton.Text = "&Cache Tools";
            this.cacheToolsDropDownButton.ToolTipText = "Tools related to cache file interop.";
            // 
            // saveTagNamesToolStripMenuItem
            // 
            this.saveTagNamesToolStripMenuItem.Name = "saveTagNamesToolStripMenuItem";
            this.saveTagNamesToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.saveTagNamesToolStripMenuItem.Text = "Save Tag &Names";
            this.saveTagNamesToolStripMenuItem.Click += new System.EventHandler(this.saveTagNamesToolStripMenuItem_Click);
            // 
            // editorToolsDropDownButton
            // 
            this.editorToolsDropDownButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.editorToolsDropDownButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.editorToolsDropDownButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveCurrentTagToolStripMenuItem,
            this.saveAllTagsToolStripMenuItem,
            this.closeCurrentTagToolStripMenuItem,
            this.closeAllTagsToolStripMenuItem});
            this.editorToolsDropDownButton.Image = ((System.Drawing.Image)(resources.GetObject("editorToolsDropDownButton.Image")));
            this.editorToolsDropDownButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.editorToolsDropDownButton.Name = "editorToolsDropDownButton";
            this.editorToolsDropDownButton.Size = new System.Drawing.Size(82, 22);
            this.editorToolsDropDownButton.Text = "Editor Tools";
            this.editorToolsDropDownButton.ToolTipText = "Tools related to tag editor interop.";
            // 
            // saveCurrentTagToolStripMenuItem
            // 
            this.saveCurrentTagToolStripMenuItem.Name = "saveCurrentTagToolStripMenuItem";
            this.saveCurrentTagToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.saveCurrentTagToolStripMenuItem.Text = "&Save Current Tag";
            this.saveCurrentTagToolStripMenuItem.Click += new System.EventHandler(this.saveCurrentTagToolStripMenuItem_Click);
            // 
            // saveAllTagsToolStripMenuItem
            // 
            this.saveAllTagsToolStripMenuItem.Name = "saveAllTagsToolStripMenuItem";
            this.saveAllTagsToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.saveAllTagsToolStripMenuItem.Text = "&Save All Tags";
            this.saveAllTagsToolStripMenuItem.Click += new System.EventHandler(this.saveAllTagsToolStripMenuItem_Click);
            // 
            // closeCurrentTagToolStripMenuItem
            // 
            this.closeCurrentTagToolStripMenuItem.Name = "closeCurrentTagToolStripMenuItem";
            this.closeCurrentTagToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.closeCurrentTagToolStripMenuItem.Text = "&Close Current Tag";
            this.closeCurrentTagToolStripMenuItem.Click += new System.EventHandler(this.closeCurrentTagToolStripMenuItem_Click);
            // 
            // closeAllTagsToolStripMenuItem
            // 
            this.closeAllTagsToolStripMenuItem.Name = "closeAllTagsToolStripMenuItem";
            this.closeAllTagsToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.closeAllTagsToolStripMenuItem.Text = "&Close All Tags";
            this.closeAllTagsToolStripMenuItem.Click += new System.EventHandler(this.closeAllTagsToolStripMenuItem_Click);
            // 
            // currentTagsComboBox
            // 
            this.currentTagsComboBox.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.currentTagsComboBox.AutoSize = false;
            this.currentTagsComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.currentTagsComboBox.Name = "currentTagsComboBox";
            this.currentTagsComboBox.Size = new System.Drawing.Size(300, 23);
            this.currentTagsComboBox.SelectedIndexChanged += new System.EventHandler(this.toolStripComboBox1_SelectedIndexChanged);
            // 
            // currentTagsLabel
            // 
            this.currentTagsLabel.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.currentTagsLabel.Enabled = false;
            this.currentTagsLabel.Name = "currentTagsLabel";
            this.currentTagsLabel.Size = new System.Drawing.Size(82, 22);
            this.currentTagsLabel.Text = "Current Tag(s)";
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel,
            this.progressBar});
            this.statusStrip.Location = new System.Drawing.Point(0, 560);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(1191, 22);
            this.statusStrip.TabIndex = 1;
            this.statusStrip.Text = "statusStrip1";
            // 
            // statusLabel
            // 
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(1074, 17);
            this.statusLabel.Spring = true;
            this.statusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // progressBar
            // 
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(100, 16);
            this.progressBar.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            // 
            // tagNodeContextMenuStrip
            // 
            this.tagNodeContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.extractToolStripMenuItem,
            this.importToolStripMenuItem,
            this.renameToolStripMenuItem});
            this.tagNodeContextMenuStrip.Name = "contextMenuStrip1";
            this.tagNodeContextMenuStrip.Size = new System.Drawing.Size(127, 70);
            // 
            // extractToolStripMenuItem
            // 
            this.extractToolStripMenuItem.Name = "extractToolStripMenuItem";
            this.extractToolStripMenuItem.Size = new System.Drawing.Size(126, 22);
            this.extractToolStripMenuItem.Text = "&Extract...";
            this.extractToolStripMenuItem.Click += new System.EventHandler(this.extractToolStripMenuItem_Click);
            // 
            // importToolStripMenuItem
            // 
            this.importToolStripMenuItem.Name = "importToolStripMenuItem";
            this.importToolStripMenuItem.Size = new System.Drawing.Size(126, 22);
            this.importToolStripMenuItem.Text = "&Import...";
            this.importToolStripMenuItem.Click += new System.EventHandler(this.importToolStripMenuItem_Click);
            // 
            // renameToolStripMenuItem
            // 
            this.renameToolStripMenuItem.Name = "renameToolStripMenuItem";
            this.renameToolStripMenuItem.Size = new System.Drawing.Size(126, 22);
            this.renameToolStripMenuItem.Text = "&Rename...";
            this.renameToolStripMenuItem.Click += new System.EventHandler(this.renameTag_Click);
            // 
            // folderContextMenuStrip
            // 
            this.folderContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.renameToolStripMenuItem1});
            this.folderContextMenuStrip.Name = "folderContextMenuStrip";
            this.folderContextMenuStrip.Size = new System.Drawing.Size(127, 26);
            // 
            // renameToolStripMenuItem1
            // 
            this.renameToolStripMenuItem1.Name = "renameToolStripMenuItem1";
            this.renameToolStripMenuItem1.Size = new System.Drawing.Size(126, 22);
            this.renameToolStripMenuItem1.Text = "Rename...";
            this.renameToolStripMenuItem1.Click += new System.EventHandler(this.renameFolder_Click);
            // 
            // CacheForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1191, 582);
            this.Controls.Add(this.splitContainer);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.toolStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "CacheForm";
            this.Text = "CacheForm";
            this.SizeChanged += new System.EventHandler(this.CacheForm_SizeChanged);
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.tagNodeContextMenuStrip.ResumeLayout(false);
            this.folderContextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.TreeView tagTreeView;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ContextMenuStrip tagNodeContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem extractToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem renameToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip folderContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem renameToolStripMenuItem1;
        private System.Windows.Forms.ToolStripDropDownButton cacheToolsDropDownButton;
        private System.Windows.Forms.ToolStripMenuItem saveTagNamesToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel;
        private System.Windows.Forms.ToolStripProgressBar progressBar;
        private System.Windows.Forms.Panel tagEditorPanel;
        private System.Windows.Forms.ToolStripComboBox currentTagsComboBox;
        private System.Windows.Forms.ToolStripDropDownButton editorToolsDropDownButton;
        private System.Windows.Forms.ToolStripLabel currentTagsLabel;
        private System.Windows.Forms.ToolStripMenuItem saveCurrentTagToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAllTagsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeCurrentTagToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeAllTagsToolStripMenuItem;
    }
}