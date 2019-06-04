namespace Sentinel.Controls
{
    partial class TagReferenceControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.textBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.browseButton = new System.Windows.Forms.Button();
            this.openButton = new System.Windows.Forms.Button();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.renameReferencedTagToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBox
            // 
            this.textBox.Location = new System.Drawing.Point(309, 3);
            this.textBox.Name = "textBox";
            this.textBox.Size = new System.Drawing.Size(360, 20);
            this.textBox.TabIndex = 5;
            this.textBox.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            // 
            // label1
            // 
            this.label1.ContextMenuStrip = this.contextMenuStrip1;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(300, 26);
            this.label1.TabIndex = 6;
            this.label1.Text = "Name";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // browseButton
            // 
            this.browseButton.ContextMenuStrip = this.contextMenuStrip1;
            this.browseButton.Location = new System.Drawing.Point(675, 3);
            this.browseButton.Name = "browseButton";
            this.browseButton.Size = new System.Drawing.Size(24, 20);
            this.browseButton.TabIndex = 8;
            this.browseButton.Text = "...";
            this.browseButton.UseVisualStyleBackColor = true;
            this.browseButton.Click += new System.EventHandler(this.browseButton_Click);
            // 
            // openButton
            // 
            this.openButton.ContextMenuStrip = this.contextMenuStrip1;
            this.openButton.Location = new System.Drawing.Point(705, 3);
            this.openButton.Name = "openButton";
            this.openButton.Size = new System.Drawing.Size(24, 20);
            this.openButton.TabIndex = 9;
            this.openButton.Text = "->";
            this.openButton.UseVisualStyleBackColor = true;
            this.openButton.Click += new System.EventHandler(this.openButton_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.renameReferencedTagToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(206, 26);
            // 
            // renameReferencedTagToolStripMenuItem
            // 
            this.renameReferencedTagToolStripMenuItem.Name = "renameReferencedTagToolStripMenuItem";
            this.renameReferencedTagToolStripMenuItem.Size = new System.Drawing.Size(205, 22);
            this.renameReferencedTagToolStripMenuItem.Text = "&Rename referenced tag...";
            this.renameReferencedTagToolStripMenuItem.Click += new System.EventHandler(this.renameReferencedTag_Click);
            // 
            // TagReferenceControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.textBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.browseButton);
            this.Controls.Add(this.openButton);
            this.Name = "TagReferenceControl";
            this.Size = new System.Drawing.Size(733, 26);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button browseButton;
        private System.Windows.Forms.Button openButton;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem renameReferencedTagToolStripMenuItem;
    }
}
