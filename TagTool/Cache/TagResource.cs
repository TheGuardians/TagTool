using TagTool.Serialization;
using System.Collections.Generic;

namespace TagTool.Cache
{
    [TagStructure(Size = 0x40, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Size = 0x48, MinVersion = CacheVersion.HaloOnline106708)]
    public class TagResource
    {
        public CachedTagInstance ParentTag;
        public ushort Salt;
        public TagResourceType Type;
        public byte Flags;

        [TagField(MaxVersion = CacheVersion.Halo3ODST)]
        public int FixupInformationOffset;

        [TagField(MaxVersion = CacheVersion.Halo3ODST)]
        public int FixupInformationLength;

        [TagField(MaxVersion = CacheVersion.Halo3ODST)]
        public int SecondaryFixupInformationOffset;

        [TagField(MaxVersion = CacheVersion.Halo3ODST)]
        public short Unknown1;

        [TagField(MaxVersion = CacheVersion.Halo3ODST)]
        public short PlaySegmentIndex;

        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public byte[] DefinitionData;

        public CacheAddress DefinitionAddress;

        public List<ResourceFixup> ResourceFixups;
        public List<ResourceDefinitionFixup> ResourceDefinitionFixups;

        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public int Unknown2;

        [TagStructure(Size = 0x8)]
        public class ResourceFixup
        {
            public uint BlockOffset;
            public CacheAddress Address;
        }

        [TagStructure(Size = 0x8)]
        public class ResourceDefinitionFixup
        {
            public CacheAddress Address;
            public int ResourceStructureTypeIndex;
        }
    }
}
