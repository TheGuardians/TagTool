using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "user_interface_hud_globals_definition", Tag = "uihg", Size = 0x160)]
    public class UserInterfaceHudGlobalsDefinition : TagStructure
    {
        public HudmotionSensorGlobalsFlags Flags;
        [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
        public byte[] Padding;
        // active Camo users show up as enemy when active camo is lower than this value (multiplayer)
        public float ActiveCamoAppearsAsEnemyWhenLowerThan;
        // relative height at which something is above (meters)
        public float HeightClassifiedAsAbove;
        // relative height at which something is below (meters)
        public float HeightClassifiedAsBelow;
        // speed at which stuff is visible on the sensor (meters/sec)
        public float ThresholdSpeed;
        // multiplier for vertical speed upwards (multiplier)
        public float UpwardsMovementScaling;
        // multiplier for vertical speed downwards (multiplier)
        public float DownwardsMovementScaling;
        // multiplier for speed when crouching (multiplier)
        public float CrouchingMovementScaling;
        // how far off the edge of the radar we can detect things (multiplier)
        public float NormalDetectionRangeExtension;
        // how far off the edge of the radar we can detect vehicles (multiplier)
        public float VehicleDetectionRangeExtension;
        // special range boost used for large flying vehicles (meters)
        public float VehicleRadarRange;
        // override for mech/mantis
        public float MechRadarRange;
        // range at which hud nav markers will show (meters)
        public float VehicleNavigationMarkerRangeDetection;
        // rate at which a contested vehicle flashes (seconds)
        public float VehicleContestedFlashTime;
        // duration to wait before showing another animation (seconds)
        public float ActivecamoXrayAnimationCooldown;
        // time for which we will see dead team mates (seconds)
        public float DeadPeopleVisibleTime;
        // The number of frames that a non moving target will fade out over on the motion tracker.
        public int MotionTrackerFadeFrames;
        // The number of seconds to show the players armor mod in the HUD (seconds).
        public int ArmorModDisplayCounter;
        public float BroadswordParallaxVelocityOverride;
        public float PelicanParallaxVelocityOverride;
        // time to display the fanfares (seconds)
        public float FanfareDisplayTime;
        // time to display when higher priority fanfare queues up (seconds)
        public float FanfareSpeedUpDisplayTime;
        // message time of the medal score message (seconds)
        public float MedalScoreMessageLifetime;
        // time to reset message to when messages collide (seconds)
        public float MedalScoreMessageRestartTime;
        // absolute range. (meters)
        public float RemoteSensorRange;
        // distance at which the remote sensor starts to fail (meters)
        public float RemoteSensorWeakDistance;
        // distance at which the remote sensor completely fails (meters)
        public float RemoteSensorFailureDistance;
        // time taken to drain an entire bar of visible damage. less damage drains faster (seconds)
        public float ShieldBarRecentDamageDuration;
        // duration which the damage indicator icons will show in the HUD (seconds)
        public float DamageIndicatorResponseDuration;
        // used to determine how long to show the fullscreen damage flash (seconds)
        public float DamageFlashResponseDuration;
        [TagField(ValidTags = new [] { "bitm" })]
        public CachedTag TiledMeshSeenWhenHitInFirstPerson;
        public float NumberOfTilesAcrossTheScreen;
        public float MeshAlphaMultiplier;
        public float MeshAlphaGradient;
        public float MeshAlphaAtCentre;
        public float MeshAlphaAtScreenEdge;
        [TagField(Length = 9)]
        public ScreenTransformBasisArrayDefinition[]  ScreenTransformBasis;
        // Maximum spread for all weapon reticules. This should be set to the largest spread angle of all the weapons.
        public float ReticuleMaximumSpreadAngle; // degrees
        [TagField(ValidTags = new [] { "scmb","sndo","lsnd","snd!" })]
        public CachedTag BannedVehicleEntranceSound;
        public HighContrastFlags HighContrastFlags1;
        [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
        public byte[] Padding1;
        // Minimum brightness value at which the dynamic contrast activates.
        public float MinimumThreshold;
        // Brightness value at which the dynamic contrast is fully activated.
        public float MaximumThreshold;
        // Clamped brightness value. Can be used to limit the intensity of the dynamic contrast, or enable over strength
        // contrast.
        public float ClampThreshold;
        // Opacity of the black layer.
        public float DarkenFactor;
        // Overbrightness factor to apply to the additive layer.
        public float BrightenFactor;
        public List<StringFileReferences> StringReferences;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag InteractMessageAppearSound;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag InteractMessageDisappearSound;
        // animation time of the medal fanfare (seconds)
        public float MedalFanfareAnimationLifetime;
        // animation in time of the medal fanfare (seconds)
        public float MedalFanfareAnimationInLifetime;
        // the time to begin the animation out sequence (seconds)
        public float MedalFanfareAnimationOutBeginTime;
        // animation time of the medal fanfare (seconds)
        public float EventFanfareAnimationLifetime;
        // animation in time of the medal fanfare (seconds)
        public float EventFanfareAnimationInLifetime;
        // the time to begin the animation out sequence (seconds)
        public float EventFanfareAnimationOutBeginTime;
        public List<PlayerTrainingEntryDataBlock> PlayerTrainingData;
        
        [Flags]
        public enum HudmotionSensorGlobalsFlags : byte
        {
            ShowScriptedPingsAtAnyDistance = 1 << 0
        }
        
        [Flags]
        public enum HighContrastFlags : byte
        {
            DisableDynamicContrast = 1 << 0,
            DisableDoubleDraw = 1 << 1
        }
        
        [TagStructure(Size = 0x8)]
        public class ScreenTransformBasisArrayDefinition : TagStructure
        {
            public RealPoint2d ScreenTransformBasisElement;
        }
        
        [TagStructure(Size = 0x10)]
        public class StringFileReferences : TagStructure
        {
            [TagField(ValidTags = new [] { "unic" })]
            public CachedTag StringList;
        }
        
        [TagStructure(Size = 0x14)]
        public class PlayerTrainingEntryDataBlock : TagStructure
        {
            // comes out of the HUD text globals
            public StringId DisplayString;
            // how long the message can be on screen before being hidden
            public ushort MaxDisplayTime;
            // how many times a training message will get displayed (0-3 only!)
            public ushort DisplayCount;
            // how long a displayed but untriggered message stays up
            public ushort DissapearDelay;
            // how long after display this message will stay hidden
            public ushort RedisplayDelay;
            // how long the event can be triggered before it's displayed
            public float DisplayDelay;
            public PlayerTrainingFlags Flags;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            
            [Flags]
            public enum PlayerTrainingFlags : ushort
            {
                NotInMultiplayer = 1 << 0
            }
        }
    }
}
