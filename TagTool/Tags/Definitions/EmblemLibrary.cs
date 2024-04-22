using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.Cache;
using TagTool.Common;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "emblem_library", Tag = "mlib", Size = 0x74, MinVersion = Cache.CacheVersion.HaloReach)]
    public class EmblemLibrary : TagStructure
    {
        public short Version;
        [TagField(Flags = Padding, Length = 0x2)]
        public byte[] PADD;
        public float BitmapResolution;
        public float AntialiasSharpen;
        public CachedTag EmblemBitmaps;
        public CachedTag EmblemBitmapsHiRez;
        public List<EmblemBitmapList> Bitmaps;
        public List<EmblemShapeList> Shapes;
        public List<EmblemFrontList> FrontEmblems;
        public List<EmblemBackList> BackEmblems;
        public List<EmblemRuntimeFrontList> RuntimeFront;
        public List<EmblemRuntimeBackList> RuntimeBack;
    }

    [TagStructure(Size = 0xC)]
    public class EmblemBitmapList : TagStructure
    {
        public StringId Name;
        public short BitmapIndex;
        [TagField(Flags = Padding, Length = 0x2)]
        public byte[] PADD;
        public float GradientSize;
    }

    [TagStructure(Size = 0x30)]
    public class EmblemShapeList : TagStructure
    {
        public StringId Name;
        public short BitmapListIndex;
        public SamplerAddressMode16 AddressModeX;
        public SamplerAddressMode16 AddressModeY;
        [TagField(Flags = Padding, Length = 0x2)]
        public byte[] PADD;
        public EmblemTransform Transform;
    }

    [TagStructure(Size = 0xB8)]
    public class EmblemFrontList : TagStructure
    {
        public StringId Name;
        public EmblemLayer Layer0;
        public EmblemLayer Layer1;
        public FrontEmblemPrimaryLayer PrimaryLayer;
        [TagField(Flags = Padding, Length = 0x3)]
        public byte[] PADD;
    }

    [TagStructure(Size = 0x5C)]
    public class EmblemBackList : TagStructure
    {
        public StringId Name;
        public EmblemLayer Layer2;
    }

    [TagStructure(Size = 0xDC)]
    public class EmblemRuntimeFrontList : TagStructure
    {
        public short Bitmap0Index;
        public SamplerAddressMode16 Bitmap0AddressX;
        public SamplerAddressMode16 Bitmap0AddressY;
        public short Bitmap1Index;
        public SamplerAddressMode16 Bitmap1AddressX;
        public SamplerAddressMode16 Bitmap1AddressY;

        public RealQuaternion BitmapTransform0;
        public RealQuaternion BitmapTransform1;
        public RealQuaternion BitmapTransform2;
        public RealQuaternion BitmapTransform3;
        public RealQuaternion BitmapParams0;
        public RealQuaternion BitmapParams1;

        public short Bitmap2Index;
        public SamplerAddressMode16 Bitmap2AddressX;
        public SamplerAddressMode16 Bitmap2AddressY;
        public short Bitmap3Index;
        public SamplerAddressMode16 Bitmap3AddressX;
        public SamplerAddressMode16 Bitmap3AddressY;

        public RealQuaternion BitmapTransform4;
        public RealQuaternion BitmapTransform5;
        public RealQuaternion BitmapTransform6;
        public RealQuaternion BitmapTransform7;
        public RealQuaternion BitmapParams2;
        public RealQuaternion BitmapParams3;

        public FrontEmblemPrimaryLayer PrimaryLayer;
        [TagField(Flags = Padding, Length = 0x3)]
        public byte[] PADD;
    }

    [TagStructure(Size = 0x6C)]
    public class EmblemRuntimeBackList : TagStructure
    {
        public short Bitmap0Index;
        public SamplerAddressMode16 Bitmap0AddressX;
        public SamplerAddressMode16 Bitmap0AddressY;
        public short Bitmap1Index;
        public SamplerAddressMode16 Bitmap1AddressX;
        public SamplerAddressMode16 Bitmap1AddressY;

        public RealQuaternion BitmapTransform0;
        public RealQuaternion BitmapTransform1;
        public RealQuaternion BitmapTransform2;
        public RealQuaternion BitmapTransform3;
        public RealQuaternion BitmapParams0;
        public RealQuaternion BitmapParams1;
    }

    [TagStructure(Size = 0x58)]
    public class EmblemLayer : TagStructure
    {
        public short Shape0;
        [TagField(Flags = Padding, Length = 0x2)]
        public byte[] PADD;
        public float Multiplier0;

        public EmblemTransform Transform0;
        public short Shape1;
        [TagField(Flags = Padding, Length = 0x2)]
        public byte[] PADD1;
        public float Multiplier1;
        public EmblemTransform Transform1;
    }

    [TagStructure(Size = 0x24)]
    public class EmblemTransform : TagStructure
    {
        public RealPoint2d Scale;
        public RealPoint2d Shear;
        public float Rotation;
        public RealPoint2d Offset;
        public float ExpandContract;
        public float Blur;
    }

    public enum FrontEmblemPrimaryLayer : byte
    {
        Foreground,
        Midground
    }

    public enum SamplerAddressMode16 : short
    {
        Wrap,
        Clamp,
        Mirror,
        BlackBorder,
        MirrorOnce,
        MirrorOnceBorder
    }
}
