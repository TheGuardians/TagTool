using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TagTool.Cache;
using TagTool.Commands.Common;
using TagTool.IO;
using TagTool.Serialization;
using TagTool.Tags;
using static TagTool.Audio.FMOD;

namespace TagTool.Audio
{
    public class FMODSoundCache
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

        public class FMODSoundBank
        {
            public IntPtr System;
            public FileInfo BankFile;
            public FileInfo IndexFile;
            public IntPtr MasterSound;
            private FMODSoundIndex Index;
            private object Mutex = new object();

            public FMODSoundBank(IntPtr system, FileInfo bankFile)
            {
                System = system;
                BankFile = bankFile;
                IndexFile = new FileInfo($"{bankFile.FullName}.info");
                Load();
            }

            private void Load()
            {
                using (var stream = IndexFile.OpenRead())
                {
                    Index = new FMODSoundIndex(stream);
                }

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

                lock (Mutex)
                {
                    FMOD_RESULT result = FMOD_Sound_GetSubSound(MasterSound, index, out IntPtr fmodSubSound);
                    if (result != FMOD_RESULT.FMOD_OK)
                    {
                        new TagToolError(CommandError.CustomError, $"FMOD_Sound_GetSubSound() failed. {result}");
                        return null;
                    }

                    result = FMOD_Sound_SeekData(fmodSubSound, (uint)(fsbSound.FirstSample / (fsbSound.ChannelCount * fsbSound.SampleSize)));
                    result = FMOD_Sound_GetFormat(fmodSubSound, out var type, out var format, out int channels, out int bits);
                    if (result != FMOD_RESULT.FMOD_OK)
                    {
                        new TagToolError(CommandError.CustomError, $"FMOD_Sound_GetFormat() failed. {result}");
                        return null;
                    }

                    byte[] buffer = new byte[(fsbSound.SampleCount - fsbSound.FirstSample) / (fsbSound.SampleSize * fsbSound.ChannelCount) * channels * bits / 8];
                    result = FMOD_Sound_ReadData(fmodSubSound, buffer, (uint)buffer.Length, out uint read);
                    if (result != FMOD_RESULT.FMOD_OK)
                    {
                        new TagToolError(CommandError.CustomError, $"FMOD_Sound_ReadData() failed. {result}");
                        return null;
                    }

                    return buffer;
                }
            }
        }
    }

    [TagStructure(Size = 0x118)]
    public class FMODSoundInfo : TagStructure
    {
        public uint Hash;
        public uint SampleCount;
        public int ChannelCount;
        public int SampleSize;
        public int FirstSample;
        public uint Unknown6; //set for looping sounds. needs to be looked into
        [TagField(Length = 256)]
        public string Filename;

        public override string ToString() => Filename;
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

        public IEnumerator<FMODSoundInfo> GetEnumerator() => Sounds.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
