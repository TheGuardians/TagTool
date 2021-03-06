using System;
using System.Diagnostics;
using System.IO;
using TagTool.Audio.Converter;
using TagTool.Cache;
using TagTool.Common;
using TagTool.IO;
using TagTool.Tags.Definitions;
using Gen2Sound = TagTool.Tags.Definitions.Gen2.Sound;
using Gen2SoundCacheFileGestalt = TagTool.Tags.Definitions.Gen2.SoundCacheFileGestalt;

namespace TagTool.Audio
{
    public static class SoundConverter
    {
        private static readonly string BaseFileName = "temp";
        private static readonly string XMAFile = BaseFileName + ".xma";
        private static readonly string WAVFile = BaseFileName + ".wav";
        private static readonly string MP3File = BaseFileName + ".mp3";
        private static readonly string ADPCMFile = BaseFileName + ".adpcm";
        private static readonly string OGGFile = BaseFileName + ".ogg";

        public static string GetSoundCacheFileName(string tagName, CacheVersion version, string cacheFilePath, int pitch_range_index, int permutation_index)
        {
            var split = tagName.Split('\\');
            var endName = split[split.Length - 1]; //get the last portion of the tag name
            var newPath = cacheFilePath;

            newPath = Path.Combine(newPath, version.ToString());

            for (int i = 0; i < split.Length - 1; i++)
            {

                var folder = split[i];

                var dir = Path.Combine(newPath, folder); //combine the new path with the current folder
                if (!Directory.Exists(dir))// check if that specific folder exists and if not create it
                    Directory.CreateDirectory(dir);

                newPath = Path.Combine(newPath, folder); // update the new path varible with the current folder
            }

            var basePermutationCacheName = Path.Combine(newPath, endName); //combine the last portion of the tag name with the new path

            return $"{basePermutationCacheName}_{pitch_range_index}_{permutation_index}.wav";
        }

        public static BlamSound ConvertGen2Sound(GameCache cache, Gen2SoundCacheFileGestalt soundGestalt, Gen2Sound sound, int pitchRangeIndex, int permutationIndex, byte[] data, Compression targetFormat, bool useSoundCache, string soundCachePath, string tagName)
        {
            ClearFiles();

            var sampleRate = sound.SampleRate.GetSampleRateHz();
            var channelCount = Encoding.GetChannelCount(sound.Encoding);

            var absolutePitchRanceIndex = sound.PitchRangeIndex + pitchRangeIndex;
            var absolutePermutationIndex = soundGestalt.PitchRanges[absolutePitchRanceIndex].FirstPermutation + permutationIndex;
            var sampleCount = (uint)soundGestalt.Permutations[absolutePermutationIndex].SampleSize;
            var encoding = channelCount == 2 ? EncodingValue.Stereo : EncodingValue.Mono;

            BlamSound blamSound = new BlamSound(sound.SampleRate, encoding, sound.Compression, sampleCount, data);

            bool cachedSoundExists = false;

            if (useSoundCache)
            {
                var fileName = GetSoundCacheFileName(tagName, cache.Version, soundCachePath, pitchRangeIndex, permutationIndex);
                if (File.Exists(fileName))
                    cachedSoundExists = true;
            }

            if (!useSoundCache || !cachedSoundExists)
            {
                if (sound.Compression == Compression.IMAADPCM)
                {
                    var soundFile = new IMAADPCM(data, channelCount, sampleRate);
                    using (var fileStream = File.Create(ADPCMFile))
                    using (var writer = new EndianWriter(fileStream, EndianFormat.LittleEndian))
                    {
                        soundFile.Write(writer);
                    }

                    ConvertADPCMToWAV(ADPCMFile, WAVFile, " -ar 44100 ");
                    blamSound.SampleRate = new SampleRate { value = SampleRate.SampleRateValue._44khz };
                }
                else if (sound.Compression == Compression.XboxADPCM)
                {
                    var soundFile = new XboxADPCM(data, channelCount, sampleRate);

                    using (var fileStream = File.Create(ADPCMFile))
                    using (var writer = new EndianWriter(fileStream, EndianFormat.LittleEndian))
                    {
                        soundFile.Write(writer);
                    }

                    ConvertADPCMToWAV(ADPCMFile, WAVFile, " -ar 44100 ");
                    blamSound.SampleRate = new SampleRate { value = SampleRate.SampleRateValue._44khz };

                }
                else if (sound.Compression == Compression.PCM_BigEndian)
                {
                    throw new NotImplementedException($"PCM in big endian not supported");
                }
                else
                {
                    var soundFile = new WAVFile(data, channelCount, sampleRate);

                    using (var fileStream = File.Create(WAVFile))
                    using (var writer = new EndianWriter(fileStream, EndianFormat.LittleEndian))
                    {
                        soundFile.Write(writer);
                    }
                }

                blamSound.UpdateFormat(Compression.PCM, LoadWAVData(WAVFile, -1, false));

                DeleteFile(WAVFile);

                WriteWAVFile(blamSound, WAVFile);

                // store WAV file in cache if it does not exist.
                if (useSoundCache && !cachedSoundExists)
                {
                    var fileName = GetSoundCacheFileName(tagName, cache.Version, soundCachePath, pitchRangeIndex, permutationIndex);
                    WriteWAVFile(blamSound, fileName);
                }

            }
            else
            {
                // read and update blamSound from existing cache file.
                var fileName = GetSoundCacheFileName(tagName, cache.Version, soundCachePath, pitchRangeIndex, permutationIndex);
                ReadWAVFile(blamSound, fileName);
                // write a temporary copy to WAVFile
                WriteWAVFile(blamSound, WAVFile);
            }

            // we know blamSound is now in PCM format with proper sample count and wav data, headerless

            if (targetFormat == Compression.MP3)
            {
                ConvertToMP3(WAVFile);
                blamSound.UpdateFormat(Compression.MP3, File.ReadAllBytes(MP3File));
            }
            else if (targetFormat == Compression.PCM)
            {
                blamSound.UpdateFormat(Compression.PCM, PrepareWAVForFMOD(WAVFile));
            }
            else if (targetFormat == Compression.Tagtool_WAV)
            {
                blamSound.Data = File.ReadAllBytes(WAVFile);
            }
            else if (targetFormat == Compression.XMA)
            {
                // convert to XMA here
                blamSound.UpdateFormat(Compression.XMA, File.ReadAllBytes(XMAFile));
            }
            ClearFiles();
            return blamSound;
        }

        public static BlamSound ConvertGen3Sound(GameCache cache, SoundCacheFileGestalt soundGestalt, Sound sound, int pitchRangeIndex, int permutationIndex, byte[] data, Compression targetFormat, bool useSoundCache, string soundCachePath, string tagName)
        {
            ClearFiles();

            BlamSound blamSound = GetXMA(cache, soundGestalt, sound, pitchRangeIndex, permutationIndex, data);
            var channelCount = Encoding.GetChannelCount(blamSound.Encoding);
            var sampleRate = blamSound.SampleRate.GetSampleRateHz();
            WriteXMAFile(blamSound);

            bool cachedSoundExists = false;

            if (useSoundCache)
            {
                var fileName = GetSoundCacheFileName(tagName, cache.Version, soundCachePath, pitchRangeIndex, permutationIndex);
                if (File.Exists(fileName))
                    cachedSoundExists = true;
            }
            
            if(!useSoundCache || !cachedSoundExists)
            {
                if (channelCount > 2)
                {
                    // channelCount is 4 or 6
                    ConvertToWAV(XMAFile, false);
                    byte[] originalWAVdata = File.ReadAllBytes(WAVFile);
                    byte[] truncatedWAVdata = TruncateWAVFile(originalWAVdata, sampleRate, channelCount, 0x4E);
                    blamSound.UpdateFormat(Compression.PCM, truncatedWAVdata);
                }
                else
                {
                    ConvertToWAV(XMAFile, true);
                    blamSound.UpdateFormat(Compression.PCM, LoadWAVData(WAVFile, -1, false));
                }
                WriteWAVFile(blamSound, WAVFile);

                // store WAV file in cache if it does not exist.
                if (useSoundCache && !cachedSoundExists)
                {
                    var fileName = GetSoundCacheFileName(tagName, cache.Version, soundCachePath, pitchRangeIndex, permutationIndex);
                    WriteWAVFile(blamSound, fileName);
                }
                    
            }
            else
            {
                // read and update blamSound from existing cache file.
                var fileName = GetSoundCacheFileName(tagName, cache.Version, soundCachePath, pitchRangeIndex, permutationIndex);
                ReadWAVFile(blamSound, fileName);
                // write a temporary copy to WAVFile
                WriteWAVFile(blamSound, WAVFile);
            }

            // we know blamSound is now in PCM format with proper sample count and wav data, headerless

            if (targetFormat == Compression.MP3)
            {
                ConvertToMP3(WAVFile);
                blamSound.UpdateFormat(Compression.MP3, File.ReadAllBytes(MP3File));
            }
            else if (targetFormat == Compression.PCM)
            {
                blamSound.UpdateFormat(Compression.PCM, PrepareWAVForFMOD(WAVFile));
            }
            else if ( targetFormat == Compression.Tagtool_WAV)
            {
                blamSound.Data = File.ReadAllBytes(WAVFile);
            }
            else if (targetFormat == Compression.XMA)
            {
                blamSound.UpdateFormat(Compression.XMA, File.ReadAllBytes(XMAFile));
            }
            ClearFiles();
            return blamSound;
        }

        public static BlamSound GetXMA(GameCache cache, SoundCacheFileGestalt soundGestalt, Sound sound, int pitchRangeIndex, int permutationIndex, byte[] data)
        {
            int pitchRangeGestaltIndex = sound.SoundReference.PitchRangeIndex + pitchRangeIndex;
            int permutationGestaltIndex = soundGestalt.PitchRanges[pitchRangeGestaltIndex].FirstPermutationIndex + permutationIndex;

            var permutationSize = soundGestalt.GetPermutationSize(permutationGestaltIndex);
            var permutationOffset = soundGestalt.GetPermutationOffset(permutationGestaltIndex);
            byte[] permutationData = new byte[permutationSize];
            Array.Copy(data, permutationOffset, permutationData, 0, permutationSize);

            return new BlamSound(sound, permutationGestaltIndex, permutationData, cache.Version, soundGestalt);
        }

        private static void ConvertToWAVWithXMADec(string XMAFileName)
        {
            ProcessStartInfo info = new ProcessStartInfo(@"Tools\xmadec.exe")
            {
                Arguments = XMAFileName + " " + WAVFile,
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                UseShellExecute = false,
                RedirectStandardError = false,
                RedirectStandardOutput = false,
                RedirectStandardInput = false
            };
            Process ffmpeg = Process.Start(info);
            ffmpeg.WaitForExit();
        }

        public static void ConvertADPCMToWAV(string ADPCMFileName, string WAVFileName, string specialOption="")
        {
            ProcessStartInfo info = new ProcessStartInfo(@"Tools\ffmpeg.exe")
            {
                Arguments = "-i " + ADPCMFileName + $" {specialOption} " + WAVFileName,
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                UseShellExecute = false,
                RedirectStandardError = false,
                RedirectStandardOutput = false,
                RedirectStandardInput = false
            };
            Process ffmpeg = Process.Start(info);
            ffmpeg.WaitForExit();
        }

        private static void ConvertToWAV(string XMAFileName, bool useTowav = true)
        {
            if (useTowav)
            {
                ProcessStartInfo info = new ProcessStartInfo(@"Tools\towav.exe")
                {
                    Arguments = " " + XMAFileName,
                    CreateNoWindow = true,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    UseShellExecute = false,
                    RedirectStandardError = false,
                    RedirectStandardOutput = false,
                    RedirectStandardInput = false
                };
                Process towav = Process.Start(info);
                towav.WaitForExit();
            }
            else
            {
                ProcessStartInfo info = new ProcessStartInfo(@"Tools\ffmpeg.exe")
                {
                    Arguments = "-i " + XMAFileName + " -q:a 0 " +WAVFile,
                    CreateNoWindow = true,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    UseShellExecute = false,
                    RedirectStandardError = false,
                    RedirectStandardOutput = false,
                    RedirectStandardInput = false
                };
                Process ffmpeg = Process.Start(info);
                ffmpeg.WaitForExit();
            }
            
        }

        private static void ConvertToMP3(string WAVFileName)
        {
            ProcessStartInfo info = new ProcessStartInfo(@"Tools\ffmpeg.exe")
            {
                Arguments = "-i " + WAVFileName + " -q:a 0 " + MP3File,
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                UseShellExecute = false,
                RedirectStandardError = false,
                RedirectStandardOutput = false,
                RedirectStandardInput = false
            };
            Process ffmpeg = Process.Start(info);
            ffmpeg.WaitForExit();
        }

        private static void WriteXMAFile(BlamSound blamSound)
        {
            using (EndianWriter output = new EndianWriter(new FileStream(XMAFile, FileMode.Create, FileAccess.Write, FileShare.None), EndianFormat.BigEndian))
            {
                XMAFile XMAfile = new XMAFile(blamSound);
                XMAfile.Write(output);
            }
        }

        private static void ReadWAVFile(BlamSound sound, string file)
        {
            using (EndianReader output = new EndianReader(new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.None), EndianFormat.LittleEndian))
            {
                WAVFile WAVfile = new WAVFile(output);
                sound.UpdateFormat(Compression.PCM, WAVfile.Data.Data);
            }
        }

        private static void WriteWAVFile(BlamSound blamSound, string destFile)
        {
            using (EndianWriter output = new EndianWriter(new FileStream(destFile, FileMode.Create, FileAccess.Write, FileShare.None), EndianFormat.BigEndian))
            {
                WAVFile WAVfile = new WAVFile(blamSound);
                WAVfile.Write(output);
            }
        }

        private static void ClearFiles()
        {
            DeleteFile(XMAFile);
            DeleteFile(WAVFile);
            DeleteFile(MP3File);
            DeleteFile(OGGFile);
            DeleteFile(ADPCMFile);
        }

        private static void DeleteFile(string name)
        {
            if (File.Exists(name))
                File.Delete(name);
        }

        private static byte[] TruncateWAVFile(byte[] data, int sampleRate, int channelCount, int additionalOffset = 0)
        {
            var bytesPerSample = 2;         //16 bit PCM
            int startOffset = (0x240 * channelCount * bytesPerSample);                       // Offset from index 0 
            int endOffset = (0xBE * channelCount * bytesPerSample);                                           // Offset from index data.Length -1
            if (channelCount == 1)
                endOffset = 0;

            int size = data.Length - startOffset - endOffset - additionalOffset;
            byte[] result = new byte[size];
            Array.Copy(data, startOffset + additionalOffset, result, 0, size);
            return result;
        }

        private static long FindRiffChunk(Stream stream, string id, out long chunkSize)
        {
            chunkSize = 0;

            long offset = -1;
            byte[] header = new byte[8];

            do
            {
                if (stream.Read(header, 0, header.Length) != header.Length)
                    break;

                string currentChunkId = System.Text.Encoding.ASCII.GetString(header, 0, 4);
                uint currentChunkSize = BitConverter.ToUInt32(header, 4);

                if (currentChunkId == id)
                {
                    chunkSize = currentChunkSize;
                    offset = stream.Position;
                }

                stream.Position += currentChunkId == "RIFF" ? 4 : currentChunkSize;
            } 
            while (offset < 0);

            return offset;
        }

        private static byte[] PrepareWAVForFMOD(string name)
        {
            using (var stream = File.OpenRead(name))
            {
                long dataOffset = FindRiffChunk(stream, "data", out long dataSize);
                byte[] result = new byte[dataSize + 0x20];
                stream.Position = dataOffset;
                stream.Read(result, 0x10, (int)dataSize);
                return result;
            }
        }

        private static byte[] LoadWAVData(string name, int length, bool matchLength=true)
        {
            var fileLength = new FileInfo(name).Length - 0x2E;
            byte[] result = null;
            if (matchLength)
            {
                if (fileLength > length)
                    fileLength = length;

                result = new byte[length];
                byte[] data = File.ReadAllBytes(name);
                // length-fileLength != 0 when merging multi channel files, otherwise they are equal and fileLengtth - 0x2E is the actual length of the wav data.
                Array.Copy(data, 0x2E, result, length - fileLength, fileLength);
            }
            else
            {
                byte[] data = File.ReadAllBytes(name);
                result = new byte[fileLength];
                Array.Copy(data, 0x2E, result, 0, fileLength);
            }
            return result;
        }

    }
}
