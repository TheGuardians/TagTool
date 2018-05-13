using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using TagTool.IO;

namespace TagTool.Cache
{
    public class CacheFileGen2 : CacheFile
    {
        public CacheFileGen2(HaloOnlineCacheContext cacheContext, FileInfo file, CacheVersion version = CacheVersion.Halo2Xbox) :
            base(cacheContext, file, version)
        {
            IndexHeader = new CacheIndexHeader(this);
            IndexItems = new IndexTable(this);
            Strings = new StringTable(this);
            LocaleTables = new List<LocaleTable>();
        }
        
        new public class CacheIndexHeader : CacheFile.CacheIndexHeader
        {
            public CacheIndexHeader(CacheFile Cache)
            {
                cache = Cache;
                var Reader = cache.Reader;

                #region Read Values
                XmlNode indexHeaderNode = cache.VersionInfo.ChildNodes[1];

                Reader.SeekTo(cache.Header.TagIndexAddress);
                cache.Header.Magic = (int)(Reader.ReadUInt32() - (cache.Header.TagIndexAddress + 32));

                XmlAttribute attr = indexHeaderNode.Attributes["tagClassCount"];
                int offset = int.Parse(attr.Value);
                Reader.SeekTo(offset + cache.Header.TagIndexAddress);
                tagClassCount = Reader.ReadInt32();

                attr = indexHeaderNode.Attributes["tagInfoOffset"];
                offset = int.Parse(attr.Value);
                Reader.SeekTo(offset + cache.Header.TagIndexAddress);
                tagInfoOffset = Reader.ReadInt32() - cache.Header.Magic;

                tagClassCount = Reader.ReadInt32();
                tagInfoOffset = Reader.ReadInt32() - cache.Header.Magic;

                Reader.SeekTo(tagInfoOffset + 8);
                cache.Magic = (int)(Reader.ReadUInt32() - (cache.Header.TagIndexAddress + cache.Header.MemoryBufferSize));

                Reader.SeekTo(cache.Header.TagIndexAddress + 24);
                tagCount = Reader.ReadInt32();
                #endregion
            }
        }

        new public class IndexTable : CacheFile.IndexTable
        {
            public IndexTable(CacheFile Cache)
            {
                cache = Cache;

                var IH = cache.IndexHeader;
                var CH = cache.Header;
                var Reader = cache.Reader;

                ClassList = new List<TagClass>();

                #region Read Tags' Info
                var classDic = new Dictionary<string, int>();
                int[] sbspOffset = new int[0];
                int[] sbspMagic = new int[0];
                int[] sbspID = new int[0];

                Reader.SeekTo(IH.tagInfoOffset);
                for (int i = 0; i < IH.tagCount; i++)
                {
                    var cname = Reader.ReadString(4);
                    var tname = cname.ToCharArray();
                    Array.Reverse(tname);
                    cname = new string(tname);

                    if (!classDic.TryGetValue(cname, out int index))
                    {
                        index = classDic.Count;
                        classDic.Add(cname, classDic.Count);
                    }

                    IndexItem item = new IndexItem() { Cache = cache };
                    item.ClassIndex = index;
                    item.ID = Reader.ReadInt32();
                    item.Offset = Reader.ReadInt32() - cache.Magic;
                    Reader.ReadInt32(); //meta size
                    this.Add(item);

                    if (cname == "scnr")
                    {
                        long tempOffset = Reader.Position;

                        Reader.SeekTo(item.Offset + 528);
                        int jCount = Reader.ReadInt32();
                        int jOffset = Reader.ReadInt32() - cache.Magic;

                        sbspOffset = new int[jCount];
                        sbspMagic = new int[jCount];
                        sbspID = new int[jCount];

                        for (int j = 0; j < jCount; j++)
                        {
                            Reader.SeekTo(jOffset + j * 68);
                            sbspOffset[j] = Reader.ReadInt32();
                            Reader.ReadInt32();
                            sbspMagic[j] = Reader.ReadInt32() - sbspOffset[j];
                            Reader.SeekTo(jOffset + j * 68 + 20);
                            sbspID[j] = Reader.ReadInt32();
                        }
                        Reader.SeekTo(tempOffset);
                    }
                }

                for (int i = 0; i < sbspID.Length; i++)
                {
                    var tag = GetItemByID(sbspID[i]);
                    tag.Offset = sbspOffset[i];
                    tag.Magic = sbspMagic[i];
                }

                foreach (var pair in classDic)
                    ClassList.Add(new TagClass() { ClassCode = pair.Key });
                #endregion

                #region Read Indices

                Reader.SeekTo(CH.TagNamesIndicesOffset);
                var offsets = new int[IH.tagCount];
                for (int i = 0; i < IH.tagCount; i++)
                    offsets[i] = Reader.ReadInt32();

                #endregion

                #region Read Names
                Reader.StreamOrigin = CH.TagNamesBufferOffset;

                for (int i = 0; i < offsets.Length; i++)
                {
                    this[i].Filename = null;

                    if (offsets[i] == -1)
                        continue;

                    Reader.SeekTo(offsets[i]);

                    var name = "";
                    for (char c; (c = Reader.ReadChar()) != '\0'; name += c) ;

                    if (name.Length > 0)
                        this[i].Filename = name;
                }

                Reader.StreamOrigin = 0;
                #endregion
            }
        }

        public override byte[] GetRawFromID(int ID, int DataLength)
        {
            EndianReader er;
            string fName = "";

            long cIndex = (ID & 0xC0000000) >> 30;
            int offset = ID & 0x3FFFFFFF;

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