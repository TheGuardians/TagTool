using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "cache_file_resource_layout_table", Tag = "play", Size = 0x44)]
    public class CacheFileResourceLayoutTable : TagStructure
    {
        public List<CacheFileCodecIdentifierBlock> CodecIdentifiers;
        public List<CacheFileSharedFileBlock> SharedFiles;
        public List<CacheFileResourcePageStruct> FilePages;
        public List<CacheFileResourceStreamingSubpageTableBlock> StreamingSubpageTables;
        public List<CacheFileResourceSectionBlock> Sections;
        public int RequiredLocationCount;
        public int RequiredDvdLocationCount;
        
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
    }
}
