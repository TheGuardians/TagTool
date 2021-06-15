using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "device_dispenser", Tag = "dspn", Size = 0x5C)]
    public class DeviceDispenser : Device
    {
        public DispenserDefinitionFlags Flags;
        public DispenserDefinitionTrigger TriggersWhen;
        // The number of seconds that must elapse before this dispenser is usable
        public byte UseCooldown; // seconds
        // When abandoned for this many seconds the object will be deleted
        public byte AbandonmentTime; // seconds
        // The maximum number of objects that can come from this dispenser
        public byte MaxQuota; // (between 0 and 8)
        [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
        public byte[] Padding;
        // How many seconds the user must hold the interaction button before the dispenser triggers
        public float InteractionHoldTime; // seconds
        // A cui_screen to display when someone is using "interaction held"-type dispensers
        public StringId InteractionScreen;
        // This object will be spawned by the dispenser
        [TagField(ValidTags = new [] { "bipd","vehi","weap","bloc" })]
        public CachedTag DispensedObject;
        // The model variant to use of the dispensed object
        public StringId DesiredVariantName;
        // This interaction text will display when usable
        public StringId ActionString;
        // Displayed when a player is in range but not on the right team
        public StringId SameTeamDenialString;
        // Displayed when the dispenser is disabled
        public StringId DisabledDenialString;
        // Displayed when the use cooldown time hasn't elapsed yet
        public StringId UseCooldownDenialString;
        // Displayed when this dispenser is out of charges
        public StringId MaxQuotaDenialString;
        // Displayed when someone is already interacting with this device
        public StringId InteractionInProgressDenialString;
        // The dispensed object will appear with this marker's position and orientation
        public StringId SpawnMarkerName;
        // Creates the dispense effect at this marker name
        public StringId EffectMarkerName;
        // An effect created when the the dispenser dispenses something
        [TagField(ValidTags = new [] { "effe" })]
        public CachedTag DispenseEffect;
        // The marker on the dispenser to use for attachment if we attach our dispensed object, origin if undefined
        public StringId DispenserAttachMarker;
        // The marker on the dispensed object to use for attachment if we attach our dispensed object, origin if undefined
        public StringId DispensedObjectAttachMarker;
        
        [Flags]
        public enum DispenserDefinitionFlags : byte
        {
            UsableBySameTeamOnly = 1 << 0,
            // Turn this on to allow device users to automatically enter vehicles, or automatically equip a weapon
            AutomatedFunctionality = 1 << 1,
            // The spawned object will inherit the dispenser's team
            ItemInheritsDispenserTeam = 1 << 2,
            // Spawned objects don't get abandoned, and only reset on death
            MonitorForDeathOnly = 1 << 3,
            // Used for Dominion turrets that are always supposed to be visually attached to their bases
            DeleteDispensedObjectsWhenGrabbedInForge = 1 << 4,
            // Dispenser waits till the dispense location is clear before dispensing and can push players off the dispenser pad
            PushPlayersClear = 1 << 5
        }
        
        public enum DispenserDefinitionTrigger : sbyte
        {
            Touched,
            InteractionHeld
        }
    }
}
