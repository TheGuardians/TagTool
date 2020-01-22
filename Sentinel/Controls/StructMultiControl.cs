using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TagTool.Cache;
using TagTool.Tags.Definitions;
using TagTool.Serialization;
using TagTool.Common;
using Sentinel.Forms;
using System.Reflection;
using System.Collections;
using TagTool.Tags;

namespace Sentinel.Controls
{
    public partial class StructMultiControl : UserControl, IFieldControl
    {
        public GameCache Cache { get; } = null;
        public CachedTag Tag { get; } = null;
        public object Definition { get; } = null;
        public bool Loading { get; private set; } = false;
        
        public CacheForm Form { get; } = null;
        public Dictionary<string, IFieldControl> FieldControls { get; set; } = new Dictionary<string, IFieldControl>();

        public string TagName => Tag.Name;

        public StructMultiControl()
        {
            InitializeComponent();
        }

        public StructMultiControl(CacheForm form, GameCache cache, CachedTag tag, object definition) :
            this()
        {
            Form = form;
            Cache = cache;
            Tag = tag;
            Definition = definition;
        }

        protected override void OnLoad(EventArgs e)
        {
            if (treeView.Nodes.Count > 0)
                treeView.SelectedNode = treeView.Nodes[0];

            base.OnLoad(e);
        }

        private void treeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            var field = treeView.SelectedNode.Tag as FieldInfo ?? null;

            splitContainer1.Panel2.Controls.Clear();
            FieldControls.Clear();
            FieldControls = CreateFieldControls(Form, splitContainer1.Panel2, field, Definition.GetType(), field == null);

            Loading = true;
            GetFieldValue(Definition, Definition, Definition);
            Loading = false;
        }

        public void GetFieldValue(object owner, object value = null, object definition = null)
        {
            var enumerator = TagStructure.GetTagFieldEnumerable(new TagStructureInfo(value.GetType(), Cache.Version));

            if (!Loading)
            {
                var tagName = TagName;

                treeView.Nodes.Clear();
                treeView.Nodes.Add(new TreeNode
                {
                    Text = $"{(tagName.Contains('\\') ? tagName.Substring(tagName.LastIndexOf('\\') + 1) : tagName).ToPascalCase().ToSpaced()} ({value.GetType().Name.ToSpaced()})",
                    Tag = Definition
                });

                foreach (var fieldInfo in enumerator)
                {
                    if (fieldInfo.Attribute.Flags.HasFlag(TagFieldFlags.Padding))
                        continue;

                    var fieldType = fieldInfo.FieldInfo.FieldType;
                    var isTagBlock = (fieldType.IsGenericType && fieldType.GetGenericTypeDefinition() == typeof(List<>));
                    var isStruct = (fieldType.GetCustomAttributes(typeof(TagStructureAttribute), false).FirstOrDefault() as TagStructureAttribute ?? null) != null;

                    if (isTagBlock || isStruct)
                    {
                        var treeNode = new TreeNode
                        {
                            Text = fieldInfo.FieldInfo.Name.ToSpaced(),
                            Tag = fieldInfo.FieldInfo
                        };

                        if (isTagBlock)
                            treeNode.Text += $" ({((fieldInfo.FieldInfo.GetValue(value) as IList)?.Count ?? 0)})";

                        treeView.Nodes.Add(treeNode);
                    }
                }

                enumerator = TagStructure.GetTagFieldEnumerable(new TagStructureInfo(value.GetType(), Cache.Version));
            }

            foreach (var fieldInfo in enumerator)
                if (FieldControls.ContainsKey(fieldInfo.FieldInfo.Name))
                    FieldControls[fieldInfo.FieldInfo.Name].GetFieldValue(value, null, definition);
        }

        public void SetFieldValue(object owner, object value = null, object definition = null)
        {
            var enumerator = TagStructure.GetTagFieldEnumerable(new TagStructureInfo(value.GetType(), Cache.Version));

            foreach (var fieldInfo in enumerator)
                if (FieldControls.ContainsKey(fieldInfo.FieldInfo.Name))
                    FieldControls[fieldInfo.FieldInfo.Name].SetFieldValue(value);
        }

        private Dictionary<string, IFieldControl> CreateFieldControls(CacheForm form, Control parent, FieldInfo field, Type type, bool toplevel)
        {
            Enabled = false;

            var result = new Dictionary<string, IFieldControl>();
            var enumerator = TagStructure.GetTagFieldEnumerable(new TagStructureInfo(type, Cache.Version));

            var currentLocation = new Point(parent is GroupBox ? 3 : 0, parent is GroupBox ? 16 : 0);

            foreach (var fieldInfo in enumerator)
            {
                if (fieldInfo.Attribute.Flags.HasFlag(TagFieldFlags.Padding))
                    continue;

                var fieldType = fieldInfo.FieldInfo.FieldType;
                var isFlagsEnum = (fieldType.GetCustomAttributes(typeof(FlagsAttribute), false).FirstOrDefault() as FlagsAttribute ?? null) != null;
                var isTagBlock = (fieldType.IsGenericType && fieldType.GetGenericTypeDefinition() == typeof(List<>));
                var isStruct = (fieldType.GetCustomAttributes(typeof(TagStructureAttribute), false).FirstOrDefault() as TagStructureAttribute ?? null) != null;
                var isBounds = (fieldType.IsGenericType && fieldType.GetGenericTypeDefinition() == typeof(Bounds<>));

                if (toplevel && (isStruct || isTagBlock))
                    continue;

                if (field != null && field != fieldInfo.FieldInfo)
                    continue;

                Control control = null;

                if (fieldType == typeof(StringId))
                {
                    control = new StringIdControl(Cache, fieldInfo.FieldInfo);
                }
                else if (fieldType.IsEnum)
                {
                    if (isFlagsEnum)
                    {
                        control = new FlagsControl(fieldInfo.FieldInfo);
                    }
                    else
                    {
                        control = new EnumControl(fieldInfo.FieldInfo);
                    }
                }
                else if (fieldType == typeof(Point2d))
                {
                    control = new Point2dControl(Cache, fieldInfo.FieldInfo);
                }
                else if (fieldType == typeof(Rectangle2d))
                {
                    control = new Rectangle2dControl(Cache, fieldInfo.FieldInfo);
                }
                else if (fieldType == typeof(ArgbColor))
                {
                    control = new ArgbColorControl(Cache, fieldInfo.FieldInfo);
                }
                else if (fieldType == typeof(RealPoint2d))
                {
                    control = new RealPoint2dControl(Cache, fieldInfo.FieldInfo);
                }
                else if (fieldType == typeof(RealPoint3d))
                {
                    control = new RealPoint3dControl(Cache, fieldInfo.FieldInfo);
                }
                else if (fieldType == typeof(RealVector2d))
                {
                    control = new RealVector2dControl(Cache, fieldInfo.FieldInfo);
                }
                else if (fieldType == typeof(RealVector3d))
                {
                    control = new RealVector3dControl(Cache, fieldInfo.FieldInfo);
                }
                else if (fieldType == typeof(RealQuaternion))
                {
                    control = new RealQuaternionControl(Cache, fieldInfo.FieldInfo);
                }
                else if (fieldType == typeof(RealRgbColor))
                {
                    control = new RealRgbColorControl(Cache, fieldInfo.FieldInfo);
                }
                else if (fieldType == typeof(RealArgbColor))
                {
                    control = new RealArgbColorControl(Cache, fieldInfo.FieldInfo);
                }
                else if (fieldType == typeof(RealEulerAngles2d))
                {
                    control = new RealEulerAngles2dControl(Cache, fieldInfo.FieldInfo);
                }
                else if (fieldType == typeof(RealEulerAngles3d))
                {
                    control = new RealEulerAngles3dControl(Cache, fieldInfo.FieldInfo);
                }
                else if (fieldType == typeof(RealPlane2d))
                {
                    control = new RealPlane2dControl(Cache, fieldInfo.FieldInfo);
                }
                else if (fieldType == typeof(RealPlane3d))
                {
                    control = new RealPlane3dControl(Cache, fieldInfo.FieldInfo);
                }
                else if (fieldType == typeof(CachedTag))
                {
                    control = new TagReferenceControl(form, Cache, fieldInfo.FieldInfo);
                }
                else if (isTagBlock)
                {
                    control = new BlockControl(form, Cache, fieldType, fieldInfo.FieldInfo);

                    if (((BlockControl)control).Struct.FieldControls.Count == 0)
                        continue;
                }
                else if (isStruct)
                {
                    control = new StructControl(form, Cache, fieldType, fieldInfo.FieldInfo);

                    if (((StructControl)control).FieldControls.Count == 0)
                        continue;
                }
                else if (isBounds)
                {
                    control = new BoundsControl(Cache, fieldInfo.FieldInfo);
                }
                else
                {
                    control = new ValueControl(Cache, fieldInfo.FieldInfo);
                }

                control.Location = new Point(currentLocation.X, currentLocation.Y);
                parent.Controls.Add(control);

                currentLocation.Y = control.Bottom;

                if (parent is GroupBox gb && gb.AutoSize == false)
                {
                    parent.Width = control.Right + 8;
                    parent.Height = control.Bottom + 6;
                }

                if (isStruct || isTagBlock)
                    control.BringToFront();

                result[fieldInfo.FieldInfo.Name] = (IFieldControl)control;

                Application.DoEvents();
            }

            Enabled = true;

            return result;
        }
    }
}