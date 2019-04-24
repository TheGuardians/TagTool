using TagTool.Cache;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "vision_mode", Tag = "vmdx", Size = 0x188, MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Name = "vision_mode", Tag = "vmdx", Size = 0x1A0, MinVersion = CacheVersion.HaloOnline106708)]
    public class VisionMode : TagStructure
	{
        public sbyte Unknown;
        public sbyte Unknown2;
        public sbyte Unknown3;
        public sbyte Unknown4;

        public float Unknown5;
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public float Unknown6;
        public float Unknown7;
        public float Unknown8;
        public float Unknown9;
        public float Unknown10;

        public float Unknown11;
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public float Unknown12;
        public float Unknown13;
        public float Unknown14;
        public float Unknown15;
        public float Unknown16;

        public float Unknown17;
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public float Unknown18;
        public float Unknown19;
        public float Unknown20;
        public float Unknown21;
        public float Unknown22;

        public CachedTagInstance Unknown23;
        public TagFunction Unknown24;
        public TagFunction Unknown25;
        public float Unknown26;
        public float Unknown27;
        public float Unknown28;
        public float Unknown29;
        public TagFunction Unknown30;
        public TagFunction Unknown31;
        public float Unknown32;
        public float Unknown33;
        public float Unknown34;
        public float Unknown35;
        public TagFunction Unknown36;
        public TagFunction Unknown37;
        public float Unknown38;
        public float Unknown39;
        public float Unknown40;
        public float Unknown41;
        public TagFunction Unknown42;
        public TagFunction Unknown43;
        public float Unknown44;
        public float Unknown45;
        public float Unknown46;
        public float Unknown47;
        public TagFunction Unknown48;
        public TagFunction Unknown49;
        public float Unknown50;
        public float Unknown51;
        public float Unknown52;
        public float Unknown53;
        public CachedTagInstance Unknown54;
        public CachedTagInstance Unknown55;

        [TagField(Flags = Padding, Length = 12, MinVersion = CacheVersion.HaloOnline106708)]
        public byte[] Unused;
    }
}