using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "emblem_library", Tag = "mlib", Size = 0x74)]
    public class EmblemLibrary : TagStructure
    {
        public short Version;
        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
        public byte[] Padding;
        // used to calculate appropriate antialiasing settings
        public float BitmapResolution; // pixels
        // default 1.0, global control on antialias sharpness
        public float AntialiasSharpen;
        [TagField(ValidTags = new [] { "bitm" })]
        public CachedTag EmblemBitmaps;
        [TagField(ValidTags = new [] { "bitm" })]
        public CachedTag EmblemBitmapsHiRez;
        public List<EmblemBitmapList> Bitmaps;
        public List<EmblemShapeList> Shapes;
        public List<EmblemFrontList> FrontEmblems;
        public List<EmblemBackList> BackEmblems;
        public List<EmblemRuntimeFrontList> RuntimeFront;
        public List<EmblemRuntimeBackList> RuntimeBack;
        
        [TagStructure(Size = 0xC)]
        public class EmblemBitmapList : TagStructure
        {
            public StringId Name;
            // the index of the bitmap in the bitmap group
            public short BitmapIndex;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            // the size of the gradient (from white to black) in this bitmap
            public float GradientSize; // pixels
        }
        
        [TagStructure(Size = 0x30)]
        public class EmblemShapeList : TagStructure
        {
            public StringId Name;
            public short Bitmap;
            public RenderMethodBitmapAddressModeEnum AddressModeX;
            public RenderMethodBitmapAddressModeEnum AddressModeY;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public EmblemTransform Transform;
            
            public enum RenderMethodBitmapAddressModeEnum : short
            {
                Wrap,
                Clamp,
                Mirror,
                BlackBorder,
                Mirroronce,
                MirroronceBorder
            }
            
            [TagStructure(Size = 0x24)]
            public class EmblemTransform : TagStructure
            {
                public RealPoint2d Scale;
                public RealPoint2d Shear;
                public float Rotation;
                public RealPoint2d Offset;
                // amount to expand (positive) or contract (negative) the shape outline
                public float ExpandContract;
                // amount to blur the shape outline
                public float Blur;
            }
        }
        
        [TagStructure(Size = 0xB8)]
        public class EmblemFrontList : TagStructure
        {
            public StringId Name;
            public EmblemLayer Layer0;
            public EmblemLayer Layer1;
            // layer that is considered "primary" and which will use the primary color
            public FrontEmblemPrimaryLayer PrimaryLayer;
            [TagField(Length = 0x1, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding1;
            
            public enum FrontEmblemPrimaryLayer : sbyte
            {
                Layer0,
                Layer1
            }
            
            [TagStructure(Size = 0x58)]
            public class EmblemLayer : TagStructure
            {
                public short Shape0;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public float Multiplier0;
                public EmblemTransform Transform0;
                public short Shape1;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding1;
                public float Multiplier1;
                public EmblemTransform Transform1;
                
                [TagStructure(Size = 0x24)]
                public class EmblemTransform : TagStructure
                {
                    public RealPoint2d Scale;
                    public RealPoint2d Shear;
                    public float Rotation;
                    public RealPoint2d Offset;
                    // amount to expand (positive) or contract (negative) the shape outline
                    public float ExpandContract;
                    // amount to blur the shape outline
                    public float Blur;
                }
            }
        }
        
        [TagStructure(Size = 0x5C)]
        public class EmblemBackList : TagStructure
        {
            public StringId Name;
            public EmblemLayer Layer2;
            
            [TagStructure(Size = 0x58)]
            public class EmblemLayer : TagStructure
            {
                public short Shape0;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public float Multiplier0;
                public EmblemTransform Transform0;
                public short Shape1;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding1;
                public float Multiplier1;
                public EmblemTransform Transform1;
                
                [TagStructure(Size = 0x24)]
                public class EmblemTransform : TagStructure
                {
                    public RealPoint2d Scale;
                    public RealPoint2d Shear;
                    public float Rotation;
                    public RealPoint2d Offset;
                    // amount to expand (positive) or contract (negative) the shape outline
                    public float ExpandContract;
                    // amount to blur the shape outline
                    public float Blur;
                }
            }
        }
        
        [TagStructure(Size = 0xE4)]
        public class EmblemRuntimeFrontList : TagStructure
        {
            public StringId Name0;
            public short Bitmap0Index;
            public RenderMethodBitmapAddressModeEnum Bitmap0AddressX;
            public RenderMethodBitmapAddressModeEnum Bitmap0AddressY;
            public short Bitmap1Index;
            public RenderMethodBitmapAddressModeEnum Bitmap1AddressX;
            public RenderMethodBitmapAddressModeEnum Bitmap1AddressY;
            public RealQuaternion BitmapTransform0;
            public RealQuaternion BitmapTransform1;
            public RealQuaternion BitmapTransform2;
            public RealQuaternion BitmapTransform3;
            public RealQuaternion BitmapParams0;
            public RealQuaternion BitmapParams1;
            public StringId Name1;
            public short Bitmap2Index;
            public RenderMethodBitmapAddressModeEnum Bitmap2AddressX;
            public RenderMethodBitmapAddressModeEnum Bitmap2AddressY;
            public short Bitmap3Index;
            public RenderMethodBitmapAddressModeEnum Bitmap3AddressX;
            public RenderMethodBitmapAddressModeEnum Bitmap3AddressY;
            public RealQuaternion BitmapTransform4;
            public RealQuaternion BitmapTransform5;
            public RealQuaternion BitmapTransform6;
            public RealQuaternion BitmapTransform7;
            public RealQuaternion BitmapParams2;
            public RealQuaternion BitmapParams3;
            public FrontEmblemPrimaryLayer PrimaryLayer;
            [TagField(Length = 0x1, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding1;
            
            public enum RenderMethodBitmapAddressModeEnum : short
            {
                Wrap,
                Clamp,
                Mirror,
                BlackBorder,
                Mirroronce,
                MirroronceBorder
            }
            
            public enum FrontEmblemPrimaryLayer : sbyte
            {
                Layer0,
                Layer1
            }
        }
        
        [TagStructure(Size = 0x70)]
        public class EmblemRuntimeBackList : TagStructure
        {
            public StringId Name;
            public short Bitmap0Index;
            public RenderMethodBitmapAddressModeEnum Bitmap0AddressX;
            public RenderMethodBitmapAddressModeEnum Bitmap0AddressY;
            public short Bitmap1Index;
            public RenderMethodBitmapAddressModeEnum Bitmap1AddressX;
            public RenderMethodBitmapAddressModeEnum Bitmap1AddressY;
            public RealQuaternion BitmapTransform0;
            public RealQuaternion BitmapTransform1;
            public RealQuaternion BitmapTransform2;
            public RealQuaternion BitmapTransform3;
            public RealQuaternion BitmapParams0;
            public RealQuaternion BitmapParams1;
            
            public enum RenderMethodBitmapAddressModeEnum : short
            {
                Wrap,
                Clamp,
                Mirror,
                BlackBorder,
                Mirroronce,
                MirroronceBorder
            }
        }
    }
}
