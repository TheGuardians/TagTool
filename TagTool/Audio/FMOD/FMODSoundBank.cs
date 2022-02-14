using System;
using System.IO;
using static TagTool.Audio.FMOD;

namespace TagTool.Audio
{
    public class FMODSoundBank : IDisposable
    {
        public FMODSystem SystemFMOD;
        private FMODSound MasterSound;

        public FileInfo BankFile;
        public FMODSoundIndex Index;

        public FMODSoundBank(FMODSystem system, FileInfo file)
        {
            SystemFMOD = system;
            BankFile = file;
            Index = new FMODSoundIndex(new FileInfo($"{BankFile.FullName}.info"));
            InitStream();
        }

        private void InitStream()
        {
            FMOD_RESULT result = SystemFMOD.CreateStream(BankFile.FullName, FMOD_MODE.OPENONLY | FMOD_MODE.ACCURATETIME, out MasterSound);
            if (result != FMOD_RESULT.OK)
                throw new Exception($"Failed to create sound bank stream. {result}");
        }

        public void Dispose()
        {
            MasterSound?.Dispose();
        }

        public byte[] ExtractSound(int index)
        {
            lock (MasterSound)
            {
                return ExtractInternal(index);
            }
        }

        public byte[] ExtractSoundByHash(uint hash)
        {
            int soundIndex = FindSound(hash);
            if (soundIndex == -1)
                return null;

            lock (MasterSound)
            {
                return ExtractInternal(soundIndex);
            }
        }

        private byte[] ExtractInternal(int index)
        {
            FMOD_RESULT result = MasterSound.GetSubSound(index, out FMODSound subsound);
            if (result != FMOD_RESULT.OK)
                throw new Exception($"Failed to get subsound index: {index}. {result}");

            using (subsound)
            {
                result = subsound.GetLength(out uint length, FMOD_TIMEUNIT.PCMBYTES);
                if (result != FMOD_RESULT.OK)
                    throw new Exception($"Failed to get sound length. {result}");

                result = subsound.SeekData(0);
                if (result != FMOD_RESULT.OK)
                    throw new Exception($"Failed to seek sound data. {result}");

                var buffer = new byte[length];
                result = subsound.ReadData(buffer, (uint)buffer.Length, out uint read);
                if (result != FMOD_RESULT.OK)
                    throw new Exception($"Failed to read sound data. {result}");

                return buffer;
            }
        }

        public int FindSound(uint hash)
        {
            return Index.FindSoundByHash(hash);
        }
    }
}
