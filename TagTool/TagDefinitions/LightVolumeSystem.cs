using TagTool.Cache;
using TagTool.Common;
using TagTool.Serialization;
using System.Collections.Generic;

namespace TagTool.TagDefinitions
{
    [TagStructure(Name = "light_volume_system", Tag = "ltvl", Size = 0xC, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Name = "light_volume_system", Tag = "ltvl", Size = 0x14, MinVersion = CacheVersion.HaloOnline106708)]
    public class LightVolumeSystem
    {
        public List<LightVolumeSystemBlock> LightVolume;

        [TagField(Padding = true, Length = 8, MinVersion = CacheVersion.HaloOnline106708)]
        public byte[] Unused1;

        [TagStructure(Size = 0x17C)]
        public class LightVolumeSystemBlock
        {
            public StringId Name;
            public RenderMethod RenderMethod;

            public float Unknown1;
            public float Unknown2;

            public TagMapping ShaderFunction1;
            public TagMapping ShaderFunction2;
            public TagMapping ShaderFunction3;
            public TagMapping ShaderFunction4;
            public TagMapping ShaderFunction5;
            public TagMapping ShaderFunction6;
            public TagMapping ShaderFunction7;
            public TagMapping ShaderFunction8;

            public int Unknown3;
            public int Unknown4;
            public int Unknown5;

            public List<UnknownBlock> Unknown6;
            public List<CompiledFunction> CompiledFunctions;
            public List<CompiledColorFunction> CompiledColorFunctions;

            [TagStructure(Size = 0x10)]
            public class UnknownBlock
            {
                public uint Unknown;
                public uint Unknown2;
                public uint Unknown3;
                public uint Unknown4;
            }

            [TagStructure(Size = 0x40)]
            public class CompiledFunction
            {
                public uint Unknown;
                public uint Unknown2;
                public uint Unknown3;
                public uint Unknown4;
                public uint Unknown5;
                public uint Unknown6;
                public uint Unknown7;
                public uint Unknown8;
                public uint Unknown9;
                public uint Unknown10;
                public uint Unknown11;
                public uint Unknown12;
                public uint Unknown13;
                public uint Unknown14;
                public uint Unknown15;
                public uint Unknown16;
            }

            [TagStructure(Size = 0x10)]
            public class CompiledColorFunction
            {
                public float Red;
                public float Green;
                public float Blue;
                public float Magnitude;
            }
        }
    }
}