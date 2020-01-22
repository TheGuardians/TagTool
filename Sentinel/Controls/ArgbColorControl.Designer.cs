namespace Sentinel.Controls
{
    partial class ArgbColorControl
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
            this.redTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.greenTextBox = new System.Windows.Forms.TextBox();
            this.blueTextBox = new System.Windows.Forms.TextBox();
            this.colorButton = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.alphaTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // redTextBox
            // 
            this.redTextBox.Location = new System.Drawing.Point(463, 3);
            this.redTextBox.Name = "redTextBox";
            this.redTextBox.Size = new System.Drawing.Size(64, 20);
            this.redTextBox.TabIndex = 11;
            this.redTextBox.TextChanged += new System.EventHandler(this.valueTextBox_TextChanged);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(300, 26);
            this.label1.TabIndex = 12;
            this.label1.Text = "Name";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(421, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(36, 26);
            this.label3.TabIndex = 14;
            this.label3.Text = "Red";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(533, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(36, 26);
            this.label4.TabIndex = 15;
            this.label4.Text = "Green";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(645, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(36, 26);
            this.label5.TabIndex = 16;
            this.label5.Text = "Blue";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // greenTextBox
            // 
            this.greenTextBox.Location = new System.Drawing.Point(575, 3);
            this.greenTextBox.Name = "greenTextBox";
            this.greenTextBox.Size = new System.Drawing.Size(64, 20);
            this.greenTextBox.TabIndex = 17;
            this.greenTextBox.TextChanged += new System.EventHandler(this.valueTextBox_TextChanged);
            // 
            // blueTextBox
            // 
            this.blueTextBox.Location = new System.Drawing.Point(687, 3);
            this.blueTextBox.Name = "blueTextBox";
            this.blueTextBox.Size = new System.Drawing.Size(64, 20);
            this.blueTextBox.TabIndex = 18;
            this.blueTextBox.TextChanged += new System.EventHandler(this.valueTextBox_TextChanged);
            // 
            // colorButton
            // 
            this.colorButton.Location = new System.Drawing.Point(757, 3);
            this.colorButton.Name = "colorButton";
            this.colorButton.Size = new System.Drawing.Size(20, 20);
            this.colorButton.TabIndex = 19;
            this.colorButton.UseVisualStyleBackColor = true;
            this.colorButton.Click += new System.EventHandler(this.button1_Click);
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(309, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(36, 26);
            this.label6.TabIndex = 20;
            this.label6.Text = "Alpha";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // alphaTextBox
            // 
            this.alphaTextBox.Location = new System.Drawing.Point(351, 3);
            this.alphaTextBox.Name = "alphaTextBox";
            this.alphaTextBox.Size = new System.Drawing.Size(64, 20);
            this.alphaTextBox.TabIndex = 21;
            this.alphaTextBox.TextChanged += new System.EventHandler(this.valueTextBox_TextChanged);
            // 
            // ArgbColorControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.redTextBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.greenTextBox);
            this.Controls.Add(this.blueTextBox);
            this.Controls.Add(this.colorButton);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.alphaTextBox);
            this.Name = "ArgbColorControl";
            this.Size = new System.Drawing.Size(784, 26);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox redTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox greenTextBox;
        private System.Windows.Forms.TextBox blueTextBox;
        private System.Windows.Forms.Button colorButton;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox alphaTextBox;
    }
}
