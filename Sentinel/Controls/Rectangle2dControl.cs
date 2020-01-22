using TagTool.Cache;
using TagTool.Common;
using Sentinel.Forms;
using System;
using System.Reflection;
using System.Windows.Forms;

namespace Sentinel.Controls
{
    public partial class Rectangle2dControl : UserControl, IFieldControl
    {
        public GameCache Cache { get; }
        public FieldInfo Field { get; }
        public bool Loading { get; set; } = false;
        public object Owner { get; set; } = null;

        public Rectangle2dControl()
        {
            InitializeComponent();
        }

        public Rectangle2dControl(GameCache cache, FieldInfo field) :
            this()
        {
            Cache = cache;
            Field = field;
            label1.Text = field.Name.ToSpaced().Replace("_", "");

            new ToolTip().SetToolTip(label1, $"{field.FieldType.Name} {CacheForm.GetDocumentation(field)}");
        }

        public void GetFieldValue(object owner, object value = null, object definition = null)
        {
            if (value == null)
                value = Field.GetValue(owner);

            Owner = owner;
            Loading = true;

            var rect = (Rectangle2d)value;

            topTextBox.Text = rect.Top.ToString();
            leftTextBox.Text = rect.Left.ToString();
            bottomTextBox.Text = rect.Bottom.ToString();
            rightTextBox.Text = rect.Right.ToString();

            Loading = false;
        }

        public void SetFieldValue(object owner, object value = null, object definition = null)
        {
            if (Loading || owner == null)
                return;

            if (value == null)
            {
                if (!short.TryParse(topTextBox.Text, out var top) ||
                    !short.TryParse(leftTextBox.Text, out var left) ||
                    !short.TryParse(bottomTextBox.Text, out var bottom) ||
                    !short.TryParse(rightTextBox.Text, out var right))
                    return;

                value = new Rectangle2d(top, left, bottom, right);
            }

            Field.SetValue(owner, value);
        }

        private void valueTextBox_TextChanged(object sender, EventArgs e)
        {
            SetFieldValue(Owner);
        }
    }
}