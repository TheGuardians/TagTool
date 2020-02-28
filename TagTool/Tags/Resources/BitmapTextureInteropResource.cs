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

        [TagStructure(Size = 0x34, MaxVersion = CacheVersion.Halo3ODST)]
        [TagStructure(Size = 0x40, MinVersion = CacheVersion.HaloOnline106708)]
        public class BitmapDefinition : TagStructure
		{
            public TagData PrimaryResourceData;
            public TagData SecondaryResourceData;
            public BitmapTextureInteropDefinition Bitmap;
        }
    }

    [TagStructure(Size = 0xC, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Size = 0x18, MinVersion = CacheVersion.HaloOnline106708)]
    public class BitmapTextureInteropDefinition
    {
        public short Width;
        public short Height;
        public byte Depth;
        public byte MipmapCount;
        public BitmapType BitmapType;
        public byte HighResInSecondaryResource;

        public int D3DFormat;

        [TagField(Gen = CacheGeneration.HaloOnline)]
        public BitmapFormat Format;

        [TagField(Gen = CacheGeneration.HaloOnline)]
        public BitmapImageCurve Curve;

        [TagField(Gen = CacheGeneration.HaloOnline)]
        public BitmapFlags Flags;

        [TagField(Gen = CacheGeneration.HaloOnline)]
        public int Unknown1;

        [TagField(Gen = CacheGeneration.HaloOnline)]
        public int Unknown2;
    }
}