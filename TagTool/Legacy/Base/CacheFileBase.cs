using TagTool.Cache;
using TagTool.Common;
using TagTool.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace TagTool.Legacy.Base
{
    public abstract class CacheFile
    {
        public FileInfo File;
        public EndianReader Reader;

        public CacheVersion Version;
        public string Build;
        public int HeaderSize;
        public int Magic;

        public CacheHeader Header;
        public CacheIndexHeader IndexHeader;
        public IndexTable IndexItems;
        public StringTable Strings;
        public List<LocaleTable> LocaleTables;
        public StringIdResolver Resolver;

        public cache_file_resource_gestalt ResourceGestalt;
        public cache_file_resource_layout_table ResourceLayoutTable;

        public string LocalesKey;
        public string StringsKey;
        public string TagsKey;
        public string NetworkKey;
        public string StringMods;

        public XmlNode buildNode;
        public XmlNode versionNode;
        public XmlNode vertexNode;

        public CacheFile(FileInfo file, CacheVersion version)
        {
            File = file;
            Version = version;
            Build = CacheVersionDetection.GetBuildName(version);
            
            Reader = new EndianReader(file.OpenRead(), EndianFormat.BigEndian);

            #region Read XML
            buildNode = GetBuildNode(Build);
            //PluginDir = buildNode.Attributes["plugins"].Value;
            versionNode = GetVersionNode(buildNode.Attributes["version"].Value);
            HeaderSize = int.Parse(buildNode.Attributes["headerSize"].Value);
            StringMods = buildNode.Attributes["stringMods"].Value;

            TagsKey = buildNode.Attributes["tagsKey"].Value;
            StringsKey = buildNode.Attributes["stringsKey"].Value;
            LocalesKey = buildNode.Attributes["localesKey"].Value;
            NetworkKey = buildNode.Attributes["networkKey"].Value;

            vertexNode = GetVertexNode(buildNode.Attributes["vertDef"].Value);
            #endregion
        }
        
        public class CacheHeader
        {
            public int Magic;

            public int FileLength;
            public int IndexOffset;
            public int IndexStreamSize;
            public int TagBufferSize;

            public uint VirtualAddress = 0;

            public int TagDependencyGraphOffset = -1;
            public bool HasTagDependencyGraph => TagDependencyGraphOffset != -1;

            public uint TagDependencyGraphSize = 0;

            public string SourceFile;
            public string Build;
            public int CacheType;

            public int StringIdsCount;
            public int StringIdsBufferSize;
            public int StringIdIndicesOffset;
            public int StringIdsBufferOffset;

            public string Name;
            public string ScenarioPath;
            public bool NeedsShared = false;

            public int TagNamesCount;
            public int TagNamesBufferOffset;
            public int TagNamesBufferSize;
            public int TagNameIndicesOffset;

            public int VirtualBaseAddress;
            public int RawTableOffset;
            public int LocaleModifier;
            public int RawTableSize;

            protected Base.CacheFile Cache;
        }

        public class CacheIndexHeader
        {
            #region Declarations
            public int tagClassCount;
            public int tagClassIndexOffset;

            public int tagCount;

            public int tagInfoOffset;
            public int tagInfoHeaderCount;
            public int tagInfoHeaderOffset;
            public int tagInfoHeaderCount2;
            public int tagInfoHeaderOffset2;

            protected Base.CacheFile cache;
            #endregion
        }

        public class StringTable : List<string>
        {
            protected Base.CacheFile Cache;

            public StringTable(CacheFile cache)
            {
                Cache = cache;
                var reader = this.Cache.Reader;
                var cacheHeader = this.Cache.Header;

                reader.SeekTo(cacheHeader.StringIdIndicesOffset);
                int[] indices = new int[cacheHeader.StringIdsCount];
                for (int i = 0; i < cacheHeader.StringIdsCount; i++)
                {
                    indices[i] = reader.ReadInt32();
                    this.Add("");
                }

                reader.SeekTo(cacheHeader.StringIdsBufferOffset);

                EndianReader newReader = null;

                if (this.Cache.StringsKey == "" || this.Cache.StringsKey == null)
                {
                    newReader = new EndianReader(new MemoryStream(reader.ReadBytes(cacheHeader.StringIdsBufferSize)), EndianFormat.BigEndian);
                }
                else
                {
                    reader.BaseStream.Position = cacheHeader.StringIdsBufferOffset;
                    newReader = new EndianReader(reader.DecryptAesSegment(cacheHeader.StringIdsBufferSize, Cache.StringsKey));
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
                        length = cacheHeader.StringIdsBufferSize - indices[i];
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

            public LocaleTable(Base.CacheFile Cache, GameLanguage Lang)
            {
                cache = Cache;
                EndianReader reader = cache.Reader;
                CacheHeader CH = cache.Header;

                int matgOffset = -1;
                foreach (IndexItem item in cache.IndexItems)
                    if (item.ClassCode == "matg")
                    {
                        matgOffset = item.Offset;
                        break;
                    }

                if (matgOffset == -1) return;

                int localeStart = int.Parse(cache.buildNode.Attributes["localesStart"].Value);
                reader.SeekTo(matgOffset + localeStart + (int)Lang * int.Parse(cache.buildNode.Attributes["languageSize"].Value));

                int localeCount = reader.ReadInt32();
                int tableSize = reader.ReadInt32();
                int indexOffset = reader.ReadInt32() + CH.LocaleModifier;
                int tableOffset = reader.ReadInt32() + CH.LocaleModifier;

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

            public string ClassCode =>
                (ClassIndex == -1) ? "____" : Cache.IndexItems.ClassList[ClassIndex].ClassCode;

            public string ClassName =>
                Cache.Strings.GetItemByID(Cache.IndexItems.ClassList[ClassIndex].StringID);

            public string ParentClass =>
                (ClassIndex == -1) ? "____" : Cache.IndexItems.ClassList[ClassIndex].Parent;

            public string ParentClass2 =>
                (ClassIndex == -1) ? "____" : Cache.IndexItems.ClassList[ClassIndex].Parent2;

            public string Filename;
            public int ID;
            public int Offset;
            public int ClassIndex;
            public int metaIndex;
            public int Magic;

            public override string ToString()
            {
                return "[" + ClassCode + "] " + Filename;
            }
        }

        public class IndexTable : List<IndexItem>
        {
            protected Base.CacheFile cache;
            public List<TagClass> ClassList;

            public IndexItem GetItemByID(int ID)
            {
                if (ID == -1)
                    return null;
                return this[ID & 0xFFFF];
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
            buildNode = null;
            versionNode = null;
            vertexNode = null;
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
