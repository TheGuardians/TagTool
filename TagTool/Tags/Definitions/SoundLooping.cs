using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;

namespace TagTool.Tags.Definitions
{
	[TagStructure(Name = "sound_looping", Tag = "lsnd", Size = 0x40, MinVersion = CacheVersion.Halo3Retail, MaxVersion = CacheVersion.HaloOnline700123)]
	[TagStructure(Name = "sound_looping", Tag = "lsnd", Size = 0x38, MinVersion = CacheVersion.HaloReach)]
	public class SoundLooping : TagStructure
	{
		public SoundLoopingFlags Flags;
		public Bounds<float> MartySMusicTime;
		public Bounds<float> DistanceBounds;

		[TagField(MaxVersion = CacheVersion.HaloOnline700123)]
		public CachedTag Unused;

		[TagField(MinVersion = CacheVersion.HaloReach)]
		public float MaximumFlybyRangeDistance;

		public SoundClassValue SoundClass;
		public short LoopTypeHO; // padding in H3, unknown in Reach

		[TagField(MinVersion = CacheVersion.HaloReach)] // This track's markers, flags, gain and fade settings are copied to the other tracks. 
		public StringId MasterMarkerTrack; // Its gain and fadeout settings are also copied to the details.

		public List<Track> Tracks; // tracks play in parallel and loop continuously for the duration of the looping sound.
		public List<DetailSound> DetailSounds; // detail sounds play at random throughout the duration of the looping sound.
		
		[Flags]
		public enum SoundLoopingFlags : int
		{
			DeafeningToAIs = 1 << 0, // When used as a background stereo track, causes nearby AIs to be unable to hear
			NotALoop = 1 << 1, // This is a collection of permutations strung together that should play once then stop.
			StopsMusic = 1 << 2, // All other music loops will stop when this one starts.
			AlwaysSpatialize = 1 << 3, // Always play as 3d sound, even in first person
			SynchronizePlayback = 1 << 4, // Aynchronizes playback with other looping sounds attached to the owner of this sound
			SynchronizeTracks = 1 << 5,
			FakeSpatializationWithDistance = 1 << 6,
			CombineAll3dPlayback = 1 << 7,
			PersistentFlyby = 1 << 8, // Like a laser blast
			DontApplyRandomSpatializationToDetails = 1 << 9,
			AllowMarkerStitching = 1 << 10, // You need to reimport the sound_looping for this to take effect
			DontDelayRetries = 1 << 11, // Don't delay retrying the looping sound, in case the bank is loaded now
			UseVehicleParentForPlayerness = 1 << 12, // Look to the parent of the vehicle. Only works on vehicles. Duh
			ImplicitSpeedRPTC = 1 << 13, // Looping sound speed
			HasOptionalPlayerSound = 1 << 14,
			DontOcclude = 1 << 15,
			NegateRadiusBasedFocus = 1 << 16 // Use the opposite of the decision for whether to apply focus feature based on whether the sound is 2D or 3D
		}
		
		public enum SoundClassValue : short
		{
			ProjectileImpact,
			ProjectileDetonation,
			ProjectileFlyby,
			ProjectileDetonationLod,
			WeaponFire,
			WeaponReady,
			WeaponReload,
			WeaponEmpty,
			WeaponCharge,
			WeaponOverheat,
			WeaponIdle,
			WeaponMelee,
			WeaponAnimation,
			ObjectImpacts,
			ParticleImpacts,
			WeaponFireLod,
			WeaponFireLodFar, // Unused1Impacts
			Unused2Impacts,
			UnitFootsteps,
			UnitDialog,
			UnitAnimation,
			UnitUnused,
			VehicleCollision,
			VehicleEngine,
			VehicleAnimation,
			VehicleEngineLod,
			DeviceDoor,
			DeviceUnused0,
			DeviceMachinery,
			DeviceStationary,
			DeviceUnused1,
			DeviceUnused2,
			Music,
			AmbientNature,
			AmbientMachinery,
			AmbientStationary,
			HugeAss,
			ObjectLooping,
			CinematicMusic,
			PlayerArmor, // UnknownUnused0
			UnknownUnused1,
			AmbientFlock,
			NoPad,
			NoPadStationary,
			Arg, // MissionUnused0
			CortanaMission,
			CortanaGravemindChannel,
			MissionDialog,
			CinematicDialog,
			ScriptedCinematicFoley,
			Hud,	// HO only?
			GameEvent,
			Ui,
			Test,
			MultilingualTest,
			AmbientNatureDetails,
			AmbientMachineryDetails,
			InsideSurroundTail,
			OutsideSurroundTail,
			VehicleDetonation,
			AmbientDetonation,
			FirstPersonInside,
			FirstPersonOutside,
			FirstPersonAnywhere,
			UiPda,
		}
		
		[TagStructure(Size = 0x90, MaxVersion = CacheVersion.Halo3Retail)]
		[TagStructure(Size = 0xA0, MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.HaloOnline700123)]
		[TagStructure(Size = 0xB0, MinVersion = CacheVersion.HaloReach)]
		public class Track : TagStructure
		{
			public StringId Name;

			[TagField(MaxVersion = CacheVersion.HaloOnline700123)]
			public LsndTrackFlags Flags;

			[TagField(MinVersion = CacheVersion.HaloReach)]
			public LsndTrackFlagsReach FlagsReach;

			[TagField(MinVersion = CacheVersion.HaloReach)]
			public OutputEffectValue OutputEffectReach;

			public float Gain; // (decibels)

			[TagField(MaxVersion = CacheVersion.HaloOnline700123)]
			public float FadeInDuration; // seconds

			[TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.HaloOnline700123)]
			public SoundFadeMode FadeInMode;
			
			[TagField(Flags = TagFieldFlags.Padding, MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.HaloOnline700123, Length = 2)]
			public byte[] Padding1;

			[TagField(MaxVersion = CacheVersion.HaloOnline700123)]
			public float FadeOutDuration; // seconds

			[TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.HaloOnline700123)]
			public SoundFadeMode FadeOutMode;
			
			[TagField(Flags = TagFieldFlags.Padding, MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.HaloOnline700123, Length = 2)]
			public byte[] Padding2;
			
			[TagField(ValidTags = new [] { "snd!" })]
            public CachedTag In;
			[TagField(ValidTags = new [] { "snd!" })]
            public CachedTag Loop;
			[TagField(ValidTags = new [] { "snd!" })]
            public CachedTag Out;
			[TagField(ValidTags = new [] { "snd!" })]
            public CachedTag AlternateLoop;
            [TagField(ValidTags = new [] { "snd!" })]
            public CachedTag AlternateOut;

			[TagField(MaxVersion = CacheVersion.HaloOnline700123)]
			public OutputEffectValue OutputEffect;

			[TagField(Length = 2, Flags = TagFieldFlags.Padding, MaxVersion = CacheVersion.HaloOnline700123)]
			public byte[] Padding3;

            [TagField(ValidTags = new[] { "snd!" })]
            public CachedTag AlternateTransitionIn;
            [TagField(ValidTags = new[] { "snd!" })]
            public CachedTag AlternateTransitionOut;

			[TagField(MinVersion = CacheVersion.HaloReach)]
			public float FadeInDurationReach; // seconds
			[TagField(MinVersion = CacheVersion.HaloReach)]
			public SoundFadeModeReach FadeInModeReach; // seconds
			[TagField(MinVersion = CacheVersion.HaloReach)]
			public float FadeOutDurationReach; // seconds
			[TagField(MinVersion = CacheVersion.HaloReach)]
			public SoundFadeModeReach FadeOutModeReach; // seconds

			public float AlternateCrossfadeDuration; // seconds

			[TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.HaloOnline700123)]
			public SoundFadeMode AlternateCrossfadeMode;

			[TagField(MinVersion = CacheVersion.HaloReach)]
			public SoundFadeModeReach AlternateCrossfadeModeReach; // seconds

			[TagField(Flags = TagFieldFlags.Padding, MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.HaloOnline700123, Length = 2)]
			public byte[] Padding4;
			
			public float AlternateFadeOutDuration; // seconds

			[TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.HaloOnline700123)]
			public SoundFadeMode AlternateFadeOutMode;

			[TagField(MinVersion = CacheVersion.HaloReach)]
			public SoundFadeModeReach AlternateFadeOutModeReach; // seconds

			[TagField(Flags = TagFieldFlags.Padding, MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.HaloOnline700123, Length = 2)]
			public byte[] Padding5;

			// Reach-only Layers section

			[TagField(MinVersion = CacheVersion.HaloReach)]
			public LsndLayerReach Layer;
			[TagField(MinVersion = CacheVersion.HaloReach)]
			public LsndLayerFlagsReach LayerFlags;
			[TagField(MinVersion = CacheVersion.HaloReach)]
			public float LayerFadeInDuration;
			[TagField(MinVersion = CacheVersion.HaloReach)]
			public SoundFadeModeReach LayerFadeInMode;
			[TagField(MinVersion = CacheVersion.HaloReach)]
			public float LayerFadeOutDuration;
			[TagField(MinVersion = CacheVersion.HaloReach)]
			public SoundFadeModeReach LayerFadeOutMode;

			[Flags]
			public enum LsndTrackFlags : uint
			{
				None = 0,
				FadeInAtStart = 1 << 0, // the loop sound should fade in while the start sound is playing.
				FadeOutAtStop = 1 << 1, // the loop sound should fade out while the stop sound is playing.
				CrossfadeAlternateLoop = 1 << 2, // when the sound changes to the alternate version,  .
				MasterSurroundSoundTrack = 1 << 3,
				FadeOutAtAlternateStop = 1 << 4,
				Bit5 = 1 << 5,
				Bit6 = 1 << 6,
				Bit7 = 1 << 7
			}
			
			public enum OutputEffectValue : short
			{
				None,
				OutputFrontSpeakers,
				OutputRearSpeakers,
				OutputCenterSpeakers,
			}
			
			public enum SoundFadeMode : short
			{
				None,
				Linear,
				Power,
				InversePower,
				EaseInOut
			}

			[Flags]
			public enum LsndTrackFlagsReach : short
			{
				None = 0,
				FadeInAtStart = 1 << 0, // the loop sound should fade in while the start sound is playing.
				FadeOutAtStop = 1 << 1, // the loop sound should fade out while the stop sound is playing.
				FadeOutAtAlternateStop = 1 << 2, //the loop should fade out while the alt stop sound is playing.
				CrossfadeAlternateLoop = 1 << 3, // when the sound changes to the alternate version,  .
				MakeFadesWaitForMarkers = 1 << 4,
				MasterSurroundSoundTrack = 1 << 5,
				Bit6 = 1 << 6,
				Bit7 = 1 << 7
			}

			[Flags]
			public enum LsndLayerFlagsReach : short
			{
				None = 0,
				MakeLayerWaitForMarkers = 1 << 0
			}
		}
		
		[TagStructure(Size = 0x3C, MaxVersion = CacheVersion.HaloOnline700123)]
		[TagStructure(Size = 0x48, MinVersion = CacheVersion.HaloReach)]
		public class DetailSound : TagStructure
		{
			public StringId Name;

            [TagField(ValidTags = new[] { "snd!" })]
            public CachedTag Sound;

			[TagField(MinVersion = CacheVersion.HaloReach)]
			public PeriodTypeReach PeriodType;

			public Bounds<float> RandomPeriodBounds; // (seconds) the time between successive playings of this sound will be randomly selected from this range.
			public float DetailGain; // (decibels)

			[TagField(MinVersion = CacheVersion.HaloReach)]
			public float FadeOutDuration;
			[TagField(MinVersion = CacheVersion.HaloReach)]
			public SoundFadeModeReach FadeOutMode;

			public LoopingSoundDetailFlags Flags;
			public Bounds<Angle> YawBounds; // (degrees) the sound's position along the horizon will be randomly selected from this range.
			public Bounds<Angle> PitchBounds; // (degrees) the sound's position above (positive values) or below (negative values) the horizon will be randomly selected from this range.
			public Bounds<float> DistanceBounds; // (world units) the sound's distance from its spatialized looping sound (or from the listener if the looping sound is stereo) will be randomly selected from this range.

			[Flags]
			public enum LoopingSoundDetailFlags : uint
			{
				DontPlayWithAlternate = 1 << 0,
				DontPlayWithoutAlternate = 1 << 1,
				StartImmediatelyWithLoop = 1 << 2,
				InheritScaleFromLoop = 1 << 3,
				StopWithLoop = 1 << 4
			}

			public enum PeriodTypeReach : int
			{
				IgnoresPlaybackTime,
				RelativeToPlaybackEnd
			}
		}

		public enum SoundFadeModeReach : int
		{
			None,
			Linear,
			Power,
			InversePower,
			EaseInOut // Sigmoid "S"-Curve
		}

		public enum LsndLayerReach : short
		{
			None,
			One,
			Two,
			Three,
			Four
		}
	}
}
