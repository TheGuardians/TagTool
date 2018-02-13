using TagTool.Cache;
using TagTool.Common;
using TagTool.Serialization;
using System;
using System.Collections.Generic;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "cache_file_resource_gestalt", Size = 0x214, Tag = "zone", MaxVersion = CacheVersion.Halo3ODST)]
    public class CacheFileResourceGestalt
    {
        [TagField(MaxVersion = CacheVersion.Halo3Retail)]
        public MapTypeHalo3RetailValue MapTypeHalo3Retail;

        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public MapTypeHalo3OdstValue MapTypeHalo3Odst;

        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public MapSubTypeHalo3OdstValue MapSubTypeHalo3Odst;

        [TagField(MaxVersion = CacheVersion.Halo3ODST)]
        public short Flags;

        [TagField(MaxVersion = CacheVersion.Halo3ODST)]
        public List<ResourceType> ResourceTypes;

        [TagField(MaxVersion = CacheVersion.Halo3ODST)]
        public List<ResourceStructureType> ResourceStructureTypes;

        [TagField(MaxVersion = CacheVersion.Halo3ODST)]
        public List<CompressionCodec> CompressionCodecs;

        [TagField(MaxVersion = CacheVersion.Halo3ODST)]
        public List<ExternalCacheReference> ExternalCacheReferences;

        [TagField(MaxVersion = CacheVersion.Halo3ODST)]
        public List<RawPage> RawPages;

        [TagField(MaxVersion = CacheVersion.Halo3ODST)]
        public List<Size> Sizes;

        [TagField(MaxVersion = CacheVersion.Halo3ODST)]
        public List<Segment> Segments;

        public List<TagResource> TagResources;
        public List<Zoneset> DesignerZonesets;
        public List<Zoneset> GlobalZoneset;
        public uint Unknown;
        public uint Unknown2;
        public uint Unknown3;
        public List<Zoneset> UnattachedZoneset;
        public List<Zoneset> DiscForbiddenZoneset;
        public List<Zoneset> DiscAlwaysStreamingZoneset;
        public List<Zoneset> BspZonesets1;
        public List<Zoneset> BspZonesets2;
        public List<Zoneset> BspZonesets3;
        public List<Zoneset> CinematicZonesets;
        public List<Zoneset> ScenarioZonesets;
        public uint Unknown4;
        public uint Unknown5;
        public uint Unknown6;
        public uint Unknown7;
        public uint Unknown8;
        public uint Unknown9;
        public List<ScenarioZonesetGroup> ScenarioZonesetGroups;
        public List<CachedTagInstance> ScenarioBsps;
        public uint Unknown10;
        public uint Unknown11;
        public uint Unknown12;
        public uint Unknown13;
        public uint Unknown14;
        public uint Unknown15;
        public uint Unknown16;
        public uint Unknown17;
        public uint Unknown18;
        public byte[] FixupInformation;
        public uint Unknown19;
        public uint Unknown20;
        public uint Unknown21;
        public uint Unknown22;
        public uint Unknown23;
        public List<UnknownBlock> Unknown24;
        public uint Unknown25;
        public uint Unknown26;
        public uint Unknown27;
        public uint Unknown28;
        public uint Unknown29;
        public uint Unknown30;
        public uint Unknown31;
        public uint Unknown32;
        public uint Unknown33;
        public uint Unknown34;
        public uint Unknown35;
        public uint Unknown36;
        public uint Unknown37;
        public uint Unknown38;
        public uint Unknown39;
        public uint Unknown40;
        public uint Unknown41;
        public uint Unknown42;
        public uint Unknown43;
        public uint Unknown44;
        public uint Unknown45;
        public uint Unknown46;
        public uint Unknown47;
        public uint Unknown48;
        public List<PredictionABlock> PredictionA;
        public List<PredictionBBlock> PredictionB;
        public List<PredictionCBlock> PredictionC;
        public List<PredictionDTag> PredictionDTags;
        public List<PredictionD2Tag> PredictionD2Tags;
        public int CampaignId;
        public int MapId;

        public enum MapTypeHalo3RetailValue : short
        {
            SinglePlayer,
            Multiplayer,
            MainMenu
        }

        public enum MapTypeHalo3OdstValue : sbyte
        {
            SinglePlayer,
            Multiplayer,
            MainMenu
        }

        public enum MapSubTypeHalo3OdstValue : sbyte
        {
            None,
            Hub,
            Level,
            Scene,
            Cinematic
        }

        [TagStructure(Size = 0x1C)]
        public class ResourceType
        {
            [TagField(Length = 16)]
            public byte[] Guid;
            public short Unknown;
            public short Unknown2;
            public short Unknown3;
            public short Unknown4;
            [TagField(Label = true)]
            public StringId Name;
        }

        [TagStructure(Size = 0x14)]
        public class ResourceStructureType
        {
            [TagField(Length = 16)]
            public byte[] Guid;
            [TagField(Label = true)]
            public StringId Name;
        }

        [TagStructure(Size = 0x10)]
        public class CompressionCodec
        {
            [TagField(Length = 16)]
            public byte[] Guid;
        }

        [TagStructure(Size = 0x108)]
        public class ExternalCacheReference
        {
            [TagField(Length = 256)] public string MapPath;
            public short Unknown;
            public short Unknown2;
            public uint Unknown3;
        }
        
        [TagStructure(Size = 0x10, Align = 0x8)]
        public class Size
        {
            public int OverallSize;
            public List<Part> Parts;

            [TagStructure(Size = 0x8)]
            public class Part
            {
                public int Unknown;
                public int Size;
            }
        }

        [TagStructure(Size = 0x10, Align = 0x8)]
        public class Segment
        {
            public short PrimaryPageIndex;
            public short SecondaryPageIndex;
            public int PrimarySegmentOffset;
            public int SecondarySegmentOffset;
            public short PrimarySizeIndex;
            public short SecondarySizeIndex;
        }
        
        [TagStructure(Size = 0x78)]
        public class Zoneset
        {
            public List<MemoryPoolBlock> RequiredRawPool;
            public uint Unknown;
            public uint Unknown2;
            public uint Unknown3;
            public List<MemoryPoolBlock> OptionalRawPool;
            public List<MemoryPoolBlock> OptionalRawPool2;
            public uint Unknown4;
            public uint Unknown5;
            public uint Unknown6;
            public uint Unknown7;
            public uint Unknown8;
            public StringId SetName;
            public List<ResourceType> ResourceTypes;
            public List<MemoryPoolBlock> RequiredTagPool;
            public List<MemoryPoolBlock> OptionalTagPool;
            public uint Unknown9;
            public uint Unknown10;
            public uint Unknown11;

            [Flags]
            public enum MemberFlagsValue : int
            {
                None = 0,
                Member0 = 1 << 0,
                Member1 = 1 << 1,
                Member2 = 1 << 2,
                Member3 = 1 << 3,
                Member4 = 1 << 4,
                Member5 = 1 << 5,
                Member6 = 1 << 6,
                Member7 = 1 << 7,
                Member8 = 1 << 8,
                Member9 = 1 << 9,
                Member10 = 1 << 10,
                Member11 = 1 << 11,
                Member12 = 1 << 12,
                Member13 = 1 << 13,
                Member14 = 1 << 14,
                Member15 = 1 << 15,
                Member16 = 1 << 16,
                Member17 = 1 << 17,
                Member18 = 1 << 18,
                Member19 = 1 << 19,
                Member20 = 1 << 20,
                Member21 = 1 << 21,
                Member22 = 1 << 22,
                Member23 = 1 << 23,
                Member24 = 1 << 24,
                Member25 = 1 << 25,
                Member26 = 1 << 26,
                Member27 = 1 << 27,
                Member28 = 1 << 28,
                Member29 = 1 << 29,
                Member30 = 1 << 30,
                Member31 = 1 << 31
            }

            [TagStructure(Size = 0x4)]
            public class MemoryPoolBlock
            {
                public MemberFlagsValue ActiveMembers;
            }

            [TagStructure(Size = 0x14)]
            public class ResourceType
            {
                public uint Unknown;
                public uint Unknown2;
                public uint Unknown3;
                public uint Unknown4;
                public uint Unknown5;
            }
        }

        [TagStructure(Size = 0x24)]
        public class ScenarioZonesetGroup
        {
            [TagField(Label = true)]
            public StringId Name;
            public int BspGroupIndex;
            public BspFlagsValue ImportLoadedBsps;
            public BspFlagsValue LoadedBsps;
            public ZonesetFlagsValue LoadedDesignerZonesets;
            public ZonesetFlagsValue UnknownLoadedDesignerZonesets;
            public ZonesetFlagsValue UnloadedDesignerZonesets;
            public ZonesetFlagsValue LoadedCinematicZonesets;
            public int BspAtlasIndex;

            [Flags]
            public enum BspFlagsValue : int
            {
                None = 0,
                Bsp0 = 1 << 0,
                Bsp1 = 1 << 1,
                Bsp2 = 1 << 2,
                Bsp3 = 1 << 3,
                Bsp4 = 1 << 4,
                Bsp5 = 1 << 5,
                Bsp6 = 1 << 6,
                Bsp7 = 1 << 7,
                Bsp8 = 1 << 8,
                Bsp9 = 1 << 9,
                Bsp10 = 1 << 10,
                Bsp11 = 1 << 11,
                Bsp12 = 1 << 12,
                Bsp13 = 1 << 13,
                Bsp14 = 1 << 14,
                Bsp15 = 1 << 15,
                Bsp16 = 1 << 16,
                Bsp17 = 1 << 17,
                Bsp18 = 1 << 18,
                Bsp19 = 1 << 19,
                Bsp20 = 1 << 20,
                Bsp21 = 1 << 21,
                Bsp22 = 1 << 22,
                Bsp23 = 1 << 23,
                Bsp24 = 1 << 24,
                Bsp25 = 1 << 25,
                Bsp26 = 1 << 26,
                Bsp27 = 1 << 27,
                Bsp28 = 1 << 28,
                Bsp29 = 1 << 29,
                Bsp30 = 1 << 30,
                Bsp31 = 1 << 31
            }

            [Flags]
            public enum ZonesetFlagsValue : int
            {
                None = 0,
                Set0 = 1 << 0,
                Set1 = 1 << 1,
                Set2 = 1 << 2,
                Set3 = 1 << 3,
                Set4 = 1 << 4,
                Set5 = 1 << 5,
                Set6 = 1 << 6,
                Set7 = 1 << 7,
                Set8 = 1 << 8,
                Set9 = 1 << 9,
                Set10 = 1 << 10,
                Set11 = 1 << 11,
                Set12 = 1 << 12,
                Set13 = 1 << 13,
                Set14 = 1 << 14,
                Set15 = 1 << 15,
                Set16 = 1 << 16,
                Set17 = 1 << 17,
                Set18 = 1 << 18,
                Set19 = 1 << 19,
                Set20 = 1 << 20,
                Set21 = 1 << 21,
                Set22 = 1 << 22,
                Set23 = 1 << 23,
                Set24 = 1 << 24,
                Set25 = 1 << 25,
                Set26 = 1 << 26,
                Set27 = 1 << 27,
                Set28 = 1 << 28,
                Set29 = 1 << 29,
                Set30 = 1 << 30,
                Set31 = 1 << 31
            }
        }
        
        [TagStructure(Size = 0x14)]
        public class UnknownBlock
        {
            public uint Unknown;
            public uint Unknown2;
            public uint Unknown3;
            public uint Unknown4;
            public uint Unknown5;
        }

        [TagStructure(Size = 0x4)]
        public class PredictionABlock
        {
            public uint Key;
        }

        [TagStructure(Size = 0x8)]
        public class PredictionBBlock
        {
            public short OverallIndex;
            public short ACount;
            public int AIndex;
        }

        [TagStructure(Size = 0x4)]
        public class PredictionCBlock
        {
            public short OverallIndex;
            public short BIndex;
        }

        [TagStructure(Size = 0x8)]
        public class PredictionDTag
        {
            public short CCount;
            public short CIndex;
            public short ACount;
            public short AIndex;
        }

        [TagStructure(Size = 0x18)]
        public class PredictionD2Tag
        {
            public CachedTagInstance Tag;
            public int FirstValue;
            public int SecondValue;
        }
    }
}