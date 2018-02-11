using TagTool.Cache;
using TagTool.Common;
using TagTool.Serialization;

namespace TagTool.TagDefinitions
{
    [TagStructure(Name = "vision_mode", Tag = "vmdx", Size = 0x188, MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Name = "vision_mode", Tag = "vmdx", Size = 0x1A0, MinVersion = CacheVersion.HaloOnline106708)]
    public class VisionMode
    {
        public sbyte Unknown;
        public sbyte Unknown2;
        public sbyte Unknown3;
        public sbyte Unknown4;
        public float Unknown5;
        public float Unknown6;
        public float Unknown7;
        public float Unknown8;
        public float Unknown9;
        public float Unknown10;
        public float Unknown11;
        public float Unknown12;
        public float Unknown13;
        public float Unknown14;
        public float Unknown15;
        public float Unknown16;
        public float Unknown17;
        public float Unknown18;
        public float Unknown19;

        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public float Unknown20;
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public float Unknown21;
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public float Unknown22;

        public CachedTagInstance Unknown23;
        public TagFunction Unknown24 = new TagFunction { Data = new byte[0] };
        public TagFunction Unknown25 = new TagFunction { Data = new byte[0] };
        public float Unknown26;
        public float Unknown27;
        public float Unknown28;
        public float Unknown29;
        public TagFunction Unknown30 = new TagFunction { Data = new byte[0] };
        public TagFunction Unknown31 = new TagFunction { Data = new byte[0] };
        public float Unknown32;
        public float Unknown33;
        public float Unknown34;
        public float Unknown35;
        public TagFunction Unknown36 = new TagFunction { Data = new byte[0] };
        public TagFunction Unknown37 = new TagFunction { Data = new byte[0] };
        public float Unknown38;
        public float Unknown39;
        public float Unknown40;
        public float Unknown41;
        public TagFunction Unknown42 = new TagFunction { Data = new byte[0] };
        public TagFunction Unknown43 = new TagFunction { Data = new byte[0] };
        public float Unknown44;
        public float Unknown45;
        public float Unknown46;
        public float Unknown47;
        public TagFunction Unknown48 = new TagFunction { Data = new byte[0] };
        public TagFunction Unknown49 = new TagFunction { Data = new byte[0] };
        public float Unknown50;
        public float Unknown51;
        public float Unknown52;
        public float Unknown53;
        public CachedTagInstance Unknown54;
        public CachedTagInstance Unknown55;

        [TagField(Padding = true, Length = 12, MinVersion = CacheVersion.HaloOnline106708)]
        public byte[] Unused;
    }
}