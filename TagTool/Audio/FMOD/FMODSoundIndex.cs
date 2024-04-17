using System.Collections;
using System.Collections.Generic;
using System.IO;
using TagTool.Cache;
using TagTool.Commands.Common;
using TagTool.IO;
using TagTool.Serialization;
using TagTool.Tags;

namespace TagTool.Audio
{
    public class FMODSoundIndex : IReadOnlyList<FMODSoundInfo>
    {
        private readonly Dictionary<uint, int> HashLookup = new Dictionary<uint, int>();
        private List<FMODSoundInfo> Sounds = new List<FMODSoundInfo>();

        public int Count => Sounds.Count;

        public FMODSoundInfo this[int index] => Sounds[index];

        public FMODSoundIndex(FileInfo file)
        {
            using (var stream = file.OpenRead())
                Load(stream);
        }

        public FMODSoundIndex(Stream stream)
        {
            Load(stream);
        }

        public int FindSoundByHash(uint hash)
        {
            if (HashLookup.TryGetValue(hash, out int index))
                return index;
            else
                return -1;
        }

        private void Load(Stream stream)
        {
            var reader = new EndianReader(stream);
            var dataContext = new DataSerializationContext(reader);
            var deserializer = new TagDeserializer(CacheVersion.Unknown, CachePlatform.MCC);

            int index = 0;
            while (!reader.EOF)
            {
                var info = deserializer.Deserialize<FMODSoundInfo>(dataContext);
                if(HashLookup.ContainsKey(info.Hash))
                {
                    new TagToolWarning($"Duplicate sound of {info.Filename} found in sound cache! Skipping...");
                    index++;
                    continue;
                }
                HashLookup.Add(info.Hash, index);
                Sounds.Add(info);
                index++;
            }
        }

        public IEnumerator<FMODSoundInfo> GetEnumerator() => Sounds.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    [TagStructure(Size = 0x118)]
    public class FMODSoundInfo : TagStructure
    {
        public uint Hash;
        public uint SampleCount;
        public int ChannelCount;
        public uint SampleSize;
        public uint LoopStart;
        public uint LoopEnd;
        [TagField(Length = 256)]
        public string Filename;

        public override string ToString() => Filename;
    }
}
