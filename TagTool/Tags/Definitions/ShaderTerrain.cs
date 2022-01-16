using TagTool.Common;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "shader_terrain", Tag = "rmtr", Size = 0x1C)]
    public class ShaderTerrain : RenderMethod
    {
        [TagField(Flags = TagFieldFlags.GlobalMaterial, Length = 4)]
        public StringId[] MaterialNames;

        [TagField(Flags = TagFieldFlags.GlobalMaterial, Length = 4)]
        public short[] GlobalMaterialIndices;

        public uint SingleMaterial;
    }
}