using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using TagTool.Common;
using TagTool.IO;
using TagTool.Serialization;
using TagTool.Tags;
using TagTool.Tags.Definitions;

namespace TagTool.Cache
{
    public abstract class CacheFile
    {
        public HaloOnlineCacheContext CacheContext { get; set; }

        public FileInfo File;
        public Stream Stream;
        public EndianReader Reader;

        public CacheVersion Version;
        public TagDeserializer Deserializer;

        public int Magic;

        public CacheFileHeader Header;
        public CacheIndexHeader IndexHeader;
        public IndexTable IndexItems;
        public StringTable Strings;
        public List<LocaleTable> LocaleTables;
        public StringIdResolver Resolver;

        public CacheFileResourceGestalt ResourceGestalt;
        public CacheFileResourceLayoutTable ResourceLayoutTable;

        public string Build => CacheVersionDetection.GetBuildName(Version);

        public XmlNode BuildInfo => GetBuildNode(Build);
        public XmlNode VersionInfo => GetVersionNode(BuildInfo.Attributes["version"].Value);

        public string LocalesKey => BuildInfo.Attributes["localesKey"].Value;
        public string StringsKey => BuildInfo.Attributes["stringsKey"].Value;
        public string TagsKey => BuildInfo.Attributes["tagsKey"].Value;
        public string NetworkKey => BuildInfo.Attributes["networkKey"].Value;
        public string StringMods => BuildInfo.Attributes["stringMods"].Value;

        public CacheFile(HaloOnlineCacheContext cacheContext, FileInfo file, CacheVersion version)
        {
            CacheContext = cacheContext;
            File = file;
            Version = version;
            Deserializer = new TagDeserializer(Version);

            Stream = new MemoryStream();

            using (var fileStream = file.OpenRead())
            {
                fileStream.Seek(0, SeekOrigin.Begin);
                fileStream.CopyTo(Stream);
            }

            Reader = new EndianReader(Stream, EndianFormat.LittleEndian);

            Reader.SeekTo(0);
            if (Reader.ReadTag() == "daeh")
                Reader.Format = EndianFormat.BigEndian;

            Reader.SeekTo(0);
            Header = Deserializer.Deserialize<CacheFileHeader>(new DataSerializationContext(Reader));
        }
        
        public class CacheIndexHeader
        {
            public int TagGroupsOffset;
            public int TagGroupCount;
            public int TagsOffset;
            public uint ScenarioHandle;
            public uint GlobalsHandle;
            public int CRC;
            public int TagCount;
            public int TagInfoHeaderCount;
            public int TagInfoHeaderOffset;
            public int TagInfoHeaderCount2;
            public int TagInfoHeaderOffset2;
        }

        public class StringTable : List<string>
        {
            protected CacheFile Cache;

            public StringTable(CacheFile cache)
            {
                Cache = cache;
                var reader = this.Cache.Reader;
                var cacheHeader = this.Cache.Header;

                reader.SeekTo(cacheHeader.StringIDsIndicesOffset);
                int[] indices = new int[cacheHeader.StringIDsCount];
                for (int i = 0; i < cacheHeader.StringIDsCount; i++)
                {
                    indices[i] = reader.ReadInt32();
                    this.Add("");
                }

                reader.SeekTo(cacheHeader.StringIDsBufferOffset);

                EndianReader newReader = null;

                if (this.Cache.StringsKey == "" || this.Cache.StringsKey == null)
                {
                    newReader = new EndianReader(new MemoryStream(reader.ReadBytes(cacheHeader.StringIDsBufferSize)), EndianFormat.BigEndian);
                }
                else
                {
                    reader.BaseStream.Position = cacheHeader.StringIDsBufferOffset;
                    newReader = new EndianReader(reader.DecryptAesSegment(cacheHeader.StringIDsBufferSize, Cache.StringsKey));
                }

                for (int i = 0; i < indices.Length; i++)
                {
                    if (indices[i] == -1)
                    {
                        this[i] = "<null>";
                        continue;
                    }

                    newReader.SeekTo(indices[i]);

                    int length;
                    if (i == indices.Length - 1)
                        length = cacheHeader.StringIDsBufferSize - indices[i];
                    else
                        length = (indices[i + 1] != -1)
                            ? indices[i + 1] - indices[i]
                            : indices[i + 2] - indices[i];

                    if (length == 1)
                    {
                        this[i] = "";
                        continue;
                    }

                    this[i] = newReader.ReadString(length);
                }
                newReader.Close();
                newReader.Dispose();
            }

            public string GetItemByID(int ID)
            {
                //go through the modifiers, if the ID matches a modifer return the correct string
                string[] mods = Cache.StringMods.Split(';');
                try
                {
                    foreach (string mod in mods)
                    {
                        string[] Params = mod.Split(','); //[0] - check, [1] - change
                        int check = int.Parse(Params[0]);
                        int change = int.Parse(Params[1]);

                        if (check < 0)
                        {
                            if (ID < check)
                            {
                                ID += change;
                                return this[ID];
                            }
                        }
                        else
                        {
                            if (ID > check)
                            {
                                ID += change;
                                return this[ID];
                            }
                        }
                    }
                }
                catch
                {
                    return "invalid";
                }

                //if no matching modifier, return the string at index of ID, or null if out of bounds
                try { return this[ID]; }
                catch { return ""; }
            }

            public string GetString(StringId stringId)
            {
                if (Cache.Version < CacheVersion.Halo3Retail)
                    return GetItemByID((int)stringId.Value);

                var index = Cache.Resolver.StringIDToIndex(stringId);

                return this[index];
            }
        }

        public class LocalizedString
        {
            public int StringIndex;
            public string String;
            public int Index;

            public LocalizedString(int index,string locale, int localeIndex)
            {
                StringIndex = index;
                String = locale;
                Index = localeIndex;
            }
        }

        public class LocaleTable : List<LocalizedString>
        {
            protected CacheFile cache;

            public LocaleTable(CacheFile Cache, GameLanguage Lang)
            {
                cache = Cache;
                var reader = cache.Reader;
                var CH = cache.Header;

                int matgOffset = -1;
                foreach (IndexItem item in cache.IndexItems)
                    if (item.GroupTag == "matg")
                    {
                        matgOffset = item.Offset;
                        break;
                    }

                if (matgOffset == -1) return;

                var buildInfo = cache.BuildInfo;
                int localeStart = int.Parse(buildInfo.Attributes["localesStart"].Value);
                reader.SeekTo(matgOffset + localeStart + (int)Lang * int.Parse(buildInfo.Attributes["languageSize"].Value));

                int localeCount = reader.ReadInt32();
                int tableSize = reader.ReadInt32();

                var indexOffset = (int)(reader.ReadInt32() + CH.Interop.UnknownBaseAddress);
                var tableOffset = (int)(reader.ReadInt32() + CH.Interop.UnknownBaseAddress);

                #region Read Indices
                reader.SeekTo(indexOffset);
                int[] indices = new int[localeCount];
                for (int i = 0; i < localeCount; i++)
                {
                    Add(new LocalizedString(reader.ReadInt32(), "", i));
                    indices[i] = reader.ReadInt32();  
                }
                #endregion

                #region Read Names
                reader.SeekTo(tableOffset);

                EndianReader newReader = null;

                if (cache.LocalesKey == "" || cache.LocalesKey == null)
                {
                    newReader = new EndianReader(new MemoryStream(reader.ReadBytes(tableSize)), EndianFormat.BigEndian);
                }
                else
                {
                    reader.BaseStream.Position = tableOffset;
                    newReader = new EndianReader(reader.DecryptAesSegment(tableSize, cache.LocalesKey));
                }

                for (int i = 0; i < indices.Length; i++)
                {
                    if (indices[i] == -1)
                    {                        
                        this[i].String = "<null>";
                        continue;
                    }

                    newReader.SeekTo(indices[i]);

                    int length;
                    if (i == indices.Length - 1)
                        length = tableSize - indices[i];
                    else
                        length = (indices[i + 1] != -1)
                            ? indices[i + 1] - indices[i]
                            : indices[i + 2] - indices[i];

                    if (length == 1)
                    {
                       
                        this[i].String="<blank>";
                        continue;
                    }
                    this[i].String= newReader.ReadString(length);
                }
                newReader.Close();
                newReader.Dispose();
                #endregion
            }
        }

        public class IndexItem
        {
            public CacheFile Cache;

            public Tag GroupTag =>
                (ClassIndex == -1 || Cache.IndexItems.ClassList[ClassIndex].ClassCode == null) ?
                    Tag.Null :
                    new Tag(Cache.IndexItems.ClassList[ClassIndex].ClassCode.ToCharArray());

            public string GroupName =>
                Cache.Version < CacheVersion.Halo3Retail ?
                    GetGen2GroupName(GroupTag) :
                    Cache.Strings.GetItemByID(Cache.IndexItems.ClassList[ClassIndex].StringID);

            public Tag ParentGroupTag =>
                (ClassIndex == -1 || Cache.IndexItems.ClassList[ClassIndex].Parent == null) ?
                    Tag.Null :
                    new Tag(Cache.IndexItems.ClassList[ClassIndex].Parent.ToCharArray());

            public Tag GrandparentGroupTag =>
                (ClassIndex == -1 || Cache.IndexItems.ClassList[ClassIndex].Parent2 == null) ?
                    Tag.Null :
                    new Tag(Cache.IndexItems.ClassList[ClassIndex].Parent2.ToCharArray());

            private string GetGen2GroupName(Tag groupTag)
            {
                if (!TagDefinition.Types.ContainsKey(groupTag))
                {
                    Console.WriteLine($"WARNING: Tag definition not found for group tag '{groupTag}'");
                    return "<unknown>";
                }

                var structure = TagDefinition.Types[groupTag].GetCustomAttributes(typeof(TagStructureAttribute), false)[0] as TagStructureAttribute;

                return structure.Name;
            }

            public string Filename;
            public int ID;
            public int Offset;
            public int ClassIndex;
            public int Size;
            public int metaIndex;
            public int Magic;
            public bool External = false;

            public override string ToString()
            {
                return "[" + GroupTag + "] " + Filename;
            }
        }

        public class IndexTable : List<IndexItem>
        {
            public List<TagClass> ClassList;

            public IndexItem GetItemByID(int ID)
            {
                if (ID == -1)
                    return null;
                return this[ID & 0xFFFF];
            }

            public IndexItem this[Tag groupTag, string tagName]
            {
                get
                {
                    foreach (var blamTag in this)
                    {
                        if ((blamTag.GroupTag == groupTag.ToString()) && (blamTag.Filename == tagName))
                        {
                            return blamTag;
                        }
                    }

                    throw new KeyNotFoundException($"[{groupTag}] {tagName}");
                }
            }
        }

        public class TagClass
        {
            public string ClassCode;
            public string Parent;
            public string Parent2;
            public int StringID;

            public override string ToString()
            {
                return ClassCode;
            }
        }

        public virtual void Close()
        {
            Reader.Close();
            Reader.Dispose();
            LocaleTables.Clear();
            Strings.Clear();
            IndexItems.Clear();
            ResourceLayoutTable = null;
            ResourceGestalt = null;
            Header = null;
            IndexHeader = null;
        }

        public abstract void LoadResourceTags();

        public byte[] GetRawFromID(int ID)
        {
            return GetRawFromID(ID, -1);
        }

        public virtual byte[] GetRawFromID(int ID, int DataLength)
        {
            throw new NotImplementedException();
        }

        public virtual byte[] GetSoundRaw(int ID, int size)
        {
            throw new NotImplementedException();
        }

        public static XmlNode GetBuildNode(string build)
        {
            XmlNode retNode = null;
            using (var xml = new MemoryStream(Encoding.ASCII.GetBytes(TagTool.Properties.Resources.Builds)))
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.Load(xml);
                var element = xmlDoc.DocumentElement;

                for (int i = 0; i < element.ChildNodes.Count; i++)
                {
                    if (element.ChildNodes[i].Name.ToLower() != "build") continue;

                    if (element.ChildNodes[i].Attributes["string"].Value == build)
                    {
                        if (element.ChildNodes[i].Attributes["inherits"].Value != "")
                            return GetBuildNode(element.ChildNodes[i].Attributes["inherits"].Value);

                        retNode = element.ChildNodes[i];
                        break;
                    }
                }
            }

            if (retNode == null)
                throw new Exception("Build " + "\"" + build + "\"" + " was not found!");

            return retNode;
        }

        public static XmlNode GetVersionNode(string ver)
        {
            XmlNode retNode = null;
            using (var xml = new MemoryStream(Encoding.ASCII.GetBytes(TagTool.Properties.Resources.Versions)))
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.Load(xml);
                var element = xmlDoc.DocumentElement;

                for (int i = 0; i < element.ChildNodes.Count; i++)
                {
                    if (element.ChildNodes[i].Name.ToLower() != "version") continue;

                    if (element.ChildNodes[i].Attributes["name"].Value == ver)
                    {
                        retNode = element.ChildNodes[i];
                        break;
                    }
                }
            }

            if (retNode == null)
                throw new Exception("Version " + "\"" + ver + "\"" + " was not found!");

            return retNode;
        }

        public static XmlNode GetVertexNode(string ver)
        {
            XmlNode retNode = null;
            using (var xml = new MemoryStream(Encoding.ASCII.GetBytes(TagTool.Properties.Resources.VertexBuffer)))
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.Load(xml);
                var element = xmlDoc.DocumentElement;

                foreach (XmlNode node in element.ChildNodes)
                {
                    if (node.Attributes["Game"].Value == ver)
                    {
                        retNode = node;
                        break;
                    }
                }
            }

            return retNode;
        }

    }
}