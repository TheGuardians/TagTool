using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using TagTool.Serialization;
using TagTool.Cache;
using TagTool.Common;
using Sentinel.Forms;
using TagTool.Tags;

namespace Sentinel.Controls
{
    public partial class StructControl : UserControl, IFieldControl
    {
        public GameCache Cache { get; } = null;
        public FieldInfo Field { get; set; } = null;
        public Dictionary<string, IFieldControl> FieldControls { get; set; } = new Dictionary<string, IFieldControl>();

        public StructControl()
        {
            InitializeComponent();
        }

        public StructControl(CacheForm form, GameCache cache, Type type, FieldInfo field) :
            this()
        {
            Cache = cache;
            Field = field;

            if (Field == null)
                groupBox.Visible = false;
            else
                groupBox.Text = Field.Name.ToSpaced();

            FieldControls = CreateFieldControls(form, Field == null ? (Control)this : groupBox, type);

            if (field != null)
                new ToolTip().SetToolTip(this, $"{field.FieldType.Name} {CacheForm.GetDocumentation(field)}");
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        public void GetFieldValue(object owner, object value = null, object definition = null)
        {
            if (value == null)
                value = Field.GetValue(owner);

            if (value == null)
                value = Activator.CreateInstance(Field.FieldType);

            var enumerator = TagStructure.GetTagFieldEnumerable(new TagStructureInfo(Field?.FieldType ?? value.GetType(), Cache.Version));

            foreach (var fieldInfo in enumerator)
                if (FieldControls.ContainsKey(fieldInfo.FieldInfo.Name))
                    FieldControls[fieldInfo.FieldInfo.Name].GetFieldValue(value, null, definition);
        }

        public void SetFieldValue(object owner, object value = null, object definition = null)
        {
            if (value == null && owner != null)
                value = Field.GetValue(owner);

            var enumerator = TagStructure.GetTagFieldEnumerable(new TagStructureInfo(Field?.FieldType ?? value.GetType(), Cache.Version));

            foreach (var fieldInfo in enumerator)
                if (FieldControls.ContainsKey(fieldInfo.FieldInfo.Name))
                    FieldControls[fieldInfo.FieldInfo.Name].SetFieldValue(value);

            Field?.SetValue(owner, value);
        }

        private Dictionary<string, IFieldControl> CreateFieldControls(CacheForm form, Control parent, Type type)
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