namespace Sentinel.Controls
{
    partial class RealPlane3dControl
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
            this.iTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.jTextBox = new System.Windows.Forms.TextBox();
            this.kTextBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.dTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // iTextBox
            // 
            this.iTextBox.Location = new System.Drawing.Point(335, 3);
            this.iTextBox.Name = "iTextBox";
            this.iTextBox.Size = new System.Drawing.Size(64, 20);
            this.iTextBox.TabIndex = 10;
            this.iTextBox.TextChanged += new System.EventHandler(this.valueTextBox_TextChanged);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(300, 26);
            this.label1.TabIndex = 11;
            this.label1.Text = "Name";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(309, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(20, 26);
            this.label3.TabIndex = 13;
            this.label3.Text = "I";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(405, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(20, 26);
            this.label4.TabIndex = 14;
            this.label4.Text = "J";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(501, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(20, 26);
            this.label5.TabIndex = 15;
            this.label5.Text = "K";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // jTextBox
            // 
            this.jTextBox.Location = new System.Drawing.Point(431, 3);
            this.jTextBox.Name = "jTextBox";
            this.jTextBox.Size = new System.Drawing.Size(64, 20);
            this.jTextBox.TabIndex = 16;
            this.jTextBox.TextChanged += new System.EventHandler(this.valueTextBox_TextChanged);
            // 
            // kTextBox
            // 
            this.kTextBox.Location = new System.Drawing.Point(527, 3);
            this.kTextBox.Name = "kTextBox";
            this.kTextBox.Size = new System.Drawing.Size(64, 20);
            this.kTextBox.TabIndex = 17;
            this.kTextBox.TextChanged += new System.EventHandler(this.valueTextBox_TextChanged);
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(597, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(20, 26);
            this.label6.TabIndex = 18;
            this.label6.Text = "D";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dTextBox
            // 
            this.dTextBox.Location = new System.Drawing.Point(623, 3);
            this.dTextBox.Name = "dTextBox";
            this.dTextBox.Size = new System.Drawing.Size(64, 20);
            this.dTextBox.TabIndex = 19;
            this.dTextBox.TextChanged += new System.EventHandler(this.valueTextBox_TextChanged);
            // 
            // RealPlane3dControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.iTextBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.jTextBox);
            this.Controls.Add(this.kTextBox);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.dTextBox);
            this.Name = "RealPlane3dControl";
            this.Size = new System.Drawing.Size(720, 26);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox iTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox jTextBox;
        private System.Windows.Forms.TextBox kTextBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox dTextBox;
    }
}
