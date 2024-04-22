using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Commands.Common;
using System.IO;
using TagTool.IO;
using Assimp;
using System.Runtime.Remoting.Contexts;
using TagTool.Layouts;
using static TagTool.IO.ConsoleHistory;
using TagTool.Tags;
using System.Runtime.Remoting.Messaging;
using TagTool.Cache.Gen2;

namespace TagTool.Commands.Tags
{
    public class GuessTagDefCommand : Command
    {
        private GameCache Cache;
        private int CacheGeneration;

        private int TagSectionMin = 0;
        private int TagSectionMax = 0;

        public GuessTagDefCommand(GameCache cache)
            : base(true,

                "GuessTagDef",
                "Infers basic tag definition sizes from cache data for a tag type",

                "GuessTagDef [group tag]",

                "")
        {
            Cache = cache;
        }

        public override object Execute(List<string> args)
        {
            if(args.Count != 1)
                return new TagToolError(CommandError.ArgInvalid, "Tag type required!");

            if (CacheVersionDetection.IsInGen(TagTool.Cache.CacheGeneration.Third, Cache.Version))
            {
                CacheGeneration = 3;
                var cacheGen3 = (GameCacheGen3)Cache;
                var headerGen3 = (CacheFileHeaderGen3)cacheGen3.BaseMapFile.Header;
                TagSectionMin = headerGen3.SectionTable.Sections[(int)CacheFileSectionType.TagSection].Offset;
                TagSectionMax = TagSectionMin + headerGen3.SectionTable.Sections[(int)CacheFileSectionType.TagSection].Size;
            }
            else if (CacheVersionDetection.IsInGen(TagTool.Cache.CacheGeneration.Fourth, Cache.Version))
            {
                CacheGeneration = 4;
                var cacheGen4 = (GameCacheGen4)Cache;
                var headerGen4 = (CacheFileHeaderGen4)cacheGen4.BaseMapFile.Header;
                TagSectionMin = headerGen4.SectionTable.Sections[(int)CacheFileSectionType.TagSection].Offset;
                TagSectionMax = TagSectionMin + headerGen4.SectionTable.Sections[(int)CacheFileSectionType.TagSection].Size;
            }
            else if(CacheVersionDetection.IsInGen(TagTool.Cache.CacheGeneration.Second, Cache.Version))
            {
                CacheGeneration = 2;
                var cacheGen2 = (GameCacheGen2)Cache;
                TagSectionMin = (int)Cache.TagCache.TagTable.First().DefinitionOffset;
                TagSectionMax = (int)Cache.TagCache.TagTable.Last().DefinitionOffset;
            }
            else
                return new TagToolError(CommandError.OperationFailed, "Current cache generation not supported yet!");

            Tag tagType = new Tag($"{args[0]}");
            if (!Cache.TagCache.TagDefinitions.TagDefinitionExists(tagType))
                return new TagToolError(CommandError.ArgInvalid, "Tag type invalid");

            List<TagFormat> tagList = new List<TagFormat>();

            var CacheStream = Cache.OpenCacheRead();
            uint previousTagOffset = 0;
            for (var i = 0; i < Cache.TagCache.TagTable.Count() - 1; i++)
            {
                CachedTag testTag = Cache.TagCache.GetTag(i);
                CachedTag nextTag = Cache.TagCache.GetTag(i + 1);

                if (testTag == null || nextTag == null || testTag.Group == null)
                    continue;

                var tagOffset = testTag.DefinitionOffset;
                var nextOffset = nextTag.DefinitionOffset;

                if(CacheGeneration == 2)
                {
                    tagOffset &= 0x3FFFFFFF;
                    nextOffset &= 0x3FFFFFFF;
                }

                TagFormat currentTag = new TagFormat
                {
                    tagType = testTag.Group.Tag,
                    tagName = testTag.Name,
                    rawPointer = tagOffset,
                    StructureBounds = new Bounds<uint>(previousTagOffset, tagOffset)
                };

                var reader = new EndianReader(CacheStream, Cache.Endianness);
                uint tagMaxSize = nextOffset - tagOffset;

                uint tagTypeCap = 0xFFFF;
                Type currenttagType = Cache.TagCache.TagDefinitions.GetTagDefinitionType(testTag.Group.Tag);
                if(currenttagType != null)
                     tagTypeCap = TagStructure.GetTagStructureInfo(currenttagType, Cache.Version, Cache.Platform).TotalSize;

                //sometimes there are gaps between tags, don't read too much
                if (tagMaxSize > 0xFFFF)
                    tagMaxSize = tagTypeCap;

                //populate the main structure first
                ScanStructure(reader, currentTag, tagOffset, tagOffset + tagMaxSize, false);

                //calculate size of main struct of previous tag based on earlier pointer of current tag
                uint mainStructCalculated = 0;
                if (currentTag.Pointers.Count > 0)
                {
                    uint firstPointer = currentTag.Pointers.Min(p => p.rawPointer);
                    mainStructCalculated = firstPointer - previousTagOffset;
                }
                else
                    mainStructCalculated = tagOffset - previousTagOffset;
                if (tagList.Count > 0)
                {
                    tagList[tagList.Count - 1].MainStructSize = mainStructCalculated;
                }

                previousTagOffset = tagOffset;
                tagList.Add(currentTag);
            }

            List<TagFormat> targetFormats = tagList.Where(t => t.tagType == tagType).ToList();
            uint mainStructSizeFinal = targetFormats.Min(s => s.MainStructSize);
            Console.WriteLine($"[Tag:'{tagType}', Main Struct Size:'0x{mainStructSizeFinal.ToString("X")}', Count:'{targetFormats.Count}']");

            //calculate struct sizes for all structures based on spacing between pointers
            List<PointerEntry> targetPointers = new List<PointerEntry>();
            foreach (var format in targetFormats)
            {
                format.Pointers = format.Pointers.OrderBy(f => f.rawPointer).ToList();
                for (var p = 0; p < format.Pointers.Count; p++)
                {
                    if (p == format.Pointers.Count - 1)
                    {
                        format.Pointers[p].Size = format.rawPointer - format.Pointers[p].rawPointer;
                        if (!format.Pointers[p].isDataRef)
                            format.Pointers[p].Size /= format.Pointers[p].Count;
                    }
                    else
                    {
                        format.Pointers[p].Size = format.Pointers[p + 1].rawPointer - format.Pointers[p].rawPointer;
                        if (!format.Pointers[p].isDataRef)
                            format.Pointers[p].Size /= format.Pointers[p].Count;
                    }
                }
            }

            List<PointerEntry> finalPointers = new List<PointerEntry>();
            //select only main struct blocks for now, excluding anything that was found beyond the calculated main struct size
            foreach (var format in targetFormats)
                finalPointers.AddRange(format.Pointers.Where(p => p.parentRawPointer == format.rawPointer &&
                p.StructOffset <= mainStructSizeFinal));
            //order ascending so the later search always gets the smallest entry
            finalPointers = finalPointers.OrderBy(p => p.Size).ToList();

            var pointersbyOffset = finalPointers.Select(p => p.StructOffset).Distinct().ToList();
            pointersbyOffset.Sort();
            foreach (var distinctEntry in pointersbyOffset)
            {
                List<PointerEntry> matches = finalPointers.FindAll(f => f.StructOffset == distinctEntry).ToList();
                uint avgCount = (uint)matches.Average(m => m.Count);
                uint minSize = matches.Min(m => m.Size);
                if (matches[0].isDataRef)
                    Console.WriteLine($"[DataBlock: MainStructOffset:'0x{distinctEntry.ToString("X")}', Size:'0x{minSize.ToString("X")}', Found:{matches.Count}, AvgCount:{avgCount}]");
                else
                    Console.WriteLine($"[TagBlock: MainStructOffset:'0x{distinctEntry.ToString("X")}', EntrySize:'0x{minSize.ToString("X")}', Found:{matches.Count}, AvgCount:{avgCount}]");
            }

            return true;
        }

        private class TagFormat
        {
            public string tagName;
            public Tag tagType;
            public uint MainStructSize;
            public Bounds<uint> StructureBounds;
            public List<PointerEntry> Pointers = new List<PointerEntry>();
            public uint rawPointer;
        }

        private class PointerEntry
        {
            public uint rawPointer;
            public uint parentRawPointer;
            public uint StructOffset;
            public uint Count;
            public uint Size;
            public bool isDataRef = false;
        }

        private void ScanStructure(EndianReader reader, TagFormat format, uint startPoint, uint endPoint, bool scanRecursively)
        {
            reader.SeekTo(startPoint);
            var lookBehind = new Stack<uint>();
            var invalidCounts = new List<uint> { 0xCDCDCDCD, 0xFFFFFFFF, 0 };
            //read until endpoint or until it hits occupied memory
            while (reader.Position < endPoint && reader.Position < reader.Length)
            {
                uint value = reader.ReadUInt32();
                var offset = TagAddressToOffset(value);

                if (offset < format.StructureBounds.Upper && offset > format.StructureBounds.Lower &&
                    reader.Position >= startPoint + 0x8)
                {
                    PointerEntry pointer = new PointerEntry
                    {
                        StructOffset = (uint)reader.Position - startPoint,
                        rawPointer = offset,
                        parentRawPointer = format.rawPointer
                    };

                    if (!invalidCounts.Contains(lookBehind.Peek()))
                    {
                        pointer.StructOffset -= 0x8;
                        pointer.Count = lookBehind.Peek();
                        format.Pointers.Add(pointer);
                        if (scanRecursively)
                        {
                            uint currentOffset = (uint)reader.Position;
                            ScanStructure(reader, format, pointer.rawPointer, uint.MaxValue, true);
                            reader.SeekTo(currentOffset);
                        }
                    }
                    else if (reader.Position >= startPoint + 0xC)
                    {
                        pointer.StructOffset -= 0x10;
                        pointer.isDataRef = true;
                        pointer.Size = lookBehind.ElementAt(2);
                        format.Pointers.Add(pointer);
                    }
                }
                lookBehind.Push(value);

                //stop at existing pointers
                if (format.Pointers.FindIndex(p => p.rawPointer == reader.Position) != -1)
                    break;
            }
        }

        private uint TagAddressToOffset(uint address)
        {
            switch (CacheGeneration)
            {
                case 2:
                    return address & 0x3FFFFFFF;
                case 3:
                    GameCacheGen3 gen3cache = (GameCacheGen3)Cache;
                    return gen3cache.TagAddressToOffset(address);
                case 4:
                    GameCacheGen4 gen4cache = (GameCacheGen4)Cache;
                    return gen4cache.TagAddressToOffset(address);
                default:
                    return 0;
            }
        }
    }
}
