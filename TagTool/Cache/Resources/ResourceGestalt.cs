using System.Collections.Generic;
using TagTool.Cache.Interops;
using TagTool.Common;
using TagTool.Tags;
using TagTool.Tags.Definitions;

namespace TagTool.Cache.Resources
{
    [TagStructure(Name = "cache_file_resource_gestalt", Tag = "zone", Size = 0x598, MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
    [TagStructure(Name = "cache_file_resource_gestalt", Tag = "zone", Size = 0x228, MaxVersion = CacheVersion.HaloReach, Platform = CachePlatform.MCC)]
    [TagStructure(Name = "cache_file_resource_gestalt", Tag = "zone", Size = 0x220, Version = CacheVersion.HaloReach11883, Platform = CachePlatform.MCC)]
    [TagStructure(Name = "cache_file_resource_gestalt", Tag = "zone", Size = 0x214, MaxVersion = CacheVersion.HaloReach11883, Platform = CachePlatform.Original)]
    public class ResourceGestalt : TagStructure
    {
        [TagField(MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
        public List<HOTagResource> HOTagResources;

        [TagField(Length = 0x8, MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
        public byte[] HOPadding1;

        [TagField(MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
        public List<HOZoneSet> HODesignerZonesets;

        [TagField(MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
        public List<HOZoneSet> HOGlobalZoneset;

        [TagField(MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
        public List<HOZoneSet> HOUnattachedZoneset;

        [TagField(MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
        public List<HOZoneSet> HODiscForbiddenZoneset;

        [TagField(MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
        public List<HOZoneSet> HODiscAlwaysStreamingZoneset;

        [TagField(MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
        public List<HOZoneSet> HOBspZonesets;

        [TagField(MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
        public List<HOZoneSet> HOBspZonesets2;

        [TagField(MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
        public List<HOZoneSet> HOBspZonesets3;

        [TagField(MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
        public List<HOZoneSet> HOCinematicZonesets;

        [TagField(MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
        public List<HOZoneSet> HOScenarioZonesets;

        [TagField(Length = 0x18, Flags = TagFieldFlags.Padding, MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
        public byte[] HOPadding2;

        [TagField(MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
        public List<HOUnknownBlock1> HOUnknownBlock1;

        [TagField(MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
        public List<HOUnknownBlock2> HOUnknownBlock2;

        [TagField(Length = 0x4, MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
        public byte[] HOPadding3;

        [TagField(MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
        public List<HOUnknownBlock3> HOUnknownBlock3;

        [TagField(Length = 0x4CC, Flags = TagFieldFlags.Padding, MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
        public byte[] HORemainingData;

        [TagField(MaxVersion = CacheVersion.HaloReach, Platform = CachePlatform.Original | CachePlatform.MCC)]
        public ScenarioTypeEnum MapType;

        [TagField(MaxVersion = CacheVersion.HaloReach, Platform = CachePlatform.Original | CachePlatform.MCC)]
        public ScenarioFlags Flags;

        [TagField(MaxVersion = CacheVersion.HaloReach, Platform = CachePlatform.Original | CachePlatform.MCC)]
        public List<ResourceDefinition> ResourceDefinitions;
        [TagField(MaxVersion = CacheVersion.HaloReach, Platform = CachePlatform.Original | CachePlatform.MCC)]
        public List<InteropDefinition> InteropDefinitions;

        [TagField(MaxVersion = CacheVersion.HaloReach, Platform = CachePlatform.Original | CachePlatform.MCC)]
        public ResourceLayoutTable LayoutTable = new ResourceLayoutTable();

        [TagField(MaxVersion = CacheVersion.HaloReach, Platform = CachePlatform.Original | CachePlatform.MCC)]
        public List<ResourceData> TagResources;

        [TagField(MaxVersion = CacheVersion.HaloReach, Platform = CachePlatform.Original | CachePlatform.MCC)]
        public List<ZoneManifest> DesignerZoneManifests;
        [TagField(MaxVersion = CacheVersion.HaloReach, Platform = CachePlatform.Original | CachePlatform.MCC)]
        public List<ZoneManifest> GlobalZoneManifests;
        [TagField(MaxVersion = CacheVersion.HaloReach, Platform = CachePlatform.Original | CachePlatform.MCC)]
        public List<ZoneManifest> HsZoneManifests;
        [TagField(MaxVersion = CacheVersion.HaloReach, Platform = CachePlatform.Original | CachePlatform.MCC)]
        public List<ZoneManifest> UnattachedDesignerZoneManifests;
        [TagField(MaxVersion = CacheVersion.HaloReach, Platform = CachePlatform.Original | CachePlatform.MCC)]
        public List<ZoneManifest> DvdForbiddenZoneManifests;
        [TagField(MaxVersion = CacheVersion.HaloReach, Platform = CachePlatform.Original | CachePlatform.MCC)]
        public List<ZoneManifest> DvdAlwaysStreamingZoneManifests;
        [TagField(MaxVersion = CacheVersion.HaloReach, Platform = CachePlatform.Original | CachePlatform.MCC)]
        public List<ZoneManifest> DefaultBspZoneManifests;
        [TagField(MaxVersion = CacheVersion.HaloReach, Platform = CachePlatform.Original | CachePlatform.MCC)]
        public List<ZoneManifest> StaticBspZoneManifests;
        [TagField(MaxVersion = CacheVersion.HaloReach, Platform = CachePlatform.Original | CachePlatform.MCC)]
        public List<ZoneManifest> DynamicBspZoneManifests;
        [TagField(MaxVersion = CacheVersion.HaloReach, Platform = CachePlatform.Original | CachePlatform.MCC)]
        public List<ZoneManifest> CinematicZoneManifests;

        [TagField(MaxVersion = CacheVersion.HaloReach, Platform = CachePlatform.Original | CachePlatform.MCC)]
        public List<ZoneManifest> ZonesOnlyZoneSetManifests;
        [TagField(MaxVersion = CacheVersion.HaloReach, Platform = CachePlatform.Original | CachePlatform.MCC)]
        public List<ZoneManifest> ExpectedZoneManifests;
        [TagField(MaxVersion = CacheVersion.HaloReach, Platform = CachePlatform.Original | CachePlatform.MCC)]
        public List<ZoneManifest> FullyPopulatedZoneManifests;

        [TagField(MaxVersion = CacheVersion.HaloReach, Platform = CachePlatform.Original | CachePlatform.MCC)]
        public List<ZoneSetZoneUsage> ZoneSetZoneUsages;

        [TagField(MaxVersion = CacheVersion.HaloReach, Platform = CachePlatform.Original | CachePlatform.MCC)]
        public List<CachedTag> BspReferences;
        [TagField(MaxVersion = CacheVersion.HaloReach, Platform = CachePlatform.Original | CachePlatform.MCC)]
        public List<TagReferenceBlock> TagReferences;

        [TagField(MaxVersion = CacheVersion.HaloReach, Platform = CachePlatform.Original | CachePlatform.MCC)]
        public List<ModelVariantUsage> ModelVariantUsages;
        [TagField(MaxVersion = CacheVersion.HaloReach, Platform = CachePlatform.Original | CachePlatform.MCC)]
        public List<CharacterUsage> CharacterUsages;

        [TagField(MaxVersion = CacheVersion.HaloReach, Platform = CachePlatform.Original | CachePlatform.MCC)]
        public byte[] DefinitionData;

        [TagField(MaxVersion = CacheVersion.HaloReach, Platform = CachePlatform.Original | CachePlatform.MCC)]
        public uint MinimumCompletePageableDataSize;
        [TagField(MaxVersion = CacheVersion.HaloReach, Platform = CachePlatform.Original | CachePlatform.MCC)]
        public uint MinimumRequiredPageableDataSize;
        [TagField(MaxVersion = CacheVersion.HaloReach, Platform = CachePlatform.Original | CachePlatform.MCC)]
        public uint MinimumRequiredDvdDataSize;

        [TagField(MaxVersion = CacheVersion.HaloReach, Platform = CachePlatform.Original | CachePlatform.MCC)]
        public uint GlobalPageableDataSize;
        [TagField(MaxVersion = CacheVersion.HaloReach, Platform = CachePlatform.Original | CachePlatform.MCC)]
        public uint OptionalControlDataSize;

        [TagField(MaxVersion = CacheVersion.HaloReach, Platform = CachePlatform.Original | CachePlatform.MCC)]
        public List<ZoneResourceUsage> GlobalResourceUsage;

        [TagField(Flags = TagFieldFlags.Padding, Length = 96, MaxVersion = CacheVersion.HaloReach, Platform = CachePlatform.Original | CachePlatform.MCC)]
        public byte[] Unused = new byte[96];

        [TagField(MaxVersion = CacheVersion.HaloReach, Platform = CachePlatform.Original | CachePlatform.MCC)]
        public List<PredictionQuantum> PredictionQuanta;
        [TagField(MaxVersion = CacheVersion.HaloReach, Platform = CachePlatform.Original | CachePlatform.MCC)]
        public List<PredictionAtom> PredictionAtoms;
        [TagField(MaxVersion = CacheVersion.HaloReach, Platform = CachePlatform.Original | CachePlatform.MCC)]
        public List<PredictionMoleculeAtom> PredictionMoleculeAtoms;
        [TagField(MaxVersion = CacheVersion.HaloReach, Platform = CachePlatform.Original | CachePlatform.MCC)]
        public List<PredictionMolecule> PredictionMolecules;
        [TagField(MaxVersion = CacheVersion.HaloReach, Platform = CachePlatform.Original | CachePlatform.MCC)]
        public List<PredictionMoleculeKey> PredictionMoleculeKeys;

        [TagField(MaxVersion = CacheVersion.HaloReach, Platform = CachePlatform.Original | CachePlatform.MCC)]
        public int CampaignId;
        [TagField(MaxVersion = CacheVersion.HaloReach, Platform = CachePlatform.Original | CachePlatform.MCC)]
        public int MapId;

        [TagField(Platform = CachePlatform.MCC, MaxVersion = CacheVersion.HaloReach)]
        public int Unknown0;
        [TagField(Platform = CachePlatform.MCC, MaxVersion = CacheVersion.HaloReach)]
        public int Unknown1;

        public enum ScenarioTypeEnum : short
        {
            SinglePlayer,
            Multiplayer,
            MainMenu,
            MultiplayerShared,
            SinglePlayerShared,
            SoundsShared
        }
    }

    [TagStructure(Size = 0x70)]
    public class HOTagResource : TagStructure
    {
        public short Unknown1;
        public ushort Flags;

        [TagField(Length = 0x4)]
        public byte[] Padding1;

        public int Offset;
        public int CompressedSize;
        public int UncompressedSize;
        public int Checksum;

        [TagField(Length = 0x10, Flags = TagFieldFlags.Padding)]
        public byte[] Padding2;

        public CachedTag UnknownTag;

        public short Unknown2;
        public sbyte ResourceTypeIndex;
        public sbyte Unknown3;

        [TagField(Length = 0x14)]
        public byte[] FixupDataRaw;

        public ushort RootDefinitionAddress;
        public byte RootDefinitionAddressUpperBits;
        public byte RootDefinitionAddressLocationHighBits;

        public List<HOResourceFixup> ResourceFixups;
        public List<HOResourceDefinitionFixup> ResourceDefinitionFixups;

        public int Unknown4;
    }

    [TagStructure(Size = 0x8)]
    public class HOResourceFixup : TagStructure
    {
        public int BlockOffset;
        public ushort Address;
        public byte AddressUpperBits;
        public byte AddressLocationHighBits;
    }

    [TagStructure(Size = 0x8)]
    public class HOResourceDefinitionFixup : TagStructure
    {
        public ushort Offset;
        public byte OffsetUpperBits;
        public byte OffsetLocationHighBits;
        public int ResourceStructureTypeIndex;
    }

    [TagStructure(Size = 0x60)]
    public class HOZoneSet : TagStructure
    {
        public List<HOActiveBitVector> RequiredRawPool;
        public List<HOActiveBitVector> OptionalRawPool;

        [TagField(Length = 0x8)]
        public byte[] Padding1;

        public StringId SetName;
        public List<HOResourceType> ResourceTypes;
        public List<HOActiveBitVector> RequiredTagPool;
        public List<HOActiveBitVector> OptionalTagPool;
        public List<HOZoneSetObject> ZonesetObjects;

        [TagField(Length = 0xC, Flags = TagFieldFlags.Padding)]
        public byte[] Padding2;
    }

    [TagStructure(Size = 0x4)]
    public class HOActiveBitVector : TagStructure
    {
        public uint ActiveMembers;
    }

    [TagStructure(Size = 0xC)]
    public class HOResourceType : TagStructure
    {
        [TagField(Length = 0xC)]
        public byte[] Unknown;
    }

    [TagStructure(Size = 0x1C)]
    public class HOZoneSetObject : TagStructure
    {
        public CachedTag Object;
        public List<HOZoneSetObjectDependency> Dependencies;
    }

    [TagStructure(Size = 0x2)]
    public class HOZoneSetObjectDependency : TagStructure
    {
        public short TagResourceIndex;
    }

    [TagStructure(Size = 0x24)]
    public class HOUnknownBlock1 : TagStructure
    {
        public StringId Unknown1;

        [TagField(Length = 0x20)]
        public byte[] Unknown;
    }

    [TagStructure(Size = 0x10)]
    public class HOUnknownBlock2 : TagStructure
    {
        public CachedTag Unknown;
    }

    [TagStructure(Size = 0xC)]
    public class HOUnknownBlock3 : TagStructure
    {
        [TagField(Length = 0xC)]
        public byte[] Unknown;
    }
}
