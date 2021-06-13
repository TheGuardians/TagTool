using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "atmosphere_globals", Tag = "atgf", Size = 0x44)]
    public class AtmosphereGlobals : TagStructure
    {
        [TagField(ValidTags = new [] { "bitm" })]
        public CachedTag FogBitmap;
        public float TextureRepeatRate;
        public float DistanceBetweenSheets;
        public float DepthFadeFactor;
        public float TransparentSortDistance;
        public GlobalSortLayerEnumDefintion TransparentSortLayer;
        [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
        public byte[] Padding;
        public ScalarFunctionNamedStructDefaultOne WindStrengthAcrossDistance;
        public List<UnderwaterSettingBlock> UnderwaterSettings;
        
        public enum GlobalSortLayerEnumDefintion : sbyte
        {
            Invalid,
            PrePass,
            Normal,
            PostPass
        }
        
        [TagStructure(Size = 0x14)]
        public class ScalarFunctionNamedStructDefaultOne : TagStructure
        {
            public MappingFunctionDefaultOne Function;
            
            [TagStructure(Size = 0x14)]
            public class MappingFunctionDefaultOne : TagStructure
            {
                public byte[] Data;
            }
        }
        
        [TagStructure(Size = 0x14)]
        public class UnderwaterSettingBlock : TagStructure
        {
            public StringId Name;
            public float Murkiness;
            public RealRgbColor FogColor;
        }
    }
}
