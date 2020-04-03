using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagTool.Shaders
{
    public enum EntryPoint : sbyte
    {
        Default,
        Albedo,
        Static_Default,
        Static_Per_Pixel,
        Static_Per_Vertex,
        Static_Sh,
        Static_Prt_Ambient,
        Static_Prt_Linear,
        Static_Prt_Quadratic,
        Dynamic_Light,
        Shadow_Generate,
        Shadow_Apply,
        Active_Camo,
        Lightmap_Debug_Mode,
        Static_Per_Vertex_Color,
        Water_Tessellation,
        Water_Shading,
        Dynamic_Light_Cinematic,
        Z_Only,
        Sfx_Distort
    }

    [Flags]
    public enum EntryPointBitMask : int
    {
        Default = 1 << 0,
        Albedo = 1 << 1,
        Static_Default = 1 << 2,
        Static_Per_Pixel = 1 << 3,
        Static_Per_Vertex = 1 << 4,
        Static_Sh = 1 << 5,
        Static_Prt_Ambient = 1 << 6,
        Static_Prt_Linear = 1 << 7,
        Static_Prt_Quadratic = 1 << 8,
        Dynamic_Light = 1 << 9,
        Shadow_Generate = 1 << 10,
        Shadow_Apply = 1 << 11,
        Active_Camo = 1 << 12,
        Lightmap_Debug_Mode = 1 << 13,
        Static_Per_Vertex_Color = 1 << 14,
        Water_Tessellation = 1 << 15,
        Water_Shading = 1 << 16,
        Dynamic_Light_Cinematic = 1 << 17,
        Z_Only = 1 << 18,
        Sfx_Distort = 1 << 19,
    }
}
