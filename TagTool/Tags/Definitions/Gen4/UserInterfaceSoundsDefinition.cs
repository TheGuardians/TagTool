using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "user_interface_sounds_definition", Tag = "uise", Size = 0x560)]
    public class UserInterfaceSoundsDefinition : TagStructure
    {
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag TabUp;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag TabLeft;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag TabRight;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag TabDown;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag AltStickUp;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag AltStickLeft;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag AltStickRight;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag AltStickDown;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag AltTabUp;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag AltTabLeft;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag AltTabRight;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag AltTabDown;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag AButtonPressed;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag BButtonPressed;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag XButtonPressed;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag YButtonPressed;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LeftTriggerPressed;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag RightTriggerPressed;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag DPadUpPressed;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag DPadLeftPressed;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag DPadRightPressed;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag DPadDownPressed;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag StartButtonPressed;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag BackButtonPressed;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LeftStickPressed;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag RightStickPressed;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LeftBumperPressed;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag RightBumperPressed;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LeftThumbstickPressed;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag RightThumbstickPressed;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LeftStickPressedLeft;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LeftStickPressedRight;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LeftStickPressedUp;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LeftStickPressedDown;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag RightStickPressedLeft;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag RightStickPressedRight;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag RightStickPressedUp;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag RightStickPressedDown;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag AButtonReleased;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag BButtonReleased;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag XButtonReleased;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag YButtonReleased;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LeftTriggerReleased;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag RightTriggerReleased;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag DPadUpReleased;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag DPadLeftReleased;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag DPadRightReleased;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag DPadDownReleased;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag StartButtonReleased;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag BackButtonReleased;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LeftStickReleased;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag RightStickReleased;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LeftBumperReleased;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag RightBumperReleased;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LeftThumbstickReleased;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag RightThumbstickReleased;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LeftStickReleasedLeft;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LeftStickReleasedRight;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LeftStickReleasedUp;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LeftStickReleasedDown;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag RightStickReleasedLeft;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag RightStickReleasedRight;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag RightStickReleasedUp;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag RightStickReleasedDown;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag Error;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag ScreenTransitionIn;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag ScreenTransitionOut;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag GameStartCountdownTimerFirstTick;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag GameStartCountdownTimerTick;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag GameStartCountdownTimerFinalTick;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag AlternateCountdownTimerFirstTick;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag AlternateCountdownTimerTick;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag AlternateCountdownTimerFinalTick;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag MatchmakingReveal;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag GameCompletion;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag WinningBonus;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag HopperBonus;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag BoostBonus;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag FasttrackBonus;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag Totals;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag SubrankUp;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag RankUp;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag Completed;
        [TagField(ValidTags = new [] { "lsnd" })]
        public CachedTag CounterLoop;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag ScoreBonus;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag Rewards;
    }
}
