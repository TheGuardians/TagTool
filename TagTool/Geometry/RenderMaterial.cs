using TagTool.Cache;
using TagTool.Common;
using TagTool.Serialization;
using System.Collections.Generic;

namespace TagTool.Geometry
{
    /// <summary>
    /// A material describing how a mesh part should be rendered.
    /// </summary>
    [TagStructure(Size = 0x20, MaxVersion = CacheVersion.Halo2Vista)]
    [TagStructure(Size = 0x24, MaxVersion = CacheVersion.HaloOnline571627)]
    [TagStructure(Size = 0x30, MinVersion = CacheVersion.HaloOnline700123)]
    public class RenderMaterial : TagStructure
	{
        /// <summary>
        /// The OLD render method tag to use to render the material.
        /// </summary>
        [TagField(Label = true, MaxVersion = CacheVersion.Halo2Vista)]
        public CachedTagInstance OldRenderMethod;

        /// <summary>
        /// The render method tag to use to render the material.
        /// </summary>
        [TagField(Label = true)]
        public CachedTagInstance RenderMethod;

        [TagField(MinVersion = CacheVersion.HaloOnline700123)]
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
            [TagField(Label = true)]
            public StringId Name;
            public CachedTagInstance RenderMethod;
        }

        [TagStructure(Size = 0x2, MaxVersion = CacheVersion.Halo2Vista)]
        [TagStructure(Size = 0x4, MinVersion = CacheVersion.Halo3Retail)]
        public class PropertyType : TagStructure
		{
            [TagField(Label = true, MaxVersion = CacheVersion.Halo2Vista)]
            public Halo2Value Halo2;

            [TagField(Label = true, MinVersion = CacheVersion.Halo3Retail)]
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