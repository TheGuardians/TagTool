using TagTool.Cache;
using TagTool.Common;
using TagTool.IO;
using TagTool.Serialization;
using TagTool.Tags.Definitions;
using TagTool.Tags.Resources;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using TagTool.Tags;
using TagTool.Audio;
using System.Threading;
using System.Security.Cryptography;
using System.Text;

namespace TagTool.Commands.Porting
{
    partial class PortTagCommand
    {
        private SoundCacheFileGestalt BlamSoundGestalt { get; set; } = null;

        private static byte[] CreateXMAHeader(int fileSize, byte channelCount, int sampleRate)
        {
            // Generates a XMA header, adapted from Adjutant

            byte[] header = new byte[60];
            using (var output = new EndianWriter(new MemoryStream(header), EndianFormat.BigEndian))
            {
                output.Write(0x52494646);                   // RIFF
                output.Format = EndianFormat.LittleEndian;
                output.Write(fileSize);
                output.Format = EndianFormat.BigEndian;
                output.Write(0x57415645);                   // WAVE

                // Generate the 'fmt ' chunk
                output.Write(0x666D7420);                   // 'fmt '
                output.Format = EndianFormat.LittleEndian;
                output.Write(0x20);
                output.Write((short)0x165);                 // WAVE_FORMAT_XMA
                output.Write((short)16);                    // 16 bits per sample
                output.Write((short)0);                     // encode options **
                output.Write((short)0);                     // largest skip
                output.Write((short)1);                     // # streams
                output.Write((byte)0);                      // loops
                output.Write((byte)3);                      // encoder version
                output.Write(0);                            // bytes per second **
                output.Write(sampleRate);                   // sample rate
                output.Write(0);                            // loop start
                output.Write(0);                            // loop end
                output.Write((byte)0);                      // subframe loop data
                output.Write(channelCount);                 // channels
                output.Write((short)0x0002);                // channel mask

                // 'data' chunk
                output.Format = EndianFormat.BigEndian;
                output.Write(0x64617461);                   // 'data'
                output.Format = EndianFormat.LittleEndian;
                output.Write((fileSize - 52));              //File offset raw

            }
            return header;
        }

        private static byte[] CreateWAVHeader(int fileSize, short channelCount, int sampleRate)
        {
            byte[] header = new byte[0x2C];
            using (var output = new EndianWriter(new MemoryStream(header), EndianFormat.BigEndian))
            {
                //RIFF header
                output.Write(0x52494646);                   // RIFF
                output.Format = EndianFormat.LittleEndian;
                output.Write(fileSize + 0x24);
                output.Format = EndianFormat.BigEndian;
                output.Write(0x57415645);                   // WAVE

                // fmt chunk
                output.Write(0x666D7420);                   // 'fmt '
                output.Format = EndianFormat.LittleEndian;
                output.Write(0x10);                         // Subchunk size (PCM)
                output.Write((short)0x1);                   // PCM Linear quantization
                output.Write(channelCount);                 // Number of channels
                output.Write(sampleRate);                   // Sample rate
                output.Write(sampleRate * channelCount * 2);    // Byte rate
                output.Write((short)(channelCount * 2));               // Block align
                output.Write((short)0x10);                  // bits per second

                // data chunk
                output.Format = EndianFormat.BigEndian;
                output.Write(0x64617461);                   // 'data'
                output.Format = EndianFormat.LittleEndian;
                output.Write(fileSize);                     // File size

            }

            return header;
        }

        public static string ConvertSoundPermutation(byte[] buffer, int index, int count, int fileSize, byte channelCount, SampleRate sampleRate, bool loop, bool use_cache, string permutation_mp3_cachename)
        {
            Directory.CreateDirectory(@"Temp");

            if (!File.Exists(@"Tools\ffmpeg.exe") || !File.Exists(@"Tools\towav.exe") || !File.Exists(@"Tools\mp3loop.exe"))
            {
                Console.WriteLine("Missing tools, please install all the required tools before porting sounds.");
                return null;
            }

            string audiofile = use_cache ? permutation_mp3_cachename : "Temp\\permutation";

            string tempXMA = $"{audiofile}.xma";
            string tempWAV = $"{audiofile}.wav";
            string fixedWAV = $"{audiofile}Truncated.wav";
            string loopMP3 = $"{audiofile}Truncated.mp3";
            string tempMP3 = $"{audiofile}.mp3";

            //If the files are still present, somehow, before the conversion happens, it will stall because ffmpeg doesn't override existing sounds.

            CLEAN_FILES:
            try
            {
                if (File.Exists(tempXMA))
                    File.Delete(tempXMA);
                if (File.Exists(tempWAV))
                    File.Delete(tempWAV);
                if (File.Exists(fixedWAV))
                    File.Delete(fixedWAV);
                if (File.Exists(loopMP3))
                    File.Delete(loopMP3);
                if (File.Exists(tempMP3))
                    File.Delete(tempMP3);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Thread.Sleep(100);
                goto CLEAN_FILES;
            }

            try
            {
                using (EndianWriter output = new EndianWriter(File.OpenWrite(tempXMA), EndianFormat.BigEndian))
                {
                    output.Write(CreateXMAHeader(fileSize, channelCount, sampleRate.GetSampleRateHz()));
                    output.Format = EndianFormat.LittleEndian;
                    output.Write(buffer, index, count);
                }

                if (channelCount == 1 || channelCount == 2)
                {
                    //Use towav as the conversion is better
                    ProcessStartInfo info = new ProcessStartInfo(@"Tools\towav.exe")
                    {
                        Arguments = tempXMA,
                        CreateNoWindow = true,
                        WindowStyle = ProcessWindowStyle.Hidden,
                        UseShellExecute = false,
                        RedirectStandardError = false,
                        RedirectStandardOutput = false,
                        RedirectStandardInput = false,
                        WorkingDirectory = Path.GetDirectoryName(tempXMA)
                    };
                    Process towav = Process.Start(info);
                    towav.WaitForExit();
                    //if(!use_cache)
                    //    File.Move(@"permutation.wav", tempWAV);

                    //towav wav header requires a modification to work with mp3loop

                    byte[] WAVstream = File.ReadAllBytes(tempWAV);
                    int removeBeginning = (int)(1152 * channelCount * ((float)sampleRate.GetSampleRateHz() / 44100));

                    //Loop will require further testing without the removed bits.

                    if (!loop)
                    {
                        var WAVFileSize = WAVstream.Length - 0x2C - removeBeginning;
                        using (EndianWriter output = new EndianWriter(File.OpenWrite(fixedWAV), EndianFormat.BigEndian))
                        {
                            output.WriteBlock(CreateWAVHeader(WAVFileSize, channelCount, sampleRate.GetSampleRateHz()));
                            output.Format = EndianFormat.LittleEndian;
                            output.WriteBlock(WAVstream, 0x2C + removeBeginning, WAVFileSize);
                        }
                    }
                    else
                    {
                        var WAVFileSize = WAVstream.Length - 0x2C;
                        using (EndianWriter output = new EndianWriter(File.OpenWrite(fixedWAV), EndianFormat.BigEndian))
                        {
                            output.WriteBlock(CreateWAVHeader(WAVFileSize, channelCount, sampleRate.GetSampleRateHz()));
                            output.Format = EndianFormat.LittleEndian;
                            output.WriteBlock(WAVstream, 0x2C, WAVFileSize);
                        }
                    }
                }
                else
                {
                    ProcessStartInfo info = new ProcessStartInfo(@"Tools\ffmpeg.exe")
                    {
                        Arguments = "-i " + tempXMA + " " + tempWAV,
                        CreateNoWindow = true,
                        WindowStyle = ProcessWindowStyle.Hidden,
                        UseShellExecute = false,
                        RedirectStandardError = false,
                        RedirectStandardOutput = false,
                        RedirectStandardInput = false
                    };
                    Process ffmpeg = Process.Start(info);
                    ffmpeg.WaitForExit();

                    int removeBeginning = (int)(1152 * channelCount * ((float)sampleRate.GetSampleRateHz() / 44100));
                    uint size = (uint)((new FileInfo(tempWAV).Length) - removeBeginning - 78);       //header size is 78 bytes.
                    byte[] WAVstream = File.ReadAllBytes(tempWAV);
                    var WAVFileSize = WAVstream.Length - 0x4E;
                    using (EndianWriter output = new EndianWriter(File.OpenWrite(fixedWAV), EndianFormat.BigEndian))
                    {
                        output.WriteBlock(CreateWAVHeader(WAVFileSize, channelCount, sampleRate.GetSampleRateHz()));
                        output.WriteBlock(WAVstream, 0x4E + removeBeginning, (int)size);
                    }
                }

                //Convert to MP3 using ffmpeg or mp3loop

                if (loop)
                {
                    if (channelCount >= 3)
                    {
                        //MP3Loop doesn't handle WAV files with more than 2 channels
                        //fixedWAV now becomes the main audio file, headerless.
                        tempMP3 = fixedWAV;
                        fixedWAV = "~";
                    }
                    else
                    {
                        ProcessStartInfo info = new ProcessStartInfo(@"Tools\mp3loop.exe")
                        {
                            Arguments = fixedWAV,
                            CreateNoWindow = true,
                            WindowStyle = ProcessWindowStyle.Hidden,
                            UseShellExecute = false,
                            RedirectStandardError = false,
                            RedirectStandardOutput = false,
                            RedirectStandardInput = false
                        };
                        Process mp3loop = Process.Start(info);
                        mp3loop.WaitForExit();
                        tempMP3 = loopMP3;
                    }
                }
                else
                {
                    ProcessStartInfo info = new ProcessStartInfo(@"Tools\ffmpeg.exe")
                    {
                        Arguments = "-i " + fixedWAV + " -q:a 0 " + tempMP3, //No imposed bitrate for now
                        CreateNoWindow = true,
                        WindowStyle = ProcessWindowStyle.Hidden,
                        UseShellExecute = false,
                        RedirectStandardError = false,
                        RedirectStandardOutput = false,
                        RedirectStandardInput = false
                    };
                    Process ffmpeg = Process.Start(info);
                    ffmpeg.WaitForExit();

                    //Remove MP3 header

                    uint size = (uint)new FileInfo(tempMP3).Length - 0x2D;
                    byte[] MP3stream = File.ReadAllBytes(tempMP3);

                    using (Stream output = new FileStream(tempMP3, FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        output.Write(MP3stream, 0x2D, (int)size);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} Exception caught. Failed to convert sound.", e);
            }
            finally
            {
                if (File.Exists(tempXMA))
                    File.Delete(tempXMA);
                if (File.Exists(tempWAV))
                    File.Delete(tempWAV);
                if (File.Exists(fixedWAV))
                    File.Delete(fixedWAV);
            }
            return tempMP3;
        }

        /// <summary>
        /// Modify gain linearly. modifier is a percentage of the ratio P_out / P_in
        /// </summary>
        private static float ModifyGain(float gain, float modifier)
        {
            // gain (dB) = 10*log(P_out / P_in) * (dB)
            double ratio = Math.Pow(10, gain / 10.0f) * (1.0 + modifier);
            return 10.0f * (float)Math.Log10(ratio);
        }

        static string ComputeSha256Hash(string rawData)
        {
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(System.Text.Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        private Sound ConvertSound(Stream cacheStream, Dictionary<ResourceLocation, Stream> resourceStreams, Sound sound, string blamTag_Name)
        {
            if (BlamSoundGestalt == null)
                BlamSoundGestalt = PortingContextFactory.LoadSoundGestalt(CacheContext, ref BlamCache);

            if (!File.Exists(@"Tools\ffmpeg.exe") || !File.Exists(@"Tools\mp3loop.exe") || !File.Exists(@"Tools\towav.exe"))
            {
                Console.WriteLine("Failed to sound conversion tools, please install ffmpeg, towav and mp3loop in the Tools folder");
                return null;
            }

            //
            // Convert Sound Tag Data
            //

            var platformCodec = BlamSoundGestalt.PlatformCodecs[sound.SoundReference.PlatformCodecIndex];
            var playbackParameters = BlamSoundGestalt.PlaybackParameters[sound.SoundReference.PlaybackParameterIndex];
            var scale = BlamSoundGestalt.Scales[sound.SoundReference.ScaleIndex];
            var promotion = sound.SoundReference.PromotionIndex != -1 ? BlamSoundGestalt.Promotions[sound.SoundReference.PromotionIndex] : new Promotion();
            var customPlayBack = sound.SoundReference.CustomPlaybackIndex != -1 ? new List<CustomPlayback> { BlamSoundGestalt.CustomPlaybacks[sound.SoundReference.CustomPlaybackIndex] } : new List<CustomPlayback>();


            sound.PlaybackParameters = playbackParameters;
            sound.Scale = scale;
            sound.PlatformCodec = platformCodec;
            sound.Promotion = promotion;
            sound.CustomPlayBacks = customPlayBack;
            //
            // Tag fixes
            //

            sound.SampleRate = platformCodec.SampleRate;
            sound.ImportType = ImportType.SingleLayer;
            //sound.PlaybackParameters.GainBase = ModifyGain(sound.PlaybackParameters.GainBase, 0.2f);
            sound.PlatformCodec.Unknown = 0;

            //
            // Process all the pitch ranges
            //

            sound.PitchRanges = new List<PitchRange>(sound.SoundReference.PitchRangeCount);

            Directory.CreateDirectory(@"Temp");
            string soundMP3 = @"Temp\soundMP3.mp3";
            if (File.Exists(soundMP3))
                File.Delete(soundMP3);


            uint LargestSampleCount = 0;
            for (int u = 0; u < sound.SoundReference.PitchRangeCount; u++)
            {
                //Need to get permlist, MaxChunkIndex,MaxIndex etc...

                int firstPermutationIndex = BlamSoundGestalt.PitchRanges[sound.SoundReference.PitchRangeIndex + u].FirstPermutationIndex;

                //Index of the permutation that contains the largest offset
                int maxIndex = 0;

                //Largest offset of a permutation
                uint maxOffset = 0;

                //Get first samplesize

                if (firstPermutationIndex < 0 || firstPermutationIndex >= BlamSoundGestalt.Permutations.Count)
                    return null;
                uint SumSamples = BlamSoundGestalt.Permutations[firstPermutationIndex].SampleSize;

                //Number of permutation
                int permutationCount = (BlamSoundGestalt.PitchRanges[sound.SoundReference.PitchRangeIndex + u].EncodedPermutationCount >> 4) & 63;

                //Next permutation, if it exists.
                int permutationIndex = firstPermutationIndex + 1;

                int i = 0;

                for (i = 0; i < permutationCount; i++)
                {
                    if (maxOffset <= (BlamSoundGestalt.PermutationChunks[BlamSoundGestalt.Permutations[firstPermutationIndex + i].FirstPermutationChunkIndex].Offset))
                    {
                        maxOffset = (BlamSoundGestalt.PermutationChunks[BlamSoundGestalt.Permutations[firstPermutationIndex + i].FirstPermutationChunkIndex].Offset);
                        maxIndex = firstPermutationIndex + i;
                    }

                    // Add the next samplesize to the total
                    SumSamples = SumSamples + BlamSoundGestalt.Permutations[firstPermutationIndex + i].SampleSize;

                    //Find largest sample count
                    if (LargestSampleCount < BlamSoundGestalt.Permutations[firstPermutationIndex + i].SampleSize)
                        LargestSampleCount = BlamSoundGestalt.Permutations[firstPermutationIndex + i].SampleSize;
                }

                //create an array that contains the ordering of the permutation, sorted by appearance in the ugh!.
                int[] permutationList = new int[permutationCount];

                for (i = 0; i < permutationCount; i++)
                {
                    permutationList[i] = BlamSoundGestalt.Permutations[firstPermutationIndex + i].OverallPermutationIndex;
                }

                sound.Promotion.TotalSampleSize = SumSamples;

                //
                // Convert Blam pitch range to ElDorado format
                //

                var pitchRange = BlamSoundGestalt.PitchRanges[sound.SoundReference.PitchRangeIndex + u];
                pitchRange.ImportName = (StringId)ConvertData(cacheStream, resourceStreams, BlamSoundGestalt.ImportNames[pitchRange.ImportNameIndex].Name, null, null);
                pitchRange.PitchRangeParameters = BlamSoundGestalt.PitchRangeParameters[pitchRange.PitchRangeParametersIndex];
                pitchRange.Unknown1 = 0;
                pitchRange.Unknown2 = 0;
                pitchRange.Unknown3 = 0;
                pitchRange.Unknown4 = 0;
                pitchRange.Unknown5 = -1;
                pitchRange.Unknown6 = -1;
                //I suspect this unknown7 to be a flag to tell if there is a Unknownblock in extrainfo. (See a sound in udlg for example)
                pitchRange.Unknown7 = 0;
                pitchRange.PermutationCount = (byte)permutationCount;
                pitchRange.Unknown8 = -1;
                sound.PitchRanges.Add(pitchRange);
                sound.PitchRanges[u].Permutations = new List<Permutation>();

                //
                // Determine the audio channel count
                //

                var channelCount = sound.PlatformCodec.Encoding.GetChannelCount();

                //
                // Set compression format
                //

                if (((ushort)sound.Flags & (ushort)Sound.FlagsValue.FitToAdpcmBlockSize) != 0 && channelCount >= 3)
                    sound.PlatformCodec.Compression = Compression.PCM;
                else
                    sound.PlatformCodec.Compression = Compression.MP3;

                //
                // Convert Blam resource data to ElDorado resource data
                //

                int chunkIndex = BlamSoundGestalt.Permutations[maxIndex].FirstPermutationChunkIndex + BlamSoundGestalt.Permutations[maxIndex].PermutationChunkCount - 1;
                int xmaFileSize = (int)(BlamSoundGestalt.PermutationChunks[chunkIndex].Offset + BlamSoundGestalt.PermutationChunks[chunkIndex].Size + 65536 * BlamSoundGestalt.PermutationChunks[chunkIndex].Unknown1);

                //No audio data present

                if (xmaFileSize <= 0)
                    return null;

                var resourceData = BlamCache.GetSoundRaw(sound.SoundReference.ZoneAssetHandle, xmaFileSize);

                if (resourceData == null)
                    return null;

                //
                // Convert Blam permutations to ElDorado format
                //

                permutationIndex = firstPermutationIndex;
                bool use_cache = Sounds.UseAudioCacheCommand.AudioCacheDirectory != null;
                var base_permutation_mp3_cachename = blamTag_Name != null ? Path.Combine(Sounds.UseAudioCacheCommand.AudioCacheDirectory.FullName, ComputeSha256Hash(blamTag_Name))  : null;

                for (i = 0; i < permutationCount; i++)
                {
                    // For the permutation conversion to work properly, we must go through the permutation in chunk order.
                    var permutation = BlamSoundGestalt.Permutations[pitchRange.FirstPermutationIndex + i];

                    permutation.ImportName = (StringId)ConvertData(cacheStream, resourceStreams, BlamSoundGestalt.ImportNames[permutation.ImportNameIndex].Name, null, null);
                    permutation.SkipFraction = new Bounds<float>(0.0f, permutation.Gain);
                    permutation.PermutationChunks = new List<PermutationChunk>();
                    permutation.PermutationNumber = (uint)permutationList[i];
                    permutation.IsNotFirstPermutation = (uint)(permutation.PermutationNumber == 0 ? 0 : 1);

                    //
                    // Get size and append MP3
                    //

                    chunkIndex = BlamSoundGestalt.Permutations[permutationIndex].FirstPermutationChunkIndex;
                    int chunkCount = BlamSoundGestalt.Permutations[permutationIndex].PermutationChunkCount;

                    int begin = (int)BlamSoundGestalt.PermutationChunks[chunkIndex].Offset;
                    int count = (int)(BlamSoundGestalt.PermutationChunks[chunkIndex + chunkCount - 1].Offset + BlamSoundGestalt.PermutationChunks[chunkIndex + chunkCount - 1].Size + 65536 * BlamSoundGestalt.PermutationChunks[chunkIndex + chunkCount - 1].Unknown1) - begin;

                    //
                    // Convert XMA permutation to MP3 headerless
                    //

                    var loop = false;
                    if (((ushort)sound.Flags & (ushort)Sound.FlagsValue.FitToAdpcmBlockSize) != 0)
                    {
                        loop = true;
                    }

                    string permutationMP3 = use_cache ? $"{base_permutation_mp3_cachename}_{i}" : null;
                    bool exists = !File.Exists($"{permutationMP3}.mp3");
                    if ((permutationMP3 != null && exists) || !use_cache)
                    {
                        permutationMP3 = ConvertSoundPermutation(resourceData, begin, count, count + 52, (byte)channelCount, sound.SampleRate, loop, use_cache, permutationMP3);
                    }
                    else
                    {
                        permutationMP3 += ".mp3";
                    }

                    uint permutationChunkSize = 0;

                    //
                    // Copy the permutation mp3 to the overall mp3
                    //

                    byte[] permBuffer = File.ReadAllBytes(permutationMP3);
                    try
                    {
                        using (Stream output = new FileStream(soundMP3, FileMode.Append, FileAccess.Write, FileShare.None))
                        {
                            output.Write(permBuffer, 0, permBuffer.Count());
                            permutationChunkSize = (uint)permBuffer.Count();
                            output.Dispose();
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("{0} Exception caught. Failed to write mp3 to file", e);
                    }

                    if(!use_cache)
                    {
                        if (File.Exists(permutationMP3))
                            File.Delete(permutationMP3);
                    }

                    var chunkSize = (ushort)(permutationChunkSize & ushort.MaxValue);

                    var permutationChunk = new PermutationChunk
                    {
                        Offset = (uint)new FileInfo(soundMP3).Length - permutationChunkSize,
                        Size = chunkSize,
                        Unknown2 = (byte)((permutationChunkSize - chunkSize) / 65536),
                        Unknown3 = 4,
                        RuntimeIndex = -1,
                        UnknownA = 0,
                        UnknownSize = 0
                    };

                    permutation.PermutationChunks.Add(permutationChunk);

                    pitchRange.Permutations.Add(permutation);

                    permutationIndex++;
                }
            }

            sound.Promotion.LongestPermutationDuration = (uint)(1000 * ((float)LargestSampleCount) / sound.SampleRate.GetSampleRateHz());

            //
            // Convert Blam extra info to ElDorado format
            //

            var extraInfo = new ExtraInfo()
            {
                LanguagePermutations = new List<ExtraInfo.LanguagePermutation>(),
                EncodedPermutationSections = new List<ExtraInfo.EncodedPermutationSection>()
            };

            for (int u = 0; u < sound.SoundReference.PitchRangeCount; u++)
            {
                var pitchRange = BlamSoundGestalt.PitchRanges[sound.SoundReference.PitchRangeIndex + u];

                var languagePermutation = new ExtraInfo.LanguagePermutation
                {
                    RawInfo = new List<ExtraInfo.LanguagePermutation.RawInfoBlock>()
                };

                for (int i = 0; i < sound.PitchRanges[u].PermutationCount; i++)
                {
                    var rawInfo = new ExtraInfo.LanguagePermutation.RawInfoBlock
                    {
                        SkipFractionName = StringId.Invalid,
                        Unknown24 = 480,
                        UnknownList = new List<ExtraInfo.LanguagePermutation.RawInfoBlock.Unknown>(),
                        Compression = 8,
                        SampleCount = (uint)Math.Floor(pitchRange.Permutations[i].SampleSize * 128000.0 / (8 * sound.SampleRate.GetSampleRateHz())),
                        ResourceSampleSize = pitchRange.Permutations[i].SampleSize,
                        ResourceSampleOffset = pitchRange.Permutations[i].PermutationChunks[0].Offset
                    };

                    languagePermutation.RawInfo.Add(rawInfo);
                }

                extraInfo.LanguagePermutations.Add(languagePermutation);
            }

            if (sound.SoundReference.ExtraInfoIndex != -1)
            {
                foreach (var section in BlamSoundGestalt.ExtraInfo[sound.SoundReference.ExtraInfoIndex].EncodedPermutationSections)
                {
                    var newSection = section.DeepClone();

                    foreach (var info in newSection.SoundDialogueInfo)
                    {
                        for (var i = ((info.MouthDataLength % 2) == 0 ? 0 : 1); (i + 1) < info.MouthDataLength; i += 2)
                            Array.Reverse(newSection.EncodedData, (int)(info.MouthDataOffset + i), 2);

                        for (var i = ((info.LipsyncDataLength % 2) == 0 ? 0 : 1); (i + 1) < info.LipsyncDataLength; i += 2)
                            Array.Reverse(newSection.EncodedData, (int)(info.LipsyncDataOffset + i), 2);
                    }

                    extraInfo.EncodedPermutationSections.Add(newSection);
                }
            }

            sound.ExtraInfo = new List<ExtraInfo> { extraInfo };

            //
            // Convert Blam languages to ElDorado format
            //

            if (sound.SoundReference.LanguageIndex != -1)
            {
                sound.Languages = new List<LanguageBlock>();

                foreach (var language in BlamSoundGestalt.Languages)
                {
                    sound.Languages.Add(new LanguageBlock
                    {
                        Language = language.Language,
                        PermutationDurations = new List<LanguageBlock.PermutationDurationBlock>(),
                        PitchRangeDurations = new List<LanguageBlock.PitchRangeDurationBlock>(),
                    });

                    //Add all the  Pitch Range Duration block (pitch range count dependent)

                    var curLanguage = sound.Languages.Last();

                    for (int i = 0; i < sound.SoundReference.PitchRangeCount; i++)
                    {
                        curLanguage.PitchRangeDurations.Add(language.PitchRangeDurations[sound.SoundReference.LanguageIndex + i]);
                    }

                    //Add all the Permutation Duration Block and adjust offsets

                    for (int i = 0; i < curLanguage.PitchRangeDurations.Count; i++)
                    {
                        var curPRD = curLanguage.PitchRangeDurations[i];

                        //Get current block count for new index
                        short newPermutationIndex = (short)curLanguage.PermutationDurations.Count();

                        for (int j = curPRD.PermutationStartIndex; j < curPRD.PermutationStartIndex + curPRD.PermutationCount; j++)
                        {
                            curLanguage.PermutationDurations.Add(language.PermutationDurations[j]);
                        }

                        //apply new index
                        curPRD.PermutationStartIndex = newPermutationIndex;
                    }

                }
            }

            //
            // Prepare resource
            //

            sound.Unused = new byte[] { 0, 0, 0, 0 };
            sound.Unknown12 = 0;

            sound.Resource = new PageableResource
            {
                Page = new RawPage
                {
                    Index = -1,
                },
                Resource = new TagResource
                {
                    Type = TagResourceType.Sound,
                    DefinitionData = new byte[20],
                    DefinitionAddress = new CacheAddress(CacheAddressType.Definition, 536870912),
                    ResourceFixups = new List<TagResource.ResourceFixup>(),
                    ResourceDefinitionFixups = new List<TagResource.ResourceDefinitionFixup>(),
                    Unknown2 = 1
                }
            };

            using (var dataStream = File.OpenRead(soundMP3))
            {
                var resourceContext = new ResourceSerializationContext(sound.Resource);
                CacheContext.Serializer.Serialize(resourceContext,
                    new SoundResourceDefinition
                    {
                        Data = new TagData(soundMP3.Length, new CacheAddress(CacheAddressType.Resource, 0))
                    });

                var definitionFixup = new TagResource.ResourceFixup()
                {
                    BlockOffset = 12,
                    Address = new CacheAddress(CacheAddressType.Resource, 1073741824)
                };
                sound.Resource.Resource.ResourceFixups.Add(definitionFixup);

                sound.Resource.ChangeLocation(ResourceLocation.Audio);
                var resource = sound.Resource;

                if (resource == null)
                    throw new ArgumentNullException("resource");

                if (!dataStream.CanRead)
                    throw new ArgumentException("The input stream is not open for reading", "dataStream");

                var cache = CacheContext.GetResourceCache(ResourceLocation.Audio);

                if (!resourceStreams.ContainsKey(ResourceLocation.Audio))
                {
                    resourceStreams[ResourceLocation.Audio] = Flags.HasFlag(PortingFlags.Memory) ?
                        new MemoryStream() :
                        (Stream)CacheContext.OpenResourceCacheReadWrite(ResourceLocation.Audio);

                    if (Flags.HasFlag(PortingFlags.Memory))
                        using (var resourceStream = CacheContext.OpenResourceCacheRead(ResourceLocation.Audio))
                            resourceStream.CopyTo(resourceStreams[ResourceLocation.Audio]);
                }

                var dataSize = (int)(dataStream.Length - dataStream.Position);
                var data = new byte[dataSize];
                dataStream.Read(data, 0, dataSize);

                resource.Page.Index = cache.Add(resourceStreams[ResourceLocation.Audio], data, out uint compressedSize);
                resource.Page.CompressedBlockSize = compressedSize;
                resource.Page.UncompressedBlockSize = (uint)dataSize;
                resource.DisableChecksum();

                for (int i = 0; i < 4; i++)
                {
                    sound.Resource.Resource.DefinitionData[i] = (byte)(sound.Resource.Page.UncompressedBlockSize >> (i * 8));
                }
            }

            if (File.Exists(soundMP3))
                File.Delete(soundMP3);

            return sound;
        }

        private SoundLooping ConvertSoundLooping(SoundLooping soundLooping)
        {
            if (BlamSoundGestalt == null)
                BlamSoundGestalt = PortingContextFactory.LoadSoundGestalt(CacheContext, ref BlamCache);

            soundLooping.Unused = null;

            soundLooping.SoundClass = ((int)soundLooping.SoundClass < 50) ? soundLooping.SoundClass : (soundLooping.SoundClass + 1);

            if (soundLooping.SoundClass == SoundLooping.SoundClassValue.FirstPersonInside)
                soundLooping.SoundClass = SoundLooping.SoundClassValue.InsideSurroundTail;

            if (soundLooping.SoundClass == SoundLooping.SoundClassValue.FirstPersonOutside)
                soundLooping.SoundClass = SoundLooping.SoundClassValue.OutsideSurroundTail;

            /* unsuccessful hacks of death and suffering
            foreach (var track in soundLooping.Tracks)
            {
                track.FadeInDuration *= 2f;
                track.Unknown1 *= 2f;
                track.FadeOutDuration *= 2f;
                track.AlternateCrossfadeDuration *= 2f;
                track.Unknown5 *= 2;
                track.AlternateFadeOutDuration *= 2f;
                track.Unknown6 *= 2f;
            }

            foreach (var detailSound in soundLooping.DetailSounds)
                detailSound.RandomPeriodBounds = new Bounds<float>(
                    detailSound.RandomPeriodBounds.Lower * 2f,
                    detailSound.RandomPeriodBounds.Upper * 2f);*/

            return soundLooping;
        }

        private Dialogue ConvertDialogue(Stream cacheStream, Dialogue dialogue)
        {
            if (BlamSoundGestalt == null)
                BlamSoundGestalt = PortingContextFactory.LoadSoundGestalt(CacheContext, ref BlamCache);

            CachedTagInstance edAdlg = null;
            AiDialogueGlobals adlg = null;
            foreach (var tag in CacheContext.TagCache.Index.FindAllInGroup("adlg"))
            {
                edAdlg = tag;
            }
            var context = new TagSerializationContext(cacheStream, CacheContext, edAdlg);
            adlg = CacheContext.Deserializer.Deserialize<AiDialogueGlobals>(context);

            //Create empty udlg vocalization block and fill it with empty blocks matching adlg

            List<Dialogue.Vocalization> newVocalization = new List<Dialogue.Vocalization>();
            foreach (var vocalization in adlg.Vocalizations)
            {
                Dialogue.Vocalization block = new Dialogue.Vocalization
                {
                    Sound = null,
                    Flags = 0,
                    Unknown = 0,
                    Name = vocalization.Name,
                };
                newVocalization.Add(block);
            }

            //Match the tags with the proper stringId

            for (int i = 0; i < 304; i++)
            {
                var vocalization = newVocalization[i];
                for (int j = 0; j < dialogue.Vocalizations.Count; j++)
                {
                    var vocalizationH3 = dialogue.Vocalizations[j];
                    if (CacheContext.StringIdCache.GetString(vocalization.Name).Equals(CacheContext.StringIdCache.GetString(vocalizationH3.Name)))
                    {
                        vocalization.Flags = vocalizationH3.Flags;
                        vocalization.Unknown = vocalizationH3.Unknown;
                        vocalization.Sound = vocalizationH3.Sound;
                        break;
                    }
                }
            }

            dialogue.Vocalizations = newVocalization;

            return dialogue;
        }

        private SoundMix ConvertSoundMix(SoundMix soundMix)
        {
            if (BlamCache.Version == CacheVersion.Halo3Retail)
                soundMix.Unknown1 = 0;

            return soundMix;
        }
    }
}