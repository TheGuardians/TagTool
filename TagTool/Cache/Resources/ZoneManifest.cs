using System.Collections.Generic;
using TagTool.Common;
using TagTool.Tags;

namespace TagTool.Cache.Resources
{
    [TagStructure(Size = 0x78, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Size = 0xA0, MinVersion = CacheVersion.HaloReach)]
    [TagStructure(Size = 0x5C, MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline449175)]
    [TagStructure(Size = 0x60, MinVersion = CacheVersion.HaloOnline498295, MaxVersion = CacheVersion.HaloOnline700123)]
    public class ZoneManifest : TagStructure
    {
        public TagBlockBitVector RequiredResourcesBitVector;

        [TagField(MaxVersion = CacheVersion.Halo3ODST)]
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public TagBlockBitVector UnusedResourcesBitVector;

        public TagBlockBitVector OptionalResourcesBitVector;

        [TagField(MaxVersion = CacheVersion.Halo3ODST)]
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public TagBlockBitVector StreamedResourcesBitVector;

        // HO only: resource sizing was tracked directly on the manifest rather than solely via OverallUsage (which HO doesn't have, see below)
        [TagField(MinVersion = CacheVersion.HaloOnline498295, MaxVersion = CacheVersion.HaloOnline700123)]
        public uint RequiredPageableSize;
        [TagField(MinVersion = CacheVersion.HaloOnline498295, MaxVersion = CacheVersion.HaloOnline700123)]
        public uint OptionalMemorySize;

        [TagField(MaxVersion = CacheVersion.Halo3ODST)]
        [TagField(MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline449175)]
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public ZoneResourceUsage OverallUsage;

        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public StringId Name;

        public List<ZoneResourceUsage> ResourceUsage;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public List<ZoneResourceUsage> BudgetUsage;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public List<ZoneResourceUsage> UniqueBudgetUsage;

        public TagBlockBitVector ActiveResourceOwners;
        public TagBlockBitVector TopLevelResourceOwners;

        [TagField(MaxVersion = CacheVersion.Halo3ODST)]
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public TagBlock<ZoneResourceVisitNode> VisitationHeirarchy;

        // HO only: per-object dependency list used to attach loose multiplayer-sandbox objects (as opposed to the H3/Reach node-tree AttachmentHeirarchy above, which HO doesn't have)
        [TagField(MinVersion = CacheVersion.HaloOnline498295, MaxVersion = CacheVersion.HaloOnline700123)]
        public List<ZoneResourceZonesetObjects> ZonesetObjects;

        // Unidentified, possibly Reach-style Active/Touched/Designer/Cinematic bsp masks (below) collapsed into fewer fields for Halo Online. Byte-exact but unconfirmed
        [TagField(MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
        public uint Unknown1;
        [TagField(MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
        public uint Unknown2;
        [TagField(MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
        public uint Unknown3;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public uint ActiveBspMask;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public uint TouchedBspMask;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public uint DesignerZoneMask;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public uint CinematicZoneMask;

        // Halo Online only
        [TagStructure(Size = 0x1C, MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
        public class ZoneResourceZonesetObjects : TagStructure
        {
            public CachedTag Object;

            public List<ZoneResourceObjectDependency> Dependencies;

            [TagStructure(Size = 0x2)]
            public class ZoneResourceObjectDependency : TagStructure
            {
                public short TagResourceIndex;
            }
        }
    }
}