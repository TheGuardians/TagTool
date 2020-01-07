using System;
using System.Collections.Generic;
using TagTool.Cache;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags
{
    [TagStructure(Size = 0x40, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Size = 0x40, MinVersion = CacheVersion.HaloReach)]
    [TagStructure(Size = 0x48, MinVersion = CacheVersion.HaloOnline106708, MaxVersion = CacheVersion.HaloOnline700123)]
    public class TagResourceGen3 : TagStructure
	{
        public CachedTag ParentTag;
        public ushort Salt;

        [TagField(Gen = CacheGeneration.Third)]
        public sbyte ResourceTypeIndex;
        [TagField(MinVersion = CacheVersion.HaloOnline106708, MaxVersion = CacheVersion.HaloOnline700123)]
        public TagResourceTypeGen3 ResourceType;

        public byte Flags;

        [TagField(Gen = CacheGeneration.Third)]
        public int DefinitionDataOffset;

        [TagField(Gen = CacheGeneration.Third)]
        public int DefinitionDataLength;

        [TagField(Gen = CacheGeneration.Third)]
        public int SecondaryFixupInformationOffset;

        [TagField(Gen = CacheGeneration.Third)]
        public UnknownFlags Unknown1;

        [TagField(Gen = CacheGeneration.Third)]
        public short SegmentIndex;

        [TagField(MinVersion = CacheVersion.HaloOnline106708, MaxVersion = CacheVersion.HaloOnline700123)]
        public byte[] DefinitionData;

        public CacheAddress DefinitionAddress;

        public List<ResourceFixup> ResourceFixups = new List<ResourceFixup>();
        public List<D3DFixup> D3DFixups = new List<D3DFixup>();

        [TagField(MinVersion = CacheVersion.HaloOnline106708, MaxVersion = CacheVersion.HaloOnline700123)]
        public int Unknown2 = 1;

        [TagStructure(Size = 0x8)]
        public class ResourceFixup : TagStructure
		{
            public uint BlockOffset;
            public CacheAddress Address;

            [TagField(Flags = Runtime)]
            public int Type;
            [TagField(Flags = Runtime)]
            public int Offset;
            [TagField(Flags = Runtime)]
            public int RawAddress;
        }

        [TagStructure(Size = 0x8)]
        public class D3DFixup : TagStructure
		{
            public CacheAddress Address;
            public int ResourceStructureTypeIndex;
        }

        [Flags]
        public enum UnknownFlags : short
        {
            Invalid = 0,
            PrimaryPageValid = 1 << 0,
            SecondaryPageValid =  1 << 1
        }
    }
}
