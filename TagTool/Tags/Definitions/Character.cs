using TagTool.Ai;
using TagTool.Cache;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;
using System;

namespace TagTool.Tags.Definitions
{
    [TagStructure(MaxVersion = CacheVersion.Halo3Retail, Size = 0x1D4, Name = "character", Tag = "char")]
    [TagStructure(MinVersion = CacheVersion.Halo3ODST, Size = 0x1F8, Name = "character", Tag = "char")]
    public class Character : TagStructure
	{
        public CharacterFlags Flags;
        public CachedTag ParentCharacter;
        public CachedTag Unit;
        public CachedTag Creature; // Creature reference for swarm characters ONLY
        public CachedTag Style;
        public CachedTag MajorCharacter;

        public List<CharacterVariant> Variants;
        public List<CharacterVoiceProperties> Voice;
        public List<CharacterGeneralProperties> GeneralProperties;
        public List<CharacterVitalityProperties> VitalityProperties;
        public List<CharacterPlacementProperties> PlacementProperties;
        public List<CharacterPerceptionProperties> PerceptionProperties;
        public List<CharacterLookProperties> LookProperties;
        public List<CharacterMovementProperties> MovementProperties;
        public List<CharacterFlockingProperties> FlockingProperties;
        public List<CharacterSwarmProperties> SwarmProperties;
        public List<CharacterReadyProperties> ReadyProperties;
        public List<CharacterEngageProperties> EngageProperties;
        public List<CharacterChargeProperties> ChargeProperties;
        public List<CharacterEvasionProperties> EvasionProperties;
        public List<CharacterCoverProperties> CoverProperties;
        public List<CharacterRetreatProperties> RetreatProperties;
        public List<CharacterSearchProperties> SearchProperties;
        public List<CharacterPreSearchProperties> PreSearchProperties;
        public List<CharacterIdleProperties> IdleProperties;
        public List<CharacterVocalizationProperties> VocalizationProperties;
        public List<CharacterBoardingProperties> BoardingProperties;

        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public List<CharacterBunkerProperties> BunkerProperties;

        public List<CharacterGuardianProperties> GuardianProperties;       
        public List<CharacterCombatformProperties> CombatformProperties;
       
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public List<CharacterEngineerProperties> EngineerProperties;

        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public List<CharacterInspectProperties> InspectProperties;

        public List<CharacterScarabProperties> ScarabProperties;
        public List<CharacterWeaponsProperties> WeaponsProperties;
        public List<CharacterFiringPatternProperties> FiringPatternProperties;
        public List<CharacterGrenadesProperties> GrenadesProperties;
        public List<CharacterVehicleProperties> VehicleProperties;
        public List<CharacterMorphProperties> MorphProperties;
        public List<CharacterEquipmentProperties> EquipmentProperties;
        public List<CharacterMetagameProperties> CampaignMetagameBucket;
        public List<CharacterActAttachment> ActivityObjects;

        [Flags]
        public enum CharacterFlags : uint
        {
            Flag1 = 1 << 0
        }
    }
}