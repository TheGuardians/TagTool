using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TagTool.Cache;
using TagTool.IO;
using TagTool.Serialization;
using TagTool.Tags;
using static TagTool.Audio.FMOD;

namespace TagTool.Audio
{
    public class FMODSoundCache : IDisposable
    {
        private IntPtr System;
        private DirectoryInfo Directory;
        public List<FMODSoundBank> SoundBanks = new List<FMODSoundBank>();

        public FMODSoundCache(DirectoryInfo directory)
        {
            Directory = directory;
            InitFMOD();
            LoadSoundBanks();
        }

        private void LoadSoundBanks()
        {
            LoadSoundBank("sfx.fsb");
            LoadSoundBank("english.fsb");
        }

        private void InitFMOD()
        {
            FMOD_System_Create(out System);
            FMOD_System_Init(System, 256, FMOD_INITFLAGS.FMOD_INIT_NORMAL, IntPtr.Zero);
        }

        private void LoadSoundBank(string name)
        {
            var file = new FileInfo(Path.Combine(Directory.FullName, name));
            SoundBanks.Add(new FMODSoundBank(System, file));
        }

        public byte[] ExtractSound(uint hash)
        {
            foreach (var bank in SoundBanks)
            {
                var data = bank.ExtractSoundData(hash);
                if (data != null)
                    return data;
            }

            return null;
        }

        public void Dispose()
        {
            FMOD_System_Release(System);
        }

        public class FMODSoundBank : IDisposable
        {
            public IntPtr System;
            public FileInfo BankFile;
            public FileInfo IndexFile;
            public IntPtr MasterSound;
            private FMODSoundIndex Index;

            public FMODSoundBank(IntPtr system, FileInfo bankFile)
            {
                System = system;
                BankFile = bankFile;
                IndexFile = new FileInfo($"{bankFile.FullName}.info");
                Load();
            }

            public void Dispose()
            {
                FMOD_Sound_Release(MasterSound);
            }

            private void Load()
            {
                using (var stream = IndexFile.OpenRead())
                    Index = new FMODSoundIndex(stream);

                FMOD_RESULT result = FMOD_System_CreateStream(System, BankFile.FullName, FMOD_MODE.FMOD_OPENONLY, IntPtr.Zero, out MasterSound);
                if (result != FMOD_RESULT.FMOD_OK)
                    throw new FMODException(result, $"Failed to create sound bank stream \"{BankFile}\"");
            }

            public byte[] ExtractSoundData(uint hash)
            {
                int index = Index.FindSoundByHash(hash);
                if (index == -1)
                    return null;

                var fsbSound = Index[index];

                if (FMOD_Sound_GetSubSound(MasterSound, index, out IntPtr fmodSubSound) != FMOD_RESULT.FMOD_OK)
                    return null;

                FMOD_Sound_SeekData(fmodSubSound, (uint)(fsbSound.FirstSample / (fsbSound.ChannelCount * fsbSound.SampleSize)));

                byte[] buffer = new byte[(fsbSound.SampleCount - fsbSound.FirstSample) * (fsbSound.ChannelCount * fsbSound.SampleSize)];
                if (FMOD_Sound_ReadData(fmodSubSound, buffer, (uint)buffer.Length, out uint read) != FMOD_RESULT.FMOD_OK)
                    return null;

                return buffer;
            }
        }
    }

    [TagStructure(Size = 0x118)]
    public class FMODSoundInfo : TagStructure
    {
        public uint Hash; // matches hash in the sound permutation definition
        public uint SampleCount; // samples
        public int ChannelCount;
        public int SampleSize;
        public int FirstSample;
        public uint Unknown6; //set for looping sounds. possibly the end sample?
        [TagField(Length = 256)]
        public string Filename;

        public override string ToString()
        {
            return Filename;
        }
    }

    class FMODSoundIndex : IReadOnlyList<FMODSoundInfo>
    {
        private readonly Dictionary<uint, int> HashLookup = new Dictionary<uint, int>();
        private List<FMODSoundInfo> Sounds = new List<FMODSoundInfo>();

        public int Count => Sounds.Count;

        public FMODSoundInfo this[int index] => Sounds[index];

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
                HashLookup.Add(info.Hash, index);
                Sounds.Add(info);
                index++;
            }
        }

        public IEnumerator<FMODSoundInfo> GetEnumerator()
        {
            return Sounds.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
