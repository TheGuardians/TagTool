using TagTool.Cache;
using TagTool.Common;
using Sentinel.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Sentinel.Forms
{
    public partial class TagDialog : Form
    {
        public GameCache Cache { get; }

        public CachedTag Value => tagTreeView.SelectedNode?.Tag as CachedTag ?? null;
        
        public TagDialog()
        {
            InitializeComponent();
        }

        public TagDialog(GameCache cache) :
            this()
        {
            Cache = cache;
        }

        protected override void OnLoad(EventArgs e)
        {
            LoadTagTreeView();
            base.OnLoad(e);
        }

        private void LoadTagTreeView(List<Tag> groupFilter = null, string nameFilter = null)
        {
            tagTreeView.Nodes.Clear();
            okButton.Enabled = false;

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

            foreach (var tag in Cache.TagCache.TagTable.Where(i => i != null && i.Name != null))
            {
                if (tag == null)
                    continue;

                if (groupFilter?.Where(groupTag => tag.IsInGroup(groupTag)).Count() == 0)
                    continue;

                if (nameFilter != null && !tag.Name.Contains(nameFilter))
                    continue;
                
                CreateTagTreeNode(tag, ref unnamed);
            }

            foreach (var tag in Cache.TagCache.TagTable.Where(i => i != null && i.Name == null))
            {
                if (groupFilter?.Where(groupTag => tag.IsInGroup(groupTag)).Count() == 0)
                    continue;

                CreateTagTreeNode(tag, ref unnamed);
            }

            if (unnamed.Nodes.Count > 0)
                tagTreeView.Nodes.Add(unnamed);
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

            var groupName = tag.Group.ToString();

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

        private void tagTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            okButton.Enabled = tagTreeView.SelectedNode?.Tag is CachedTag;
        }

        private void searchButton_Click(object sender, EventArgs e)
        {
            tagTreeView.SelectedNode = null;
            okButton.Enabled = false;
            
            LoadTagTreeView(nameFilter: searchTextBox.Text);
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}