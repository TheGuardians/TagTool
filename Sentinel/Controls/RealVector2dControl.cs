using TagTool.Cache;
using TagTool.Common;
using Sentinel.Forms;
using System;
using System.Reflection;
using System.Windows.Forms;

namespace Sentinel.Controls
{
    public partial class RealVector2dControl : UserControl, IFieldControl
    {
        public HaloOnlineCacheContext CacheContext { get; }
        public FieldInfo Field { get; }
        public bool Loading { get; set; } = false;
        public object Owner { get; set; } = null;

        public RealVector2dControl()
        {
            InitializeComponent();
        }

        public RealVector2dControl(HaloOnlineCacheContext cacheContext, FieldInfo field) :
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

            var point = (RealVector2d)value;

            iTextBox.Text = point.I.ToString();
            jTextBox.Text = point.J.ToString();

            Loading = false;
        }

        public void SetFieldValue(object owner, object value = null, object definition = null)
        {
            if (Loading || owner == null)
                return;

            if (value == null)
            {
                if (!float.TryParse(iTextBox.Text, out var i) ||
                    !float.TryParse(jTextBox.Text, out var j))
                    return;

                value = new RealVector2d(i, j);
            }

            Field.SetValue(owner, value);
        }

        private void valueTextBox_TextChanged(object sender, EventArgs e)
        {
            SetFieldValue(Owner);
        }
    }
}