using TagTool.Cache;
using TagTool.Common;
using Sentinel.Forms;
using System;
using System.Reflection;
using System.Windows.Forms;

namespace Sentinel.Controls
{
    public partial class RealEulerAngles3dControl : UserControl, IFieldControl
    {
        public GameCache Cache { get; }
        public FieldInfo Field { get; }
        public bool Loading { get; set; } = false;
        public object Owner { get; set; } = null;

        public RealEulerAngles3dControl()
        {
            InitializeComponent();
        }

        public RealEulerAngles3dControl(GameCache cache, FieldInfo field) :
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

            var angles = (RealEulerAngles3d)value;

            yawTextBox.Text = angles.Yaw.Degrees.ToString();
            pitchTextBox.Text = angles.Pitch.Degrees.ToString();
            rollTextBox.Text = angles.Roll.Degrees.ToString();

            Loading = false;
        }

        public void SetFieldValue(object owner, object value = null, object definition = null)
        {
            if (Loading || owner == null)
                return;

            if (value == null)
            {
                if (!float.TryParse(yawTextBox.Text, out var yaw) ||
                    !float.TryParse(pitchTextBox.Text, out var pitch) ||
                    !float.TryParse(rollTextBox.Text, out var roll))
                    return;

                value = new RealEulerAngles3d(Angle.FromDegrees(yaw), Angle.FromDegrees(pitch), Angle.FromDegrees(roll));
            }

            Field.SetValue(owner, value);
        }

        private void valueTextBox_TextChanged(object sender, EventArgs e)
        {
            SetFieldValue(Owner);
        }
    }
}