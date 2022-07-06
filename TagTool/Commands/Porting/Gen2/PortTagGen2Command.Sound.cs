using System.Collections.Generic;
using System.IO;
using TagTool.Cache;
using TagTool.Cache.Gen2;
using TagTool.Common;
using TagTool.Tags;
using TagTool.Audio;
using TagTool.Tags.Definitions;
using Gen2Sound = TagTool.Tags.Definitions.Gen2.Sound;
using Gen2SoundCacheFileGestalt = TagTool.Tags.Definitions.Gen2.SoundCacheFileGestalt;
using System;
using System.Linq;

namespace TagTool.Commands.Porting.Gen2
{
    partial class PortTagGen2Command : Command
    {
        private Gen2SoundCacheFileGestalt SoundGestalt { get; set; } = null;
        private Gen2SoundCacheFileGestalt SharedSoundGestalt { get; set; } = null;

        private void LoadGen2SoundGestalt(GameCache cache, Stream stream)
        {
            if(SoundGestalt == null)
            {
                CachedTag blamTag = cache.TagCache.FindFirstInGroup("ugh!");
                if (blamTag != null)
                    SoundGestalt = cache.Deserialize<Gen2SoundCacheFileGestalt>(stream, blamTag);
            }
            
            if(cache.Version == CacheVersion.Halo2Vista && (cache as GameCacheGen2).VistaSharedTagCache != null && SharedSoundGestalt == null)
            {
                CachedTag blamTag = (cache as GameCacheGen2).VistaSharedTagCache.TagCache.FindFirstInGroup("ugh!");
                if (blamTag != null)
                    SharedSoundGestalt = cache.Deserialize<Gen2SoundCacheFileGestalt>(stream, blamTag);
            }

        }

        public Sound ConvertSound(CachedTagGen2 gen2Tag, Gen2Sound gen2Sound, Stream gen2Stream, string gen2Name)
        {
            Sound sound = new Sound();

            LoadGen2SoundGestalt(Gen2Cache, gen2Stream);
            Gen2SoundCacheFileGestalt ugh;

            if (Gen2Cache.Version == CacheVersion.Halo2Vista && gen2Tag.IsShared)
                ugh = SharedSoundGestalt;
            else
                ugh = SoundGestalt;

            var targetFormat = Compression.MP3;
            var originalSampleRate = gen2Sound.SampleRate.GetSampleRateHz();
            var originalCompression = gen2Sound.Compression;
            var originalChannelCount = Encoding.GetChannelCount(gen2Sound.Encoding);

            Scale scale = ugh.Scales[gen2Sound.ScaleIndex];
            Promotion promotion = gen2Sound.PromotionIndex != -1 ? ugh.Promotions[gen2Sound.PromotionIndex] : new Promotion();
            List<CustomPlayback> customPlayback = gen2Sound.CustomPlaybackIndex != -1 ? new List<CustomPlayback> { ugh.CustomPlaybacks[gen2Sound.CustomPlaybackIndex] } : new List<CustomPlayback>();

            ExtraInfo extraInfo = gen2Sound.ExtraInfoIndex != -1 ? ugh.ExtraInfos[gen2Sound.ExtraInfoIndex] : null;
            PlaybackParameter playbackParameters = (ugh.Playbacks[gen2Sound.PlaybackParamaterIndex]).DeepClone();

            //
            // convert playbackParameters to gen3 format
            //

            playbackParameters.DistanceParameters = new SoundDistanceParameters();
            if (playbackParameters.MininumDistance != 0)
            {
                playbackParameters.DistanceParameters.MinimumDistance = playbackParameters.MininumDistance;
                playbackParameters.FieldDisableFlags |= PlaybackParameter.FieldDisableFlagsValue.DistanceA;
            }

            if (playbackParameters.MaximumDistance != 0)
            {
                playbackParameters.DistanceParameters.MaximumDistance = playbackParameters.MaximumDistance;
                playbackParameters.FieldDisableFlags |= PlaybackParameter.FieldDisableFlagsValue.DistanceB;
            }

            sound.Playback = playbackParameters;

            //
            // Initialize other blocks
            //

            sound.Scale = scale;
            sound.Promotion = promotion;
            sound.CustomPlaybacks = customPlayback;

            //
            // Gen3 / HO specifics
            //

            sound.ImportType = ImportType.SingleLayer;
            sound.Unknown12 = 0;
            sound.Promotion.LongestPermutationDuration = (uint)gen2Sound.MaximumPlayTime;

            //
            // convert sound flags
            //

            if (gen2Sound.Flags.HasFlag(Gen2Sound.Gen2SoundFlags.AlwaysSpatialize))
                sound.Flags |= Sound.FlagsValue.AlwaysSpatialize;

            if (gen2Sound.Flags.HasFlag(Gen2Sound.Gen2SoundFlags.NeverObstruct))
                sound.Flags |= Sound.FlagsValue.NeverObstruct;

            if (gen2Sound.Flags.HasFlag(Gen2Sound.Gen2SoundFlags.LinkCountToOwnerUnit))
                sound.Flags |= Sound.FlagsValue.LinkCountToOwnerUnit ;

            if (gen2Sound.Flags.HasFlag(Gen2Sound.Gen2SoundFlags.PitchRangeIsLanguage))
                sound.Flags |= Sound.FlagsValue.PitchRangeIsLanguage;

            if (gen2Sound.Flags.HasFlag(Gen2Sound.Gen2SoundFlags.DonTUseSoundClassSpeakerFlag))
                sound.Flags |= Sound.FlagsValue.DontUseSoundClassSpeakerFlag;

            if (gen2Sound.Flags.HasFlag(Gen2Sound.Gen2SoundFlags.DonTUseLipsyncData))
                sound.Flags |= Sound.FlagsValue.DontUseLipsyncData;

            //
            // Convert tags and strings from ugh references (ugh is not ported)
            //

            // TODO customplayback, etc

            //
            // convert audio data
            //

            var soundDataAggregate = new byte[0].AsEnumerable();

            sound.PitchRanges = new List<PitchRange>(gen2Sound.PitchRangeCount);

            uint totalSampleCount = 0;
            int currentSoundDataOffset = 0;

            for (int pitchRangeIndex = gen2Sound.PitchRangeIndex; pitchRangeIndex < gen2Sound.PitchRangeIndex + gen2Sound.PitchRangeCount; pitchRangeIndex++)
            {
                totalSampleCount += ugh.GetSamplesPerPitchRange(pitchRangeIndex);

                //
                // Convert Blam pitch range to ElDorado format
                //

                var gen2PitchRange = ugh.PitchRanges[pitchRangeIndex];
                PitchRange pitchRange = new PitchRange();


                pitchRange.ImportName = ConvertStringId(ugh.ImportNames[gen2PitchRange.Name].Name);
                pitchRange.PitchRangeParameters = ugh.PitchRangeParameters[gen2PitchRange.Parameters];
                pitchRange.RuntimePermutationFlags = -1;
                pitchRange.RuntimeDiscardedPermutationIndex = -1;
                pitchRange.PermutationCount = (byte)ugh.GetPermutationCount(pitchRangeIndex);
                pitchRange.RuntimeLastPermutationIndex = -1;

                // Add pitch range to ED sound
                sound.PitchRanges.Add(pitchRange);
                var newPitchRangeIndex = pitchRangeIndex - gen2Sound.PitchRangeIndex;
                sound.PitchRanges[newPitchRangeIndex].Permutations = new List<Permutation>();

                //
                // Convert Blam permutations to ElDorado format
                //

                var useCache = Sounds.UseAudioCacheCommand.AudioCacheDirectory != null;
                var soundCachePath = useCache ? Sounds.UseAudioCacheCommand.AudioCacheDirectory.FullName : "";


                var permutationCount = ugh.GetPermutationCount(pitchRangeIndex);
                var relativePitchRangeIndex = pitchRangeIndex - gen2Sound.PitchRangeIndex;

                for (int i = 0; i < permutationCount; i++)
                {
                    var permutationIndex = pitchRange.FirstPermutationIndex + i;

                    var gen2Permutation = ugh.GetPermutation(permutationIndex);
                    var permutation = new Permutation();

                    permutation.ImportName = ConvertStringId(ugh.ImportNames[gen2Permutation.Name].Name);
                    permutation.SkipFraction = gen2Permutation.EncodedSkipFraction / 32767.0f;
                    permutation.Gain = gen2Permutation.EncodedGain * 127.0f;  // need proper sbyte decoding
                    permutation.PermutationChunks = new List<PermutationChunk>();
                    permutation.PermutationNumber = (uint)i;
                    permutation.IsNotFirstPermutation = (uint)(i == 0 ? 0 : 1);


                    // build sound data[]

                    var permutationSize = 0;

                    // compute total size
                    for (int k = 0; k < gen2Permutation.ChunkCount; k++)
                    {
                        var permutationChunkIndex = gen2Permutation.FirstChunk + k;
                        var chunk = ugh.Chunks[permutationChunkIndex];
                        permutationSize += chunk.GetSize();
                    }

                    byte[] permutationData = new byte[permutationSize];
                    var currentOffset = 0;

                    // move Data
                    for (int k = 0; k < gen2Permutation.ChunkCount; k++)
                    {
                        var permutationChunkIndex = gen2Permutation.FirstChunk + k;
                        var chunk = ugh.Chunks[permutationChunkIndex];
                        var chunkSize = chunk.GetSize();
                        byte[] chunkData = (Cache.ResourceCache as ResourceCacheGen2).GetResourceDataFromHandle(chunk.ResourceReference, chunkSize);
                        Array.Copy(chunkData, 0, permutationData, currentOffset, chunkSize);
                        currentOffset += chunkSize;
                    }

                    BlamSound blamSound = SoundConverter.ConvertGen2Sound(Gen2Cache, ugh, gen2Sound, relativePitchRangeIndex, i, permutationData, targetFormat, useCache, soundCachePath, gen2Name);

                    permutationData = blamSound.Data;
                    permutation.SampleCount = blamSound.SampleCount;
                    permutation.FirstSample = blamSound.FirstSample;

                    var newChunk = new PermutationChunk(currentSoundDataOffset, permutationData.Length, 0, 0);
                    permutation.PermutationChunks.Add(newChunk);
                    currentSoundDataOffset += permutationData.Length;
                    pitchRange.Permutations.Add(permutation);

                    soundDataAggregate = soundDataAggregate.Concat(permutationData);
                }
            }

            sound.Promotion.TotalSampleSize = totalSampleCount;


            //
            // create resource
            //

            var data = soundDataAggregate.ToArray();
            var resourceDefinition = AudioUtils.CreateSoundResourceDefinition(data);
            var resourceReference = Cache.ResourceCache.CreateSoundResource(resourceDefinition);
            sound.Resource = resourceReference;

            //
            // generate platform codec struct
            //

            // assume all sounds get converted to mp3, 44100 Hz
            sound.PlatformCodec = new PlatformCodec
            {
                Unknown1 = 0,
                LoadMode = 0,
                Compression = targetFormat,
                Encoding = originalChannelCount == 2 ? EncodingValue.Stereo : EncodingValue.Mono,
                SampleRate = new SampleRate { value = SampleRate.SampleRateValue._44khz },
            };

            //
            // Convert ExtraInfo block
            //

            sound.ExtraInfo = new List<ExtraInfo> { extraInfo };


            return sound;
        }
    }
}
