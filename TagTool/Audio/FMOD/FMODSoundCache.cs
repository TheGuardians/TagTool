using System.Collections.Generic;
using System.IO;
using TagTool.Tags;

namespace TagTool.Audio
{
    public class FMODSoundCache
    {
        public DirectoryInfo Directory;
        public List<FMODSoundBank> LoadedBanks;

        public FMODSoundCache(DirectoryInfo directory)
        {
            Directory = directory;
            LoadedBanks = new List<FMODSoundBank>();

            LoadBanks();
        }

        private void LoadBanks()
        {
            LoadBank("sfx.fsb");
            LoadBank("english.fsb");
        }

        private void LoadBank(string name)
        {
            var bankFile = new FileInfo(Path.Combine(Directory.FullName, name));
            LoadedBanks.Add(new FMODSoundBank(FMODTagTool.System, bankFile));
        }

        public int FindSound(uint hash, out FMODSoundBank outBank)
        {
            outBank = null;
            foreach (var bank in LoadedBanks)
            {
                int soundIndex = bank.FindSound(hash);
                if (soundIndex != -1)
                {
                    outBank = bank;
                    return soundIndex;
                }
            }
            return -1;
        }

        public FMODSoundInfo GetSoundInfo(uint hash)
        {
            int soundIndex = FindSound(hash, out FMODSoundBank bank);
            if (soundIndex == -1)
                return null;

            return bank.Index[soundIndex];
        }

        public byte[] ExtractSound(uint hash)
        {
            int soundIndex = FindSound(hash, out FMODSoundBank bank);
            if (soundIndex == -1)
                return null;

            return bank.ExtractSound(soundIndex);
        }
    }
}
