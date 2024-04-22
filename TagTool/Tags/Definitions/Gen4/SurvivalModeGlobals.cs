using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "survival_mode_globals", Tag = "smdt", Size = 0x68)]
    public class SurvivalModeGlobals : TagStructure
    {
        // NO
        public float RespawnTime; // Use game_engine_settings for this
        [TagField(ValidTags = new [] { "unic" })]
        public CachedTag SurvivalModeText;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag CountdownSound;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag RespawnSound;
        [TagField(ValidTags = new [] { "coop" })]
        public CachedTag CoOpSpawningGlobals;
        public List<SurvivalModeWaveTemplatesStruct> WaveTemplates;
        public List<GameEngineStatusResponseBlock> StateResponses;
        public List<MultiplayerColorBlock> TeamColors;
        
        [TagStructure(Size = 0x14)]
        public class SurvivalModeWaveTemplatesStruct : TagStructure
        {
            public StringId Name;
            [TagField(ValidTags = new [] { "wave" })]
            public CachedTag WaveTemplate;
        }
        
        [TagStructure(Size = 0x24)]
        public class GameEngineStatusResponseBlock : TagStructure
        {
            public GameEngineStatusFlags Flags;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public GameEngineStatusEnum State;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding1;
            public StringId FfaMessage;
            public StringId TeamMessage;
            public CachedTag Unused;
            [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
            public byte[] Padding2;
            
            [Flags]
            public enum GameEngineStatusFlags : ushort
            {
                Unused = 1 << 0
            }
            
            public enum GameEngineStatusEnum : short
            {
                WaitingForSpaceToClear,
                Observing,
                RespawningSoon,
                SittingOut,
                OutOfLives,
                Playing,
                Playing1,
                Playing2,
                GameOver,
                GameOver1,
                GameOver2,
                GameOver3,
                YouHaveFlag,
                EnemyHasFlag,
                FlagNotHome,
                CarryingOddball,
                YouAreJuggy,
                YouControlHill,
                SwitchingSidesSoon,
                PlayerRecentlyStarted,
                YouHaveBomb,
                FlagContested,
                BombContested,
                LimitedLivesLeft,
                LimitedLivesLeft1,
                LimitedLivesLeft2,
                Playing3,
                Playing4,
                Playing5,
                WaitingToSpawn,
                WaitingForGameStart,
                Blank
            }
        }
        
        [TagStructure(Size = 0xC)]
        public class MultiplayerColorBlock : TagStructure
        {
            public RealRgbColor Color;
        }
    }
}
