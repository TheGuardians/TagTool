using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "shader_terrain", Tag = "rmtr", Size = 0x1C)]
    public class ShaderTerrain : RenderMethod
    {
        // defines global material type for channel 0 of the terrain shader
        public StringId MaterialName0;
        // defines global material type for channel 1 of the terrain shader
        public StringId MaterialName1;
        // defines global material type for channel 2 of the terrain shader
        public StringId MaterialName2;
        // defines global material type for channel 3 of the terrain shader
        public StringId MaterialName3;
        public MaterialTypeStruct MaterialType0;
        public MaterialTypeStruct MaterialType1;
        public MaterialTypeStruct MaterialType2;
        public MaterialTypeStruct MaterialType3;
        public int SingleMaterial;
        
        [TagStructure(Size = 0x2)]
        public class MaterialTypeStruct : TagStructure
        {
            public short GlobalMaterialIndex;
        }
    }
}
