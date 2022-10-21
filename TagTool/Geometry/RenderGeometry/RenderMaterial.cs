using TagTool.Cache;
using TagTool.Common;
using TagTool.Tags;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Geometry
{
    /// <summary>
    /// A material describing how a mesh part should be rendered.
    /// </summary>
    [TagStructure(Size = 0x20, MaxVersion = CacheVersion.Halo2Vista)]
    [TagStructure(Size = 0x24, MaxVersion = CacheVersion.HaloOnline604673)]
    [TagStructure(Size = 0x30, MaxVersion = CacheVersion.HaloOnline700123)]
    [TagStructure(Size = 0x2C, MinVersion = CacheVersion.HaloReach)]
    public class RenderMaterial : TagStructure
	{
        /// <summary>
        /// The OLD render method tag to use to render the material.
        /// </summary>
        [TagField(Flags = Label, MaxVersion = CacheVersion.Halo2Vista)]
        public CachedTag OldRenderMethod;

        /// <summary>
        /// The render method tag to use to render the material.
        /// </summary>
        [TagField(Flags = Label)]
        public CachedTag RenderMethod;

        [TagField(MinVersion = CacheVersion.HaloOnline700123, MaxVersion = CacheVersion.HaloOnline700123)]
        public List<Skin> Skins;

        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public List<Property> Properties;

        public int ImportedMaterialIndex;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public float LightmapResolutionScale;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public ArgbColor LightmapAdditiveTransparencyColor;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public ArgbColor LightmapTraslucencyTintColor;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public float LightmapAnalyticalLightAbsorb;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public float LightmapNormalLightAbsorb;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public byte LightmapFlags;

        public sbyte BreakableSurfaceIndex;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public short LightmapChartGroupIndex;

        [TagField(Length = 3, Flags = TagFieldFlags.Padding, MaxVersion = CacheVersion.HaloOnline700123)]
        public byte[] Padding1;

        [TagStructure(Size = 0x14)]
        public class Skin : TagStructure
		{
            [TagField(Flags = Label)]
            public StringId Name;
            public CachedTag RenderMethod;
        }

      

        [TagStructure(Size = 0x8, MaxVersion = CacheVersion.Halo2Vista)]
        [TagStructure(Size = 0xC, MinVersion = CacheVersion.Halo3Retail)]
        public class Property : TagStructure
		{
            public PropertyType Type;

            [TagField(Flags = TagFieldFlags.Padding, Length = 2, MaxVersion = CacheVersion.Halo3ODST)]
            public byte[] Padding;
            [TagField(MinVersion = CacheVersion.HaloOnlineED)]
            public ushort Unknown1;

            [TagField(MaxVersion = CacheVersion.Halo2Vista)]
            public short ShortValue;

            [TagField(MinVersion = CacheVersion.Halo3Retail)]
            public int IntValue;

            public float RealValue;

            public enum PropertyType : short
            {
                LightmapResolution,
                LightmapPower,
                LightmapHalfLife,
                LightmapDiffuseScale,
                LightmapPhotonFidelity,
                LightmapTranslucencyTintColor,
                LightmapTransparencyOverride,
                LightmapAdditiveTransparency,
                LightmapIgnoreDefaultResScale
            }
        }
    }
}