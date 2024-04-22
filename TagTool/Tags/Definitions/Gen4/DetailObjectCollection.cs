using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "detail_object_collection", Tag = "dobc", Size = 0x80)]
    public class DetailObjectCollection : TagStructure
    {
        public DetailObjectCollectionTypeEnum CollectionType;
        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
        public byte[] Padding;
        public float GlobalZOffset; // applied to all detail objects of in this collection so they don't float above the ground
        [TagField(Length = 0x2C, Flags = TagFieldFlags.Padding)]
        public byte[] Padding1;
        [TagField(ValidTags = new [] { "bitm" })]
        public CachedTag SpritePlate;
        public List<DetailObjectTypeBlock> Types;
        [TagField(Length = 0x30, Flags = TagFieldFlags.Padding)]
        public byte[] Padding2;
        
        public enum DetailObjectCollectionTypeEnum : short
        {
            ScreenFacing,
            ViewerFacing
        }
        
        [TagStructure(Size = 0x60)]
        public class DetailObjectTypeBlock : TagStructure
        {
            [TagField(Length = 32)]
            public string Name;
            public sbyte SequenceIndex; // [0,15]
            public DetailObjectTypeFlags TypeFlags;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            // fraction of detail object color to use instead of the base map color in the environment
            public float ColorOverrideFactor; // [0,1]
            [TagField(Length = 0x8, Flags = TagFieldFlags.Padding)]
            public byte[] Padding1;
            public float NearFadeDistance; // world units
            public float FarFadeDistance; // world units
            public float Size; // world units per pixel
            [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
            public byte[] Padding2;
            public RealRgbColor MinimumColor; // [0,1]
            public RealRgbColor MaximumColor; // [0,1]
            public ArgbColor AmbientColor; // [0,255]
            [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
            public byte[] Padding3;
            
            [Flags]
            public enum DetailObjectTypeFlags : byte
            {
                Unused0 = 1 << 0,
                Unused1 = 1 << 1,
                InterpolateColorInHsv = 1 << 2,
                MoreColors = 1 << 3
            }
        }
    }
}
