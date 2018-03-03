using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.Shaders;

namespace TagTool.ShaderGenerator.Types
{
    public class TemplateParameter
    {
        public string Name;
        public byte Size;
        public ShaderParameter.RType Type;
        public Type Target_Type;
        public Boolean enabled = true;

        public TemplateParameter(Type target_method_type, string _name, ShaderParameter.RType _type, byte _size = 1)
        {
            Target_Type = target_method_type;
            Name = _name;
            Type = _type;
            Size = _size;
        }
    }
}
