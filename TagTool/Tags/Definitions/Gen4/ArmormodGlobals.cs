using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "armormod_globals", Tag = "armg", Size = 0x50)]
    public class ArmormodGlobals : TagStructure
    {
        [TagField(ValidTags = new [] { "proj" })]
        // spawned by Explode On Death armormod
        public CachedTag Projectile;
        [TagField(ValidTags = new [] { "lsnd" })]
        public CachedTag LoopingSoundEffect;
        // beyond this distance volume is attenuated, far audio settings are applied
        public float NearThreshold; // world units
        // how much to attenuate volume
        public float AttenuationPct;
        public List<AuralEnhancementAudioSettingsBlock> NearAudioSettings;
        public List<AuralEnhancementAudioSettingsBlock> FarAudioSettings;
        public float Range; // world units
        public StealthflagsDefs Flags;
        [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
        public byte[] Padding;
        // how often stealthed unit will ping
        public float StealthPingFrequency; // seconds
        // length of ping
        public float StealthPingDuration; // seconds
        
        [Flags]
        public enum StealthflagsDefs : byte
        {
            IsInvisibleToXRay = 1 << 0,
            // if enabled player will not ping on the radar gutter (circumference of radar) when on foot
            PingsInvisibleOnRadarEdgeOnFoot = 1 << 1,
            // if enabled player will not ping on the radar gutter (circumference of radar) when driving a vehicle
            PingsInvisibleOnRadarEdgeIfDriving = 1 << 2,
            // if enabled player will not ping on the radar gutter (circumference of radar) when riding in a vehicle, in any seat
            PingsInvisibleOnRadarEdgeIfInVehicle = 1 << 3,
            // if this is enabled, stealth player will only not ping for the enemy team
            PingsInvisibleOnRadarEdgeOnlyForEnemyTeam = 1 << 4,
            IsInvisibleToTurretsWhenCamoIsActive = 1 << 5,
            // enabling this will use the values set below for the radar ping and frequency
            UseModPingFrequencyAndDuration = 1 << 6,
            DisableFootstepAudio = 1 << 7
        }
        
        [TagStructure(Size = 0x8)]
        public class AuralEnhancementAudioSettingsBlock : TagStructure
        {
            // beeps per second
            public float Frequency;
            public float DutyCyclePct;
        }
    }
}
