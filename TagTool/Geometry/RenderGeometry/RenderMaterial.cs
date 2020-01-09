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
    [TagStructure(Size = 0x24, MaxVersion = CacheVersion.HaloOnline571627)]
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

        [TagField(Version = CacheVersion.HaloOnline700123)]
        public List<Skin> Skins;

        public List<Property> Properties;

        public int Unknown;
        public sbyte BreakableSurfaceIndex;
        public sbyte Unknown2;
        public sbyte Unknown3;
        public sbyte Unknown4;

        [TagStructure(Size = 0x14)]
        public class Skin : TagStructure
		{
            [TagField(Flags = Label)]
            public StringId Name;
            public CachedTag RenderMethod;
        }

        [TagStructure(Size = 0x2, MaxVersion = CacheVersion.Halo2Vista)]
        [TagStructure(Size = 0x4, MinVersion = CacheVersion.Halo3Retail)]
        public class PropertyType : TagStructure
		{
            [TagField(Flags = Label, MaxVersion = CacheVersion.Halo2Vista)]
            public Halo2Value Halo2;

            [TagField(Flags = Label, MinVersion = CacheVersion.Halo3Retail)]
            public Halo3Value Halo3;

            public enum Halo2Value : short
            {
                LightmapResolution,
                LightmapPower,
                LightmapHalfLife,
                LightmapDiffuseScale
            }

            public enum Halo3Value : int
            {
                LightmapResolution,
                LightmapPower,
                LightmapHalfLife,
                LightmapDiffuseScale
            }
        }

        [TagStructure(Size = 0x8, MaxVersion = CacheVersion.Halo2Vista)]
        [TagStructure(Size = 0xC, MinVersion = CacheVersion.Halo3Retail)]
        public class Property : TagStructure
		{
            public PropertyType Type;

            [TagField(MaxVersion = CacheVersion.Halo2Vista)]
            public short ShortValue;

            [TagField(MinVersion = CacheVersion.Halo3Retail)]
            public int IntValue;

            public float RealValue;
        }
    }
}