using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "cache_file_resource_gestalt", Tag = "zone", Size = 0x22C)]
    public class CacheFileResourceGestalt : TagStructure
    {
        public ScenarioTypeEnum ScenarioType;
        public ScenarioFlags ScenarioFlags1;
        public List<CacheFileResourceTypeIdentifierBlock> ResourceTypeIdentifiers;
        public List<CacheFileInteropTypeIdentifierBlock> InteropTypeIdentifiers;
        public List<CacheFileCodecIdentifierBlock> CodecIdentifiers;
        public List<CacheFileSharedFileBlock> SharedFiles;
        public List<CacheFileResourcePageStruct> FilePages;
        public List<CacheFileResourceStreamingSubpageTableBlock> StreamingSubpageTables;
        public List<CacheFileResourceSectionBlock> Sections;
        public List<CacheFileResourceDataBlock> Resources;
        public List<CacheFileTagZoneManifestStruct> DesignerZoneManifests;
        public List<CacheFileTagZoneManifestStruct> GlobalZoneManifest;
        public List<CacheFileTagZoneManifestStruct> HsZoneManifest;
        public List<CacheFileTagZoneManifestStruct> UnattachedDesignerZoneManifest;
        public List<CacheFileTagZoneManifestStruct> DvdForbiddenZoneManifest;
        public List<CacheFileTagZoneManifestStruct> DvdAlwaysStreamingZoneManifest;
        public List<CacheFileTagZoneManifestStruct> DefaultBspZoneManifests;
        public List<CacheFileTagZoneManifestStruct> StaticBspZoneManifests;
        public List<CacheFileTagZoneManifestStruct> DynamicBspZoneManifests;
        public List<CacheFileTagZoneManifestStruct> CinematicZoneManifests;
        public List<CacheFileTagZoneManifestStruct> RequiredMapVariantManifests;
        public List<CacheFileTagZoneManifestStruct> SandboxMapVariantManifests;
        public List<CacheFileTagZoneManifestStruct> ZoneOnlyZoneSetManifests;
        public List<CacheFileTagZoneManifestStruct> ExpectedZoneSetManifests;
        public List<CacheFileTagZoneManifestStruct> FullyPopulatedZoneSetManifests;
        public List<CacheFileZoneSetZoneUsageBlock> ZoneSetZoneUsage;
        public List<CacheFileBspReferenceBlock> BspReferences;
        public List<CacheFileResourceOwnerBlock> ResourceOwners;
        public List<CacheFileModelVariantUsageBlock> ModelVariantUsage;
        public List<CacheFileCharacterUsageBlock> CharacterUsage;
        public byte[] NaiveResourceControlData;
        public int MinimumCompleteResourceSize;
        public int MinimumRequiredResourceSize;
        public int MinimumDvdResourceSize;
        // intersection of resources amongst all zone sets
        public int GlobalRequiredResourceSize;
        public int TotalOptionalControlDataSize;
        public List<CacheFileTagResourceUsageBlockStruct> OverallResourceUsage;
        public List<CacheFileBspGameAttachmentsBlockStruct> BspGameAttachments;
        public List<DebugCacheFileZoneManifestStruct> ModelVariantZones;
        public List<DebugCacheFileZoneManifestStruct> CombatDialogueZones;
        public List<DebugCacheFileZoneManifestStruct> TagZones;
        public List<CacheFileDebugResourceDefinitionBlock> DebugResourceDefinitions;
        public List<CacheFileResourceLayoutBlockStruct> ResourceLayouts;
        public List<CacheFileTagResourcePropertiesBlock> ResourceProperties;
        public List<CacheFileTagParentageBlockStruct> Parentages;
        public CacheFileTagResourcePredictionTable PredictionTable;
        public int MatIsInAReallyBadMoodCampaignId;
        public int NextTimeWeDonTPutThingsThatTheGameDependsOnOutsideOfToolGuerillaOrSapienMapId;
        
        public enum ScenarioTypeEnum : short
        {
            Solo,
            Multiplayer,
            MainMenu,
            MultiplayerShared,
            SinglePlayerShared
        }
        
        [Flags]
        public enum ScenarioFlags : ushort
        {
            // always draw sky 0, even if no +sky polygons are visible
            AlwaysDrawSky = 1 << 0,
            // always leave pathfinding in, even for a multiplayer scenario
            DonTStripPathfinding = 1 << 1,
            SymmetricMultiplayerMap = 1 << 2,
            QuickLoading = 1 << 3,
            CharactersUsePreviousMissionWeapons = 1 << 4,
            LightmapsSmoothPalettesWithNeighbors = 1 << 5,
            SnapToWhiteAtStart = 1 << 6,
            OverrideGlobals = 1 << 7,
            BigVehicleUseCenterPointForLightSampling = 1 << 8,
            DonTUseCampaignSharing = 1 << 9,
            IgnoreSizeAndCanTShip = 1 << 10,
            AlwaysRunLightmapsPerBsp = 1 << 11,
            // so we can hide hud elements like the compass
            InSpace = 1 << 12,
            // so we can strip the elite from the global player representations
            Survival = 1 << 13,
            // so we can test the impact of variant stripping
            DoNotStripVariants = 1 << 14
        }
        
        [TagStructure(Size = 0x20)]
        public class CacheFileResourceTypeIdentifierBlock : TagStructure
        {
            public int IdentifierPart0;
            public int IdentifierPart1;
            public int IdentifierPart2;
            public int IdentifierPart3;
            public int DefinitionFlags;
            public StringId Name;
            [TagField(Length = 3)]
            public TagResourceAlignmentBitsArrayDefinition[]  PageAlignmentBits;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            
            [TagStructure(Size = 0x2)]
            public class TagResourceAlignmentBitsArrayDefinition : TagStructure
            {
                public short PageAlignmentBits;
            }
        }
        
        [TagStructure(Size = 0x14)]
        public class CacheFileInteropTypeIdentifierBlock : TagStructure
        {
            public int IdentifierPart0;
            public int IdentifierPart1;
            public int IdentifierPart2;
            public int IdentifierPart3;
            public StringId Name;
        }
        
        [TagStructure(Size = 0x10)]
        public class CacheFileCodecIdentifierBlock : TagStructure
        {
            public int IdentifierPart0;
            public int IdentifierPart1;
            public int IdentifierPart2;
            public int IdentifierPart3;
        }
        
        [TagStructure(Size = 0x108)]
        public class CacheFileSharedFileBlock : TagStructure
        {
            [TagField(Length = 256)]
            public string DvdRelativePath;
            public CacheFileSharedFileFlags Flags;
            public short GlobalSharedLocationOffset;
            public int IoOffset;
            
            [Flags]
            public enum CacheFileSharedFileFlags : ushort
            {
                UseHeaderIoOffset = 1 << 0,
                NotRequired = 1 << 1,
                UseHeaderLocations = 1 << 2
            }
        }
        
        [TagStructure(Size = 0x58)]
        public class CacheFileResourcePageStruct : TagStructure
        {
            public short HeaderSaltAtRuntime;
            public CacheFileTagResourcePageFlags Flags;
            public sbyte Codec;
            public short SharedFile;
            public short SharedFileLocationIndex;
            public int FileOffset;
            public int FileSize;
            public int Size;
            public ResourceChecksumStruct Checksum;
            public short ResourceReferenceCount;
            public short StreamingSubpageTable;
            
            [Flags]
            public enum CacheFileTagResourcePageFlags : byte
            {
                ValidChecksum = 1 << 0,
                SharedAndRequired = 1 << 1,
                DvdOnlySharedAndRequired = 1 << 2,
                DvdOnlyAndRequired = 1 << 3,
                ReferencedByCacheFileHeader = 1 << 4,
                OnlyFullValidChecksum = 1 << 5
            }
            
            [TagStructure(Size = 0x40)]
            public class ResourceChecksumStruct : TagStructure
            {
                public int Checksum;
                [TagField(Length = 20)]
                public ResourceHashDefinition[]  EntireHash;
                [TagField(Length = 20)]
                public ResourceHashDefinition[]  FirstChunkHash;
                [TagField(Length = 20)]
                public ResourceHashDefinition[]  LastChunkHash;
                
                [TagStructure(Size = 0x1)]
                public class ResourceHashDefinition : TagStructure
                {
                    public byte HashByte;
                }
            }
        }
        
        [TagStructure(Size = 0x10)]
        public class CacheFileResourceStreamingSubpageTableBlock : TagStructure
        {
            public int TotalMemorySize;
            public List<CacheFileResourceStreamingSubpageBlock> StreamingSubpages;
            
            [TagStructure(Size = 0x8)]
            public class CacheFileResourceStreamingSubpageBlock : TagStructure
            {
                public int MemoryOffset;
                public int MemorySize;
            }
        }
        
        [TagStructure(Size = 0x18)]
        public class CacheFileResourceSectionBlock : TagStructure
        {
            [TagField(Length = 3)]
            public LocationOffsetsArrayDefinition[]  PageOffsets;
            [TagField(Length = 3)]
            public FileLocationIndexesArrayDefinition[]  FilePageIndexes;
            [TagField(Length = 3)]
            public SublocationTableIndexesArrayDefinition[]  SubpageTableIndexes;
            
            [TagStructure(Size = 0x4)]
            public class LocationOffsetsArrayDefinition : TagStructure
            {
                public int Offset;
            }
            
            [TagStructure(Size = 0x2)]
            public class FileLocationIndexesArrayDefinition : TagStructure
            {
                public short PageIndex;
            }
            
            [TagStructure(Size = 0x2)]
            public class SublocationTableIndexesArrayDefinition : TagStructure
            {
                public short SubpageTableIndex;
            }
        }
        
        [TagStructure(Size = 0x44)]
        public class CacheFileResourceDataBlock : TagStructure
        {
            public CachedTag OwnerTag;
            public short ResourceSalt;
            public sbyte ResourceTypeIndex;
            public sbyte ControlAlignmentBits;
            public int ControlSize;
            public CacheFileResourceDataFlags Flags;
            public short Page;
            public CacheAddress RootFixup;
            public List<CacheFileResourceFixupLocationBlock> ControlFixups;
            public List<CacheFileResourceInteropLocationBlock> InteropLocations;
            public List<CacheFileResourcePriorityDataBlock> PriorityLevelData;
            
            [Flags]
            public enum CacheFileResourceDataFlags : ushort
            {
                HasHighestPriorityData = 1 << 0,
                HasMediumPriorityData = 1 << 1,
                HasLowPriorityData = 1 << 2
            }
            
            [TagStructure(Size = 0x8)]
            public class CacheFileResourceFixupLocationBlock : TagStructure
            {
                public uint BlockOffset;
                public CacheAddress Address;
            }
            
            [TagStructure(Size = 0x8)]
            public class CacheFileResourceInteropLocationBlock : TagStructure
            {
                public int EncodedInteropLocation;
                public int InteropTypeIndex;
            }
            
            [TagStructure(Size = 0x4)]
            public class CacheFileResourcePriorityDataBlock : TagStructure
            {
                public int NaiveDataOffset;
            }
        }
        
        [TagStructure(Size = 0x90)]
        public class CacheFileTagZoneManifestStruct : TagStructure
        {
            public List<CacheFileTagResourcesBitvectorBlock> CachedResourceBitvector;
            public List<CacheFileTagResourcesBitvectorBlock> StreamedResourceBitvector;
            public StringId Name;
            [TagField(Length = 3)]
            public ResourceUsagePageSizeArrayDefinition[]  PageSizes;
            public int DeferredRequiredSize;
            public int StreamedResourceSize;
            public int DvdInMemoryResourceSize;
            public List<CacheFileTagResourceUsageBlockStruct> ResourceUsage;
            public List<CacheFileTagResourceUsageBlockStruct> BudgetUsage;
            public List<CacheFileTagResourceUsageBlockStruct> UniqueBudgetUsage;
            public List<CacheFileTagResourcesBitvectorBlock> ActiveResourceOwners;
            public List<CacheFileTagResourcesBitvectorBlock> TopLevelResourceOwners;
            public List<CacheFileZoneResourceVisitNodeBlockStruct> VisitationHierarchy;
            public int ActiveBspMask;
            public int TouchedBspMask;
            public int CinematicZoneMask;
            public ulong DesignerZoneMask;
            
            [TagStructure(Size = 0x4)]
            public class CacheFileTagResourcesBitvectorBlock : TagStructure
            {
                public int _32Bits;
            }
            
            [TagStructure(Size = 0x4)]
            public class ResourceUsagePageSizeArrayDefinition : TagStructure
            {
                public int PageSize;
            }
            
            [TagStructure(Size = 0x1C)]
            public class CacheFileTagResourceUsageBlockStruct : TagStructure
            {
                public StringId Name;
                [TagField(Length = 3)]
                public ResourceUsagePageSizeArrayDefinition[]  PageSizes;
                public int DeferredRequiredSize;
                public int StreamedResourceSize;
                public int DvdInMemoryResourceSize;
            }
            
            [TagStructure(Size = 0x10)]
            public class CacheFileZoneResourceVisitNodeBlockStruct : TagStructure
            {
                public short ParentTag;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public List<CacheFileZoneResourceVisitNodeLinkBlock> Children;
                
                [TagStructure(Size = 0x2)]
                public class CacheFileZoneResourceVisitNodeLinkBlock : TagStructure
                {
                    public short ChildTag;
                }
            }
        }
        
        [TagStructure(Size = 0x28)]
        public class CacheFileZoneSetZoneUsageBlock : TagStructure
        {
            public StringId Name;
            public ScenarioZoneSetFlags Flags;
            public uint RequiredBspZones;
            public uint ExpectedTouchedBspZones;
            public ulong RequiredDesignerZones;
            public ulong ExpectedDesignerZones;
            public uint RequiredCinematicZones;
            public int HintPreviousZoneSet;
            
            [Flags]
            public enum ScenarioZoneSetFlags : uint
            {
                BeginLoadingNextLevel = 1 << 0,
                DebugPurposesOnly = 1 << 1,
                InteralZoneSet = 1 << 2,
                DisableSkyClearing = 1 << 3,
                OverrideSkyClearColor = 1 << 4
            }
        }
        
        [TagStructure(Size = 0x10)]
        public class CacheFileBspReferenceBlock : TagStructure
        {
            public CachedTag Bsp;
        }
        
        [TagStructure(Size = 0x14)]
        public class CacheFileResourceOwnerBlock : TagStructure
        {
            public CachedTag ResourceOwner;
            public int ActualTagIndex;
        }
        
        [TagStructure(Size = 0x14)]
        public class CacheFileModelVariantUsageBlock : TagStructure
        {
            public short Model;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public StringId Variant;
            public List<CacheFileResourceOwnerReferenceBlock> UsedMaterials;
            
            [TagStructure(Size = 0x2)]
            public class CacheFileResourceOwnerReferenceBlock : TagStructure
            {
                public short Tag;
            }
        }
        
        [TagStructure(Size = 0x10)]
        public class CacheFileCharacterUsageBlock : TagStructure
        {
            public short Model;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public List<CacheFileModelVariantUsageReferenceBlock> UsedModelVariants;
            
            [TagStructure(Size = 0x2)]
            public class CacheFileModelVariantUsageReferenceBlock : TagStructure
            {
                public short ModelVariant;
            }
        }
        
        [TagStructure(Size = 0x1C)]
        public class CacheFileTagResourceUsageBlockStruct : TagStructure
        {
            public StringId Name;
            [TagField(Length = 3)]
            public ResourceUsagePageSizeArrayDefinition[]  PageSizes;
            public int DeferredRequiredSize;
            public int StreamedResourceSize;
            public int DvdInMemoryResourceSize;
            
            [TagStructure(Size = 0x4)]
            public class ResourceUsagePageSizeArrayDefinition : TagStructure
            {
                public int PageSize;
            }
        }
        
        [TagStructure(Size = 0x24)]
        public class CacheFileBspGameAttachmentsBlockStruct : TagStructure
        {
            public List<CacheFileBspAttachmentBlock> Static;
            public List<CacheFileBspAttachmentBlock> Persistent;
            public List<CacheFileBspAttachmentBlock> Dynamic;
            
            [TagStructure(Size = 0x10)]
            public class CacheFileBspAttachmentBlock : TagStructure
            {
                public CachedTag Attachment;
            }
        }
        
        [TagStructure(Size = 0xA8)]
        public class DebugCacheFileZoneManifestStruct : TagStructure
        {
            public CacheFileTagZoneManifestStruct CacheZoneManifest;
            public int DiskSize;
            public int UnusedSize;
            public CachedTag OwnerTag;
        }
        
        [TagStructure(Size = 0xC)]
        public class CacheFileDebugResourceDefinitionBlock : TagStructure
        {
            public List<ResourceCategoryBlock> Categories;
            
            [TagStructure(Size = 0x4)]
            public class ResourceCategoryBlock : TagStructure
            {
                public StringId Name;
            }
        }
        
        [TagStructure(Size = 0x38)]
        public class CacheFileResourceLayoutBlockStruct : TagStructure
        {
            [TagField(Length = 3)]
            public ResourceLayoutMemorySizeArrayDefinition[]  MemorySizes;
            [TagField(Length = 3)]
            public ResourceLayoutCompressedSizeArrayDefinition[]  CompressedSizes;
            public int DeferredRequiredResourceSize;
            public int UnusedResourceSize;
            public CacheFileResourceGlobalZoneAttachmentFlags GlobalZoneAttachment;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public uint BspZoneAttachment;
            public ulong DesignerZoneAttachment;
            public uint CinematicZoneAttachment;
            [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
            public byte[] Padding1;
            
            [Flags]
            public enum CacheFileResourceGlobalZoneAttachmentFlags : ushort
            {
                Global = 1 << 0,
                Script = 1 << 1,
                HddOnly = 1 << 2,
                AlwaysStreaming = 1 << 3,
                Unattached = 1 << 4
            }
            
            [TagStructure(Size = 0x4)]
            public class ResourceLayoutMemorySizeArrayDefinition : TagStructure
            {
                public int MemorySize;
            }
            
            [TagStructure(Size = 0x4)]
            public class ResourceLayoutCompressedSizeArrayDefinition : TagStructure
            {
                public int CompressedSize;
            }
        }
        
        [TagStructure(Size = 0xC)]
        public class CacheFileTagResourcePropertiesBlock : TagStructure
        {
            public List<CacheFileTagResourceNamedValueBlock> NamedValues;
            
            [TagStructure(Size = 0x18)]
            public class CacheFileTagResourceNamedValueBlock : TagStructure
            {
                public StringId Name;
                public NamedValueTypeEnum Type;
                public int Row;
                public StringId StringValue;
                public float RealValue;
                public int IntValue;
                
                public enum NamedValueTypeEnum : int
                {
                    Unknown,
                    String,
                    Real,
                    Int
                }
            }
        }
        
        [TagStructure(Size = 0x2C)]
        public class CacheFileTagParentageBlockStruct : TagStructure
        {
            public CachedTag Tag;
            public CacheFileTagParentageFlags Flags;
            public short ResourceOwnerIndex;
            public List<CacheFileTagParentageReferenceBlock> Parents;
            public List<CacheFileTagParentageReferenceBlock> Children;
            
            [Flags]
            public enum CacheFileTagParentageFlags : ushort
            {
                LoadedByGame = 1 << 0,
                Unloaded = 1 << 1
            }
            
            [TagStructure(Size = 0x4)]
            public class CacheFileTagParentageReferenceBlock : TagStructure
            {
                public int Link;
            }
        }
        
        [TagStructure(Size = 0x3C)]
        public class CacheFileTagResourcePredictionTable : TagStructure
        {
            public List<CacheFileTagResourcePredictionQuantumBlock> PredictionQuanta;
            public List<CacheFileTagResourcePredictionAtomBlock> PredictionAtoms;
            public List<CacheFileTagResourcePredictionMoleculeAtomReferenceBlock> PredictionMoleculeAtoms;
            public List<CacheFileTagResourcePredictionMoleculeBlock> PredictionMolecules;
            public List<CacheFileTagResourcePredictionMoleculeKeysBlock> PredictionMoleculeKeys;
            
            [TagStructure(Size = 0x4)]
            public class CacheFileTagResourcePredictionQuantumBlock : TagStructure
            {
                public int InternalResourceHandle;
            }
            
            [TagStructure(Size = 0x8)]
            public class CacheFileTagResourcePredictionAtomBlock : TagStructure
            {
                public short IndexSalt;
                public ushort PredictionQuantumCount;
                public int FirstPredictionQuantumIndex;
            }
            
            [TagStructure(Size = 0x4)]
            public class CacheFileTagResourcePredictionMoleculeAtomReferenceBlock : TagStructure
            {
                public int PredictionAtomHandle;
            }
            
            [TagStructure(Size = 0x8)]
            public class CacheFileTagResourcePredictionMoleculeBlock : TagStructure
            {
                public ushort PredictionAtomCount;
                public short FirstPredictionAtomIndex;
                public ushort PredictionQuantumCount;
                public ushort FirstPredictionQuantumIndex;
            }
            
            [TagStructure(Size = 0xC)]
            public class CacheFileTagResourcePredictionMoleculeKeysBlock : TagStructure
            {
                public int IndexA;
                public int IndexB;
                public int IndexC;
            }
        }
    }
}
