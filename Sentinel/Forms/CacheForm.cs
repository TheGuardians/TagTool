using TagTool.Cache;
using TagTool.Serialization;
using TagTool.Tags;
using Sentinel.Controls;
using Sentinel.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using TagTool.Cache.HaloOnline;
using TagTool.Tags.Definitions;

namespace Sentinel.Forms
{
    public partial class CacheForm : Form
    {
        public GameCache Cache { get; }
        public static XmlDocument Documentation { get; } = new XmlDocument();

        public bool LoadingTag { get; private set; } = false;

        public Dictionary<int, object> CurrentTags { get; private set; } = new Dictionary<int, object>();
        public CachedTag CurrentTag { get; private set; } = null;

        public class TagInstanceItem
        {
            public GameCache Cache { get; set; }
            public CachedTag Tag { get; set; }

            public override string ToString()
            {
                if (Tag == null)
                    return "<null>";

                var tagName = Tag.Name != null ?
                    $"[0x{Tag.Index:X4}] {Tag.Name}" :
                    $"0x{Tag.Index:X4}";

                return $"{tagName}.{Cache.StringTable.GetString(Tag.Group.Name)}";
            }
        }

        public CacheForm()
        {
            InitializeComponent();
        }

        public CacheForm(DirectoryInfo cacheDirectory)
        {
            InitializeComponent();

            // TODO: add gen3 cache support (resources need work)
            Text = cacheDirectory.FullName;
            Cache = GameCache.Open(new FileInfo(Text + "\\tags.dat"));

            var docFile = new FileInfo(Path.Combine(Application.StartupPath, "BlamCore.xml"));

            if (Documentation.ChildNodes.Count == 0 && docFile.Exists)
                Documentation.Load(docFile.FullName);
        }
        
        public static string GetDocumentation(FieldInfo field)
        {
            var fieldName = $"{field.DeclaringType.FullName}.{field.Name}".Replace("+", ".");
            var documentationNode = Documentation.SelectSingleNode($"//member[starts-with(@name, 'F:{fieldName}')]");

            var result = documentationNode?.FirstChild.InnerText.Replace("\r\n", "").TrimStart().TrimEnd() ?? "";

            return result.Length == 0 ? "" : $"; {result}";
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            tagEditorPanel_SizeChanged(this, e);

            statusLabel.Text = "";

            progressBar.Style = ProgressBarStyle.Continuous;
            progressBar.MarqueeAnimationSpeed = 0;

            LoadTagTree();
        }

        private void LoadTagTree()
        {
            tagTreeView.Enabled = false;

            statusLabel.Text = $"Loading tags...";

            progressBar.Style = ProgressBarStyle.Marquee;
            progressBar.MarqueeAnimationSpeed = 30;

            tagTreeView.Nodes.Clear();

            var imageList = new ImageList();
            imageList.Images.Add("file", Resources.file);
            imageList.Images.Add("file_animation", Resources.file_animation);
            imageList.Images.Add("file_bitmap", Resources.file_bitmap);
            imageList.Images.Add("file_settings", Resources.file_settings);
            imageList.Images.Add("file_sound", Resources.file_sound);
            imageList.Images.Add("file_ui", Resources.file_ui);
            imageList.Images.Add("file_video", Resources.file_video);
            imageList.Images.Add("folder", Resources.folder);

            tagTreeView.ImageList = imageList;

            var unnamed = new TreeNode
            {
                Name = "<unnamed>",
                Text = "<unnamed>",
                ImageKey = "folder",
                SelectedImageKey = "folder"
            };

            foreach (var instance in Cache.TagCache.TagTable)
                if (instance != null)
                    CreateTagTreeNode(instance, ref unnamed);

            tagTreeView.Nodes.Add(unnamed);

            statusLabel.Text = "";

            progressBar.Style = ProgressBarStyle.Continuous;
            progressBar.MarqueeAnimationSpeed = 0;

            tagTreeView.Enabled = true;
        }

        private string GetTagTreeNodeImageKey(CachedTag tag)
        {
            if (tag.IsInGroup("cfgt") || tag.IsInGroup("matg") || tag.IsInGroup("mulg") ||
                tag.IsInGroup("aigl") || tag.IsInGroup("smdt") ||
                tag.IsInGroup("inpg") || tag.IsInGroup("rasg") ||
                tag.IsInGroup("wezr") || tag.IsInGroup("wgtz") || tag.IsInGroup("wigl"))
                return "file_settings";
            if (tag.IsInGroup("jmad"))
                return "file_animation";
            else if (tag.IsInGroup("bitm"))
                return "file_bitmap";
            else if (tag.IsInGroup("snd!") || tag.IsInGroup("lsnd"))
                return "file_sound";
            else
                return "file";
        }

        private TreeNode CreateTagTreeNode(CachedTag tag, ref TreeNode unnamed)
        {
            if (tag == null)
                return null;

            if (unnamed == null)
            {
                var nodes = tagTreeView.Nodes.Find("<unnamed>", false);

                if (nodes == null || nodes.Length == 0)
                {
                    unnamed = new TreeNode("<unnamed>")
                    {
                        Name = "<unnamed>",
                        ImageKey = "folder",
                        SelectedImageKey = "folder"
                    };
                }
                else
                {
                    unnamed = nodes[0];
                }
            }

            var groupName = Cache.StringTable.GetString(tag.Group.Name);

            if (tag.Name == null)
            {
                var nodes = unnamed.Nodes.Find(groupName, false);

                TreeNode groupNode = null;

                if (nodes == null || nodes.Length == 0)
                {
                    groupNode = unnamed.Nodes.Add(groupName);
                    groupNode.Name = groupName;
                    groupNode.ImageKey = groupNode.SelectedImageKey = "folder";
                }
                else
                {
                    groupNode = nodes[0];
                }

                var unnamedNode = groupNode.Nodes.Add($"0x{tag.Index:X4}");
                unnamedNode.Tag = tag;
                unnamedNode.ImageKey = unnamedNode.SelectedImageKey = GetTagTreeNodeImageKey(tag);

                return unnamedNode;
            }

            var tagName = tag.Name;

            if (!tagName.Contains('\\'))
            {
                var node = new TreeNode($"{tagName}.{groupName}")
                {
                    Name = $"0x{tag.Index}",
                    Tag = tag,
                    ImageKey = GetTagTreeNodeImageKey(tag),
                    SelectedImageKey = GetTagTreeNodeImageKey(tag)
                };

                tagTreeView.Nodes.Add(node);

                return node;
            }

            TreeNode root = null;
            TreeNode folder = null;
            var tokens = tagName.Split('\\').ToList();

            while (tokens.Count > 1)
            {
                var token = tokens[0];

                if (root == null)
                {
                    var nodes = tagTreeView.Nodes.Find(token, false);

                    if (nodes == null || nodes.Length == 0)
                    {
                        root = tagTreeView.Nodes.Add(token);
                        root.Name = token;
                        root.Tag = token + '\\';
                        root.ImageKey = root.SelectedImageKey = "folder";
                    }
                    else
                    {
                        root = nodes[0];
                    }

                    folder = root;
                }
                else
                {
                    var nodes = folder.Nodes.Find(token, false);

                    TreeNode node = null;

                    if (nodes == null || nodes.Length == 0)
                    {
                        node = folder.Nodes.Add(token);
                        node.Name = token;
                        node.Tag = $"{folder.Tag}{token}\\";
                        node.ImageKey = node.SelectedImageKey = "folder";
                    }
                    else
                    {
                        node = nodes[0];
                    }

                    folder = node;
                }

                tokens.RemoveAt(0);
            }

            var tagNode = folder.Nodes.Add($"{tokens[0]}.{groupName}");
            tagNode.Name = $"0x{tag.Index:X4}";
            tagNode.Tag = tag;
            tagNode.ImageKey = tagNode.SelectedImageKey = GetTagTreeNodeImageKey(tag);

            return tagNode;
        }

        public void LoadTagEditor(CachedTag tag)
        {
            if (tag == null || (CurrentTag != null && CurrentTag.Index == tag.Index))
                return;

            LoadingTag = true;

            object definition = null;

            if (CurrentTags.ContainsKey(tag.Index))
                definition = CurrentTags[tag.Index];

            tagTreeView.Enabled = false;

            tagEditorPanel.Controls.Clear();

            var tagName = tag.Name ?? $"0x{tag.Index:X4}";

            var groupName = Cache.StringTable.GetString(tag.Group.Name);

            statusLabel.Text = $"Loading {tagName}.{ groupName}...";

            progressBar.Style = ProgressBarStyle.Marquee;
            progressBar.MarqueeAnimationSpeed = 30;

            if (definition == null)
                using (var stream = Cache.TagCache.OpenTagCacheRead())
                    definition = Cache.Deserialize(stream, tag);

            if (tagName.Contains("\\"))
            {
                var index = tagName.LastIndexOf('\\') + 1;
                tagName = tagName.Substring(index, tagName.Length - index);
            }

            statusLabel.Text = $"Generating {groupName} interface...";
            Application.DoEvents();

            var point = new Point();

            if (tag.IsInGroup("matg") || tag.IsInGroup("mulg") || tag.IsInGroup("scnr") || tag.IsInGroup("sbsp"))
            {
                var control = new StructMultiControl(this, Cache, tag, definition)
                {
                    Dock = DockStyle.Fill
                };

                control.GetFieldValue(null, definition, definition);

                tagEditorPanel.Controls.Add(control);
            }
            // todo: fixup/clean model rendering code
            // todo: finish bitm editing (save/import dds, add size scaling to image)
            /*else if (tag.IsInGroup("bitm") || tag.IsInGroup("obje"))
            {
                var splitContainer = new SplitContainer
                {
                    Dock = DockStyle.Fill,
                    Orientation = Orientation.Horizontal
                };

                splitContainer.FixedPanel = FixedPanel.Panel1;

                tagEditorPanel.Controls.Add(splitContainer);
                splitContainer.BringToFront();

                if (tag.IsInGroup("bitm"))
                    splitContainer.SplitterDistance = Math.Min((short)512, Math.Max((short)16, ((TagTool.Tags.Definitions.Bitmap)definition).Images[0].Height));
                else if (tag.IsInGroup("obje"))
                    splitContainer.SplitterDistance = 384;

                if (tag.IsInGroup("bitm"))
                {
                    var bitmDefinition = (TagTool.Tags.Definitions.Bitmap)definition;

                    var bitmapControl = new BitmapControl(Cache, bitmDefinition)
                    {
                        Dock = DockStyle.Fill
                    };

                    splitContainer.Panel1.Controls.Add(bitmapControl);
                    bitmapControl.BringToFront();
                }

                else if (tag.IsInGroup("obje"))
                {
                    var modelControl = new ObjectControl(Cache, (GameObject)definition)
                    {
                        Dock = DockStyle.Fill
                    };

                    splitContainer.Panel1.Controls.Add(modelControl);
                    modelControl.BringToFront();
                }

                var control = tag.IsInGroup("obje") ?
                    (Control)new StructMultiControl(this, Cache, tag, definition) { Dock = DockStyle.Fill } :
                    new StructControl(this, Cache, definition.GetType(), null);

                ((IFieldControl)control).GetFieldValue(null, definition, definition);
                control.Location = point;

                splitContainer.Panel2.Controls.Add(control);
                splitContainer.Panel2.AutoScroll = true;
            }*/
            else
            {
                if (tag.IsInGroup("snd!"))
                {
                    var soundControl = new SoundControl(Cache, tag, (Sound)definition)
                    {
                        Dock = DockStyle.Top
                    };

                    tagEditorPanel.Controls.Add(soundControl);
                    soundControl.BringToFront();

                    point.Y = soundControl.Bottom;
                }

                var control = new StructControl(this, Cache, definition.GetType(), null);
                control.GetFieldValue(null, definition, definition);

                control.Location = point;

                tagEditorPanel.Controls.Add(control);
            }

            statusLabel.Text = "";

            progressBar.Style = ProgressBarStyle.Continuous;
            progressBar.MarqueeAnimationSpeed = 0;

            tagTreeView.Enabled = true;
            CurrentTag = tag;

            if (!CurrentTags.ContainsKey(tag.Index))
            {
                CurrentTags[tag.Index] = definition;

                var item = new TagInstanceItem { Cache = Cache, Tag = tag };
                currentTagsComboBox.Items.Add(item);

                currentTagsComboBox.SelectedItem = item;
            }
            else
            {
                for (var i = 0; i < currentTagsComboBox.Items.Count; i++)
                {
                    var item = (TagInstanceItem)currentTagsComboBox.Items[i];

                    if (item.Tag.Index == tag.Index)
                    {
                        currentTagsComboBox.SelectedIndex = i;
                        break;
                    }
                }
            }

            LoadingTag = false;
        }

        private void tagTreeView_DoubleClick(object sender, EventArgs e)
        {
            var nodeTag = tagTreeView.SelectedNode?.Tag;

            switch (nodeTag)
            {
                case CachedTag tag:
                    tagTreeView.ContextMenuStrip = tagNodeContextMenuStrip;
                    LoadTagEditor(tag);
                    break;

                case string path:
                    tagTreeView.ContextMenuStrip = folderContextMenuStrip;
                    break;

                default:
                    tagTreeView.ContextMenuStrip = null;
                    break;
            }
        }

        private void extractToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tagTreeView.SelectedNode?.Tag is CachedTag tag)
            {
                using (var sfd = new SaveFileDialog())
                {
                    var groupName = Cache.StringTable.GetString(tag.Group.Name);

                    sfd.Filter = $"{groupName} files (*.{groupName})|*.{groupName}";

                    if (sfd.ShowDialog() != DialogResult.OK)
                        return;

                    // needs to be updated for gen3 

                    GameCacheHaloOnline cacheHaloOnline = Cache as GameCacheHaloOnline;

                    byte[] data;

                    using (var stream = Cache.TagCache.OpenTagCacheRead())
                        data = cacheHaloOnline.TagCacheGenHO.ExtractTagRaw(stream, (CachedTagHaloOnline)tag);

                    using (var stream = File.Open(sfd.FileName, FileMode.Create, FileAccess.Write))
                        stream.Write(data, 0, data.Length);

                    MessageBox.Show($"Extracted {sfd.FileName} successfully.", "Extract Tag", MessageBoxButtons.OK);
                }
            }
        }

        private void importToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tagTreeView.SelectedNode?.Tag is CachedTag tag)
            {
                using (var ofd = new OpenFileDialog())
                {
                    var groupName = Cache.StringTable.GetString(tag.Group.Name);

                    ofd.Filter = $"{groupName} files (*.{groupName})|*.{groupName}";

                    if (ofd.ShowDialog() != DialogResult.OK)
                        return;

                    byte[] data;

                    using (var stream = File.OpenRead(ofd.FileName))
                    {
                        data = new byte[stream.Length];
                        stream.Read(data, 0, data.Length);
                    }

                    GameCacheHaloOnline cacheHaloOnline = Cache as GameCacheHaloOnline;

                    using (var stream = Cache.TagCache.OpenTagCacheReadWrite())
                        cacheHaloOnline.TagCacheGenHO.SetTagDataRaw(stream, (CachedTagHaloOnline)tag, data);

                    MessageBox.Show($"Imported {ofd.FileName} successfully.", "Import Tag", MessageBoxButtons.OK);
                }
            }
        }

        public DialogResult RenameTag(CachedTag tag)
        {
            var result = DialogResult.OK;

            using (var rd = new RenameDialog(Cache, tag))
            {
                if ((result = rd.ShowDialog()) != DialogResult.OK)
                    return result;

                if (rd.Value == "" && tag.Name != null)
                    tag.Name = null;
                else if (rd.Value != "")
                    tag.Name = rd.Value;
            }

            LoadTagTree();

            return result;
        }

        private void renameTag_Click(object sender, EventArgs e)
        {
            if (tagTreeView.SelectedNode?.Tag is CachedTag tag)
            {
                using (var rd = new RenameDialog(Cache, tag))
                {
                    if (rd.ShowDialog() != DialogResult.OK)
                        return;

                    if (rd.Value == "" && tag.Name != null)
                        tag.Name = null;
                    else if (rd.Value != "")
                        tag.Name = rd.Value;
                }

                tagTreeView.SelectedNode.Remove();

                var unnamed = new TreeNode();
                var newNode = CreateTagTreeNode(tag, ref unnamed);

                tagTreeView.Sort();
            }
        }

        private void renameFolder_Click(object sender, EventArgs e)
        {
            if (tagTreeView.SelectedNode?.Tag is string path)
            {
                using (var rd = new RenameDialog(path))
                {
                    if (rd.ShowDialog() != DialogResult.OK)
                        return;

                    if (rd.Value == "")
                        throw new NotImplementedException();

                    if (!rd.Value.EndsWith("\\"))
                        rd.Value += '\\';

                    var tagNames = new Dictionary<int, string>();

                    foreach (var entry in Cache.TagCache.TagTable)
                    {
                        if (entry == null || entry.Name == null || !entry.Name.StartsWith(path))
                            continue;
                        
                        entry.Name = $"{rd.Value}{entry.Name.Substring(path.Length, entry.Name.Length - path.Length)}";
                    }

                    tagTreeView.SelectedNode.Remove();

                    TreeNode unnamed = tagTreeView.Nodes.Find("<unnamed>", false)[0];

                    foreach (var instance in Cache.TagCache.TagTable)
                        if (instance != null && instance.Name == null)
                            CreateTagTreeNode(instance, ref unnamed);
                }
            }
        }

        private void saveTagNamesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GameCacheHaloOnline cacheHaloOnline = Cache as GameCacheHaloOnline;

            cacheHaloOnline.TagCacheGenHO.SaveTagNames();
            MessageBox.Show("Saved tag names successfully.", "Save Tag Names", MessageBoxButtons.OK);
        }

        private void tagEditorPanel_SizeChanged(object sender, EventArgs e)
        {
            currentTagsComboBox.Width = tagEditorPanel.Width - currentTagsLabel.Width - editorToolsDropDownButton.Width - 6;
        }

        private void CacheForm_SizeChanged(object sender, EventArgs e)
        {
            tagEditorPanel_SizeChanged(sender, e);
        }

        private void toolStripComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (LoadingTag)
                return;

            LoadTagEditor(((TagInstanceItem)currentTagsComboBox.SelectedItem).Tag);
        }

        private string SaveTagChanges(CachedTag tag, object definition)
        {
            using (var stream = Cache.TagCache.OpenTagCacheReadWrite())
                Cache.Serialize(stream, tag, definition);

            var tagName = tag.Name ?? $"0x{tag.Index:X4}";

            if (tagName.Contains('\\'))
            {
                var index = tagName.LastIndexOf('\\') + 1;
                tagName = tagName.Substring(index, tagName.Length - index);
            }

            return $"{tagName}.{ Cache.StringTable.GetString(CurrentTag.Group.Name)}";
        }

        private void saveCurrentTagToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CurrentTag == null || !CurrentTags.ContainsKey(CurrentTag.Index))
                return;

            var tagName = SaveTagChanges(CurrentTag, CurrentTags[CurrentTag.Index]);

            MessageBox.Show($"Saved changes to {tagName} successfully.", "Save Tag Changes", MessageBoxButtons.OK);
        }

        private void saveAllTagsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var message = "Saved changes to the following tags successfully:\n";

            foreach (var entry in CurrentTags)
            {
                Cache.TryGetTag("0x" + entry.Key.ToString("X"), out var tag); // hacky, works for now
                message += $"\n{SaveTagChanges(tag, entry.Value)}";
            }

            MessageBox.Show(message, "Save Tag Changes", MessageBoxButtons.OK);
        }

        private void closeCurrentTagToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CurrentTag == null)
                return;

            tagEditorPanel.Controls.Clear();

            if (currentTagsComboBox.Items.Count > 0)
                currentTagsComboBox.Items.RemoveAt(currentTagsComboBox.SelectedIndex);

            if (!CurrentTags.ContainsKey(CurrentTag.Index))
                return;

            CurrentTags.Remove(CurrentTag.Index);
        }

        private void closeAllTagsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            currentTagsComboBox.Items.Clear();
            tagEditorPanel.Controls.Clear();
            CurrentTags.Clear();
            CurrentTag = null;
        }
    }
}