using TagTool.IO;

using zone = TagTool.Legacy.cache_file_resource_gestalt;
namespace TagTool.Legacy.Halo3Retail
{
    public class cache_file_resource_gestalt : Halo3Beta.cache_file_resource_gestalt
    {
        protected cache_file_resource_gestalt() { }

        public cache_file_resource_gestalt(Base.CacheFile Cache, int Address)
        {
            EndianReader Reader = Cache.Reader;
            Reader.SeekTo(Address);

            #region Raw Entries
            Reader.SeekTo(Address + 88);
            int iCount = Reader.ReadInt32();
            int iOffset = Reader.ReadInt32() - Cache.Magic;
            for (int i = 0; i < iCount; i++)
                DefinitionEntries.Add(new RawEntry(Cache, iOffset + 64 * i));
            #endregion

            #region Fixup Data
            Reader.SeekTo(Address + 316);
            iCount = Reader.ReadInt32();
            Reader.ReadInt32();
            Reader.ReadInt32();
            iOffset = Reader.ReadInt32() - Cache.Magic;
            Reader.SeekTo(iOffset);
            DefinitionData = Reader.ReadBytes(iCount);
            #endregion
        }

        new public class RawEntry : zone.RawEntry
        {
            public RawEntry(Base.CacheFile Cache, int Address)
            {
                EndianReader Reader = Cache.Reader;
                Reader.SeekTo(Address);

                Reader.SeekTo(Address + 12);               
                TagID = Reader.ReadInt32();
                RawID = Reader.ReadInt32();
                Offset = Reader.ReadInt32();
                Size = Reader.ReadInt32();
                Reader.ReadInt32();
                LocationType = Reader.ReadInt16();
                SegmentIndex = Reader.ReadInt16();
                DefinitionAddress = Reader.ReadInt32();

                #region Resource Fixups
                Reader.SeekTo(Address + 40);
                int iCount = Reader.ReadInt32();
                int iOffset = Reader.ReadInt32() - Cache.Magic;
                for (int i = 0; i < iCount; i++)
                    Fixups.Add(new ResourceFixup(Cache, iOffset + 8 * i));
                #endregion

                #region Resource Definition Fixups
                Reader.SeekTo(Address + 52);
                iCount = Reader.ReadInt32();
                iOffset = Reader.ReadInt32() - Cache.Magic;
                for (int i = 0; i < iCount; i++)
                    DefinitionFixups.Add(new ResourceDefinitionFixup(Cache, iOffset + 8 * i));
                #endregion
            }

            new public class ResourceFixup : zone.RawEntry.ResourceFixup
            {
                public ResourceFixup(Base.CacheFile Cache, int Address)
                {
                    EndianReader Reader = Cache.Reader;
                    Reader.SeekTo(Address);

                    BlockOffset = Reader.ReadInt32();
                    //value is masked, 4bit unknown, 28bit offset
                    int val = Reader.ReadInt32();
                    FixupType = val >> 28;
                    Offset = val & 0x0FFFFFFF;
					RawFixup = val;
                }
            }

            new public class ResourceDefinitionFixup : zone.RawEntry.ResourceDefinitionFixup
            {
                public ResourceDefinitionFixup(Base.CacheFile Cache, int Address)
                {
                    EndianReader Reader = Cache.Reader;
                    Reader.SeekTo(Address);

                    Offset = Reader.ReadInt32();
                    Type = Reader.ReadInt32();
                }
            }
        }
    }
}
