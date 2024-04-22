using TagTool.Cache;
using TagTool.Common;
using TagTool.Shaders;
using System.Collections.Generic;
using System;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "atmosphere_globals", Tag = "atgf", Size = 0x30, MinVersion = CacheVersion.HaloReach)]
    public class AtmosphereGlobals : TagStructure
    {
        public CachedTag FogBitmap;
        public float TextureRepeatRate;
        public float DistanceBetweenSheets;
        public float DepthFadeFactor;
        public float TransparentSortDistance;

        public SortingLayerValue TransparentSortLayer;
        [TagField(Flags = TagFieldFlags.Padding, Length = 0x3)]
        public byte[] Padding1;

        public List<SkyAtmParameters.UnderwaterBlock> UnderwaterSettings;
    }
}
