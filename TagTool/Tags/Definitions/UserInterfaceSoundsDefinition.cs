using TagTool.Cache;
using TagTool.Common;
using System.Collections.Generic;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "user_interface_sounds_definition", Tag = "uise", Size = 0x140, MaxVersion = CacheVersion.Halo3Retail)]
    [TagStructure(Name = "user_interface_sounds_definition", Tag = "uise", Size = 0x14C, MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.HaloOnline700123)]
    [TagStructure(Name = "user_interface_sounds_definition", Tag = "uise", Size = 0x570, MinVersion = CacheVersion.HaloReach, MaxVersion = CacheVersion.HaloReach)]
    public class UserInterfaceSoundsDefinition : TagStructure
	{
        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public CachedTag Error;
        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public CachedTag CursorSoundVertical;
        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public CachedTag CursorSoundHorizontal;
        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public CachedTag AButton;
        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public CachedTag BButton;
        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public CachedTag XButton;
        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public CachedTag YButton;
        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public CachedTag StartButton;
        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public CachedTag BackButton;
        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public CachedTag LeftBumper;
        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public CachedTag RightBumper;
        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public CachedTag LeftTrigger;
        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public CachedTag RightTrigger;
        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public CachedTag TimerSound;
        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public CachedTag TimerSoundZero;
        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public CachedTag AltTimerSound;
        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public CachedTag AltTimerSoundZero;
        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public CachedTag MatchmakingAdvanceSound;
        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public CachedTag RankUpSound;
        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public CachedTag MatchmakingPartyUpSound;

        [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.HaloOnline700123)]
        public List<AtlasSound> AtlasSounds;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public ControllerInputEventsStruct GroupControllerInputEvents;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public MiscStruct GroupGeneral;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public ScreenStruct GroupScreen;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public TimersStruct GroupTimers;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public MiscStruct GroupMisc;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public ExitExperienceStruct GroupExitExperience;

        [TagStructure(Size = 0x14)]
        public class AtlasSound : TagStructure
		{
            public StringId Name;
            public CachedTag Sound;
        }

        [TagStructure(Size = 0x400)]
        public class ControllerInputEventsStruct : TagStructure
        {
            public CachedTag TabUp;
            public CachedTag TabLeft;
            public CachedTag TabRight;
            public CachedTag TabDown;
            public CachedTag AltStickUp;
            public CachedTag AltStickLeft;
            public CachedTag AltStickRight;
            public CachedTag AltStickDown;
            public CachedTag AltTabUp;
            public CachedTag AltTabLeft;
            public CachedTag AltTabRight;
            public CachedTag AltTabDown;
            public CachedTag AButtonPressed;
            public CachedTag BButtonPressed;
            public CachedTag XButtonPressed;
            public CachedTag YButtonPressed;
            public CachedTag LeftTriggerPressed;
            public CachedTag RightTriggerPressed;
            public CachedTag DPadUpPressed;
            public CachedTag DPadLeftPressed;
            public CachedTag DPadRightPressed;
            public CachedTag DPadDownPressed;
            public CachedTag StartButtonPressed;
            public CachedTag BackButtonPressed;
            public CachedTag LeftStickPressed;
            public CachedTag RightStickPressed;
            public CachedTag LeftBumperPressed;
            public CachedTag RightBumperPressed;
            public CachedTag LeftThumbstickPressed;
            public CachedTag RightThumbstickPressed;
            public CachedTag LeftStickPressedLeft;
            public CachedTag LeftStickPressedRight;
            public CachedTag LeftStickPressedUp;
            public CachedTag LeftStickPressedDown;
            public CachedTag RightStickPressedLeft;
            public CachedTag RightStickPressedRight;
            public CachedTag RightStickPressedUp;
            public CachedTag RightStickPressedDown;
            public CachedTag AButtonReleased;
            public CachedTag BButtonReleased;
            public CachedTag XButtonReleased;
            public CachedTag YButtonReleased;
            public CachedTag LeftTriggerReleased;
            public CachedTag RightTriggerReleased;
            public CachedTag DPadUpReleased;
            public CachedTag DPadLeftReleased;
            public CachedTag DPadRightReleased;
            public CachedTag DPadDownReleased;
            public CachedTag StartButtonReleased;
            public CachedTag BackButtonReleased;
            public CachedTag LeftStickReleased;
            public CachedTag RightStickReleased;
            public CachedTag LeftBumperReleased;
            public CachedTag RightBumperReleased;
            public CachedTag LeftThumbstickReleased;
            public CachedTag RightThumbstickReleased;
            public CachedTag LeftStickReleasedLeft;
            public CachedTag LeftStickReleasedRight;
            public CachedTag LeftStickReleasedUp;
            public CachedTag LeftStickReleasedDown;
            public CachedTag RightStickReleasedLeft;
            public CachedTag RightStickReleasedRight;
            public CachedTag RightStickReleasedUp;
            public CachedTag RightStickReleasedDown;
        }

        [TagStructure(Size = 0x10)]
        public class GeneralStruct : TagStructure
        {
            public CachedTag Error;
        }

        [TagStructure(Size = 0x20)]
        public class ScreenStruct : TagStructure
        {
            public CachedTag ScreenTransitionIn;
            public CachedTag ScreenTransitionOut;
        }

        [TagStructure(Size = 0x40)]
        public class TimersStruct : TagStructure
        {
            public CachedTag GameStartCountdownTimerTick;
            public CachedTag GameStartCountdownTimerFinalTick;
            public CachedTag AlternateCountdownTimerTick;
            public CachedTag AlternateCountdownTimerFinalTick;
        }

        [TagStructure(Size = 0x10)]
        public class MiscStruct : TagStructure
        {
            public CachedTag MatchmakingReveal;
        }

        [TagStructure(Size = 0xF0)]
        public class ExitExperienceStruct : TagStructure
        {
            public CachedTag GameCompletion;
            public CachedTag TimeSpent;
            public CachedTag WinningBonus;
            public CachedTag HopperBonus;
            public CachedTag InvasionBonus;
            public CachedTag Commendations;
            public CachedTag DailyChallenges;
            public CachedTag Achievements;
            public CachedTag SlotMachine;
            public CachedTag HeatBonus;
            public CachedTag Totals;
            public CachedTag SubrankUp;
            public CachedTag RankUp;
            public CachedTag Completed;
            public CachedTag CounterLoop;
        }
    }
}