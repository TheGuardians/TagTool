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
using TagTool.IO;
using TagTool.Tags.Resources;
using TagTool.Geometry;

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
            {
                using (var stream = CacheContext.OpenTagCacheRead())
                    definition = CacheContext.Deserializer.Deserialize(
                        new TagSerializationContext(stream, CacheContext, tag),
                        TagDefinition.Find(tag.Group.Tag));

                switch (definition)
                {
                    case ScenarioStructureBsp sbsp:
                        {
                            LoadStructureBspTagResources(sbsp);
                            LoadStructureBspCacheFileTagResources(sbsp);
                            break;
                        }
                }
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

        private void LoadStructureBspCacheFileTagResources(ScenarioStructureBsp sbsp)
        {
            var resourceDefinition = CacheContext.Deserialize<StructureBspCacheFileTagResources>(sbsp.PathfindingResource);

            using (var resourceStream = new MemoryStream())
            using (var resourceReader = new EndianReader(resourceStream, EndianFormat.LittleEndian))
            {
                CacheContext.ExtractResource(sbsp.PathfindingResource, resourceStream);

                var resourceDataContext = new DataSerializationContext(resourceReader);

                resourceStream.Position = resourceDefinition.SurfacePlanes.Address.Offset;

                for (var i = 0; i < resourceDefinition.SurfacePlanes.Count; i++)
                    resourceDefinition.SurfacePlanes.Add(
                        CacheContext.Deserialize<ScenarioStructureBsp.SurfacePlane>(
                            resourceDataContext));

                resourceStream.Position = resourceDefinition.Planes.Address.Offset;

                for (var i = 0; i < resourceDefinition.Planes.Count; i++)
                    resourceDefinition.Planes.Add(
                        CacheContext.Deserialize<ScenarioStructureBsp.Plane>(
                            resourceDataContext));

                resourceStream.Position = resourceDefinition.EdgeToSeamMappings.Address.Offset;

                for (var i = 0; i < resourceDefinition.EdgeToSeamMappings.Count; i++)
                    resourceDefinition.EdgeToSeamMappings.Add(
                        CacheContext.Deserialize<ScenarioStructureBsp.EdgeToSeamMapping>(
                            resourceDataContext));

                foreach (var pathfindingData in resourceDefinition.PathfindingData)
                {
                    resourceStream.Position = pathfindingData.Sectors.Address.Offset;

                    for (var i = 0; i < pathfindingData.Sectors.Count; i++)
                        pathfindingData.Sectors.Add(
                            CacheContext.Deserialize<ScenarioStructureBsp.PathfindingDatum.Sector>(
                                resourceDataContext));

                    resourceStream.Position = pathfindingData.Links.Address.Offset;

                    for (var i = 0; i < pathfindingData.Links.Count; i++)
                        pathfindingData.Links.Add(
                            CacheContext.Deserialize<ScenarioStructureBsp.PathfindingDatum.Link>(
                                resourceDataContext));

                    resourceStream.Position = pathfindingData.References.Address.Offset;

                    for (var i = 0; i < pathfindingData.References.Count; i++)
                        pathfindingData.References.Add(
                            CacheContext.Deserialize<ScenarioStructureBsp.PathfindingDatum.Reference>(
                                resourceDataContext));

                    resourceStream.Position = pathfindingData.Bsp2dNodes.Address.Offset;

                    for (var i = 0; i < pathfindingData.Bsp2dNodes.Count; i++)
                        pathfindingData.Bsp2dNodes.Add(
                            CacheContext.Deserialize<ScenarioStructureBsp.PathfindingDatum.Bsp2dNode>(
                                resourceDataContext));

                    resourceStream.Position = pathfindingData.Vertices.Address.Offset;

                    for (var i = 0; i < pathfindingData.Vertices.Count; i++)
                        pathfindingData.Vertices.Add(
                            CacheContext.Deserialize<ScenarioStructureBsp.PathfindingDatum.Vertex>(
                                resourceDataContext));

                    foreach (var objectReference in pathfindingData.ObjectReferences)
                    {
                        foreach (var bspReference in objectReference.Bsps)
                        {
                            resourceStream.Position = bspReference.Bsp2dRefs.Address.Offset;

                            for (var i = 0; i < bspReference.Bsp2dRefs.Count; i++)
                                bspReference.Bsp2dRefs.Add(
                                    CacheContext.Deserialize<ScenarioStructureBsp.PathfindingDatum.ObjectReference.BspReference.Bsp2dRef>(
                                        resourceDataContext));
                        }
                    }

                    resourceStream.Position = pathfindingData.PathfindingHints.Address.Offset;

                    for (var i = 0; i < pathfindingData.PathfindingHints.Count; i++)
                        pathfindingData.PathfindingHints.Add(
                            CacheContext.Deserialize<ScenarioStructureBsp.PathfindingDatum.PathfindingHint>(
                                resourceDataContext));

                    resourceStream.Position = pathfindingData.InstancedGeometryReferences.Address.Offset;

                    for (var i = 0; i < pathfindingData.InstancedGeometryReferences.Count; i++)
                        pathfindingData.InstancedGeometryReferences.Add(
                            CacheContext.Deserialize<ScenarioStructureBsp.PathfindingDatum.InstancedGeometryReference>(
                                resourceDataContext));

                    resourceStream.Position = pathfindingData.GiantPathfinding.Address.Offset;

                    for (var i = 0; i < pathfindingData.GiantPathfinding.Count; i++)
                        pathfindingData.GiantPathfinding.Add(
                            CacheContext.Deserialize<ScenarioStructureBsp.PathfindingDatum.GiantPathfindingBlock>(
                                resourceDataContext));

                    foreach (var seam in pathfindingData.Seams)
                    {
                        resourceStream.Position = seam.LinkIndices.Address.Offset;

                        for (var i = 0; i < seam.LinkIndices.Count; i++)
                            seam.LinkIndices.Add(
                                CacheContext.Deserialize<ScenarioStructureBsp.PathfindingDatum.Seam.LinkIndexBlock>(
                                    resourceDataContext));
                    }

                    foreach (var jumpSeam in pathfindingData.JumpSeams)
                    {
                        resourceStream.Position = jumpSeam.JumpIndices.Address.Offset;

                        for (var i = 0; i < jumpSeam.JumpIndices.Count; i++)
                            jumpSeam.JumpIndices.Add(
                                CacheContext.Deserialize<ScenarioStructureBsp.PathfindingDatum.JumpSeam.JumpIndexBlock>(
                                    resourceDataContext));
                    }

                    resourceStream.Position = pathfindingData.Doors.Address.Offset;

                    for (var i = 0; i < pathfindingData.Doors.Count; i++)
                        pathfindingData.Doors.Add(
                            CacheContext.Deserialize<ScenarioStructureBsp.PathfindingDatum.Door>(
                                resourceDataContext));
                }
            }

            sbsp.PathfindingResource.Instance = resourceDefinition;
        }

        private void LoadStructureBspTagResources(ScenarioStructureBsp sbsp)
        {
            var resourceDefinition = CacheContext.Deserialize<StructureBspTagResources>(sbsp.CollisionBspResource);

            using (var resourceDataStream = new MemoryStream())
            using (var reader = new EndianReader(resourceDataStream))
            {
                CacheContext.ExtractResource(sbsp.CollisionBspResource, resourceDataStream);

                #region collision bsps

                foreach (var cbsp in resourceDefinition.CollisionBsps)
                {
                    reader.BaseStream.Position = cbsp.Bsp3dNodes.Address.Offset;
                    for (var i = 0; i < cbsp.Bsp3dNodes.Count; i++)
                    {
                        var element = CacheContext.Deserializer.DeserializeValue(reader, null, null, typeof(CollisionGeometry.Bsp3dNode));
                        cbsp.Bsp3dNodes.Add((CollisionGeometry.Bsp3dNode)element);
                    }

                    reader.BaseStream.Position = cbsp.Planes.Address.Offset;
                    for (var i = 0; i < cbsp.Planes.Count; i++)
                    {
                        var element = CacheContext.Deserializer.DeserializeValue(reader, null, null, typeof(CollisionGeometry.Plane));
                        cbsp.Planes.Add((CollisionGeometry.Plane)element);
                    }

                    reader.BaseStream.Position = cbsp.Leaves.Address.Offset;
                    for (var i = 0; i < cbsp.Leaves.Count; i++)
                    {
                        var element = CacheContext.Deserializer.DeserializeValue(reader, null, null, typeof(CollisionGeometry.Leaf));
                        cbsp.Leaves.Add((CollisionGeometry.Leaf)element);
                    }

                    reader.BaseStream.Position = cbsp.Bsp2dReferences.Address.Offset;
                    for (var i = 0; i < cbsp.Bsp2dReferences.Count; i++)
                    {
                        var element = CacheContext.Deserializer.DeserializeValue(reader, null, null, typeof(CollisionGeometry.Bsp2dReference));
                        cbsp.Bsp2dReferences.Add((CollisionGeometry.Bsp2dReference)element);
                    }

                    reader.BaseStream.Position = cbsp.Bsp2dNodes.Address.Offset;
                    for (var i = 0; i < cbsp.Bsp2dNodes.Count; i++)
                    {
                        var element = CacheContext.Deserializer.DeserializeValue(reader, null, null, typeof(CollisionGeometry.Bsp2dNode));
                        cbsp.Bsp2dNodes.Add((CollisionGeometry.Bsp2dNode)element);
                    }

                    reader.BaseStream.Position = cbsp.Surfaces.Address.Offset;
                    for (var i = 0; i < cbsp.Surfaces.Count; i++)
                    {
                        var element = CacheContext.Deserializer.DeserializeValue(reader, null, null, typeof(CollisionGeometry.Surface));
                        cbsp.Surfaces.Add((CollisionGeometry.Surface)element);
                    }

                    reader.BaseStream.Position = cbsp.Edges.Address.Offset;
                    for (var i = 0; i < cbsp.Edges.Count; i++)
                    {
                        var element = CacheContext.Deserializer.DeserializeValue(reader, null, null, typeof(CollisionGeometry.Edge));
                        cbsp.Edges.Add((CollisionGeometry.Edge)element);
                    }

                    reader.BaseStream.Position = cbsp.Vertices.Address.Offset;
                    for (var i = 0; i < cbsp.Vertices.Count; i++)
                    {
                        var element = CacheContext.Deserializer.DeserializeValue(reader, null, null, typeof(CollisionGeometry.Vertex));
                        cbsp.Vertices.Add((CollisionGeometry.Vertex)element);
                    }
                }

                #endregion

                #region large collision bsps

                foreach (var cbsp in resourceDefinition.LargeCollisionBsps)
                {
                    reader.BaseStream.Position = cbsp.Bsp3dNodes.Address.Offset;
                    for (var i = 0; i < cbsp.Bsp3dNodes.Count; i++)
                    {
                        var element = CacheContext.Deserializer.DeserializeValue(reader, null, null, typeof(StructureBspTagResources.LargeCollisionBspBlock.Bsp3dNode));
                        cbsp.Bsp3dNodes.Add((StructureBspTagResources.LargeCollisionBspBlock.Bsp3dNode)element);
                    }

                    reader.BaseStream.Position = cbsp.Planes.Address.Offset;
                    for (var i = 0; i < cbsp.Planes.Count; i++)
                    {
                        var element = CacheContext.Deserializer.DeserializeValue(reader, null, null, typeof(CollisionGeometry.Plane));
                        cbsp.Planes.Add((CollisionGeometry.Plane)element);
                    }

                    reader.BaseStream.Position = cbsp.Leaves.Address.Offset;
                    for (var i = 0; i < cbsp.Leaves.Count; i++)
                    {
                        var element = CacheContext.Deserializer.DeserializeValue(reader, null, null, typeof(CollisionGeometry.Leaf));
                        cbsp.Leaves.Add((CollisionGeometry.Leaf)element);
                    }

                    reader.BaseStream.Position = cbsp.Bsp2dReferences.Address.Offset;
                    for (var i = 0; i < cbsp.Bsp2dReferences.Count; i++)
                    {
                        var element = CacheContext.Deserializer.DeserializeValue(reader, null, null, typeof(StructureBspTagResources.LargeCollisionBspBlock.Bsp2dReference));
                        cbsp.Bsp2dReferences.Add((StructureBspTagResources.LargeCollisionBspBlock.Bsp2dReference)element);
                    }

                    reader.BaseStream.Position = cbsp.Bsp2dNodes.Address.Offset;
                    for (var i = 0; i < cbsp.Bsp2dNodes.Count; i++)
                    {
                        var element = CacheContext.Deserializer.DeserializeValue(reader, null, null, typeof(StructureBspTagResources.LargeCollisionBspBlock.Bsp2dNode));
                        cbsp.Bsp2dNodes.Add((StructureBspTagResources.LargeCollisionBspBlock.Bsp2dNode)element);
                    }

                    reader.BaseStream.Position = cbsp.Surfaces.Address.Offset;
                    for (var i = 0; i < cbsp.Surfaces.Count; i++)
                    {
                        var element = CacheContext.Deserializer.DeserializeValue(reader, null, null, typeof(StructureBspTagResources.LargeCollisionBspBlock.Surface));
                        cbsp.Surfaces.Add((StructureBspTagResources.LargeCollisionBspBlock.Surface)element);
                    }

                    reader.BaseStream.Position = cbsp.Edges.Address.Offset;
                    for (var i = 0; i < cbsp.Edges.Count; i++)
                    {
                        var element = CacheContext.Deserializer.DeserializeValue(reader, null, null, typeof(StructureBspTagResources.LargeCollisionBspBlock.Edge));
                        cbsp.Edges.Add((StructureBspTagResources.LargeCollisionBspBlock.Edge)element);
                    }

                    reader.BaseStream.Position = cbsp.Vertices.Address.Offset;
                    for (var i = 0; i < cbsp.Vertices.Count; i++)
                    {
                        var element = CacheContext.Deserializer.DeserializeValue(reader, null, null, typeof(StructureBspTagResources.LargeCollisionBspBlock.Vertex));
                        cbsp.Vertices.Add((StructureBspTagResources.LargeCollisionBspBlock.Vertex)element);
                    }
                }

                #endregion

                #region compressions

                foreach (var instance in resourceDefinition.InstancedGeometry)
                {
                    #region compression's resource data

                    reader.BaseStream.Position = instance.CollisionInfo.Bsp3dNodes.Address.Offset;
                    for (var i = 0; i < instance.CollisionInfo.Bsp3dNodes.Count; i++)
                    {
                        var element = CacheContext.Deserializer.DeserializeValue(reader, null, null, typeof(CollisionGeometry.Bsp3dNode));
                        instance.CollisionInfo.Bsp3dNodes.Add((CollisionGeometry.Bsp3dNode)element);
                    }

                    reader.BaseStream.Position = instance.CollisionInfo.Planes.Address.Offset;
                    for (var i = 0; i < instance.CollisionInfo.Planes.Count; i++)
                    {
                        var element = CacheContext.Deserializer.DeserializeValue(reader, null, null, typeof(CollisionGeometry.Plane));
                        instance.CollisionInfo.Planes.Add((CollisionGeometry.Plane)element);
                    }

                    reader.BaseStream.Position = instance.CollisionInfo.Leaves.Address.Offset;
                    for (var i = 0; i < instance.CollisionInfo.Leaves.Count; i++)
                    {
                        var element = CacheContext.Deserializer.DeserializeValue(reader, null, null, typeof(CollisionGeometry.Leaf));
                        instance.CollisionInfo.Leaves.Add((CollisionGeometry.Leaf)element);
                    }

                    reader.BaseStream.Position = instance.CollisionInfo.Bsp2dReferences.Address.Offset;
                    for (var i = 0; i < instance.CollisionInfo.Bsp2dReferences.Count; i++)
                    {
                        var element = CacheContext.Deserializer.DeserializeValue(reader, null, null, typeof(CollisionGeometry.Bsp2dReference));
                        instance.CollisionInfo.Bsp2dReferences.Add((CollisionGeometry.Bsp2dReference)element);
                    }

                    reader.BaseStream.Position = instance.CollisionInfo.Bsp2dNodes.Address.Offset;
                    for (var i = 0; i < instance.CollisionInfo.Bsp2dNodes.Count; i++)
                    {
                        var element = CacheContext.Deserializer.DeserializeValue(reader, null, null, typeof(CollisionGeometry.Bsp2dNode));
                        instance.CollisionInfo.Bsp2dNodes.Add((CollisionGeometry.Bsp2dNode)element);
                    }

                    reader.BaseStream.Position = instance.CollisionInfo.Surfaces.Address.Offset;
                    for (var i = 0; i < instance.CollisionInfo.Surfaces.Count; i++)
                    {
                        var element = CacheContext.Deserializer.DeserializeValue(reader, null, null, typeof(CollisionGeometry.Surface));
                        instance.CollisionInfo.Surfaces.Add((CollisionGeometry.Surface)element);
                    }

                    reader.BaseStream.Position = instance.CollisionInfo.Edges.Address.Offset;
                    for (var i = 0; i < instance.CollisionInfo.Edges.Count; i++)
                    {
                        var element = CacheContext.Deserializer.DeserializeValue(reader, null, null, typeof(CollisionGeometry.Edge));
                        instance.CollisionInfo.Edges.Add((CollisionGeometry.Edge)element);
                    }

                    reader.BaseStream.Position = instance.CollisionInfo.Vertices.Address.Offset;
                    for (var i = 0; i < instance.CollisionInfo.Vertices.Count; i++)
                    {
                        var element = CacheContext.Deserializer.DeserializeValue(reader, null, null, typeof(CollisionGeometry.Vertex));
                        instance.CollisionInfo.Vertices.Add((CollisionGeometry.Vertex)element);
                    }

                    #endregion

                    #region compression's other resource data

                    foreach (var cbsp in instance.CollisionGeometries)
                    {
                        reader.BaseStream.Position = cbsp.Bsp3dNodes.Address.Offset;
                        for (var i = 0; i < cbsp.Bsp3dNodes.Count; i++)
                        {
                            var element = CacheContext.Deserializer.DeserializeValue(reader, null, null, typeof(CollisionGeometry.Bsp3dNode));
                            cbsp.Bsp3dNodes.Add((CollisionGeometry.Bsp3dNode)element);
                        }

                        reader.BaseStream.Position = cbsp.Planes.Address.Offset;
                        for (var i = 0; i < cbsp.Planes.Count; i++)
                        {
                            var element = CacheContext.Deserializer.DeserializeValue(reader, null, null, typeof(CollisionGeometry.Plane));
                            cbsp.Planes.Add((CollisionGeometry.Plane)element);
                        }

                        reader.BaseStream.Position = cbsp.Leaves.Address.Offset;
                        for (var i = 0; i < cbsp.Leaves.Count; i++)
                        {
                            var element = CacheContext.Deserializer.DeserializeValue(reader, null, null, typeof(CollisionGeometry.Leaf));
                            cbsp.Leaves.Add((CollisionGeometry.Leaf)element);
                        }

                        reader.BaseStream.Position = cbsp.Bsp2dReferences.Address.Offset;
                        for (var i = 0; i < cbsp.Bsp2dReferences.Count; i++)
                        {
                            var element = CacheContext.Deserializer.DeserializeValue(reader, null, null, typeof(CollisionGeometry.Bsp2dReference));
                            cbsp.Bsp2dReferences.Add((CollisionGeometry.Bsp2dReference)element);
                        }

                        reader.BaseStream.Position = cbsp.Bsp2dNodes.Address.Offset;
                        for (var i = 0; i < cbsp.Bsp2dNodes.Count; i++)
                        {
                            var element = CacheContext.Deserializer.DeserializeValue(reader, null, null, typeof(CollisionGeometry.Bsp2dNode));
                            cbsp.Bsp2dNodes.Add((CollisionGeometry.Bsp2dNode)element);
                        }

                        reader.BaseStream.Position = cbsp.Surfaces.Address.Offset;
                        for (var i = 0; i < cbsp.Surfaces.Count; i++)
                        {
                            var element = CacheContext.Deserializer.DeserializeValue(reader, null, null, typeof(CollisionGeometry.Surface));
                            cbsp.Surfaces.Add((CollisionGeometry.Surface)element);
                        }

                        reader.BaseStream.Position = cbsp.Edges.Address.Offset;
                        for (var i = 0; i < cbsp.Edges.Count; i++)
                        {
                            var element = CacheContext.Deserializer.DeserializeValue(reader, null, null, typeof(CollisionGeometry.Edge));
                            cbsp.Edges.Add((CollisionGeometry.Edge)element);
                        }

                        reader.BaseStream.Position = cbsp.Vertices.Address.Offset;
                        for (var i = 0; i < cbsp.Vertices.Count; i++)
                        {
                            var element = CacheContext.Deserializer.DeserializeValue(reader, null, null, typeof(CollisionGeometry.Vertex));
                            cbsp.Vertices.Add((CollisionGeometry.Vertex)element);
                        }
                    }

                    #endregion

                    #region Unknown Data

                    for (var i = 0; i < instance.Unknown1.Count; i++)
                    {
                        var element = CacheContext.Deserializer.DeserializeValue(reader, null, null, typeof(StructureBspTagResources.InstancedGeometryBlock.Unknown1Block));
                        instance.Unknown1.Add((StructureBspTagResources.InstancedGeometryBlock.Unknown1Block)element);
                    }

                    for (var i = 0; i < instance.Unknown2.Count; i++)
                    {
                        var element = CacheContext.Deserializer.DeserializeValue(reader, null, null, typeof(StructureBspTagResources.InstancedGeometryBlock.Unknown2Block));
                        instance.Unknown2.Add((StructureBspTagResources.InstancedGeometryBlock.Unknown2Block)element);
                    }

                    for (var i = 0; i < instance.Unknown3.Count; i++)
                    {
                        var element = CacheContext.Deserializer.DeserializeValue(reader, null, null, typeof(StructureBspTagResources.InstancedGeometryBlock.Unknown3Block));
                        instance.Unknown3.Add((StructureBspTagResources.InstancedGeometryBlock.Unknown3Block)element);
                    }

                    #endregion

                    #region compression's havok collision data

                    foreach (var collision in instance.BspPhysics)
                    {
                        for (var i = 0; i < collision.Data.Count; i++)
                        {
                            var element = CacheContext.Deserializer.DeserializeValue(reader, null, null, typeof(byte));
                            collision.Data.Add(new StructureBspTagResources.CollisionBspPhysicsBlock.Datum { Value = (byte)element });
                        }
                    }

                    #endregion
                }

                #endregion
            }

            sbsp.CollisionBspResource.Instance = resourceDefinition;
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