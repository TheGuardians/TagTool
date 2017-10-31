using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BlamCore.Cache;
using BlamCore.Common;

namespace TagTool.Analysis
{
    public class TagAnalyzer
    {
        private readonly TagCache _cache;
        private MemoryMap _tagMap;
        private readonly HashSet<Tag> _tagGroups = new HashSet<Tag>();
        private Dictionary<uint, CachedTagData.PointerFixup> _dataFixupsByWriteOffset;
        private HashSet<uint> _resourceFixupsByWriteOffset;

        public TagAnalyzer(TagCache cache)
        {
            _cache = cache;
            foreach (var group in cache.Index.NonNull().Select(t => t.Group.Tag).Distinct())
                _tagGroups.Add(group);
        }

        public TagLayoutGuess Analyze(CachedTagData data)
        {
            _tagMap = BuildTagMap(data);
            _dataFixupsByWriteOffset = data.PointerFixups.ToDictionary(f => f.WriteOffset);
            _resourceFixupsByWriteOffset = new HashSet<uint>(data.ResourcePointerOffsets);
            using (var reader = new BinaryReader(new MemoryStream(data.Data)))
            {
                reader.BaseStream.Position = data.MainStructOffset;
                return AnalyzeStructure(reader, 1);
            }
        }

        public TagLayoutGuess AnalyzeStructure(BinaryReader reader, uint count)
        {
            if (count == 0)
                throw new ArgumentException("count is 0", "count");
            var startOffset = (uint)reader.BaseStream.Position;
            if (!_tagMap.IsBoundary(startOffset))
                throw new InvalidOperationException("Cannot analyze a structure which does not start on a boundary");
            var endOffset = _tagMap.GetNextBoundary(startOffset);
            if (startOffset == endOffset)
                throw new InvalidOperationException("Structure is empty");
            var offset = startOffset;
            var elementSize = (endOffset - startOffset) / count;
            var result = new TagLayoutGuess(elementSize);
            for (var i = 0; i < count; i++)
            {
                AnalyzeStructure(reader, offset, elementSize, result);
                offset += elementSize;
                reader.BaseStream.Position = offset;
            }
            return result;
        }

        private void AnalyzeStructure(BinaryReader reader, uint baseOffset, uint size, TagLayoutGuess result)
        {
            var lookBehind = new uint[4];
            CachedTagData.PointerFixup potentialGuess = null;
            for (uint offset = 0; offset < size; offset += 4)
            {
                var val = reader.ReadUInt32();
                if (_resourceFixupsByWriteOffset.Contains(baseOffset + offset))
                {
                    // Value is a resource reference
                    result.Add(offset, new ResourceReferenceGuess());
                }
                else if (_dataFixupsByWriteOffset.TryGetValue(baseOffset + offset, out CachedTagData.PointerFixup fixup))
                {
                    // Value is a pointer
                    if (offset >= 0x4)
                    {
                        // Tag block or data reference - need another padding value to confirm it
                        potentialGuess = fixup;
                    }
                }
                else if (offset >= 0xC && lookBehind[0] == 0 && lookBehind[1] == 0 && _tagGroups.Contains(new Tag((int)lookBehind[2])))
                {
                    // Tag reference
                    if (val != 0xFFFFFFFF && val < _cache.Index.Count)
                    {
                        var referencedTag = _cache.Index[(int)val];
                        if (referencedTag != null && referencedTag.Group.Tag.Value == (int)lookBehind[2])
                            result.Add(offset - 0xC, new TagReferenceGuess());
                    }
                }
                else if (val == 0 && potentialGuess != null)
                {
                    // Found a potential padding value - check if we can confirm the potential guess's type
                    if (lookBehind[1] != 0)
                    {
                        // Tag block - seek to it and analyze it
                        reader.BaseStream.Position = potentialGuess.TargetOffset;
                        var elementLayout = AnalyzeStructure(reader, lookBehind[1]);
                        reader.BaseStream.Position = baseOffset + offset + 4;
                        result.Add(offset - 0x8, new TagBlockGuess(elementLayout, CalculateAlignment(lookBehind[0])));
                    }
                    else if (offset >= 0x10 && lookBehind[1] == 0 && lookBehind[2] == 0 && lookBehind[3] != 0)
                    {
                        // Data reference
                        result.Add(offset - 0x10, new DataReferenceGuess(CalculateAlignment(lookBehind[0])));
                    }
                    potentialGuess = null;
                }
                else
                {
                    // Tag block and data reference guesses must be followed by padding
                    potentialGuess = null;
                }
                for (var i = 3; i > 0; i--)
                    lookBehind[i] = lookBehind[i - 1];
                lookBehind[0] = val;
            }
        }

        private static uint CalculateAlignment(uint pointer)
        {
            if (pointer == 0 || pointer == 0x40000000)
                return 0;
            uint align = 1;
            while (align < 0x10 && (pointer & align) == 0)
                align <<= 1;
            return align;
        }

        /// <summary>
        /// Builds a memory map for a tag.
        /// </summary>
        /// <param name="data">The tag data to build a memory map for.</param>
        /// <returns>The built map.</returns>
        private static MemoryMap BuildTagMap(CachedTagData data)
        {
            // Create a memory map with a boundary at each fixup target
            // and at the main structure
            var result = new MemoryMap(0, (uint)data.Data.Length);
            result.AddBoundary(data.MainStructOffset);
            result.AddBoundaries(data.PointerFixups.Select(f => f.TargetOffset));
            return result;
        }
    }
}
