namespace Sentinel.Controls
{
    partial class RealEulerAngles2dControl
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
            this.yawTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.pitchTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // yawTextBox
            // 
            this.yawTextBox.Location = new System.Drawing.Point(347, 3);
            this.yawTextBox.Name = "yawTextBox";
            this.yawTextBox.Size = new System.Drawing.Size(64, 20);
            this.yawTextBox.TabIndex = 7;
            this.yawTextBox.TextChanged += new System.EventHandler(this.valueTextBox_TextChanged);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(300, 26);
            this.label1.TabIndex = 8;
            this.label1.Text = "Name";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(309, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(32, 26);
            this.label3.TabIndex = 10;
            this.label3.Text = "Yaw";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(417, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(32, 26);
            this.label4.TabIndex = 11;
            this.label4.Text = "Pitch";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // pitchTextBox
            // 
            this.pitchTextBox.Location = new System.Drawing.Point(455, 3);
            this.pitchTextBox.Name = "pitchTextBox";
            this.pitchTextBox.Size = new System.Drawing.Size(64, 20);
            this.pitchTextBox.TabIndex = 12;
            this.pitchTextBox.TextChanged += new System.EventHandler(this.valueTextBox_TextChanged);
            // 
            // RealEulerAngles2dControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.yawTextBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.pitchTextBox);
            this.Name = "RealEulerAngles2dControl";
            this.Size = new System.Drawing.Size(720, 26);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox yawTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox pitchTextBox;
    }
}
