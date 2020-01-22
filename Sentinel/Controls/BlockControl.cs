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

namespace Sentinel.Controls
{
    public partial class BlockControl : UserControl, IFieldControl
    {
        public GameCache Cache { get; }
        public FieldInfo Field { get; } = null;
        public StructControl Struct { get; } = null;
        public object Definition { get; set; } = null;
        public object Owner { get; set; } = null;

        public BlockControl()
        {
            InitializeComponent();
        }

        public BlockControl(CacheForm form, GameCache cache, Type type, FieldInfo field) :
            this()
        {
            Cache = cache;
            Field = field;

            groupBox.Text = field.Name.ToSpaced().Replace("_", "");
            new ToolTip().SetToolTip(groupBox, $"{groupBox.Text} Block {CacheForm.GetDocumentation(field)}");

            Struct = new StructControl(form, Cache, type.GenericTypeArguments[0], null);
            groupBox.Controls.Add(Struct);
            Struct.Dock = DockStyle.Fill;
            Struct.BringToFront();

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

            var elements = value as IList;

            FieldInfo labelField = null;

            var enumerator = TagStructure.GetTagFieldEnumerable(new TagStructureInfo((Field?.FieldType ?? value.GetType()).GenericTypeArguments[0], Struct.Cache.Version));
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
                        label = Cache.StringTable.GetString((StringId)labelField.GetValue(elements[i]));
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
                    else if (labelField.FieldType == typeof(CachedTag))
                    {
                        var instance = (CachedTag)labelField.GetValue(elements[i]);

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

                elementsComboBox.Items.Add(new TagBlockElement(i, label, elements[i]));
            }

            if (elementsComboBox.Items.Count > 0)
                elementsComboBox.SelectedIndex = 0;
            else
            {
                Struct.Enabled = false;
            }
        }

        public void SetFieldValue(object owner, object value = null, object definition = null)
        {
            if (value == null)
            {
                var elements = Activator.CreateInstance(Field.FieldType) as IList;

                for (var i = 0; i < elementsComboBox.Items.Count; i++)
                {
                    var element = elementsComboBox.Items[i] as TagBlockElement;
                    Struct.SetFieldValue(null, element.Value);
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

                var enumerator = TagStructure.GetTagFieldEnumerable(new TagStructureInfo((Field?.FieldType ?? value.GetType()).GenericTypeArguments[0], Struct.Cache.Version));
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
                            label = Cache.StringTable.GetString((StringId)labelField.GetValue(elements[i]));
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
                        else if (labelField.FieldType == typeof(CachedTag))
                        {
                            var instance = (CachedTag)labelField.GetValue(elements[i]);

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

                    elementsComboBox.Items.Add(new TagBlockElement(i, label, elements[i]));
                }
            }
            
            if (owner != null)
                Field?.SetValue(owner, value);
        }

        private class TagBlockElement
        {
            public int Index { get; }
            public string Label { get; }
            public object Value { get; }

            public TagBlockElement(int index, string label, object value)
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
            Struct.GetFieldValue(null, ((TagBlockElement)elementsComboBox.SelectedItem).Value);
            Struct.Enabled = (elementsComboBox.Items.Count != 0);
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            var elements = Activator.CreateInstance(Field.FieldType) as IList;

            foreach (TagBlockElement element in elementsComboBox.Items)
                elements.Add(element.Value);

            elements.Add(Activator.CreateInstance(Field.FieldType.GenericTypeArguments[0]));

            SetFieldValue(Owner, elements);

            elementsComboBox.SelectedIndex = elementsComboBox.Items.Count - 1;
        }

        private void insertButton_Click(object sender, EventArgs e)
        {
            var elements = Activator.CreateInstance(Field.FieldType) as IList;

            foreach (TagBlockElement element in elementsComboBox.Items)
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

            foreach (TagBlockElement element in elementsComboBox.Items)
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