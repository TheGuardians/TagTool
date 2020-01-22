namespace Sentinel.Controls
{
    partial class Rectangle2dControl
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
            this.topTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.leftTextBox = new System.Windows.Forms.TextBox();
            this.bottomTextBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.rightTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // topTextBox
            // 
            this.topTextBox.Location = new System.Drawing.Point(355, 3);
            this.topTextBox.Name = "topTextBox";
            this.topTextBox.Size = new System.Drawing.Size(64, 20);
            this.topTextBox.TabIndex = 10;
            this.topTextBox.TextChanged += new System.EventHandler(this.valueTextBox_TextChanged);
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
            this.label3.Size = new System.Drawing.Size(40, 26);
            this.label3.TabIndex = 13;
            this.label3.Text = "Top";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(425, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(40, 26);
            this.label4.TabIndex = 14;
            this.label4.Text = "Left";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(541, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(40, 26);
            this.label5.TabIndex = 15;
            this.label5.Text = "Bottom";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // leftTextBox
            // 
            this.leftTextBox.Location = new System.Drawing.Point(471, 3);
            this.leftTextBox.Name = "leftTextBox";
            this.leftTextBox.Size = new System.Drawing.Size(64, 20);
            this.leftTextBox.TabIndex = 16;
            this.leftTextBox.TextChanged += new System.EventHandler(this.valueTextBox_TextChanged);
            // 
            // bottomTextBox
            // 
            this.bottomTextBox.Location = new System.Drawing.Point(587, 3);
            this.bottomTextBox.Name = "bottomTextBox";
            this.bottomTextBox.Size = new System.Drawing.Size(64, 20);
            this.bottomTextBox.TabIndex = 17;
            this.bottomTextBox.TextChanged += new System.EventHandler(this.valueTextBox_TextChanged);
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(657, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(40, 26);
            this.label6.TabIndex = 18;
            this.label6.Text = "Right";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // rightTextBox
            // 
            this.rightTextBox.Location = new System.Drawing.Point(703, 3);
            this.rightTextBox.Name = "rightTextBox";
            this.rightTextBox.Size = new System.Drawing.Size(64, 20);
            this.rightTextBox.TabIndex = 19;
            this.rightTextBox.TextChanged += new System.EventHandler(this.valueTextBox_TextChanged);
            // 
            // Rectangle2dControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.topTextBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.leftTextBox);
            this.Controls.Add(this.bottomTextBox);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.rightTextBox);
            this.Name = "Rectangle2dControl";
            this.Size = new System.Drawing.Size(771, 26);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox topTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox leftTextBox;
        private System.Windows.Forms.TextBox bottomTextBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox rightTextBox;
    }
}
