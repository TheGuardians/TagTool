using TagTool.IO;

using zone = TagTool.Legacy.cache_file_resource_gestalt;
namespace TagTool.Legacy.Halo3Beta
{
    public class cache_file_resource_gestalt : zone
    {
        protected cache_file_resource_gestalt() { }

        public cache_file_resource_gestalt(Base.CacheFile Cache, int Address)
        {
            EndianReader Reader = Cache.Reader;
            Reader.SeekTo(Address);

            #region Raw Entries
            Reader.SeekTo(Address + 36);
            int iCount = Reader.ReadInt32();
            int iOffset = Reader.ReadInt32() - Cache.Magic;
            for (int i = 0; i < iCount; i++)
                DefinitionEntries.Add(new RawEntry(Cache, iOffset + 96 * i));
            #endregion

            #region Fixup Data
            Reader.SeekTo(Address + 132);
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
            public int CacheIndex;
            public int RequiredOffset;
            public int RequiredSize;
            public int CacheIndex2;
            public int OptionalOffset;
            public int OptionalSize;

            public RawEntry(Base.CacheFile Cache, int Address)
            {
                EndianReader Reader = Cache.Reader;
                Reader.SeekTo(Address);

                Reader.SeekTo(Address + 12);
                TagID = Reader.ReadInt32();

                Reader.ReadInt16();
                Reader.ReadInt16();
                Reader.ReadInt32();

                Offset = Reader.ReadInt32();
                Size = Reader.ReadInt32();

                Reader.ReadInt32();

                CacheIndex = Reader.ReadInt32();
                RequiredOffset = Reader.ReadInt32();
                RequiredSize = Reader.ReadInt32();

                Reader.ReadInt32();

                CacheIndex2 = Reader.ReadInt32();
                OptionalOffset = Reader.ReadInt32();
                OptionalSize = Reader.ReadInt32();

                Reader.ReadInt32();
                Reader.ReadInt32();

                #region Resource Fixups
                int iCount = Reader.ReadInt32();
                int iOffset = Reader.ReadInt32() - Cache.Magic;
                for (int i = 0; i < iCount; i++)
                    Fixups.Add(new ResourceFixup(Cache, iOffset + 8 * i));
                #endregion

                #region Resource Definition Fixups
                Reader.SeekTo(Address + 84);
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

                    Reader.ReadInt32();
                    int val = Reader.ReadInt32();
                    FixupType = val >> 28;
                    Offset = val & 0x0FFFFFFF;
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
