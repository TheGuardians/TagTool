using System;
using System.Collections.Generic;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Tags;
using TagTool.Tags.Definitions.Gen4;

namespace TagTool.Tags.Resources.Gen4
{
    [TagStructure(Size = 0x50)]
    public class RenderTextureInteropDefinition : TagStructure
    {
        public TagData PixelData;
        public TagData MediumResData;
        public TagData HighResData;
        public short Width;
        public short Height;
        public sbyte Depth;
        public sbyte TotalMipmapCount;
        public BitmapTypes Type;
        public sbyte Pad11;
        public BooleanEnum IsHighResBitmap;
        public BooleanEnum IsMediumResBitmap;
        public BooleanEnum Pad21;
        public BooleanEnum Pad22;
        public int ExponentBias;
        public int XenonD3dFormat;
        
        public enum BitmapTypes : sbyte
        {
            _2dTexture,
            _3dTexture,
            CubeMap,
            Array
        }
        
        public enum BooleanEnum : sbyte
        {
            False,
            True
        }
    }
}
