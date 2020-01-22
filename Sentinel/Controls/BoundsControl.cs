using TagTool.Cache;
using Sentinel.Forms;
using System;
using System.Reflection;
using System.Windows.Forms;

namespace Sentinel.Controls
{
    public partial class BoundsControl : UserControl, IFieldControl
    {
        public GameCache Cache { get; }
        public FieldInfo Field { get; }
        public bool Loading { get; set; } = false;
        public object Owner { get; set; } = null;

        public BoundsControl()
        {
            InitializeComponent();
        }

        public BoundsControl(GameCache cache, FieldInfo field) :
            this()
        {
            Cache = cache;
            Field = field;
            label1.Text = field.Name.ToSpaced().Replace("_", "");

            new ToolTip().SetToolTip(label1, $"{field.FieldType.GenericTypeArguments[0].Name} Bounds {CacheForm.GetDocumentation(field)}");
        }

        public void GetFieldValue(object owner, object value = null, object definition = null)
        {
            if (value == null)
                value = Field.GetValue(owner);

            Owner = owner;
            Loading = true;

            var type = value.GetType();
            var lower = type.GetProperty("Lower").GetValue(value);
            var upper = type.GetProperty("Upper").GetValue(value);

            lowerTextBox.Text = lower.ToString();
            upperTextBox.Text = upper.ToString();

            Loading = false;
        }

        public void SetFieldValue(object owner, object value = null, object definition = null)
        {
            if (Loading || owner == null)
                return;

            if (value == null)
            {
                if (!ValueControl.TryParseValue(Cache, Field.FieldType.GenericTypeArguments[0], lowerTextBox.Text, out var lower) ||
                    !ValueControl.TryParseValue(Cache, Field.FieldType.GenericTypeArguments[0], upperTextBox.Text, out var upper))
                    return;

                value = Activator.CreateInstance(Field.FieldType, new[] { lower, upper });
            }

            Field.SetValue(owner, value);
        }

        private void valueTextBox_TextChanged(object sender, EventArgs e)
        {
            SetFieldValue(Owner);
        }
    }
}