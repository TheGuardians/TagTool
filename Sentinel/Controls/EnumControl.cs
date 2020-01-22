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
using Sentinel.Forms;

namespace Sentinel.Controls
{
    public partial class EnumControl : UserControl, IFieldControl
    {
        public FieldInfo Field { get; }
        public bool Loading { get; set; } = false;
        public object Owner { get; set; } = null;

        public EnumControl()
        {
            InitializeComponent();
        }

        public EnumControl(FieldInfo field) :
            this()
        {
            Field = field;
            label1.Text = field.Name.ToSpaced();

            new ToolTip().SetToolTip(label1, $"{field.FieldType.GetEnumUnderlyingType().Name} Enum {CacheForm.GetDocumentation(field)}");

            var names = Enum.GetNames(field.FieldType);
            var values = Enum.GetValues(field.FieldType);
            
            for (var i = 0; i < names.Length; i++)
            {
                var value = values.GetValue(i);
                itemsComboBox.Items.Add(new EnumOption(names[i].ToSpaced().Replace("_", ""), value));
            }
        }

        public void GetFieldValue(object owner, object value = null, object definition = null)
        {
            if (value == null)
                value = Field.GetValue(owner);

            Owner = owner;
            Loading = true;

            foreach (EnumOption item in itemsComboBox.Items)
            {
                if (item.Value.Equals(value))
                {
                    itemsComboBox.SelectedItem = item;
                    break;
                }
            }

            Loading = false;
        }

        public void SetFieldValue(object owner, object value = null, object definition = null)
        {
            if (Loading || owner == null)
                return;

            if (value == null)
                value = (itemsComboBox.SelectedItem as EnumOption)?.Value;

            if (value != null)
                Field.SetValue(owner, value);
        }

        private void itemsComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetFieldValue(Owner);
        }

        private class EnumOption
        {
            public string Name;
            public object Value;

            public EnumOption(string name, object value)
            {
                Name = name;
                Value = value;
            }

            public override string ToString()
            {
                return Name;
            }
        }
    }
}