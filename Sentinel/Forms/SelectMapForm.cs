using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sentinel.Forms
{
    public partial class SelectMapForm : Form
    {
        public FileInfo SelectedFile => (comboBox1.SelectedItem as MapItem)?.File ?? null;

        public SelectMapForm()
        {
            InitializeComponent();
        }

        public SelectMapForm(DirectoryInfo directory) :
            this()
        {
            foreach (var file in directory?.GetFiles("*.map"))
                comboBox1.Items.Add(new MapItem { File = file });

            if (comboBox1.Items.Count == 0)
                comboBox1.Enabled = false;
            else
                comboBox1.SelectedIndex = 0;
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private class MapItem
        {
            public FileInfo File { get; set; }

            public override string ToString() => File.Name;
        }
    }
}
