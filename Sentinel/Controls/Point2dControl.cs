using TagTool.Cache;
using TagTool.Common;
using Sentinel.Forms;
using System;
using System.Reflection;
using System.Windows.Forms;

namespace Sentinel.Controls
{
    public partial class Point2dControl : UserControl, IFieldControl
    {
        public HaloOnlineCacheContext CacheContext { get; }
        public FieldInfo Field { get; }
        public bool Loading { get; set; } = false;
        public object Owner { get; set; } = null;

        public Point2dControl()
        {
            InitializeComponent();
        }

        public Point2dControl(HaloOnlineCacheContext cacheContext, FieldInfo field) :
            this()
        {
            CacheContext = cacheContext;
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

            var point = (Point2d)value;

            xTextBox.Text = point.X.ToString();
            yTextBox.Text = point.Y.ToString();

            Loading = false;
        }

        public void SetFieldValue(object owner, object value = null, object definition = null)
        {
            if (Loading || owner == null)
                return;

            if (value == null)
            {
                if (!short.TryParse(xTextBox.Text, out var x) ||
                    !short.TryParse(yTextBox.Text, out var y))
                    return;

                value = new Point2d(x, y);
            }

            Field.SetValue(owner, value);
        }

        private void valueTextBox_TextChanged(object sender, EventArgs e)
        {
            SetFieldValue(Owner);
        }
    }
}