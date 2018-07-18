using TagTool.Cache;
using TagTool.Common;
using TagTool.Serialization;
using System.Collections.Generic;
using System;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "light_volume_system", Tag = "ltvl", Size = 0xC, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Name = "light_volume_system", Tag = "ltvl", Size = 0x14, MinVersion = CacheVersion.HaloOnline106708)]
    public class LightVolumeSystem
    {
        public List<LightVolumeSystemBlock> LightVolume;

        [TagField(Padding = true, Length = 8, MinVersion = CacheVersion.HaloOnline106708)]
        public byte[] Unused1;

        [Flags]
        public enum LightVolumeFlags : ushort
        {
            None = 0,
            Bit0 = 1 << 0,
            Bit1 = 1 << 1,
            Bit2 = 1 << 2,
            Bit3 = 1 << 3,
            Bit4 = 1 << 4,
            Bit5 = 1 << 5,
            Bit6 = 1 << 6,
            Bit7 = 1 << 7,
            Bit8 = 1 << 8,
            Bit9 = 1 << 9,
            Bit10 = 1 << 10,
            Bit11 = 1 << 12,
            Bit13 = 1 << 13,
            Bit14 = 1 << 14,
            Bit15 = 1 << 15
        }

        [TagStructure(Size = 0x17C)]
        public class LightVolumeSystemBlock
        {
            public StringId LightVolumeName;
            public RenderMethod RenderMethod;

            public LightVolumeFlags Flags;

            [TagField(Padding = true, Length = 2)]
            public byte[] Unused = new byte[2];

            /// <summary>
            /// Average brightness head-on/side-view.
            /// </summary>
            public float BrightnessRatio;

            public TagMapping Length;
            public TagMapping Offset;
            public TagMapping ProfileDensity;
            public TagMapping ProfileLength;
            public TagMapping ProfileThickness;
            public TagMapping ProfileColor;
            public TagMapping ProfileAlpha;
            public TagMapping ProfileIntensity;

            public uint RuntimeMConstantPerProfileProperties;
            public uint RuntimeMUsedStates;
            public uint RuntimeMMaxProfileCount;

            public List<RuntimeGpuProperty> RuntimeGpuProperties;
            public List<RuntimeGpuFunction> RuntimeGpuFunctions;
            public List<RuntimeGpuColor> RuntimeGpuColors;

            [TagStructure(Size = 0x10)]
            public class RuntimeGpuProperty
            {
                [TagField(Length = 4)]
                public float[] Values = new float[4];
            }

            [TagStructure(Size = 0x40)]
            public class RuntimeGpuFunction
            {
                [TagField(Length = 16)]
                public float[] Values = new float[16];
            }

            [TagStructure(Size = 0x10)]
            public class RuntimeGpuColor
            {
                public float Red;
                public float Green;
                public float Blue;
                public float Magnitude;
            }
        }
    }
}