using System;
using System.Collections.Generic;
using System.Linq;
using TagTool.Cache.Resources;
using TagTool.Tags;

namespace TagTool.Cache.Monolithic
{
    [TagStructure(Size = 0x6C)]
    public class TagResourceXSyncState
    {
        public XSyncStateHeader Header;
        public List<ResourceFixupLocation> ControlFixups;
        public List<ResourceFixupLocation> PagebleFixups;
        public List<ResourceFixupLocation> OptionalFixups;
        public List<Guid> InteropTypes;
        public byte[] ControlData;
        public uint ResourceOwner;
        public int Version;

        public TagResourceXSyncState()
        {

        }

        public TagResourceXSyncState(uint resourceOwner, PersistChunkReader reader, int version)
        {
            ResourceOwner = resourceOwner;
            Version = version;
            Header = ReadHeader(reader);
            ReadChunks(reader);
        }

        private void ReadChunks(PersistChunkReader reader)
        {
            foreach (var chunk in reader.ReadChunks())
            {
                var chunkReader = new PersistChunkReader(chunk.Stream, reader.Format);
                switch (chunk.Header.Signature.ToString())
                {
                    case "xsrc": // resource xsync state
                    case "inrc": // resource interop state
                        ReadChunks(chunkReader);
                        break;
                    case "inus": // interop usage
                        {
                            InteropTypes = new List<Guid>(Header.InteropUsageCount);
                            for (int i = 0; i < Header.InteropUsageCount; i++)
                                InteropTypes.Add(new Guid(chunkReader.ReadBytes(16)));
                        }
                        break;
                    case "ctrl": // control fixups
                        ControlFixups = chunkReader.Deserialize<ResourceFixupLocation>(Header.ControlFixupCount).ToList(); 
                        break;
                    case "data": // control data
                        ControlData = chunkReader.ReadBytes(Header.ControlDataSize);
                        break;
                    case "page": // pageable fixups
                        PagebleFixups = chunkReader.Deserialize<ResourceFixupLocation>(chunk.Header.Size / 8).ToList();
                        break;
                    case "opti": // optional fixups
                        OptionalFixups = chunkReader.Deserialize<ResourceFixupLocation>(chunk.Header.Size / 8).ToList();
                        break;
                    default:
                        break;
                }
            }
        }

        public XSyncStateHeader ReadHeader(PersistChunkReader reader)
        {
            XSyncStateHeader header = new XSyncStateHeader();
            switch (Version)
            {
                case 3:
                    XSyncStateHeaderV3 headerV3 = reader.Deserialize<XSyncStateHeaderV3>();
                    header = new XSyncStateHeader
                    {
                        CacheLocationOffset = headerV3.CacheLocationOffset,
                        CacheLocationSize = headerV3.CacheLocationSize,
                        OptionalLocationOffset = headerV3.OptionalLocationOffset,
                        OptionalLocationSize = headerV3.OptionalLocationSize,
                        ControlAlignmentBits = headerV3.ControlAlignmentBits,
                        ControlDataSize = headerV3.ControlDataSize,
                        ControlFixupCount = headerV3.ControlFixupCount,
                        InteropUsageCount = headerV3.InteropUsageCount,
                        RootAddress = headerV3.RootAddress
                    };
                    break;
                default:
                    header = reader.Deserialize<XSyncStateHeader>();
                    break;
            }
            return header;
        }

        [TagStructure(Size = 0x24)]
        public class XSyncStateHeader : TagStructure
        {
            public uint CacheLocationOffset;
            public uint CacheLocationSize;
            public uint OptionalLocationOffset;
            public uint OptionalLocationSize;
            public int ControlAlignmentBits;
            public int ControlDataSize;
            public int ControlFixupCount;
            public int InteropUsageCount;
            public CacheAddress RootAddress;
        }

        [TagStructure(Size = 0x2C)]
        public class XSyncStateHeaderV3 : TagStructure
        {
            public uint Unknown1;
            public uint Unknown2;
            public uint CacheLocationOffset;
            public uint CacheLocationSize;
            public uint OptionalLocationOffset;
            public uint OptionalLocationSize;
            public int ControlAlignmentBits;
            public int ControlDataSize;
            public int ControlFixupCount;
            public int InteropUsageCount;
            public CacheAddress RootAddress;
        }
    }
}


