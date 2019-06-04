namespace Sentinel.Controls
{
    partial class RealPoint3dControl
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
            this.xTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.yTextBox = new System.Windows.Forms.TextBox();
            this.zTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // xTextBox
            // 
            this.xTextBox.Location = new System.Drawing.Point(335, 3);
            this.xTextBox.Name = "xTextBox";
            this.xTextBox.Size = new System.Drawing.Size(64, 20);
            this.xTextBox.TabIndex = 8;
            this.xTextBox.TextChanged += new System.EventHandler(this.valueTextBox_TextChanged);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(300, 26);
            this.label1.TabIndex = 9;
            this.label1.Text = "Name";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(309, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(20, 26);
            this.label3.TabIndex = 11;
            this.label3.Text = "X";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(405, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(20, 26);
            this.label4.TabIndex = 12;
            this.label4.Text = "Y";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(501, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(20, 26);
            this.label5.TabIndex = 13;
            this.label5.Text = "Z";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // yTextBox
            // 
            this.yTextBox.Location = new System.Drawing.Point(431, 3);
            this.yTextBox.Name = "yTextBox";
            this.yTextBox.Size = new System.Drawing.Size(64, 20);
            this.yTextBox.TabIndex = 14;
            this.yTextBox.TextChanged += new System.EventHandler(this.valueTextBox_TextChanged);
            // 
            // zTextBox
            // 
            this.zTextBox.Location = new System.Drawing.Point(527, 3);
            this.zTextBox.Name = "zTextBox";
            this.zTextBox.Size = new System.Drawing.Size(64, 20);
            this.zTextBox.TabIndex = 15;
            this.zTextBox.TextChanged += new System.EventHandler(this.valueTextBox_TextChanged);
            // 
            // RealPoint3dControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.xTextBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.yTextBox);
            this.Controls.Add(this.zTextBox);
            this.Name = "RealPoint3dControl";
            this.Size = new System.Drawing.Size(720, 26);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox xTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox yTextBox;
        private System.Windows.Forms.TextBox zTextBox;
    }
}
