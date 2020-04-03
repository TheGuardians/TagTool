using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagTool.Shaders
{
    public enum ParameterUsage
    {
        Texture,
        VS_Real,
        VS_Integer,
        VS_Boolean,
        PS_Real,
        PS_Integer,
        PS_Boolean,
        TextureExtern,
        VS_RealExtern,
        VS_IntegerExtern,
        PS_RealExtern,
        PS_IntegerExtern,
        Unused1,
        Unused2,
        Count
    }
}
