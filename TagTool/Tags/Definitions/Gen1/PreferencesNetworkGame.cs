using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen1
{
    [TagStructure(Name = "preferences_network_game", Tag = "ngpr", Size = 0x380)]
    public class PreferencesNetworkGame : TagStructure
    {
        [TagField(Length = 32)]
        public string Name;
        public RealRgbColor PrimaryColor;
        public RealRgbColor SecondaryColor;
        [TagField(ValidTags = new [] { "bitm" })]
        public CachedTag Pattern;
        public short BitmapIndex;
        [TagField(Length = 0x2)]
        public byte[] Padding;
        [TagField(ValidTags = new [] { "bitm" })]
        public CachedTag Decal;
        public short BitmapIndex1;
        [TagField(Length = 0x2)]
        public byte[] Padding1;
        [TagField(Length = 0x320)]
        public byte[] Padding2;
    }
}

