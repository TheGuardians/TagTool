using System;
using System.Collections.Generic;
using System.IO;
using TagTool.Common;
using TagTool.IO;

namespace TagTool.Cache
{
    public class CacheFileGen2 : CacheFile
    {
        public static CacheFileGen2 MainMenuCache { get; private set; } = null;
        public static CacheFileGen2 SharedCache { get; private set; } = null;
        public static CacheFileGen2 SinglePlayerSharedCache { get; private set; } = null;

        public override string LocalesKey => null;

        public override string StringsKey => null;

        public override string TagsKey => null;

        public override string NetworkKey => null;

        public override string StringMods => null;

        public override int LocaleGlobalsOffset => -1;

        public override int LocaleGlobalsSize => -1;

        public CacheFileGen2(HaloOnlineCacheContext cacheContext, FileInfo file, CacheVersion version, bool memory) :
            base(cacheContext, file, version, memory)
        {
            if (file.Name == "mainmenu.map" && MainMenuCache?.File.FullName != file.FullName)
                MainMenuCache = this;
            else if (file.Name == "shared.map" && SharedCache?.File.FullName != file.FullName)
                SharedCache = this;
            else if (file.Name == "single_player_shared.map" && SharedCache?.File.FullName != file.FullName)
                SinglePlayerSharedCache = this;

            IndexHeader = new CacheIndexHeader(this);
            IndexItems = new IndexTable(this);
            Strings = new StringTable(this);
            LocaleTables = new List<LocaleTable>();
        }
        
        new public class CacheIndexHeader : CacheFile.CacheIndexHeader
        {
            public CacheIndexHeader(CacheFile cache)
            {
                var reader = cache.Reader;
                
                reader.SeekTo(cache.Header.TagIndexOffset);

                TagGroupsOffset = reader.ReadInt32();
                cache.Header.Magic = TagGroupsOffset - (cache.Header.TagIndexOffset + 32);
                TagGroupCount = reader.ReadInt32();
                TagsOffset = reader.ReadInt32() - cache.Header.Magic;
                ScenarioHandle = reader.ReadDatumIndex();
                GlobalsHandle = reader.ReadDatumIndex();
                CRC = reader.ReadInt32();
                TagCount = reader.ReadInt32();
                reader.ReadTag(); // 'tags'

                reader.SeekTo(TagsOffset + 8);
                cache.Magic = reader.ReadInt32() - (cache.Header.TagIndexOffset + cache.Header.MemoryBufferOffset);
            }
        }

        new public class IndexTable : CacheFile.IndexTable
        {
            public IndexTable(CacheFile cache)
            {
                Clear();

                var IH = cache.IndexHeader;
                var CH = cache.Header;
                var reader = cache.Reader;

                ClassList = new List<TagClass>();
                
                var classDic = new Dictionary<string, int>();

                var sbspOffset = new int[0];
                var sbspMagic = new int[0];
                var sbspID = new int[0];

                reader.SeekTo(IH.TagsOffset);
                for (int i = 0; i < IH.TagCount; i++)
                {
                    var cname = reader.ReadString(4);
                    var tname = cname.ToCharArray();
                    Array.Reverse(tname);
                    cname = new string(tname);

                    if (!classDic.TryGetValue(cname, out int index))
                    {
                        index = classDic.Count;
                        classDic.Add(cname, classDic.Count);
                    }

                    var item = new IndexItem
                    {
                        Cache = cache,
                        ClassIndex = index,
                        ID = reader.ReadInt32(),
                        Offset = reader.ReadInt32(),
                        Size = reader.ReadInt32()
                    };

                    if (item.Offset == 0 || item.ID == -1)
                        item.External = true;
                    else
                        item.Offset -= cache.Magic;
                    
                    Add(item);
                    
                    if (cname == "scnr" && cache.Version < CacheVersion.Halo2Vista)
                    {
                        long tempOffset = reader.Position;

                        reader.SeekTo(item.Offset + 528);
                        int jCount = reader.ReadInt32();
                        int jOffset = reader.ReadInt32() - cache.Magic;

                        sbspOffset = new int[jCount];
                        sbspMagic = new int[jCount];
                        sbspID = new int[jCount];

                        for (int j = 0; j < jCount; j++)
                        {
                            reader.SeekTo(jOffset + j * 68);
                            sbspOffset[j] = reader.ReadInt32();
                            reader.ReadInt32();
                            sbspMagic[j] = reader.ReadInt32() - sbspOffset[j];
                            reader.SeekTo(jOffset + j * 68 + 20);
                            sbspID[j] = reader.ReadInt32();
                        }

                        reader.SeekTo(tempOffset);
                    }
                }

                foreach (var pair in classDic)
                    ClassList.Add(new TagClass() { ClassCode = pair.Key });

                #region Read Indices

                reader.SeekTo(CH.TagNamesIndicesOffset);
                var offsets = new int[IH.TagCount];
                for (int i = 0; i < IH.TagCount; i++)
                    offsets[i] = reader.ReadInt32();

                #endregion

                #region Read Names
                reader.StreamOrigin = CH.TagNamesBufferOffset;

                for (int i = 0; i < offsets.Length; i++)
                {
                    this[i].Name = null;

                    if (offsets[i] == -1)
                        continue;

                    reader.SeekTo(offsets[i]);

                    var name = "";
                    for (char c; (c = reader.ReadChar()) != '\0'; name += c) ;

                    if (name.Length > 0)
                        this[i].Name = name;
                }

                reader.StreamOrigin = 0;
                #endregion
            }
        }

        public override byte[] GetRawFromID(DatumIndex ID, int DataLength)
        {
            EndianReader er;
            string fName = "";

            long cIndex = (ID.Value & 0xC0000000) >> 30;
            int offset = (int)ID.Value & 0x3FFFFFFF;

            if (cIndex != 0)
            {
                switch (cIndex)
                {
                    case 1:
                        fName = Path.Combine(File.Directory.FullName, "mainmenu.map");
                        break;
                    case 2:
                        fName = Path.Combine(File.Directory.FullName, "shared.map");
                        break;
                    case 3:
                        fName = Path.Combine(File.Directory.FullName, "single_player_shared.map");
                        break;
                }
                FileStream fs = new FileStream(fName, FileMode.Open, FileAccess.Read);
                er = new EndianReader(fs, EndianFormat.LittleEndian);
            }
            else er = Reader;

            er.SeekTo(offset);

            var data = er.ReadBytes(DataLength);

            if (er != Reader)
            {
                er.Close();
                er.Dispose();
            }

            return data;
        }

        public override void LoadResourceTags()
        {
        }
    }
}