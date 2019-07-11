using TagTool.Cache;
using TagTool.Common;
using TagTool.Serialization;
using TagTool.Tags.Definitions;
using Sentinel.Forms;
using System;
using System.Collections;
using System.Reflection;
using System.Windows.Forms;
using TagTool.Tags;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;

namespace Sentinel.Controls
{
    public partial class ArrayControl : UserControl, IFieldControl
    {
        public HaloOnlineCacheContext CacheContext { get; }
        public FieldInfo Field { get; } = null;
        public UserControl FieldControl { get; } = null;
        public object Definition { get; set; } = null;
        public object Owner { get; set; } = null;

        public ArrayControl()
        {
            InitializeComponent();
        }

        public ArrayControl(CacheForm form, HaloOnlineCacheContext cacheContext, Type type, FieldInfo field) :
            this()
        {
            CacheContext = cacheContext;
            Field = field;

            groupBox.Text = field.Name.ToSpaced().Replace("_", "");
            new ToolTip().SetToolTip(groupBox, $"{groupBox.Text} Block {CacheForm.GetDocumentation(field)}");

            var elementType = type.GetElementType();
            var fieldAttr = field.GetCustomAttributes<TagFieldAttribute>()
                .Where(x => (x.Version != CacheVersion.Unknown && cacheContext.Version == x.Version) || CacheVersionDetection.IsBetween(cacheContext.Version, x.MinVersion, x.MaxVersion))
                .FirstOrDefault();

            if (!fieldAttr.Flags.HasFlag(TagFieldFlags.Padding))
            {
                var fieldType = elementType;
                var isFlagsEnum = (fieldType.GetCustomAttributes(typeof(FlagsAttribute), false).FirstOrDefault() as FlagsAttribute ?? null) != null;
                var isTagBlock = (fieldType.IsGenericType && fieldType.GetGenericTypeDefinition() == typeof(List<>));
                var isStruct = (fieldType.GetCustomAttributes(typeof(TagStructureAttribute), false).FirstOrDefault() as TagStructureAttribute ?? null) != null;
                var isBounds = (fieldType.IsGenericType && fieldType.GetGenericTypeDefinition() == typeof(Bounds<>));

                UserControl control = null;

                if (fieldType == typeof(StringId))
                {
                    control = new StringIdControl(CacheContext, field);
                }
                else if (fieldType.IsEnum)
                {
                    if (isFlagsEnum)
                    {
                        control = new FlagsControl(field);
                    }
                    else
                    {
                        control = new EnumControl(field);
                    }
                }
                else if (fieldType.IsArray && fieldAttr.Length != 0 && !fieldAttr.Flags.HasFlag(TagFieldFlags.Padding))
                {
                    control = new ArrayControl(form, CacheContext, fieldType, field);
                }
                else if (fieldType == typeof(Point2d))
                {
                    control = new Point2dControl(CacheContext, field);
                }
                else if (fieldType == typeof(Rectangle2d))
                {
                    control = new Rectangle2dControl(CacheContext, field);
                }
                else if (fieldType == typeof(ArgbColor))
                {
                    control = new ArgbColorControl(CacheContext, field);
                }
                else if (fieldType == typeof(RealPoint2d))
                {
                    control = new RealPoint2dControl(CacheContext, field);
                }
                else if (fieldType == typeof(RealPoint3d))
                {
                    control = new RealPoint3dControl(CacheContext, field);
                }
                else if (fieldType == typeof(RealVector2d))
                {
                    control = new RealVector2dControl(CacheContext, field);
                }
                else if (fieldType == typeof(RealVector3d))
                {
                    control = new RealVector3dControl(CacheContext, field);
                }
                else if (fieldType == typeof(RealQuaternion))
                {
                    control = new RealQuaternionControl(CacheContext, field);
                }
                else if (fieldType == typeof(RealRgbColor))
                {
                    control = new RealRgbColorControl(CacheContext, field);
                }
                else if (fieldType == typeof(RealArgbColor))
                {
                    control = new RealArgbColorControl(CacheContext, field);
                }
                else if (fieldType == typeof(RealEulerAngles2d))
                {
                    control = new RealEulerAngles2dControl(CacheContext, field);
                }
                else if (fieldType == typeof(RealEulerAngles3d))
                {
                    control = new RealEulerAngles3dControl(CacheContext, field);
                }
                else if (fieldType == typeof(RealPlane2d))
                {
                    control = new RealPlane2dControl(CacheContext, field);
                }
                else if (fieldType == typeof(RealPlane3d))
                {
                    control = new RealPlane3dControl(CacheContext, field);
                }
                else if (fieldType == typeof(CachedTagInstance))
                {
                    control = new TagReferenceControl(form, CacheContext, field);
                }
                else if (isTagBlock)
                {
                    control = new BlockControl(form, CacheContext, fieldType, field);

                    if (((BlockControl)control).Struct.FieldControls.Count == 0)
                        control = new UserControl();
                }
                else if (isStruct)
                {
                    control = new StructControl(form, CacheContext, fieldType, field);

                    if (((StructControl)control).FieldControls.Count == 0)
                        control = new UserControl();
                }
                else if (isBounds)
                {
                    control = new BoundsControl(CacheContext, field);
                }
                else
                {
                    control = new ValueControl(CacheContext, field);
                }

                groupBox.Controls.Add(control);

                control.Location = new Point(3, tableLayoutPanel1.Bottom + 3);

                groupBox.AutoSize = true;
                groupBox.AutoSizeMode = AutoSizeMode.GrowAndShrink;

                if (isStruct || isTagBlock)
                    control.Dock = DockStyle.Fill;

                control.BringToFront();
                FieldControl = control;
            }

            Height = groupBox.Height;
        }
        
        public void GetFieldValue(object owner, object value = null, object definition = null)
        {
            if (value == null)
                value = Field.GetValue(owner);

            if (value == null)
                value = Activator.CreateInstance(Field.FieldType);

            Owner = owner;
            elementsComboBox.Items.Clear();

            Definition = definition;

            var elements = value as Array;

            FieldInfo labelField = null;

            if (Field.FieldType.IsSubclassOf(typeof(TagStructure)))
            {
                var enumerator = TagStructure.GetTagFieldEnumerable(new TagStructureInfo((Field?.FieldType ?? value.GetType()).GenericTypeArguments[0], CacheContext.Version));
                foreach (var fieldInfo in enumerator)
                {
                    if (fieldInfo.Attribute.Flags.HasFlag(TagFieldFlags.Label))
                    {
                        labelField = fieldInfo.FieldInfo;
                        break;
                    }
                }

                int i = 0;
                foreach (var element in elements)
                {
                    var label = "";

                    if (labelField != null)
                    {
                        if (labelField.FieldType == typeof(string))
                            label = (string)labelField.GetValue(element);
                        else if (labelField.FieldType == typeof(StringId))
                            label = CacheContext.GetString((StringId)labelField.GetValue(element));
                        else if (labelField.FieldType.IsEnum)
                            label = labelField.GetValue(element).ToString().ToSnakeCase();
                        else if (labelField.FieldType == typeof(short))
                        {
                            if (definition != null)
                            {
                                if (definition.GetType() == typeof(Scenario))
                                {
                                    var index = (short)labelField.GetValue(element);
                                    var names = ((Scenario)definition).ObjectNames;

                                    if (index >= 0 && index < names.Count)
                                        label = ((Scenario)definition).ObjectNames[index].Name;
                                }
                            }
                        }
                        else if (labelField.FieldType == typeof(CachedTagInstance))
                        {
                            var instance = (CachedTagInstance)labelField.GetValue(element);

                            if (instance != null)
                            {
                                var tagName = instance.Name ?? $"0x{instance.Index:X4}";

                                if (tagName.Contains("\\"))
                                {
                                    var index = tagName.LastIndexOf('\\') + 1;
                                    tagName = tagName.Substring(index, tagName.Length - index);
                                }

                                label = tagName;
                            }
                        }
                    }

                    elementsComboBox.Items.Add(new ArrayElement(i, label, element));
                    i++;
                }
            }
            else
            {
                int i = 0;
                foreach (var element in elements)
                    elementsComboBox.Items.Add(new ArrayElement(i++, "", element));
            }

            if (elementsComboBox.Items.Count > 0)
                elementsComboBox.SelectedIndex = 0;
            else
            {
                FieldControl.Enabled = false;
            }
        }

        public void SetFieldValue(object owner, object value = null, object definition = null)
        {
            if (value == null)
            {
                var elements = Activator.CreateInstance(Field.FieldType) as IList;

                for (var i = 0; i < elementsComboBox.Items.Count; i++)
                {
                    var element = elementsComboBox.Items[i] as ArrayElement;
                    if (FieldControl is IFieldControl)
                        ((IFieldControl)FieldControl).SetFieldValue(null, element.Value);
                    elements.Add(element.Value);
                }

                if (elements.Count > 0)
                    value = elements;
            }
            else
            {
                var elements = value as IList;

                elementsComboBox.Items.Clear();

                FieldInfo labelField = null;

                var enumerator = TagStructure.GetTagFieldEnumerable(new TagStructureInfo((Field?.FieldType ?? value.GetType()).GenericTypeArguments[0], CacheContext.Version));
                foreach (var fieldInfo in enumerator)
                {
                    if (fieldInfo.Attribute.Flags.HasFlag(TagFieldFlags.Label))
                    {
                        labelField = fieldInfo.FieldInfo;
                        break;
                    }
                }

                for (var i = 0; i < elements.Count; i++)
                {
                    var label = "";

                    if (labelField != null)
                    {
                        if (labelField.FieldType == typeof(string))
                            label = (string)labelField.GetValue(elements[i]);
                        else if (labelField.FieldType == typeof(StringId))
                            label = CacheContext.GetString((StringId)labelField.GetValue(elements[i]));
                        else if (labelField.FieldType.IsEnum)
                            label = labelField.GetValue(elements[i]).ToString().ToSnakeCase();
                        else if (labelField.FieldType == typeof(short))
                        {
                            if (definition != null)
                            {
                                if (definition.GetType() == typeof(Scenario))
                                {
                                    var index = (short)labelField.GetValue(elements[i]);
                                    var names = ((Scenario)definition).ObjectNames;

                                    if (index >= 0 && index < names.Count)
                                        label = ((Scenario)definition).ObjectNames[index].Name;
                                }
                            }
                        }
                        else if (labelField.FieldType == typeof(CachedTagInstance))
                        {
                            var instance = (CachedTagInstance)labelField.GetValue(elements[i]);

                            if (instance != null)
                            {
                                var tagName = instance.Name ?? $"0x{instance.Index:X4}";

                                if (tagName.Contains("\\"))
                                {
                                    var index = tagName.LastIndexOf('\\') + 1;
                                    tagName = tagName.Substring(index, tagName.Length - index);
                                }

                                label = tagName;
                            }
                        }
                    }

                    elementsComboBox.Items.Add(new ArrayElement(i, label, elements[i]));
                }
            }
            
            if (owner != null)
                Field?.SetValue(owner, value);
        }

        private class ArrayElement
        {
            public int Index { get; }
            public string Label { get; }
            public object Value { get; }

            public ArrayElement(int index, string label, object value)
            {
                Index = index;
                Label = label;
                Value = value;
            }

            public override string ToString()
            {
                return $"{Index}. {Label}";
            }
        }
        
        private void elementsComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (FieldControl is IFieldControl)
                ((IFieldControl)FieldControl).GetFieldValue(null, ((ArrayElement)elementsComboBox.SelectedItem).Value);
            FieldControl.Enabled = (elementsComboBox.Items.Count != 0);
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            var elements = Activator.CreateInstance(Field.FieldType) as IList;

            foreach (ArrayElement element in elementsComboBox.Items)
                elements.Add(element.Value);

            elements.Add(Activator.CreateInstance(Field.FieldType.GenericTypeArguments[0]));

            SetFieldValue(Owner, elements);

            elementsComboBox.SelectedIndex = elementsComboBox.Items.Count - 1;
        }

        private void insertButton_Click(object sender, EventArgs e)
        {
            var elements = Activator.CreateInstance(Field.FieldType) as IList;

            foreach (ArrayElement element in elementsComboBox.Items)
                elements.Add(element.Value);

            if (elementsComboBox.SelectedItem != null)
            {
                var index = elementsComboBox.SelectedIndex;

                elements.Insert(index, Activator.CreateInstance(Field.FieldType.GenericTypeArguments[0]));

                SetFieldValue(Owner, elements);

                elementsComboBox.SelectedIndex = index;
            }
            else
            {
                elements.Add(Activator.CreateInstance(Field.FieldType.GenericTypeArguments[0]));
                elementsComboBox.SelectedIndex = elementsComboBox.Items.Count - 1;
            }
        }

        private void removeButton_Click(object sender, EventArgs e)
        {
            var elements = Activator.CreateInstance(Field.FieldType) as IList;

            foreach (ArrayElement element in elementsComboBox.Items)
                if (!element.Equals(elementsComboBox.SelectedItem))
                    elements.Add(element.Value);
            
            if (elementsComboBox.SelectedItem != null)
            {
                var index = elementsComboBox.SelectedIndex;

                SetFieldValue(Owner, elements);

                elementsComboBox.SelectedIndex = Math.Min(index, elementsComboBox.Items.Count - 1);
            }
        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            SetFieldValue(Owner, Activator.CreateInstance(Field.FieldType));
        }

        private void collapseButton_Click(object sender, EventArgs e)
        {

        }
    }
}