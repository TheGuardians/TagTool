using TagTool.Bitmaps;
using TagTool.Cache;
using TagTool.Direct3D.D3D9;
using TagTool.Direct3D.D3D9x;

namespace TagTool.Tags.Resources
{
    /// <summary>
    /// Resource definition data for bitmap textures.
    /// </summary>
    [TagStructure(Name = "bitmap_texture_interop_resource", Size = 0xC)]
    public class BitmapTextureInteropResource : TagStructure
	{
        public D3DStructure<BitmapDefinition> Texture;

        [TagStructure(Size = 0x38, MaxVersion = CacheVersion.Halo3ODST, Platform = CachePlatform.MCC)]
        [TagStructure(Size = 0x34, MaxVersion = CacheVersion.Halo3ODST, Platform = CachePlatform.Original)]
        [TagStructure(Size = 0x40, MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
        [TagStructure(Size = 0x38, MinVersion = CacheVersion.HaloReach, Platform = CachePlatform.Original)]
        public class BitmapDefinition : TagStructure
		{
            public TagData PrimaryResourceData;
            public TagData SecondaryResourceData;
            public BitmapTextureInteropDefinition Bitmap;
        }
    }

    
  
    [TagStructure(Size = 0x10, MaxVersion = CacheVersion.Halo3ODST, Platform = CachePlatform.MCC)]
    [TagStructure(Size = 0xC, MaxVersion = CacheVersion.Halo3ODST, Platform = CachePlatform.Original)]
    [TagStructure(Size = 0x10, MinVersion = CacheVersion.HaloReach, Platform = CachePlatform.Original)]
    [TagStructure(Size = 0x18, MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
    public class BitmapTextureInteropDefinition : TagStructure
    {
        public short Width;
        public short Height;
        public byte Depth;
        public sbyte MipmapCount;
        public BitmapType BitmapType;
        public byte HighResInSecondaryResource;

        [TagField(MinVersion = CacheVersion.HaloReach, MaxVersion = CacheVersion.HaloReach11883)]
        public int ExponentBias;

        public int D3DFormat;

        [TagField(Platform = CachePlatform.MCC)]
        [TagField(Gen = CacheGeneration.HaloOnline)]
        public BitmapFormat Format;

        [TagField(Platform = CachePlatform.MCC)]
        [TagField(Gen = CacheGeneration.HaloOnline)]
        public BitmapImageCurve Curve;

        [TagField(Platform = CachePlatform.MCC)]
        [TagField(Gen = CacheGeneration.HaloOnline)]
        public BitmapFlags Flags;

        [TagField(Gen = CacheGeneration.HaloOnline)]
        public int Unknown1;

        [TagField(Gen = CacheGeneration.HaloOnline)]
        public int Unknown2;
    }
}