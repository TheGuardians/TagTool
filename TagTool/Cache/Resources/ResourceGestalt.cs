using System.Collections.Generic;
using TagTool.Cache.Interops;
using TagTool.Common;
using TagTool.Tags;
using TagTool.Tags.Definitions;

namespace TagTool.Cache.Resources
{
    [TagStructure(Name = "cache_file_resource_gestalt", Tag = "zone", Size = 0x220, MaxVersion = CacheVersion.HaloReach11883, Platform = CachePlatform.MCC)]
    [TagStructure(Name = "cache_file_resource_gestalt", Tag = "zone", Size = 0x214, MaxVersion = CacheVersion.HaloReach11883, Platform = CachePlatform.Original)]
    public class ResourceGestalt : TagStructure
	{
        public ScenarioTypeEnum MapType;

        public ScenarioFlags Flags;

        public List<ResourceDefinition> ResourceDefinitions;
        public List<InteropDefinition> InteropDefinitions;

        public ResourceLayoutTable LayoutTable = new ResourceLayoutTable();

        public List<ResourceData> TagResources;

        public List<ZoneManifest> DesignerZoneManifests;
        public List<ZoneManifest> GlobalZoneManifests;
        public List<ZoneManifest> HsZoneManifests;
        public List<ZoneManifest> UnattachedDesignerZoneManifests;
        public List<ZoneManifest> DvdForbiddenZoneManifests;
        public List<ZoneManifest> DvdAlwaysStreamingZoneManifests;
        public List<ZoneManifest> DefaultBspZoneManifests;
        public List<ZoneManifest> StaticBspZoneManifests;
        public List<ZoneManifest> DynamicBspZoneManifests;
        public List<ZoneManifest> CinematicZoneManifests;

        public List<ZoneManifest> ZonesOnlyZoneSetManifests;
        public List<ZoneManifest> ExpectedZoneManifests;
        public List<ZoneManifest> FullyPopulatedZoneManifests;

        public List<ZoneSetZoneUsage> ZoneSetZoneUsages;

        public List<CachedTag> BspReferences;
        public List<TagReferenceBlock> TagReferences;

        public List<ModelVariantUsage> ModelVariantUsages;
        public List<CharacterUsage> CharacterUsages;

        public byte[] DefinitionData;

        public uint MinimumCompletePageableDataSize;
        public uint MinimumRequiredPageableDataSize;
        public uint MinimumRequiredDvdDataSize;

        public uint GlobalPageableDataSize;
        public uint OptionalControlDataSize;

        public List<ZoneResourceUsage> GlobalResourceUsage;

        [TagField(Flags = TagFieldFlags.Padding, Length = 96)]
        public byte[] Unused = new byte[96];

        public List<PredictionQuantum> PredictionQuanta;
        public List<PredictionAtom> PredictionAtoms;
        public List<PredictionMoleculeAtom> PredictionMoleculeAtoms;
        public List<PredictionMolecule> PredictionMolecules;
        public List<PredictionMoleculeKey> PredictionMoleculeKeys;

        public int CampaignId;
        public int MapId;

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
}