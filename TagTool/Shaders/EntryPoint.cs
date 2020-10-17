using System;

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

    public enum EntryPointMs30 : sbyte
    {
        Default,
        Albedo,
        Albedo_Low,
        Static_Default,
        Static_Per_Pixel,
        Static_Per_Vertex,
        Static_Sh,
        Static_Prt_Ambient,
        Static_Prt_Linear,
        Static_Prt_Quadratic,
        Static_Per_Pixel_Low,
        Static_Per_Vertex_Low,
        Static_Sh_Low,
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
    public enum EntryPointBitMaskMs30 : int
    {
        Default = 1 << 0,
        Albedo = 1 << 1,
        Albedo_Low = 1 << 2,
        Static_Default = 1 << 3,
        Static_Per_Pixel = 1 << 4,
        Static_Per_Vertex = 1 << 5,
        Static_Sh = 1 << 6,
        Static_Prt_Ambient = 1 << 7,
        Static_Prt_Linear = 1 << 8,
        Static_Prt_Quadratic = 1 << 9,
        Static_Per_Pixel_Low = 1 << 10,
        Static_Per_Vertex_Low = 1 << 11,
        Static_Sh_Low = 1 << 12,
        Dynamic_Light = 1 << 13,
        Shadow_Generate = 1 << 14,
        Shadow_Apply = 1 << 15,
        Active_Camo = 1 << 16,
        Lightmap_Debug_Mode = 1 << 17,
        Static_Per_Vertex_Color = 1 << 18,
        Water_Tessellation = 1 << 19,
        Water_Shading = 1 << 20,
        Dynamic_Light_Cinematic = 1 << 21,
        Z_Only = 1 << 22,
        Sfx_Distort = 1 << 23
    }

    public enum EntryPointReach : sbyte
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
        Single_Pass_Per_Pixel,
        Single_Pass_Per_Vertex,
        Single_Pass_Single_Probe,
        Single_Pass_Single_Probe_Ambient,
        Imposter_Static_Sh,
        Imposter_Static_Prt_Ambient,
    }

    [Flags]
    public enum EntryPointBitMaskReach : int
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
        Single_Pass_Per_Pixel = 1 << 18,
        Single_Pass_Per_Vertex = 1 << 19,
        Single_Pass_Single_Probe = 1 << 20,
        Single_Pass_Single_Probe_Ambient = 1 << 21,
        Imposter_Static_Sh = 1 << 22,
        Imposter_Static_Prt_Ambient = 1 << 23,
    }
}
