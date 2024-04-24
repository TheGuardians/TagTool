using System.Collections.Generic;
using System.IO;
using TagTool.Tags;

namespace TagTool.Audio
{
    public class FMODSoundCache
    {
        public List<DirectoryInfo> Directories;
        public List<FMODSoundBank> LoadedBanks;

        public FMODSoundCache(List<DirectoryInfo> directories)
        {
            Directories = directories;
            LoadedBanks = new List<FMODSoundBank>();

            LoadBanks();
        }

        private void LoadBanks()
        {
            foreach(var directory in Directories)
            {
                foreach (var file in directory.EnumerateFiles())
                {
                    if (file.Extension == ".fsb")
                        LoadBank(file);
                }
            }
        }

        private void LoadBank(FileInfo file)
        {
            LoadedBanks.Add(new FMODSoundBank(FMODTagTool.System, file));
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
