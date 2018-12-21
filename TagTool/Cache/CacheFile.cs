using System;
using System.Collections.Generic;
using System.IO;
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

        public abstract string LocalesKey {get;}
        public abstract string StringsKey {get;}
        public abstract string TagsKey {get;}
        public abstract string NetworkKey {get;}
        public abstract string StringMods { get; }

        public abstract int LocaleGlobalsOffset { get; }
        public abstract int LocaleGlobalsSize { get; }

        public CacheFile(HaloOnlineCacheContext cacheContext, FileInfo file, CacheVersion version, bool memory)
        {
            CacheContext = cacheContext;
            File = file;
            Version = version;
            Deserializer = new TagDeserializer(Version);

            Stream = memory ? new MemoryStream() : (Stream)file.OpenRead();

            if (memory)
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
                var reader = Cache.Reader;
                var cacheHeader = Cache.Header;

                reader.SeekTo(cacheHeader.StringIDsIndicesOffset);
                int[] indices = new int[cacheHeader.StringIDsCount];
                for (var i = 0; i < cacheHeader.StringIDsCount; i++)
                {
                    indices[i] = reader.ReadInt32();
                    Add("");
                }

                reader.SeekTo(cacheHeader.StringIDsBufferOffset);

                EndianReader newReader = null;

                if (Cache.StringsKey == "" || Cache.StringsKey == null)
                {
                    newReader = new EndianReader(new MemoryStream(reader.ReadBytes(cacheHeader.StringIDsBufferSize)), reader.Format);
                }
                else
                {
                    reader.BaseStream.Position = cacheHeader.StringIDsBufferOffset;
                    newReader = new EndianReader(reader.DecryptAesSegment(cacheHeader.StringIDsBufferSize, Cache.StringsKey), reader.Format);
                }

                for (var i = 0; i < indices.Length; i++)
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
                string[] mods = Cache.StringMods?.Split(';') ?? new string[] { "0,0" };
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
            public LocaleTable(CacheFile cache, GameLanguage language)
            {
                int matgOffset = -1;

                foreach (var item in cache.IndexItems)
                {
                    if (item.IsInGroup("matg"))
                    {
                        matgOffset = item.Offset;
                        break;
                    }
                }

                if (matgOffset == -1)
                    return;

                cache.Reader.SeekTo(matgOffset + cache.LocaleGlobalsOffset + ((int)language * cache.LocaleGlobalsSize));

                var localeCount = cache.Reader.ReadInt32();
                var tableSize = cache.Reader.ReadInt32();
                var indexOffset = (int)(cache.Reader.ReadInt32() + cache.Header.Interop.UnknownBaseAddress);
                var tableOffset = (int)(cache.Reader.ReadInt32() + cache.Header.Interop.UnknownBaseAddress);

                cache.Reader.SeekTo(indexOffset);
                var indices = new int[localeCount];

                for (var i = 0; i < localeCount; i++)
                {
                    Add(new LocalizedString(cache.Reader.ReadInt32(), "", i));
                    indices[i] = cache.Reader.ReadInt32();  
                }

                cache.Reader.SeekTo(tableOffset);

                EndianReader newReader = null;

                if (cache.LocalesKey == null || cache.LocalesKey == "")
                {
                    newReader = new EndianReader(new MemoryStream(cache.Reader.ReadBytes(tableSize)), EndianFormat.BigEndian);
                }
                else
                {
                    cache.Reader.BaseStream.Position = tableOffset;
                    newReader = new EndianReader(cache.Reader.DecryptAesSegment(tableSize, cache.LocalesKey));
                }

                for (var i = 0; i < indices.Length; i++)
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

            /// <summary>
            /// Determines whether the tag belongs to a tag group.
            /// </summary>
            /// <param name="groupTag">The group tag.</param>
            /// <returns><c>true</c> if the tag belongs to the group.</returns>
            public bool IsInGroup(Tag groupTag)
            {
                return GroupTag == groupTag || ParentGroupTag == groupTag || GrandparentGroupTag == groupTag;
            }

            /// <summary>
            /// Determines whether the tag belongs to a tag group.
            /// </summary>
            /// <param name="groupTag">A 4-character string representing the group tag, e.g. "scnr".</param>
            /// <returns><c>true</c> if the tag belongs to the group.</returns>
            public bool IsInGroup(string groupTag) => IsInGroup(new Tag(groupTag));

            /// <summary>
            /// Determines whether the tag belongs to a tag group.
            /// </summary>
            /// <param name="group">The tag group.</param>
            /// <returns><c>true</c> if the tag belongs to the group.</returns>
            public bool IsInGroup(TagGroup group) => IsInGroup(group.Tag);

            public bool IsInGroup<T>() => IsInGroup(typeof(T).GetGroupTag());

            private string GetGen2GroupName(Tag groupTag)
            {
                if (!Tags.TagDefinition.Types.ContainsKey(groupTag))
                {
                    Console.WriteLine($"WARNING: Tag definition not found for group tag '{groupTag}'");
                    return "<unknown>";
                }

				var type = Tags.TagDefinition.Types[groupTag];
				var structure = TagStructure.GetTagStructureAttribute(type);

                return structure.Name;
            }

            public string Name;
            public int ID;
            public int Offset;
            public int ClassIndex;
            public int Size;
            public int Index;
            public int Magic;
            public bool External = false;

            public override string ToString()
            {
                return "[" + GroupTag + "] " + Name;
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
                        if ((blamTag.GroupTag == groupTag.ToString()) && (blamTag.Name == tagName))
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

        public IndexItem GetIndexItemFromID(int id)
        {
            return IndexItems.GetItemByID(id);
        }

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
    }
}