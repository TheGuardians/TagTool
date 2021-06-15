using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "device_control", Tag = "ctrl", Size = 0x88)]
    public class DeviceControl : Device
    {
        public ControlTypes Type;
        public ControlTriggers TriggersWhen;
        public float CallValue; // [0,1]
        public StringId ActionString;
        public StringId SecondaryActionString;
        // A string to display when someone else is already using "interaction held"-type controls
        public StringId ActionDeniedString;
        // A string displayed when someone else is using "interaction held"-type controls and it's in secondary mode
        public StringId ActionDeniedSecondaryString;
        // A string to display if the reason for denial is because of the MP team use restriction
        public StringId MpTeamUseDeniedString;
        // A string displayed if denied because of MP team use restriction and in secondary mode
        public StringId MpTeamUseDeniedSecondaryString;
        // Displayed when someone else already using "interaction held"-type controls and that player's team also triggers the
        // mp use restriction
        public StringId ActionAndMpTeamUseDeniedString;
        // Displayed when "interaction held"-type controls in use, the MP use restriction applies, and in secondary mode
        public StringId ActionAndMpTeamUseDeniedSecondaryString;
        // How many seconds the user must hold the interaction button before the control triggers
        public float InteractionHoldTime; // seconds
        // A cui_screen to display when someone is using "interaction held"-type controls
        public StringId InteractionScreen;
        // An effect to play when a user starts holding the interaction button on this control
        [TagField(ValidTags = new [] { "snd!","sndo","effe" })]
        public CachedTag HoldStart;
        // A sound to play when someone attempts to use this control while it is in use
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag ActionDeniedSound;
        public TeamuseRestrictionEnum MpTeamUseRestriction;
        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
        public byte[] Padding;
        [TagField(ValidTags = new [] { "snd!","sndo","effe" })]
        public CachedTag On;
        [TagField(ValidTags = new [] { "snd!","sndo","effe" })]
        public CachedTag Off;
        [TagField(ValidTags = new [] { "snd!","sndo","effe" })]
        public CachedTag Deny;
        public StringId ScriptName;
        
        public enum ControlTypes : short
        {
            ToggleSwitch,
            OnButton,
            OffButton,
            CallButton,
            // touching this device plays the 'on' effect set below and refills the unit's health.
            // It also deletes itself if it runs out of charges (set in sapien)
            HealthStation
        }
        
        public enum ControlTriggers : short
        {
            TouchedByPlayer,
            Destroyed,
            InteractionHeld
        }
        
        public enum TeamuseRestrictionEnum : short
        {
            AnyTeam,
            RestrictToOwnerTeam,
            ExcludeOwnerTeam
        }
    }
}
