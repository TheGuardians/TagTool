using TagTool.Cache;
using TagTool.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace TagTool.Legacy.Halo2Xbox
{
    public class CacheFile : Base.CacheFile
    {
        public CacheFile(FileInfo file, CacheVersion version = CacheVersion.Halo2Xbox) :
            base(file, version)
        {
            Reader.Format = EndianFormat.LittleEndian;
            Header = new CacheHeader(this);
            IndexHeader = new CacheIndexHeader(this);
            IndexItems = new IndexTable(this);
            Strings = new StringTable(this);
            LocaleTables = new List<LocaleTable>();
        }
        
        new public class CacheHeader : Base.CacheFile.CacheHeader
        {
            public int LanguagePacksOffset = -1;
            public uint LanguagePacksSize = 0;
            public uint SecondarySoundGestaltDatumIndex = uint.MaxValue;
            public int FastLoadGeometryBlockOffset = -1;
            public uint FastLoadGeometryBlockSize = 0;
            public uint Checksum = 0;
            public uint MoppCodesChecksum = 0;
            
            public bool UsesCustomSoundGestalt => SecondarySoundGestaltDatumIndex != uint.MaxValue;
            public bool UsesCustomLanguagePack => LanguagePacksOffset != -1;

            public CacheHeader(Base.CacheFile Cache)
            {
                base.Cache = Cache;
                EndianReader Reader = base.Cache.Reader;

                Reader.SeekTo(0);

                Reader.ReadInt32();
                Reader.ReadInt32();

                FileLength = Reader.ReadInt32();

                Reader.ReadInt32();

                IndexOffset = Reader.ReadInt32();
                IndexStreamSize = Reader.ReadInt32();
                TagBufferSize = Reader.ReadInt32();

                Reader.ReadInt32();

                if (base.Cache.Version == CacheVersion.Halo2Vista)
                {
                    VirtualAddress = Reader.ReadUInt32();
                    TagDependencyGraphOffset = Reader.ReadInt32();
                    TagDependencyGraphSize = Reader.ReadUInt32();
                }

                SourceFile = Reader.ReadString(256);
                Build = Reader.ReadString(32);
                CacheType = Reader.ReadInt32();

                Reader.ReadInt32();
                Reader.ReadInt32();
                Reader.ReadInt32();
                Reader.ReadInt32();
                Reader.ReadInt32();
                Reader.ReadInt32();
                Reader.ReadInt32();
                Reader.ReadInt32();

                StringIdsCount = Reader.ReadInt32();
                StringIdsBufferSize = Reader.ReadInt32();
                StringIdIndicesOffset = Reader.ReadInt32();
                StringIdsBufferOffset = Reader.ReadInt32();

                Reader.ReadInt32();
                Reader.ReadInt32();
                Reader.ReadInt32();
                Reader.ReadInt32();
                Reader.ReadInt32();
                Reader.ReadInt32();
                Reader.ReadInt32();
                Reader.ReadInt32();
                Reader.ReadInt32();

                Name = Reader.ReadString(32);

                Reader.ReadInt32();

                ScenarioPath = Reader.ReadString(256);
                NeedsShared = Reader.ReadInt32() == 1;
                TagNamesCount = Reader.ReadInt32();
                TagNamesBufferOffset = Reader.ReadInt32();
                TagNamesBufferSize = Reader.ReadInt32();
                TagNameIndicesOffset = Reader.ReadInt32();

                if (base.Cache.Version == CacheVersion.Halo2Vista)
                {
                    LanguagePacksOffset = Reader.ReadInt32();
                    LanguagePacksSize = Reader.ReadUInt32();
                    SecondarySoundGestaltDatumIndex = Reader.ReadUInt32();
                    FastLoadGeometryBlockOffset = Reader.ReadInt32();
                    FastLoadGeometryBlockSize = Reader.ReadUInt32();
                    Checksum = Reader.ReadUInt32();
                    MoppCodesChecksum = Reader.ReadUInt32();
                    Reader.BaseStream.Seek(1288, SeekOrigin.Current);
                }
                else
                {
                    Checksum = Reader.ReadUInt32();
                    Reader.BaseStream.Seek(1324, SeekOrigin.Current);
                }
            }
        }

        new public class CacheIndexHeader : Base.CacheFile.CacheIndexHeader
        {
            public CacheIndexHeader(Base.CacheFile Cache)
            {
                cache = Cache;
                var Reader = cache.Reader;

                #region Read Values
                XmlNode indexHeaderNode = cache.versionNode.ChildNodes[1];

                XmlAttribute attr = indexHeaderNode.Attributes["tagClassCount"];
                int offset = int.Parse(attr.Value);
                Reader.SeekTo(offset + cache.Header.IndexOffset);
                tagClassCount = Reader.ReadInt32();

                attr = indexHeaderNode.Attributes["tagInfoOffset"];
                offset = int.Parse(attr.Value);
                Reader.SeekTo(offset + cache.Header.IndexOffset);
                tagInfoOffset = Reader.ReadInt32() - cache.Header.Magic;

                Reader.SeekTo(cache.Header.IndexOffset);
                cache.Header.Magic = Reader.ReadInt32() - (cache.Header.IndexOffset + 32);

                tagClassCount = Reader.ReadInt32();
                tagInfoOffset = Reader.ReadInt32() - cache.Header.Magic;

                Reader.SeekTo(tagInfoOffset + 8);
                cache.Magic = Reader.ReadInt32() - (cache.Header.IndexOffset + cache.Header.IndexStreamSize);

                Reader.SeekTo(cache.Header.IndexOffset + 24);
                tagCount = Reader.ReadInt32();
                #endregion
            }
        }

        new public class IndexTable : Base.CacheFile.IndexTable
        {
            public IndexTable(Base.CacheFile Cache)
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

                Reader.SeekTo(CH.TagNameIndicesOffset);
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
