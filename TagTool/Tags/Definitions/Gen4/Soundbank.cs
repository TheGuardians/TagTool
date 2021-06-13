using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "soundbank", Tag = "sbnk", Size = 0x20)]
    public class Soundbank : TagStructure
    {
        public SoundBankDefinitionFlags Flags;
        public SoundImportFlags ImportFlags;
        public SoundXsyncFlags XsyncFlags;
        // List of names of soundbanks. If more than one, one will be randomly chosen at load
        public List<SoundBankBlock> SoundBankList;
        // the importance of this bank over others. 1 is highest priority (ie will bump all others)
        public int BankPriority;
        public int BankUniqueId;
        
        [Flags]
        public enum SoundBankDefinitionFlags : uint
        {
            // don't use the high quality first person bank in split-screen
            DonTUseFpBankInSplitScreen = 1 << 0,
            // Bank contains deterministic sounds (voices)
            Deterministic = 1 << 1,
            // Files will not be played off HD, only DVD (for music, etc)
            StreamOffDvdOnly = 1 << 2,
            // Can delay start time for a short period waiting for bank to load
            CanDelayStart = 1 << 3,
            // Use this if there's a special player bank that's loaded by other means (ie weapon, vehicle).
            DonTLoadForPlayer = 1 << 4,
            // Don't load this bank if player is in a vehicle (ie bipeds, footsteps)
            GroundForcesSoundBank = 1 << 5
        }
        
        [Flags]
        public enum SoundImportFlags : uint
        {
            DuplicateDirectoryName = 1 << 0,
            CutToBlockSize = 1 << 1,
            UseMarkers = 1 << 2,
            UseLayerMarkers = 1 << 3
        }
        
        [Flags]
        public enum SoundXsyncFlags : uint
        {
            ProcessedLanguageTimes = 1 << 0,
            OptimizedFacialAnimation = 1 << 1
        }
        
        [TagStructure(Size = 0x4)]
        public class SoundBankBlock : TagStructure
        {
            // Name of the main sound bank.
            public StringId SoundBankName;
        }
    }
}
