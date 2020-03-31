using TagTool.Tags;

namespace TagTool.Shaders
{
    [TagStructure(Size = 0x2)]
    public class ShaderDrawMode : TagStructure
	{
        public byte Offset;
        public byte Count;
    }

    public enum ShaderDrawModes
    {
        Default,
        Albedo,
        StaticDefault,
        StaticPerPixel,
        StaticPerVertex,
        StaticSH,
        StaticPrtAmbient,
        StaticPrtLinear,
        StaticPrtQuadratic,
        DynamicLight,
        ShadowGenerate,
        ShadowApply,
        ActiveCamo,
        LightmapDebugMode,
        StaticPerVertexColor,
        WaterTesselation,
        WaterShading,
        DynamicLightCinematic,
        ZOnly,
        SfxDistort,
        // find others
    }
}