using TagTool.Cache;
using TagTool.Common;
using Sentinel.Forms;
using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace Sentinel.Controls
{
    public partial class StringIdControl : UserControl, IFieldControl
    {
        public GameCache Cache { get; }
        public FieldInfo Field { get; }
        public object Owner { get; set; } = null;
        public bool Loading { get; set; } = false;

        public StringIdControl()
        {
            InitializeComponent();
        }

        public StringIdControl(GameCache cache, FieldInfo field) :
            this()
        {
            Cache = cache;
            Field = field;
            label1.Text = field.Name.ToSpaced();

            new ToolTip().SetToolTip(label1, $"{field.FieldType.Name} {CacheForm.GetDocumentation(field)}");
        }

        public void GetFieldValue(object owner, object value = null, object definition = null)
        {
            if (value == null)
                value = Field.GetValue(owner);

            Owner = owner;
            Loading = true;

            textBox.Text = Cache.StringTable.GetString((StringId)value);

            Loading = false;
        }

        public void SetFieldValue(object owner, object value = null, object definition = null)
        {
            if (Loading || owner == null)
                return;

            if (value == null)
            {
                try
                {
                    value = Cache.StringTable.GetStringId(textBox.Text);
                    textBox.ForeColor = SystemColors.WindowText;
                }
                catch (Exception)
                {
                    textBox.ForeColor = Color.Red;
                    return;
                }
            }

            Field.SetValue(owner, value);
        }

        private void browseButton_Click(object sender, EventArgs e)
        {
            using (var sid = new StringIdDialog(Cache))
            {
                if (sid.ShowDialog() != DialogResult.OK)
                    return;

                textBox.Text = Cache.StringTable.GetString(sid.Value);
            }
        }

        private void textBox_TextChanged(object sender, EventArgs e)
        {
            SetFieldValue(Owner);
        }
    }
}