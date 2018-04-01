using TagTool.Bitmaps;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Serialization;
using System.Collections.Generic;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "bitmap", Tag = "bitm", Size = 0xA4, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Name = "bitmap", Tag = "bitm", Size = 0xB8, MaxVersion = CacheVersion.HaloOnline106708)]
    [TagStructure(Name = "bitmap", Tag = "bitm", Size = 0xAC, MinVersion = CacheVersion.HaloOnline235640)]
    [TagStructure(Name = "bitmap", Tag = "bitm", Size = 0xC0, MinVersion = CacheVersion.HaloReach)]
    public class Bitmap
    {
        /// <summary>
        /// Choose how you are using this bitmap
        /// </summary>
        public int Usage;

        /// <summary>
        /// The runtime flags of this bitmap
        /// </summary>
        public BitmapRuntimeFlags Flags;

        /// <summary>
        /// Number of pixels between adjacent sprites (0 uses default, negative numbers set no spacing)
        /// </summary>
        public short SpriteSpacing;
        
        public float Unknown10;
        public float Unknown14;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public float Unknown1;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public float Unknown2;

        public BitmapCurveMode BitmapCurveMode;

        public byte MaxMipMapLevel;
        public short MaxResolution;

        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public short AtlasIndex;

        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public short ForceBitmapFormatEnum;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public float Unknown3;

        [TagField(MinVersion = CacheVersion.HaloOnline106708, MaxVersion = CacheVersion.HaloOnline106708)]
        public List<TightBinding> TightBounds;

        public List<UsageOverride> UsageOverrides;
        public List<Sequence> ManualSequences;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public List<TightBinding> TightBounds2;

        public byte[] SourceData;
        public byte[] ProcessedPixelData;
        public List<Sequence> Sequences;
        public List<Image> Images;

        public byte[] XenonProcessedPixelData;
        [TagField(MaxVersion = CacheVersion.HaloOnline106708)]
        public List<Image> XenonImages;

        public List<BitmapResource> Resources;

        [TagField(MaxVersion = CacheVersion.HaloOnline106708)]
        public List<BitmapResource> InterleavedResources;

        [TagField(MaxVersion = CacheVersion.HaloOnline106708)]
        public int UnknownB4;

        [TagStructure(Size = 0x8)]
        public class TightBinding
        {
            public RealPoint2d UV;
        }

        [TagStructure(Size = 0x28)]
        public class UsageOverride
        {
            /// <summary>
            /// 0.0 to use xenon curve (default)
            /// </summary>
            public float SourceGamma;

            public int BitmapCurveEnum;

            public int Unknown8;
            public int UnknownC;
            public int Unknown10;
            public int Unknown14;
            public int Unknown18;
            public int Unknown1C;
            public int Unknown20;
            public int Unknown24;
        }

        [TagStructure(Size = 0x40)]
        public class Sequence
        {
            [TagField(Label = true, Length = 32)]
            public string Name;

            public short FirstBitmapIndex;
            public short BitmapCount;

            [TagField(Padding = true, Length = 16)]
            public byte[] Unused;

            public List<Sprite> Sprites;

            [TagStructure(Size = 0x20)]
            public class Sprite
            {
                public short BitmapIndex;

                [TagField(Padding = true, Length = 2)]
                public byte[] Unused1;

                [TagField(Padding = true, Length = 4)]
                public byte[] Unused2;

                public float Left;
                public float Right;
                public float Top;
                public float Bottom;

                public RealPoint2d RegistrationPoint;
            }
        }

        [TagStructure(Size = 0x30, MaxVersion = CacheVersion.HaloOnline106708)]
        [TagStructure(Size = 0x2C, MinVersion = CacheVersion.HaloReach)]
        public class Image
        {
            /// <summary>
            /// The group tag signature of the image.
            /// </summary>
            public Tag Signature;

            /// <summary>
            /// Pixels; DO NOT CHANGE
            /// </summary>
            public short Width;

            /// <summary>
            /// Pixels; DO NOT CHANGE
            /// </summary>
            public short Height;

            /// <summary>
            /// Pixels; DO NOT CHANGE
            /// </summary>
            public sbyte Depth;

            /// <summary>
            /// The xbox 360 flags of the bitmap image. DO NOT CHANGE
            /// </summary>
            public BitmapFlagsXbox XboxFlags;

            [TagField(Padding = true, Length = 1, MaxVersion = CacheVersion.Halo3ODST)]
            public byte[] Unused1;
            [TagField(Padding = true, Length = 1, MinVersion = CacheVersion.HaloReach)]
            public byte[] Unused2;

            /// <summary>
            /// The type of the bitmap image. DO NOT CHANGE
            /// </summary>
            public BitmapType Type;

            /// <summary>
            /// DO NOT CHANGE
            /// </summary>
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public byte FourTimesLog2Size;

            [TagField(Padding = true, Length = 1, MaxVersion = CacheVersion.Halo3ODST)]
            public byte[] Unused2_1;
            /// <summary>
            /// The format of the bitmap image. DO NOT CHANGE
            /// </summary>
            public BitmapFormat Format;

            [TagField(Padding = true, Length = 1, MinVersion = CacheVersion.HaloOnline106708)]
            public byte[] Unused2_2;

            /// <summary>
            /// The flags of the bitmap image. DO NOT CHANGE
            /// </summary>
            public BitmapFlags Flags;

            /// <summary>
            /// The 'center' of the bitmap - i.e. for particles
            /// </summary>
            public Point2d RegistrationPoint;

            /// <summary>
            /// DO NOT CHANGE (not counting the highest resolution)
            /// </summary>
            public sbyte MipmapCount;

            /// <summary>
            /// How to convert from pixel value to linear.
            /// </summary>
            public BitmapImageCurve Curve;

            public byte InterleavedTextureIndex1;
            public byte InterleavedTextureIndex2;

            public int DataOffset;
            public int DataSize;

            public float Unknown20;
            public sbyte Unknown24;
            public sbyte Unknown25;
            public sbyte Unknown26;
            public sbyte Unknown27;
            public int Unknown28;
            [TagField(MaxVersion = CacheVersion.HaloOnline106708)]
            public int Unknown2C;  
        }

        [TagStructure(Size = 0x8)]
        public class BitmapResource
        {
            [TagField(MaxVersion = CacheVersion.Halo3ODST)]
            public int ZoneAssetHandle;
            [TagField(Pointer = true, MinVersion = CacheVersion.HaloOnline106708)]
            public PageableResource Resource;

            public int Unknown4;
        }
    }
}