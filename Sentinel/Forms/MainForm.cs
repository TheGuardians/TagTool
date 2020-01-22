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
using System.Xml;

namespace Sentinel.Forms
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            var recentFile = new FileInfo(Path.Combine(Application.StartupPath, "RecentFolders.xml"));

            if (!recentFile.Exists)
            {
                using (var writer = new StreamWriter(recentFile.Create()))
                {
                    writer.WriteLine("<folders>");
                    writer.WriteLine("</folders>");
                }
            }

            var recentXml = new XmlDocument();
            recentXml.Load(recentFile.FullName);

            foreach (XmlNode node in recentXml["folders"])
            {
                if (node.Name != "folder")
                    throw new FormatException();

                var folderItem = new ToolStripMenuItem(node.InnerText);
                folderItem.Click += folderItem_Click;
                recentToolStripMenuItem.DropDownItems.Add(folderItem);
            }

            recentToolStripMenuItem.Enabled = (recentToolStripMenuItem.DropDownItems.Count != 0);
        }

        private void folderItem_Click(object sender, EventArgs e)
        {
            var cacheForm = new CacheForm(new DirectoryInfo(((ToolStripMenuItem)sender).Text))
            {
                MdiParent = this
            };

            if (!cacheForm.IsDisposed)
                cacheForm.Show();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                fbd.Description = "Select the cache directory:";

                if (fbd.ShowDialog() != DialogResult.OK)
                    return;

                var found = false;

                foreach (ToolStripMenuItem item in recentToolStripMenuItem.DropDownItems)
                {
                    if (item.Text == fbd.SelectedPath)
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    var folderItem = new ToolStripMenuItem(fbd.SelectedPath);
                    folderItem.Click += folderItem_Click;
                    recentToolStripMenuItem.DropDownItems.Add(folderItem);

                    var recentFile = new FileInfo(Path.Combine(Application.StartupPath, "RecentFolders.xml"));

                    if (!recentFile.Exists)
                        recentFile.Create().Close();

                    using (var writer = new StreamWriter(recentFile.FullName))
                    {
                        writer.WriteLine("<folders>");

                        foreach (ToolStripMenuItem item in recentToolStripMenuItem.DropDownItems)
                            writer.WriteLine($"\t<folder>{item.Text}</folder>");

                        writer.WriteLine("</folders>");
                    }
                }

                recentToolStripMenuItem.Enabled = (recentToolStripMenuItem.DropDownItems.Count != 0);

                var directory = new DirectoryInfo(fbd.SelectedPath);

                var cacheForm = new CacheForm(directory)
                {
                    MdiParent = this
                };

                if (!cacheForm.IsDisposed)
                    cacheForm.Show();
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
