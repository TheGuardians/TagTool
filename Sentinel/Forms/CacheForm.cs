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
using TagTool.Tags.Definitions;
using TagTool.Tags.Resources;
using TagTool.Geometry;
using TagTool.IO;

namespace Sentinel.Forms
{
    public partial class CacheForm : Form
    {
        public HaloOnlineCacheContext CacheContext { get; }
        public static XmlDocument Documentation { get; } = new XmlDocument();

        public bool LoadingTag { get; private set; } = false;

        public Dictionary<int, object> CurrentTags { get; private set; } = new Dictionary<int, object>();
        public CachedTagInstance CurrentTag { get; private set; } = null;

        public class TagInstanceItem
        {
            public HaloOnlineCacheContext CacheContext { get; set; }
            public CachedTagInstance Tag { get; set; }

            public override string ToString()
            {
                if (Tag == null)
                    return "<null>";

                var tagName = Tag.Name != null ?
                    $"[0x{Tag.Index:X4}] {Tag.Name}" :
                    $"0x{Tag.Index:X4}";

                return $"{tagName}.{CacheContext.GetString(Tag.Group.Name)}";
            }
        }

        public CacheForm()
        {
            InitializeComponent();
        }

        public CacheForm(DirectoryInfo cacheDirectory)
        {
            InitializeComponent();
            CacheContext = new HaloOnlineCacheContext(cacheDirectory);
            Text = cacheDirectory.FullName;

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

            foreach (var instance in CacheContext.TagCache.Index)
                if (instance != null)
                    CreateTagTreeNode(instance, ref unnamed);

            tagTreeView.Nodes.Add(unnamed);

            statusLabel.Text = "";

            progressBar.Style = ProgressBarStyle.Continuous;
            progressBar.MarqueeAnimationSpeed = 0;

            tagTreeView.Enabled = true;
        }

        private string GetTagTreeNodeImageKey(CachedTagInstance tag)
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

        private TreeNode CreateTagTreeNode(CachedTagInstance tag, ref TreeNode unnamed)
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

            var groupName = CacheContext.GetString(tag.Group.Name);

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

        public void LoadTagEditor(CachedTagInstance tag)
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

            var groupName = CacheContext.GetString(tag.Group.Name);

            statusLabel.Text = $"Loading {tagName}.{ groupName}...";

            progressBar.Style = ProgressBarStyle.Marquee;
            progressBar.MarqueeAnimationSpeed = 30;

            if (definition == null)
                using (var stream = CacheContext.OpenTagCacheRead())
                {
                    definition = CacheContext.Deserializer.Deserialize(
                        new TagSerializationContext(stream, CacheContext, tag),
                        TagDefinition.Find(tag.Group.Tag));

                    if (definition is Bink bink)
                        LoadBinkResource(bink);
                    else if (definition is TagTool.Tags.Definitions.Bitmap bitm)
                        LoadBitmapResource(bitm);
                    else if (definition is ModelAnimationGraph jmad)
                        LoadModelAnimationGraphResources(jmad);
                    else if (definition is ParticleModel prtm)
                        LoadParticleModelResource(prtm);
                    else if (definition is RenderModel mode)
                        LoadRenderModelResource(mode);
                    else if (definition is ScenarioStructureBsp sbsp)
                        LoadScenarioStructureBspResources(sbsp);
                    else if (definition is ScenarioLightmapBspData Lbsp)
                        LoadScenarioLightmapBspDataResources(Lbsp);
                    else if (definition is Sound snd)
                        LoadSoundResource(snd);

                }

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
                var control = new StructMultiControl(this, CacheContext, tag, definition)
                {
                    Dock = DockStyle.Fill
                };

                control.GetFieldValue(null, definition, definition);

                tagEditorPanel.Controls.Add(control);
            }
            else if (tag.IsInGroup("bitm") || tag.IsInGroup("obje"))
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

                    var bitmapControl = new BitmapControl(CacheContext, bitmDefinition)
                    {
                        Dock = DockStyle.Fill
                    };

                    splitContainer.Panel1.Controls.Add(bitmapControl);
                    bitmapControl.BringToFront();
                }
                else if (tag.IsInGroup("obje"))
                {
                    var modelControl = new ObjectControl(CacheContext, (GameObject)definition)
                    {
                        Dock = DockStyle.Fill
                    };

                    splitContainer.Panel1.Controls.Add(modelControl);
                    modelControl.BringToFront();
                }

                var control = tag.IsInGroup("obje") ?
                    (Control)new StructMultiControl(this, CacheContext, tag, definition) { Dock = DockStyle.Fill } :
                    new StructControl(this, CacheContext, definition.GetType(), null);

                ((IFieldControl)control).GetFieldValue(null, definition, definition);
                control.Location = point;

                splitContainer.Panel2.Controls.Add(control);
                splitContainer.Panel2.AutoScroll = true;
            }
            else
            {
                if (tag.IsInGroup("snd!"))
                {
                    var soundControl = new SoundControl(CacheContext, tag, (TagTool.Tags.Definitions.Sound)definition)
                    {
                        Dock = DockStyle.Top
                    };

                    tagEditorPanel.Controls.Add(soundControl);
                    soundControl.BringToFront();

                    point.Y = soundControl.Bottom;
                }

                var control = new StructControl(this, CacheContext, definition.GetType(), null);
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

                var item = new TagInstanceItem { CacheContext = CacheContext, Tag = tag };
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

        private void LoadSoundResource(Sound snd)
        {
            var location = snd.Resource.GetLocation();
            if (location == TagTool.Common.ResourceLocation.None)
                return;

            using (var resourceStream = new MemoryStream())
            {
                CacheContext.ExtractResource(snd.Resource, resourceStream);

                var definition = snd.Resource.Definition =
                    CacheContext.Deserialize<SoundResourceDefinition>(snd.Resource);

                definition.Data.Data = new byte[definition.Data.Size];
                resourceStream.Position = definition.Data.Address.Offset;
                resourceStream.Read(definition.Data.Data, 0, definition.Data.Size);
            }
        }

        private void LoadRenderGeometryResource(RenderGeometry geometry)
        {
            var location = geometry.Resource.GetLocation();
            if (location == TagTool.Common.ResourceLocation.None)
                return;

            using (var edResourceStream = new MemoryStream())
            using (var edResourceReader = new EndianReader(edResourceStream, EndianFormat.LittleEndian))
            {
                CacheContext.ExtractResource(geometry.Resource, edResourceStream);

                var definition = geometry.Resource.Definition =
                    CacheContext.Deserialize<RenderGeometryApiResourceDefinition>(geometry.Resource);

                for (var i = 0; i < definition.VertexBuffers.Count; i++)
                {
                    definition.VertexBuffers[i].Definition.Data.Data = new byte[definition.VertexBuffers[i].Definition.Data.Size];
                    edResourceStream.Position = definition.VertexBuffers[i].Definition.Data.Address.Offset;
                    edResourceStream.Read(definition.VertexBuffers[i].Definition.Data.Data, 0, definition.VertexBuffers[i].Definition.Data.Size);
                }

                for (var i = 0; i < definition.IndexBuffers.Count; i++)
                {
                    definition.IndexBuffers[i].Definition.Data.Data = new byte[definition.IndexBuffers[i].Definition.Data.Size];
                    edResourceStream.Position = definition.IndexBuffers[i].Definition.Data.Address.Offset;
                    edResourceStream.Read(definition.IndexBuffers[i].Definition.Data.Data, 0, definition.IndexBuffers[i].Definition.Data.Size);
                }
            }
        }

        private void LoadScenarioLightmapBspDataResources(ScenarioLightmapBspData Lbsp)
        {
            LoadRenderGeometryResource(Lbsp.Geometry);
        }

        private void LoadStructureBspTagResources(ScenarioStructureBsp sbsp)
        {
            var location = sbsp.TagResources.GetLocation();
            if (location == TagTool.Common.ResourceLocation.None)
                return;

            using (var resourceStream = new MemoryStream())
            using (var resourceReader = new EndianReader(resourceStream))
            {
                CacheContext.ExtractResource(sbsp.TagResources, resourceStream);

                var definition = sbsp.TagResources.Definition =
                    CacheContext.Deserialize<StructureBspTagResources>(sbsp.TagResources);
                
                foreach (var cbsp in definition.CollisionBsps)
                {
                    resourceReader.BaseStream.Position = cbsp.Bsp3dNodes.Address.Offset;
                    for (var i = 0; i < cbsp.Bsp3dNodes.Count; i++)
                    {
                        var element = CacheContext.Deserializer.DeserializeValue(resourceReader, null, null, typeof(CollisionGeometry.Bsp3dNode));
                        cbsp.Bsp3dNodes.Add((CollisionGeometry.Bsp3dNode)element);
                    }

                    resourceReader.BaseStream.Position = cbsp.Planes.Address.Offset;
                    for (var i = 0; i < cbsp.Planes.Count; i++)
                    {
                        var element = CacheContext.Deserializer.DeserializeValue(resourceReader, null, null, typeof(CollisionGeometry.Plane));
                        cbsp.Planes.Add((CollisionGeometry.Plane)element);
                    }

                    resourceReader.BaseStream.Position = cbsp.Leaves.Address.Offset;
                    for (var i = 0; i < cbsp.Leaves.Count; i++)
                    {
                        var element = CacheContext.Deserializer.DeserializeValue(resourceReader, null, null, typeof(CollisionGeometry.Leaf));
                        cbsp.Leaves.Add((CollisionGeometry.Leaf)element);
                    }

                    resourceReader.BaseStream.Position = cbsp.Bsp2dReferences.Address.Offset;
                    for (var i = 0; i < cbsp.Bsp2dReferences.Count; i++)
                    {
                        var element = CacheContext.Deserializer.DeserializeValue(resourceReader, null, null, typeof(CollisionGeometry.Bsp2dReference));
                        cbsp.Bsp2dReferences.Add((CollisionGeometry.Bsp2dReference)element);
                    }

                    resourceReader.BaseStream.Position = cbsp.Bsp2dNodes.Address.Offset;
                    for (var i = 0; i < cbsp.Bsp2dNodes.Count; i++)
                    {
                        var element = CacheContext.Deserializer.DeserializeValue(resourceReader, null, null, typeof(CollisionGeometry.Bsp2dNode));
                        cbsp.Bsp2dNodes.Add((CollisionGeometry.Bsp2dNode)element);
                    }

                    resourceReader.BaseStream.Position = cbsp.Surfaces.Address.Offset;
                    for (var i = 0; i < cbsp.Surfaces.Count; i++)
                    {
                        var element = CacheContext.Deserializer.DeserializeValue(resourceReader, null, null, typeof(CollisionGeometry.Surface));
                        cbsp.Surfaces.Add((CollisionGeometry.Surface)element);
                    }

                    resourceReader.BaseStream.Position = cbsp.Edges.Address.Offset;
                    for (var i = 0; i < cbsp.Edges.Count; i++)
                    {
                        var element = CacheContext.Deserializer.DeserializeValue(resourceReader, null, null, typeof(CollisionGeometry.Edge));
                        cbsp.Edges.Add((CollisionGeometry.Edge)element);
                    }

                    resourceReader.BaseStream.Position = cbsp.Vertices.Address.Offset;
                    for (var i = 0; i < cbsp.Vertices.Count; i++)
                    {
                        var element = CacheContext.Deserializer.DeserializeValue(resourceReader, null, null, typeof(CollisionGeometry.Vertex));
                        cbsp.Vertices.Add((CollisionGeometry.Vertex)element);
                    }
                }

                foreach (var cbsp in definition.LargeCollisionBsps)
                {
                    resourceReader.BaseStream.Position = cbsp.Bsp3dNodes.Address.Offset;
                    for (var i = 0; i < cbsp.Bsp3dNodes.Count; i++)
                    {
                        var element = CacheContext.Deserializer.DeserializeValue(resourceReader, null, null, typeof(StructureBspTagResources.LargeCollisionBspBlock.Bsp3dNode));
                        cbsp.Bsp3dNodes.Add((StructureBspTagResources.LargeCollisionBspBlock.Bsp3dNode)element);
                    }

                    resourceReader.BaseStream.Position = cbsp.Planes.Address.Offset;
                    for (var i = 0; i < cbsp.Planes.Count; i++)
                    {
                        var element = CacheContext.Deserializer.DeserializeValue(resourceReader, null, null, typeof(CollisionGeometry.Plane));
                        cbsp.Planes.Add((CollisionGeometry.Plane)element);
                    }

                    resourceReader.BaseStream.Position = cbsp.Leaves.Address.Offset;
                    for (var i = 0; i < cbsp.Leaves.Count; i++)
                    {
                        var element = CacheContext.Deserializer.DeserializeValue(resourceReader, null, null, typeof(CollisionGeometry.Leaf));
                        cbsp.Leaves.Add((CollisionGeometry.Leaf)element);
                    }

                    resourceReader.BaseStream.Position = cbsp.Bsp2dReferences.Address.Offset;
                    for (var i = 0; i < cbsp.Bsp2dReferences.Count; i++)
                    {
                        var element = CacheContext.Deserializer.DeserializeValue(resourceReader, null, null, typeof(StructureBspTagResources.LargeCollisionBspBlock.Bsp2dReference));
                        cbsp.Bsp2dReferences.Add((StructureBspTagResources.LargeCollisionBspBlock.Bsp2dReference)element);
                    }

                    resourceReader.BaseStream.Position = cbsp.Bsp2dNodes.Address.Offset;
                    for (var i = 0; i < cbsp.Bsp2dNodes.Count; i++)
                    {
                        var element = CacheContext.Deserializer.DeserializeValue(resourceReader, null, null, typeof(StructureBspTagResources.LargeCollisionBspBlock.Bsp2dNode));
                        cbsp.Bsp2dNodes.Add((StructureBspTagResources.LargeCollisionBspBlock.Bsp2dNode)element);
                    }

                    resourceReader.BaseStream.Position = cbsp.Surfaces.Address.Offset;
                    for (var i = 0; i < cbsp.Surfaces.Count; i++)
                    {
                        var element = CacheContext.Deserializer.DeserializeValue(resourceReader, null, null, typeof(StructureBspTagResources.LargeCollisionBspBlock.Surface));
                        cbsp.Surfaces.Add((StructureBspTagResources.LargeCollisionBspBlock.Surface)element);
                    }

                    resourceReader.BaseStream.Position = cbsp.Edges.Address.Offset;
                    for (var i = 0; i < cbsp.Edges.Count; i++)
                    {
                        var element = CacheContext.Deserializer.DeserializeValue(resourceReader, null, null, typeof(StructureBspTagResources.LargeCollisionBspBlock.Edge));
                        cbsp.Edges.Add((StructureBspTagResources.LargeCollisionBspBlock.Edge)element);
                    }

                    resourceReader.BaseStream.Position = cbsp.Vertices.Address.Offset;
                    for (var i = 0; i < cbsp.Vertices.Count; i++)
                    {
                        var element = CacheContext.Deserializer.DeserializeValue(resourceReader, null, null, typeof(StructureBspTagResources.LargeCollisionBspBlock.Vertex));
                        cbsp.Vertices.Add((StructureBspTagResources.LargeCollisionBspBlock.Vertex)element);
                    }
                }

                foreach (var instance in definition.InstancedGeometry)
                {
                    resourceReader.BaseStream.Position = instance.CollisionInfo.Bsp3dNodes.Address.Offset;
                    for (var i = 0; i < instance.CollisionInfo.Bsp3dNodes.Count; i++)
                    {
                        var element = CacheContext.Deserializer.DeserializeValue(resourceReader, null, null, typeof(CollisionGeometry.Bsp3dNode));
                        instance.CollisionInfo.Bsp3dNodes.Add((CollisionGeometry.Bsp3dNode)element);
                    }

                    resourceReader.BaseStream.Position = instance.CollisionInfo.Planes.Address.Offset;
                    for (var i = 0; i < instance.CollisionInfo.Planes.Count; i++)
                    {
                        var element = CacheContext.Deserializer.DeserializeValue(resourceReader, null, null, typeof(CollisionGeometry.Plane));
                        instance.CollisionInfo.Planes.Add((CollisionGeometry.Plane)element);
                    }

                    resourceReader.BaseStream.Position = instance.CollisionInfo.Leaves.Address.Offset;
                    for (var i = 0; i < instance.CollisionInfo.Leaves.Count; i++)
                    {
                        var element = CacheContext.Deserializer.DeserializeValue(resourceReader, null, null, typeof(CollisionGeometry.Leaf));
                        instance.CollisionInfo.Leaves.Add((CollisionGeometry.Leaf)element);
                    }

                    resourceReader.BaseStream.Position = instance.CollisionInfo.Bsp2dReferences.Address.Offset;
                    for (var i = 0; i < instance.CollisionInfo.Bsp2dReferences.Count; i++)
                    {
                        var element = CacheContext.Deserializer.DeserializeValue(resourceReader, null, null, typeof(CollisionGeometry.Bsp2dReference));
                        instance.CollisionInfo.Bsp2dReferences.Add((CollisionGeometry.Bsp2dReference)element);
                    }

                    resourceReader.BaseStream.Position = instance.CollisionInfo.Bsp2dNodes.Address.Offset;
                    for (var i = 0; i < instance.CollisionInfo.Bsp2dNodes.Count; i++)
                    {
                        var element = CacheContext.Deserializer.DeserializeValue(resourceReader, null, null, typeof(CollisionGeometry.Bsp2dNode));
                        instance.CollisionInfo.Bsp2dNodes.Add((CollisionGeometry.Bsp2dNode)element);
                    }

                    resourceReader.BaseStream.Position = instance.CollisionInfo.Surfaces.Address.Offset;
                    for (var i = 0; i < instance.CollisionInfo.Surfaces.Count; i++)
                    {
                        var element = CacheContext.Deserializer.DeserializeValue(resourceReader, null, null, typeof(CollisionGeometry.Surface));
                        instance.CollisionInfo.Surfaces.Add((CollisionGeometry.Surface)element);
                    }

                    resourceReader.BaseStream.Position = instance.CollisionInfo.Edges.Address.Offset;
                    for (var i = 0; i < instance.CollisionInfo.Edges.Count; i++)
                    {
                        var element = CacheContext.Deserializer.DeserializeValue(resourceReader, null, null, typeof(CollisionGeometry.Edge));
                        instance.CollisionInfo.Edges.Add((CollisionGeometry.Edge)element);
                    }

                    resourceReader.BaseStream.Position = instance.CollisionInfo.Vertices.Address.Offset;
                    for (var i = 0; i < instance.CollisionInfo.Vertices.Count; i++)
                    {
                        var element = CacheContext.Deserializer.DeserializeValue(resourceReader, null, null, typeof(CollisionGeometry.Vertex));
                        instance.CollisionInfo.Vertices.Add((CollisionGeometry.Vertex)element);
                    }

                    foreach (var cbsp in instance.CollisionGeometries)
                    {
                        resourceReader.BaseStream.Position = cbsp.Bsp3dNodes.Address.Offset;
                        for (var i = 0; i < cbsp.Bsp3dNodes.Count; i++)
                        {
                            var element = CacheContext.Deserializer.DeserializeValue(resourceReader, null, null, typeof(CollisionGeometry.Bsp3dNode));
                            cbsp.Bsp3dNodes.Add((CollisionGeometry.Bsp3dNode)element);
                        }

                        resourceReader.BaseStream.Position = cbsp.Planes.Address.Offset;
                        for (var i = 0; i < cbsp.Planes.Count; i++)
                        {
                            var element = CacheContext.Deserializer.DeserializeValue(resourceReader, null, null, typeof(CollisionGeometry.Plane));
                            cbsp.Planes.Add((CollisionGeometry.Plane)element);
                        }

                        resourceReader.BaseStream.Position = cbsp.Leaves.Address.Offset;
                        for (var i = 0; i < cbsp.Leaves.Count; i++)
                        {
                            var element = CacheContext.Deserializer.DeserializeValue(resourceReader, null, null, typeof(CollisionGeometry.Leaf));
                            cbsp.Leaves.Add((CollisionGeometry.Leaf)element);
                        }

                        resourceReader.BaseStream.Position = cbsp.Bsp2dReferences.Address.Offset;
                        for (var i = 0; i < cbsp.Bsp2dReferences.Count; i++)
                        {
                            var element = CacheContext.Deserializer.DeserializeValue(resourceReader, null, null, typeof(CollisionGeometry.Bsp2dReference));
                            cbsp.Bsp2dReferences.Add((CollisionGeometry.Bsp2dReference)element);
                        }

                        resourceReader.BaseStream.Position = cbsp.Bsp2dNodes.Address.Offset;
                        for (var i = 0; i < cbsp.Bsp2dNodes.Count; i++)
                        {
                            var element = CacheContext.Deserializer.DeserializeValue(resourceReader, null, null, typeof(CollisionGeometry.Bsp2dNode));
                            cbsp.Bsp2dNodes.Add((CollisionGeometry.Bsp2dNode)element);
                        }

                        resourceReader.BaseStream.Position = cbsp.Surfaces.Address.Offset;
                        for (var i = 0; i < cbsp.Surfaces.Count; i++)
                        {
                            var element = CacheContext.Deserializer.DeserializeValue(resourceReader, null, null, typeof(CollisionGeometry.Surface));
                            cbsp.Surfaces.Add((CollisionGeometry.Surface)element);
                        }

                        resourceReader.BaseStream.Position = cbsp.Edges.Address.Offset;
                        for (var i = 0; i < cbsp.Edges.Count; i++)
                        {
                            var element = CacheContext.Deserializer.DeserializeValue(resourceReader, null, null, typeof(CollisionGeometry.Edge));
                            cbsp.Edges.Add((CollisionGeometry.Edge)element);
                        }

                        resourceReader.BaseStream.Position = cbsp.Vertices.Address.Offset;
                        for (var i = 0; i < cbsp.Vertices.Count; i++)
                        {
                            var element = CacheContext.Deserializer.DeserializeValue(resourceReader, null, null, typeof(CollisionGeometry.Vertex));
                            cbsp.Vertices.Add((CollisionGeometry.Vertex)element);
                        }
                    }

                    for (var i = 0; i < instance.Unknown1.Count; i++)
                    {
                        var element = CacheContext.Deserializer.DeserializeValue(resourceReader, null, null, typeof(StructureBspTagResources.InstancedGeometryBlock.Unknown1Block));
                        instance.Unknown1.Add((StructureBspTagResources.InstancedGeometryBlock.Unknown1Block)element);
                    }

                    for (var i = 0; i < instance.Unknown2.Count; i++)
                    {
                        var element = CacheContext.Deserializer.DeserializeValue(resourceReader, null, null, typeof(StructureBspTagResources.InstancedGeometryBlock.Unknown2Block));
                        instance.Unknown2.Add((StructureBspTagResources.InstancedGeometryBlock.Unknown2Block)element);
                    }

                    for (var i = 0; i < instance.Unknown3.Count; i++)
                    {
                        var element = CacheContext.Deserializer.DeserializeValue(resourceReader, null, null, typeof(StructureBspTagResources.InstancedGeometryBlock.Unknown3Block));
                        instance.Unknown3.Add((StructureBspTagResources.InstancedGeometryBlock.Unknown3Block)element);
                    }

                    foreach (var collision in instance.BspPhysics)
                    {
                        for (var i = 0; i < collision.Data.Count; i++)
                        {
                            var element = CacheContext.Deserializer.DeserializeValue(resourceReader, null, null, typeof(byte));
                            collision.Data.Add(new StructureBspTagResources.CollisionBspPhysicsBlock.Datum { Value = (byte)element });
                        }
                    }
                }
            }
        }

        private void LoadStructureBspCacheFileTagResources(ScenarioStructureBsp sbsp)
        {
            var location = sbsp.CacheFileTagResources.GetLocation();
            if (location == TagTool.Common.ResourceLocation.None)
                return;

            using (var resourceStream = new MemoryStream())
            using (var resourceReader = new EndianReader(resourceStream))
            {
                CacheContext.ExtractResource(sbsp.CacheFileTagResources, resourceStream);

                var definition = sbsp.CacheFileTagResources.Definition =
                    CacheContext.Deserialize<StructureBspCacheFileTagResources>(sbsp.CacheFileTagResources);

                var dataContext = new DataSerializationContext(resourceReader);

                resourceStream.Position = definition.SurfacePlanes.Address.Offset;

                for (var i = 0; i < definition.SurfacePlanes.Count; i++)
                    definition.SurfacePlanes.Add(
                        CacheContext.Deserialize<ScenarioStructureBsp.SurfacePlane>(dataContext));

                resourceStream.Position = definition.Planes.Address.Offset;

                for (var i = 0; i < definition.Planes.Count; i++)
                    definition.Planes.Add(
                        CacheContext.Deserialize<ScenarioStructureBsp.Plane>(dataContext));

                resourceStream.Position = definition.EdgeToSeams.Address.Offset;

                for (var i = 0; i < definition.EdgeToSeams.Count; i++)
                    definition.EdgeToSeams.Add(
                        CacheContext.Deserialize<ScenarioStructureBsp.EdgeToSeamMapping>(dataContext));

                foreach (var pathfindingDatum in definition.PathfindingData)
                {
                    resourceStream.Position = pathfindingDatum.Sectors.Address.Offset;

                    for (var i = 0; i < pathfindingDatum.Sectors.Count; i++)
                        pathfindingDatum.Sectors.Add(
                            CacheContext.Deserialize<ScenarioStructureBsp.PathfindingDatum.Sector>(dataContext));

                    resourceStream.Position = pathfindingDatum.Links.Address.Offset;

                    for (var i = 0; i < pathfindingDatum.Links.Count; i++)
                        pathfindingDatum.Links.Add(
                            CacheContext.Deserialize<ScenarioStructureBsp.PathfindingDatum.Link>(dataContext));

                    resourceStream.Position = pathfindingDatum.References.Address.Offset;

                    for (var i = 0; i < pathfindingDatum.References.Count; i++)
                        pathfindingDatum.References.Add(
                            CacheContext.Deserialize<ScenarioStructureBsp.PathfindingDatum.Reference>(dataContext));

                    resourceStream.Position = pathfindingDatum.Bsp2dNodes.Address.Offset;

                    for (var i = 0; i < pathfindingDatum.Bsp2dNodes.Count; i++)
                        pathfindingDatum.Bsp2dNodes.Add(
                            CacheContext.Deserialize<ScenarioStructureBsp.PathfindingDatum.Bsp2dNode>(dataContext));

                    resourceStream.Position = pathfindingDatum.Vertices.Address.Offset;

                    for (var i = 0; i < pathfindingDatum.Vertices.Count; i++)
                        pathfindingDatum.Vertices.Add(
                            CacheContext.Deserialize<ScenarioStructureBsp.PathfindingDatum.Vertex>(dataContext));

                    for (var objRefIdx = 0; objRefIdx < pathfindingDatum.ObjectReferences.Count; objRefIdx++)
                    {
                        for (var bspRefIdx = 0; bspRefIdx < pathfindingDatum.ObjectReferences[objRefIdx].Bsps.Count; bspRefIdx++)
                        {
                            var bspRef = pathfindingDatum.ObjectReferences[objRefIdx].Bsps[bspRefIdx];

                            resourceStream.Position = bspRef.Bsp2dRefs.Address.Offset;

                            for (var bsp2dRefIdx = 0; bsp2dRefIdx < bspRef.Bsp2dRefs.Count; bsp2dRefIdx++)
                                bspRef.Bsp2dRefs.Add(
                                    CacheContext.Deserialize<ScenarioStructureBsp.PathfindingDatum.ObjectReference.BspReference.Bsp2dRef>(dataContext));
                        }
                    }

                    resourceStream.Position = pathfindingDatum.PathfindingHints.Address.Offset;

                    for (var i = 0; i < pathfindingDatum.PathfindingHints.Count; i++)
                        pathfindingDatum.PathfindingHints.Add(
                            CacheContext.Deserialize<ScenarioStructureBsp.PathfindingDatum.PathfindingHint>(dataContext));

                    resourceStream.Position = pathfindingDatum.InstancedGeometryReferences.Address.Offset;

                    for (var i = 0; i < pathfindingDatum.InstancedGeometryReferences.Count; i++)
                        pathfindingDatum.InstancedGeometryReferences.Add(
                            CacheContext.Deserialize<ScenarioStructureBsp.PathfindingDatum.InstancedGeometryReference>(dataContext));

                    resourceStream.Position = pathfindingDatum.GiantPathfinding.Address.Offset;

                    for (var i = 0; i < pathfindingDatum.GiantPathfinding.Count; i++)
                        pathfindingDatum.GiantPathfinding.Add(
                            CacheContext.Deserialize<ScenarioStructureBsp.PathfindingDatum.GiantPathfindingBlock>(dataContext));

                    foreach (var seam in pathfindingDatum.Seams)
                    {
                        resourceStream.Position = seam.LinkIndices.Address.Offset;

                        for (var i = 0; i < seam.LinkIndices.Count; i++)
                            seam.LinkIndices.Add(
                                CacheContext.Deserialize<ScenarioStructureBsp.PathfindingDatum.Seam.LinkIndexBlock>(dataContext));
                    }

                    foreach (var jumpSeam in pathfindingDatum.JumpSeams)
                    {
                        resourceStream.Position = jumpSeam.JumpIndices.Address.Offset;

                        for (var i = 0; i < jumpSeam.JumpIndices.Count; i++)
                            jumpSeam.JumpIndices.Add(
                                CacheContext.Deserialize<ScenarioStructureBsp.PathfindingDatum.JumpSeam.JumpIndexBlock>(dataContext));
                    }

                    resourceStream.Position = pathfindingDatum.Doors.Address.Offset;

                    for (var i = 0; i < pathfindingDatum.Doors.Count; i++)
                        pathfindingDatum.Doors.Add(
                            CacheContext.Deserialize<ScenarioStructureBsp.PathfindingDatum.Door>(dataContext));
                }
            }
        }

        private void LoadScenarioStructureBspResources(ScenarioStructureBsp sbsp)
        {
            LoadRenderGeometryResource(sbsp.Geometry);
            LoadRenderGeometryResource(sbsp.Geometry2);
            LoadStructureBspTagResources(sbsp);
            LoadStructureBspCacheFileTagResources(sbsp);
        }

        private void LoadRenderModelResource(RenderModel mode)
        {
            LoadRenderGeometryResource(mode.Geometry);
        }

        private void LoadParticleModelResource(ParticleModel prtm)
        {
            LoadRenderGeometryResource(prtm.Geometry);
        }

        private void LoadModelAnimationGraphResources(ModelAnimationGraph jmad)
        {
            foreach (var entry in jmad.ResourceGroups)
            {
                var location = entry.Resource.GetLocation();
                if (location == TagTool.Common.ResourceLocation.None)
                    return;

                using (var resourceStream = new MemoryStream())
                {
                    CacheContext.ExtractResource(entry.Resource, resourceStream);

                    var definition = entry.Resource.Definition =
                        CacheContext.Deserialize<ModelAnimationTagResource>(entry.Resource);

                    foreach (var resourceEntry in definition.GroupMembers)
                    {
                        resourceEntry.AnimationData.Data = new byte[resourceEntry.AnimationData.Size];
                        resourceStream.Position = resourceEntry.AnimationData.Address.Offset;
                        resourceStream.Read(resourceEntry.AnimationData.Data, 0, resourceEntry.AnimationData.Size);
                    }
                }
            }
        }

        private void LoadBitmapResource(TagTool.Tags.Definitions.Bitmap bitm)
        {
            foreach (var entry in bitm.Resources)
            {
                var location = entry.Resource.GetLocation();
                if (location == TagTool.Common.ResourceLocation.None)
                    return;

                using (var resourceStream = new MemoryStream())
                {
                    CacheContext.ExtractResource(entry.Resource, resourceStream);

                    var definition = entry.Resource.Definition =
                        CacheContext.Deserialize<BitmapTextureInteropResource>(entry.Resource);

                    definition.Texture.Definition.Data.Data = new byte[definition.Texture.Definition.Data.Size];
                    resourceStream.Position = 0;
                    resourceStream.Read(definition.Texture.Definition.Data.Data, 0, definition.Texture.Definition.Data.Size);
                }
            }
        }

        private void LoadBinkResource(Bink bink)
        {
            var location = bink.Resource.GetLocation();
            if (location == TagTool.Common.ResourceLocation.None)
                return;

            using (var resourceStream = new MemoryStream())
            {
                CacheContext.ExtractResource(bink.Resource, resourceStream);
                var definition = bink.Resource.Definition = CacheContext.Deserialize<BinkResource>(bink.Resource);

                definition.Data.Data = new byte[definition.Data.Size];
                resourceStream.Position = definition.Data.Address.Offset;
                resourceStream.Read(definition.Data.Data, 0, definition.Data.Size);
            }
        }

        private void tagTreeView_DoubleClick(object sender, EventArgs e)
        {
            var nodeTag = tagTreeView.SelectedNode?.Tag;

            switch (nodeTag)
            {
                case CachedTagInstance tag:
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
            if (tagTreeView.SelectedNode?.Tag is CachedTagInstance tag)
            {
                using (var sfd = new SaveFileDialog())
                {
                    var groupName = CacheContext.GetString(tag.Group.Name);

                    sfd.Filter = $"{groupName} files (*.{groupName})|*.{groupName}";

                    if (sfd.ShowDialog() != DialogResult.OK)
                        return;

                    byte[] data;

                    using (var stream = CacheContext.OpenTagCacheRead())
                        data = CacheContext.TagCache.ExtractTagRaw(stream, tag);

                    using (var stream = File.Open(sfd.FileName, FileMode.Create, FileAccess.Write))
                        stream.Write(data, 0, data.Length);

                    MessageBox.Show($"Extracted {sfd.FileName} successfully.", "Extract Tag", MessageBoxButtons.OK);
                }
            }
        }

        private void importToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tagTreeView.SelectedNode?.Tag is CachedTagInstance tag)
            {
                using (var ofd = new OpenFileDialog())
                {
                    var groupName = CacheContext.GetString(tag.Group.Name);

                    ofd.Filter = $"{groupName} files (*.{groupName})|*.{groupName}";

                    if (ofd.ShowDialog() != DialogResult.OK)
                        return;

                    byte[] data;

                    using (var stream = File.OpenRead(ofd.FileName))
                    {
                        data = new byte[stream.Length];
                        stream.Read(data, 0, data.Length);
                    }

                    using (var stream = CacheContext.OpenTagCacheReadWrite())
                        CacheContext.TagCache.SetTagDataRaw(stream, tag, data);

                    MessageBox.Show($"Imported {ofd.FileName} successfully.", "Import Tag", MessageBoxButtons.OK);
                }
            }
        }

        public DialogResult RenameTag(CachedTagInstance tag)
        {
            var result = DialogResult.OK;

            using (var rd = new RenameDialog(CacheContext, tag))
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
            if (tagTreeView.SelectedNode?.Tag is CachedTagInstance tag)
            {
                using (var rd = new RenameDialog(CacheContext, tag))
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

                    foreach (var entry in CacheContext.TagCache.Index)
                    {
                        if (entry.Name == null || !entry.Name.StartsWith(path))
                            continue;
                        
                        entry.Name = $"{rd.Value}{entry.Name.Substring(path.Length, entry.Name.Length - path.Length)}";
                    }

                    tagTreeView.SelectedNode.Remove();

                    TreeNode unnamed = tagTreeView.Nodes.Find("<unnamed>", false)[0];

                    foreach (var instance in CacheContext.TagCache.Index)
                        if (instance != null && instance.Name == null)
                            CreateTagTreeNode(instance, ref unnamed);
                }
            }
        }

        private void saveTagNamesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CacheContext.SaveTagNames();
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

        private string SaveTagChanges(CachedTagInstance tag, object definition)
        {
            using (var stream = CacheContext.OpenTagCacheReadWrite())
            {
                var context = new TagSerializationContext(stream, CacheContext, tag);
                CacheContext.Serializer.Serialize(context, definition);
            }

            var tagName = tag.Name ?? $"0x{tag.Index:X4}";

            if (tagName.Contains('\\'))
            {
                var index = tagName.LastIndexOf('\\') + 1;
                tagName = tagName.Substring(index, tagName.Length - index);
            }

            return $"{tagName}.{ CacheContext.GetString(CurrentTag.Group.Name)}";
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
                message += $"\n{SaveTagChanges(CacheContext.GetTag(entry.Key), entry.Value)}";

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