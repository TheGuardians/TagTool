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
    public partial class FlagsControl : UserControl, IFieldControl
    {
        public FieldInfo Field { get; }
        public bool Loading { get; set; } = false;
        public object Owner { get; set; } = null;

        public FlagsControl()
        {
            InitializeComponent();
        }

        public FlagsControl(FieldInfo field) :
            this()
        {
            Field = field;
            label1.Text = field.Name.ToSpaced();

            new ToolTip().SetToolTip(label1, $"{field.FieldType.GetEnumUnderlyingType().Name} Flags {CacheForm.GetDocumentation(field)}");

            var names = Enum.GetNames(field.FieldType);
            var values = Enum.GetValues(field.FieldType);

            for (var i = 1; i < names.Length; i++)
            {
                var value = values.GetValue(i);
                checkedListBox1.Items.Add(new FlagsBit(names[i].ToSpaced().Replace("_", ""), value));
            }
            
            checkedListBox1.Height = (checkedListBox1.Items.Count * 15) + 7;

            Height = label1.Height = checkedListBox1.Bottom;
        }

        public void GetFieldValue(object owner, object value = null, object definition = null)
        {
            if (value == null)
                value = Field.GetValue(owner);

            Owner = owner;
            Loading = true;

            var bits = Convert.ToInt32(value);

            if (bits == 0)
                return;

            for (var i = 0; i < checkedListBox1.Items.Count; i++)
            {
                var bit = (FlagsBit)checkedListBox1.Items[i];
                var index = Convert.ToInt32(bit.Value);

                if ((bits & index) != 0)
                    checkedListBox1.SetItemChecked(i, true);
            }

            Loading = false;
        }

        public void SetFieldValue(object owner, object value = null, object definition = null)
        {
            if (Loading || owner == null)
                return;

            if (value == null)
            {
                var values = Enum.GetValues(Field.FieldType);
                var flags = 0;

                foreach (FlagsBit bit in checkedListBox1.CheckedItems)
                    flags |= (int)bit.Value;

                value = Enum.ToObject(Field.FieldType, flags);
            }
            
            Field.SetValue(owner, value);
        }

        private void checkedListBox1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            SetFieldValue(Owner);
        }

        private struct FlagsBit
        {
            public string Name;
            public object Value;

            public FlagsBit(string name, object value)
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