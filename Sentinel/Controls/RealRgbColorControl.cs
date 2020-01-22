using TagTool.Cache;
using TagTool.Common;
using Sentinel.Forms;
using System;
using System.Reflection;
using System.Windows.Forms;

namespace Sentinel.Controls
{
    public partial class RealRgbColorControl : UserControl, IFieldControl
    {
        public GameCache Cache { get; }
        public FieldInfo Field { get; }
        public bool Loading { get; set; } = false;
        public object Owner { get; set; } = null;

        public RealRgbColorControl()
        {
            InitializeComponent();
        }

        public RealRgbColorControl(GameCache cache, FieldInfo field) :
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

            var point = (RealRgbColor)value;

            redTextBox.Text = point.Red.ToString();
            greenTextBox.Text = point.Green.ToString();
            blueTextBox.Text = point.Blue.ToString();

            colorButton.BackColor = System.Drawing.Color.FromArgb(
                (byte)(point.Red * 255.0f),
                (byte)(point.Green * 255.0f),
                (byte)(point.Blue * 255.0f));

            Loading = false;
        }

        public void SetFieldValue(object owner, object value = null, object definition = null)
        {
            if (Loading || owner == null)
                return;

            if (value == null)
            {
                if (!float.TryParse(redTextBox.Text, out var red) ||
                    !float.TryParse(greenTextBox.Text, out var green) ||
                    !float.TryParse(blueTextBox.Text, out var blue))
                    return;

                value = new RealRgbColor(red, green, blue);
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

                redTextBox.Text = (colorPicker.Color.R / 255.0f).ToString();
                greenTextBox.Text = (colorPicker.Color.G / 255.0f).ToString();
                blueTextBox.Text = (colorPicker.Color.B / 255.0f).ToString();

                colorButton.BackColor = colorPicker.Color;
            }
        }

        private void valueTextBox_TextChanged(object sender, EventArgs e)
        {
            SetFieldValue(Owner);
        }
    }
}