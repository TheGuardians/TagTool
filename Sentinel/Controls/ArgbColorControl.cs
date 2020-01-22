using TagTool.Cache;
using TagTool.Common;
using Sentinel.Forms;
using System;
using System.Reflection;
using System.Windows.Forms;

namespace Sentinel.Controls
{
    public partial class ArgbColorControl : UserControl, IFieldControl
    {
        public GameCache Cache { get; }
        public FieldInfo Field { get; }
        public object Owner { get; set; } = null;
        public bool Loading { get; set; } = false;

        public ArgbColorControl()
        {
            InitializeComponent();
        }

        public ArgbColorControl(GameCache cache, FieldInfo field) :
            this()
        {
            Cache = cache;
            Field = field;
            label1.Text = field.Name.ToSpaced().Replace("_", "");
            new ToolTip().SetToolTip(label1, $"{field.FieldType} {CacheForm.GetDocumentation(field)}");
        }

        public void GetFieldValue(object owner, object value = null, object definition = null)
        {
            if (value == null)
                value = Field.GetValue(owner);

            Owner = owner;

            var point = (ArgbColor)value;

            Loading = true;
            alphaTextBox.Text = point.Alpha.ToString();
            redTextBox.Text = point.Red.ToString();
            greenTextBox.Text = point.Green.ToString();
            blueTextBox.Text = point.Blue.ToString();
            Loading = false;

            colorButton.BackColor = System.Drawing.Color.FromArgb(point.Red, point.Green, point.Blue);
        }

        public void SetFieldValue(object owner, object value = null, object definition = null)
        {
            if (Loading || owner == null)
                return;

            if (value == null)
            {
                if (!byte.TryParse(alphaTextBox.Text, out var alpha) ||
                    !byte.TryParse(redTextBox.Text, out var red) ||
                    !byte.TryParse(greenTextBox.Text, out var green) ||
                    !byte.TryParse(blueTextBox.Text, out var blue))
                    return;

                value = new ArgbColor(alpha, red, green, blue);
            }

            Field.SetValue(owner, value);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (var colorPicker = new ColorDialog())
            {
                colorPicker.AnyColor = true;
                colorPicker.FullOpen = true;

                if (colorPicker.ShowDialog() != DialogResult.OK)
                    return;

                alphaTextBox.Text = colorPicker.Color.A.ToString();
                redTextBox.Text = colorPicker.Color.R.ToString();
                greenTextBox.Text = colorPicker.Color.G.ToString();
                blueTextBox.Text = colorPicker.Color.B.ToString();

                colorButton.BackColor = colorPicker.Color;
            }
        }

        private void valueTextBox_TextChanged(object sender, EventArgs e)
        {
            SetFieldValue(Owner);
        }
    }
}