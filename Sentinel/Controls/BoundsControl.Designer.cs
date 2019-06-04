namespace Sentinel.Controls
{
    partial class BoundsControl
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
            this.label3 = new System.Windows.Forms.Label();
            this.lowerTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.upperTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(412, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(21, 26);
            this.label3.TabIndex = 10;
            this.label3.Text = "to";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lowerTextBox
            // 
            this.lowerTextBox.Location = new System.Drawing.Point(306, 3);
            this.lowerTextBox.Name = "lowerTextBox";
            this.lowerTextBox.Size = new System.Drawing.Size(100, 20);
            this.lowerTextBox.TabIndex = 6;
            this.lowerTextBox.TextChanged += new System.EventHandler(this.valueTextBox_TextChanged);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(300, 26);
            this.label1.TabIndex = 7;
            this.label1.Text = "Name";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // upperTextBox
            // 
            this.upperTextBox.Location = new System.Drawing.Point(439, 3);
            this.upperTextBox.Name = "upperTextBox";
            this.upperTextBox.Size = new System.Drawing.Size(100, 20);
            this.upperTextBox.TabIndex = 8;
            this.upperTextBox.TextChanged += new System.EventHandler(this.valueTextBox_TextChanged);
            // 
            // BoundsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lowerTextBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.upperTextBox);
            this.Name = "BoundsControl";
            this.Size = new System.Drawing.Size(720, 26);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox lowerTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox upperTextBox;
    }
}
