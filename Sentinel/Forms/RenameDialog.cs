using TagTool.Cache;
using System;
using System.Windows.Forms;

namespace Sentinel.Forms
{
    public partial class RenameDialog : Form
    {
        public string Value { get => textBox1.Text; set => textBox1.Text = value; }

        public RenameDialog()
        {
            InitializeComponent();
        }

        public RenameDialog(string value)
        {
            InitializeComponent();
            Value = value;
        }

        public RenameDialog(HaloOnlineCacheContext cacheContext, CachedTagInstance tag)
        {
            if (tag == null)
                throw new NullReferenceException();

            InitializeComponent();

            Text = $"Rename {cacheContext.GetString(tag.Group.Name)} tag...";

            Value = tag.Name != null ?
                tag.Name :
                "";
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void onKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    okButton_Click(sender, null);
                    break;

                case Keys.Escape:
                    cancelButton_Click(sender, null);
                    break;
            }
        }
    }
}