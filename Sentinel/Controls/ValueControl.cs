using System;
using System.Windows.Forms;
using System.Reflection;
using TagTool.Common;
using TagTool.Cache;
using Sentinel.Forms;
using System.Drawing;

namespace Sentinel.Controls
{
    public partial class ValueControl : UserControl, IFieldControl
    {
        public GameCache Cache { get; }
        public FieldInfo Field { get; }
        public object Owner { get; set; } = null;
        public bool Loading { get; set; } = false;

        public ValueControl()
        {
            InitializeComponent();
        }

        public ValueControl(GameCache cache, FieldInfo field) :
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

            if (value == null)
            {
                textBox1.Text = "<null>";
                Loading = false;
                return;
            }

            var type = value.GetType();

            if (type == typeof(StringId))
            {
                textBox1.Text = Cache.StringTable.GetString((StringId)value);
            }
            else if (type == typeof(Angle))
            {
                textBox1.Text = ((Angle)value).Degrees.ToString();
            }
            else
            {
                textBox1.Text = value.ToString();
            }

            Loading = false;
        }

        public void SetFieldValue(object owner, object value = null, object definition = null)
        {
            if (Loading || owner == null)
                return;

            if (value == null)
            {
                if (TryParseValue(Cache, Field.FieldType, textBox1.Text, out value))
                {
                    textBox1.ForeColor = SystemColors.WindowText;
                }
                else
                {
                    textBox1.ForeColor = Color.Red;
                    return;
                }
            }

            if (value != null)
                Field.SetValue(owner, value);
        }

        public static bool TryParseValue(GameCache cache, Type type, string value, out object result)
        {
            if (type == typeof(Tag))
            {
                if (!cache.TryParseGroupTag(value, out var tag))
                    goto end;
            }
            else if (type == typeof(StringId))
            {
                try
                {
                    result = cache.StringTable.GetStringId(value);
                    return true;
                }
                catch (Exception)
                {
                    goto end;
                }
            }
            else if (type == typeof(sbyte))
            {
                if (!sbyte.TryParse(value, out var s))
                    goto end;
                result = s;
                return true;
            }
            else if (type == typeof(byte))
            {
                if (!byte.TryParse(value, out var s))
                    goto end;
                result = s;
                return true;
            }
            else if (type == typeof(short))
            {
                if (!short.TryParse(value, out var s))
                    goto end;
                result = s;
                return true;
            }
            else if (type == typeof(ushort))
            {
                if (!ushort.TryParse(value, out var s))
                    goto end;
                result = s;
                return true;
            }
            else if (type == typeof(int))
            {
                if (!int.TryParse(value, out var s))
                    goto end;
                result = s;
                return true;
            }
            else if (type == typeof(uint))
            {
                if (!uint.TryParse(value, out var s))
                    goto end;
                result = s;
                return true;
            }
            else if (type == typeof(long))
            {
                if (!long.TryParse(value, out var s))
                    goto end;
                result = s;
                return true;
            }
            else if (type == typeof(ulong))
            {
                if (!ulong.TryParse(value, out var s))
                    goto end;
                result = s;
                return true;
            }
            else if (type == typeof(float))
            {
                if (!float.TryParse(value, out var s))
                    goto end;
                result = s;
                return true;
            }
            else if (type == typeof(Angle))
            {
                if (!float.TryParse(value, out var s))
                    goto end;
                result = Angle.FromDegrees(s);
                return true;
            }
            else goto end;

        end:
            result = null;
            return false;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            SetFieldValue(Owner);
        }
    }
}