using System;
using System.Collections.Generic;
using TagTool.Animations;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Damage;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "model_animation_graph", Tag = "jmad", Size = 0x104, MaxVersion = CacheVersion.HaloOnline700123)]
    [TagStructure(Name = "model_animation_graph", Tag = "jmad", Size = 0x1B8, MinVersion = CacheVersion.HaloReach)]
    public class ModelAnimationGraph : TagStructure
	{
        [TagField(ValidTags = new[] { "jmad" })]
        public CachedTag ParentAnimationGraph;
        public AnimationInheritanceFlags InheritanceFlags;
        public AnimationPrivateFlags PrivateFlags;
        public short AnimationCodecPack;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public short ForceCompressionSetting;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public short MiscGraphFlags;
        [TagField(MinVersion = CacheVersion.HaloReach, Length = 0xC)]
        public byte[] NodeUsageBlock = new byte[0xC];
        [TagField(MinVersion = CacheVersion.HaloReach, Length = 0xC)]
        public byte[] NodeMasksBlock = new byte[0xC];
        [TagField(MinVersion = CacheVersion.HaloReach, Length = 0xC)]
        public byte[] FunctionsBlock = new byte[0xC];
        [TagField(MinVersion = CacheVersion.HaloReach, Length = 0xC)]
        public byte[] ModelAnimationVariantsBlock = new byte[0xC];

        public List<SkeletonNode> SkeletonNodes;
        public List<AnimationTagReference> SoundReferences;
        public List<AnimationTagReference> EffectReferences;
        public List<BlendScreen> BlendScreens;       
        public List<FootMarkersBlock> FootMarkers;

        [TagField(ValidTags = new[] { "frms" }, MinVersion = CacheVersion.HaloReach)]
        public CachedTag FrameEvents;

        public List<Animation> Animations;

        [TagField(MinVersion = CacheVersion.HaloReach, Length = 0xC)]
        public byte[] NewBlendScreens = new byte[0xC];
        [TagField(MinVersion = CacheVersion.HaloReach, Length = 0xC)]
        public byte[] NewFunctionOverlays = new byte[0xC];
        [TagField(MinVersion = CacheVersion.HaloReach, Length = 0xC)]
        public byte[] OverlayGroups = new byte[0xC];
        [TagField(MinVersion = CacheVersion.HaloReach, Length = 0xC)]
        public byte[] Gaits = new byte[0xC];
        [TagField(MinVersion = CacheVersion.HaloReach, Length = 0xC)]
        public byte[] GaitGroups = new byte[0xC];
        [TagField(MinVersion = CacheVersion.HaloReach, Length = 0xC)]
        public byte[] ikData = new byte[0xC];
        [TagField(MinVersion = CacheVersion.HaloReach, Length = 0xC)]
        public byte[] ikGroups = new byte[0xC];
        [TagField(MinVersion = CacheVersion.HaloReach, Length = 0xC)]
        public byte[] ikChains = new byte[0xC];
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public short DefaultGaitGroup;
        [TagField(MinVersion = CacheVersion.HaloReach, Length = 0x2)]
        public byte[] Pad = new byte[0x2];

        public List<Mode> Modes;
        public List<VehicleSuspensionBlock> VehicleSuspension;
        public List<ObjectOverlay> ObjectOverlays;
        public List<Inheritance> InheritanceList;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public List<Inheritance> NewInheritanceList;

        public List<WeaponListBlock> WeaponList;

        [TagField(Length = 8)]
        public uint[] LeftArmNodes;

        [TagField(Length = 8)]
        public uint[] RightArmNodes;

        public byte[] LastImportResults;

        public List<AdditionalNodeDataBlock> AdditionalNodeData;
        
        public List<ResourceGroup> ResourceGroups;

        [Flags]
        public enum AnimationInheritanceFlags : byte
        {
            None = 0,
            InheritRootTransScaleOnly = 1 << 0,
            InheritForUseOnPlayer = 1 << 1,
            AllowFacialAnimationMaskRemap = 1 << 2
        }

        [Flags]
        public enum AnimationPrivateFlags : byte
        {
            None = 0,
            PreparedForCache = 1 << 0,
            Unused = 1 << 1,
            ImportedWithCodecCompressors = 1 << 2,
            Unused1 = 1 << 3,
            WrittenToCache = 1 << 4,
            AnimationDataReordered = 1 << 5,
            ReadyForUse = 1 << 6
        }

        [TagStructure(Size = 0x20, MaxVersion = CacheVersion.HaloOnline700123)]
        [TagStructure(Size = 0x2C, MinVersion = CacheVersion.HaloReach)]
        public class SkeletonNode : TagStructure
		{
            [TagField(Flags = Label)]
            public StringId Name;
            public short NextSiblingNodeIndex;
            public short FirstChildNodeIndex;
            public short ParentNodeIndex;
            public SkeletonModelFlags ModelFlags;
            public SkeletonNodeJointFlags NodeJointFlags;

            [TagField(MinVersion = CacheVersion.HaloReach)]
            public byte AdditionalFlags;
            [TagField(MinVersion = CacheVersion.HaloReach, Flags = Padding, Length = 3)]
            public byte[] Padding0;

            public RealVector3d BaseVector;
            public float VectorRange;
            public float ZPosition;

            [TagField(MinVersion = CacheVersion.HaloReach)]
            public uint FrameID1;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public uint FrameID2;

            [Flags]
            public enum SkeletonModelFlags : byte
            {
                None = 0,
                PrimaryModel = 1 << 0,
                SecondaryModel = 1 << 1,
                LocalRoot = 1 << 2,
                LeftHand = 1 << 3,
                RightHand = 1 << 4,
                LeftArmMember = 1 << 5
            }

            [Flags]
            public enum SkeletonNodeJointFlags : byte
            {
                None = 0,
                BallSocket = 1 << 0,
                Hinge = 1 << 1,
                NoMovement = 1 << 2
            }
        }

        [Flags]
        public enum AnimationTagReferenceFlags : ushort
        {
            None = 0,
            AllowOnPlayer = 1 << 0,
            LeftArmOnly = 1 << 1,
            RightArmOnly = 1 << 2,
            FirstPersonOnly = 1 << 3,
            ForwardOnly = 1 << 4,
            ReverseOnly = 1 << 5,
            FpNoAgedWeapons = 1 << 6,
            Bit7 = 1 << 7,
            Bit8 = 1 << 8,
            Bit9 = 1 << 9,
            Bit10 = 1 << 10,
            Bit11 = 1 << 11,
            Bit12 = 1 << 12,
            Bit13 = 1 << 13,
            Bit14 = 1 << 14,
            Bit15 = 1 << 15
        }

        [Flags]
        public enum ReachAnimationTagReferenceFlags : ushort
        {
            AllowOnPlayer = 1 << 0,
            LeftArmOnly = 1 << 1,
            RightArmOnly = 1 << 2,
            FirstPersonOnly = 1 << 3,
            ThirdPersonOnly = 1 << 4,
            ForwardOnly = 1 << 5,
            ReverseOnly = 1 << 6,
            FpNoAgedWeapons = 1 << 7
        }

        [TagStructure(Size = 0x14)]
        public class AnimationTagReference : TagStructure
		{
            [TagField(ValidTags = new[] { "effe", "scmb", "snd!" })]
            public CachedTag Reference;

            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public AnimationTagReferenceFlags Flags;

            [TagField(MinVersion = CacheVersion.HaloReach)]
            public ReachAnimationTagReferenceFlags FlagsReach;

            [TagField(Length = 2, Flags = Padding)]
            public byte[] Padding0;
        }

        [TagStructure(Size = 0x1C)]
        public class BlendScreen : TagStructure
		{
            [TagField(Flags = TagFieldFlags.Label)]
            public StringId Label;

            public Angle RightYawPerFrame;
            public Angle LeftYawPerFrame;
            public short RightFrameCount;
            public short LeftFrameCount;
            public Angle DownPitchPerFrame;
            public Angle UpPitchPerFrame;
            public short DownPitchFrameCount;
            public short UpPitchFrameCount;
        }

        [TagStructure(Size = 0x1C)]
        public class FootMarkersBlock : TagStructure
		{
            public StringId FootMarker;
            public Bounds<float> FootIkRange;
            public StringId AnkleMarker;
            public Bounds<float> AnkleIkRange;
            public FootTrackingDefaultValues DefaultState;

            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding1;

            public enum FootTrackingDefaultValues : short
            {
                Off,
                On
            }
        }

        public enum FrameType : sbyte
        {
            Base,
            Overlay,
            Replacement
        }

        public enum FrameTypeReach : sbyte
        {
            None,
            Base,
            Overlay,
            Replacement
        }

        [TagStructure(Size = 0x88, MaxVersion = CacheVersion.HaloOnline700123)]
        [TagStructure(Size = 0x3C, MinVersion = CacheVersion.HaloReach)]
        public class Animation : TagStructure
		{
            [TagField(Flags = Label)]
            public StringId Name;

            public float Weight;            
            public short LoopFrameIndex;            
            public PlaybackFlagsValue PlaybackFlags;

            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float OverrideBlendInTime;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float OverrideBlendOutTime;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public short PreviousVariantSiblingReach;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public short NextVariantSiblingReach;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public ProductionFlagsValue ProductionFlagsReach;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public sbyte BlendScreenReach;
            [TagField(MinVersion = CacheVersion.HaloReach, Length = 0x1, Flags = Padding)]
            public byte[] PaddingReach;
            [TagField(ValidTags = new[] { "jmad" }, MinVersion = CacheVersion.HaloReach)]
            public CachedTag GraphReference;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public short SharedAnimationIndex;
            [TagField(MinVersion = CacheVersion.HaloReach, Flags = Padding, Length = 2)]
            public byte[] Padding2;

            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public SharedAnimationData AnimationData; //this block is inline up until Reach
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public List<SharedAnimationData> AnimationDataBlock;

            [TagStructure(Size = 0xD4, MinVersion = CacheVersion.HaloReach)]
            [TagStructure(Size = 0x7C, MaxVersion = CacheVersion.HaloOnline700123)]
            public class SharedAnimationData : TagStructure
            {
                [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
                public sbyte BlendScreen;
                [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
                public CompressionValue DesiredCompression;
                [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
                public CompressionValue CurrentCompression;

                public sbyte NodeCount;

                [TagField(MinVersion = CacheVersion.HaloReach, Length = 0x1, Flags = Padding)]
                public byte[] PaddingReach;

                public short FrameCount;

                [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
                public FrameType AnimationType;
                [TagField(MinVersion = CacheVersion.HaloReach)]
                public FrameTypeReach AnimationTypeReach;

                public FrameInfoType FrameInfoType;
                [TagField(MinVersion = CacheVersion.HaloReach)]
                public CompressionValue DesiredCompressionReach;
                [TagField(MinVersion = CacheVersion.HaloReach)]
                public CompressionValue CurrentCompressionReach;

                [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
                public ProductionFlagsValue ProductionFlags;

                [TagField(MinVersion = CacheVersion.HaloReach, Length = 0x2, Flags = Padding)]
                public byte[] PaddingReach1;

                public InternalFlagsValue InternalFlags;

                [TagField(MinVersion = CacheVersion.HaloReach)]
                public int Uid;
                [TagField(MinVersion = CacheVersion.HaloReach)]
                public StringId SharedId;

                public int NodeListChecksum;
                public int ProductionChecksum;
                public short ImporterVersion;
                public short CompressorVersion;

                [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
                public short ParentAnimation;
                [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
                public short NextAnimation;

                public short ResourceGroupIndex;
                public short ResourceGroupMemberIndex;

                [TagField(MinVersion = CacheVersion.HaloReach)]
                public RealVector3d HeadingReach;
                [TagField(MinVersion = CacheVersion.HaloReach)]
                public float AverageTranslationMagnitudeReach;
                [TagField(MinVersion = CacheVersion.HaloReach)]
                public float AveragePivotYawReach;

                public List<FrameEvent> FrameEvents;
                public List<SoundEvent> SoundEvents;
                public List<EffectEvent> EffectEvents;
                public List<DialogueEvent> DialogueEvents;
                public List<ObjectSpaceParentNode> ObjectSpaceParentNodes;
                public List<FootTrackingBlock> FootTracking;

                [TagField(MinVersion = CacheVersion.HaloReach, Length = 0xC)]
                public byte[] ObjectSpaceOffsetNodes = new byte[0xC];
                [TagField(MinVersion = CacheVersion.HaloReach, Length = 0xC)]
                public byte[] ForwardInvertKineticAnchorNodes = new byte[0xC];
                [TagField(MinVersion = CacheVersion.HaloReach, Length = 0xC)]
                public byte[] ikChainEvents = new byte[0xC];
                [TagField(MinVersion = CacheVersion.HaloReach, Length = 0xC)]
                public byte[] ikChainProxies = new byte[0xC];
                [TagField(MinVersion = CacheVersion.HaloReach, Length = 0xC)]
                public byte[] FacialWrinkleEvents = new byte[0xC];
                [TagField(MinVersion = CacheVersion.HaloReach, Length = 0xC)]
                public byte[] ExtendedDataEvents = new byte[0xC];
                [TagField(MinVersion = CacheVersion.HaloReach, Length = 0xC)]
                public byte[] AnimationObjectFunctions = new byte[0xC];

                //First float is usually 1
                [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
                public RealVector3d Heading;
                [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
                public float AverageTranslationMagnitude;
                [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
                public float AveragePivotYaw;

            }

            [Flags]
            public enum InternalFlagsValue : ushort
            {
                None,
                Unused1 = 1 << 0,
                WorldRelative = 1 << 1,
                Unused2 = 1 << 2,
                Unused3 = 1 << 3,
                Unused4 = 1 << 4,
                CompressionDisabled = 1 << 5,
                OldProductionChecksum = 1 << 6,
                ValidProductionChecksum = 1 << 7
            }

            [Flags]
            public enum ProductionFlagsValue : ushort
            {
                None,
                DoNotMonitorChanges = 1 << 0,
                VerifySoundEvents = 1 << 1,
                DoNotInheritForPlayerGraphs = 1 << 2
            }

            [Flags]
            public enum PlaybackFlagsValue : ushort
            {
                None,
                DisableInterpolationIn = 1 << 0,
                DisableInterpolationOut = 1 << 1,
                DisableModeIk = 1 << 2,
                DisableWeaponIk = 1 << 3,
                DisableWeaponAim1stPerson = 1 << 4,
                DisableLookScreen = 1 << 5,
                DisableTransitionAdjustment = 1 << 6,
                ForceWeaponIkOn = 1 << 7
            }

            public enum CompressionValue : sbyte
            {
                BestScore,
                BestCompression,
                BestAccuracy,
                BestFullframe,
                BestSmallKeyframe,
                BestLargeKeyframe
            }
            public enum FrameEventType : short
            {
                PrimaryKeyframe,
                SecondaryKeyframe,
                LeftFoot,
                RightFoot,
                AllowInterruption,
                TransitionA,
                TransitionB,
                TransitionC,
                TransitionD,
                BothFeetShuffle,
                BodyImpact,
                LeftBackFoot,
                RightBackFoot
            }

            public enum FrameEventTypeHO : short
            {
                PrimaryKeyframe,
                SecondaryKeyframe,
                TertiaryKeyframe,
                LeftFoot,
                RightFoot,
                AllowInterruption,
                TransitionA,
                TransitionB,
                TransitionC,
                TransitionD,
                BothFeetShuffle,
                BodyImpact,
                LeftBackFoot,
                RightBackFoot,
                RagdollKeyframe,
                DropWeaponKeyframe
            }

            public enum FrameEventTypeED : short
            {
                PrimaryKeyframe,
                SecondaryKeyframe,
                TertiaryKeyframe,
                LeftFoot,
                RightFoot,
                AllowInterruption,
                TransitionA,
                TransitionB,
                TransitionC,
                TransitionD,
                BothFeetShuffle,
                BodyImpact,
                LeftBackFoot,
                RightBackFoot,
                RagdollKeyframe,
                DropWeaponKeyframe,
                AssassinationA,
                AssassinationB
            }

            public enum FrameEventTypeReach : short
            {
                None,
                PrimaryKeyframe,
                SecondaryKeyframe,
                TertiaryKeyframe,
                LeftFoot,
                RightFoot,
                AllowInterruption,
                DoNotAllowInterruption,
                BothFeetShuffle,
                BodyImpact,
                LeftFootLock,
                LeftFootUnlock,
                RightFootLock,
                RightFootUnlock,
                BlendRangeMarker,
                StrideExpansion,
                StrideContraction,
                RagdollKeyframe,
                DropWeaponKeyframe,
                MatchA,
                MatchB,
                MatchC,
                MatchD,
                JetpackClosed,
                JetpackOpen,
                SoundEvent,
                EffectEvent
            }

            [TagStructure(Size = 0x4)]
            public class FrameEvent : TagStructure
			{
                [TagField(MaxVersion = CacheVersion.Halo3ODST)]
                public FrameEventType Type;
                [TagField(MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnlineED)]
                public FrameEventTypeED TypeED;
                [TagField(MinVersion = CacheVersion.HaloOnline106708, MaxVersion = CacheVersion.HaloOnline700123)]
                public FrameEventTypeHO TypeHO;
                [TagField(MinVersion = CacheVersion.HaloReach)]
                public FrameEventTypeReach ReachType;
                public short Frame;
            }

            [TagStructure(Size = 0x8)]
            public class SoundEvent : TagStructure
			{
                public short Sound;
                public short Frame;
                public StringId MarkerName;
            }

            [TagStructure(Size = 0x8, MaxVersion = CacheVersion.Halo3Retail)]
            [TagStructure(Size = 0xC, MinVersion = CacheVersion.Halo3ODST)]
            public class EffectEvent : TagStructure
			{
                public short Effect;
                public short Frame;
                public StringId MarkerName;

                [TagField(MinVersion = CacheVersion.Halo3ODST)]
                public DamageReportingType DamageEffectReportingType;

                [TagField(MinVersion = CacheVersion.Halo3ODST, Flags = Padding, Length = 3)]
                public byte[] Padding3;
            }

            public enum DialogueEventType : short
            {
                Bump,
                Dive,
                Evade,
                Lift,
                Sigh,
                Contempt,
                Anger,
                Fear,
                Relief,
                Sprint,
                SprintEnd,
                AssGrabber,
                KillAss,
                AssGrabbed,
                DieAss
            }

            [TagStructure(Size = 0x4)]
            public class DialogueEvent : TagStructure
			{
                public DialogueEventType EventType;
                public short Frame;
            }

            [Flags]
            public enum ObjectSpaceParentNodeFlags : ushort
            {
                None = 0,
                Rotation = 1 << 0,
                Translation = 1 << 1,
                Scale = 1 << 2,
                MotionRoot = 1 << 3
            }

            [TagStructure(Size = 0x1C)]
            public class ObjectSpaceParentNode : TagStructure
			{
                public short NodeIndex;
                public ObjectSpaceParentNodeFlags Flags;
                public short RotationX;
                public short RotationY;
                public short RotationZ;
                public short RotationW;
                public RealPoint3d DefaultTranslation;
                public float DefaultScale;
            }

            [TagStructure(Size = 0x10)]
            public class FootTrackingBlock : TagStructure
			{
                public short Foot;
                [TagField(Flags = Padding, Length = 2)]
                public byte[] Padding4;
                public List<FootLockCycleBlock> FootLockCycles;

                [TagStructure(Size = 0x14)]
                public class FootLockCycleBlock : TagStructure
				{
                    public short StartedLocking;
                    public short Locked;
                    public short StartedUnlocking;
                    public short Unlocked;
                    public RealPoint3d LockPoint;
                }
            }
        }

        [TagStructure(Size = 0x28, MaxVersion = CacheVersion.HaloOnline700123)]
        [TagStructure(Size = 0x30, MinVersion = CacheVersion.HaloReach)]
        public class Mode : TagStructure
		{
            [TagField(Flags = Label)]
            public StringId Name;

            [TagField(MinVersion = CacheVersion.HaloReach)]
            public short OverlayGroup;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public short ikGroup;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public int StanceFlags;

            public List<WeaponClassBlock> WeaponClass;
            public List<ModeIkBlock> ModeIk;
            public List<FootTrackingDefaultsBlock> FootDefaults;

            [TagStructure(Size = 0x1C, MaxVersion = CacheVersion.Halo3ODST)]
            [TagStructure(Size = 0x28, MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
            [TagStructure(Size = 0x38, MinVersion = CacheVersion.HaloReach)]
            public class WeaponClassBlock : TagStructure
			{
                [TagField(Flags = TagFieldFlags.Label)]
                public StringId Label;

                [TagField(MinVersion = CacheVersion.HaloReach)]
                public short OverlayGroup;
                [TagField(MinVersion = CacheVersion.HaloReach)]
                public short ikGroup;

                public List<WeaponTypeBlock> WeaponType;
                public List<ModeIkBlock> WeaponIk;

                [TagField(MinVersion = CacheVersion.HaloReach, Length = 0xC)]
                public byte[] RangedActions = new byte[0xC];

                [TagField(MinVersion = CacheVersion.HaloOnlineED)]
                public List<SyncActionGroup> SyncActionGroups;

                [TagStructure(Size = 0x34, MaxVersion = CacheVersion.HaloOnline700123)]
                [TagStructure(Size = 0x14, MinVersion = CacheVersion.HaloReach)]
                public class WeaponTypeBlock : TagStructure
				{
                    [TagField(Flags = TagFieldFlags.Label)]
                    public StringId Label;

                    [TagField(MinVersion = CacheVersion.HaloReach)]
                    public short OverlayGroup;
                    [TagField(MinVersion = CacheVersion.HaloReach)]
                    public short ikGroup;
                    [TagField(MinVersion = CacheVersion.HaloReach)]
                    public List<AnimationSet> AnimationSetsReach;

                    [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
                    public AnimationSet Set; //block is inlined up until Reach

                    [TagStructure(Size = 0x48, MinVersion = CacheVersion.HaloReach)]
                    [TagStructure(Size = 0x30, MaxVersion = CacheVersion.HaloOnline700123)]
                    public class AnimationSet : TagStructure
                    {
                        [TagField(MinVersion = CacheVersion.HaloReach, Flags = TagFieldFlags.Label)]
                        public StringId Label;
                        [TagField(MinVersion = CacheVersion.HaloReach)]
                        public short OverlayGroup;
                        [TagField(MinVersion = CacheVersion.HaloReach)]
                        public short ikGroup;
                        [TagField(MinVersion = CacheVersion.HaloReach)]
                        public short gaitGroup;
                        [TagField(MinVersion = CacheVersion.HaloReach, Flags = Padding, Length = 0x2)]
                        public byte[] Padding5;

                        public List<Entry> Actions;
                        public List<Entry> Overlays;
                        public List<DeathAndDamageBlock> DeathAndDamage;
                        public List<Transition> Transitions;
                        [TagField(MinVersion = CacheVersion.HaloReach, Length = 0xC)]
                        public byte[] VelocityBoundaries = new byte[0xC];
                    }

                    [TagStructure(Size = 0x8, MaxVersion = CacheVersion.HaloOnline700123)]
                    [TagStructure(Size = 0xC, MinVersion = CacheVersion.HaloReach)]
                    public class Entry : TagStructure
					{
                        [TagField(Flags = TagFieldFlags.Label)]
                        public StringId Label;
                        [TagField(MinVersion = CacheVersion.HaloReach)]
                        public short OverlayGroup;
                        [TagField(MinVersion = CacheVersion.HaloReach)]
                        public short ikGroup;
                        public short GraphIndex;
                        public short Animation;
                    }

                    [TagStructure(Size = 0x10)]
                    public class DeathAndDamageBlock : TagStructure
					{
                        [TagField(Flags = TagFieldFlags.Label)]
                        public StringId Label;

                        public List<Direction> Directions;

                        [TagStructure(Size = 0xC)]
                        public class Direction : TagStructure
						{
                            public List<Region> Regions;

                            [TagStructure(Size = 0x4)]
                            public class Region : TagStructure
							{
                                public short GraphIndex;
                                public short Animation;
                            }
                        }
                    }

                    [TagStructure(Size = 0x18, MaxVersion = CacheVersion.HaloOnline700123)]
                    [TagStructure(Size = 0x10, MinVersion = CacheVersion.HaloReach)]
                    public class Transition : TagStructure
					{
                        [TagField(Flags = TagFieldFlags.Label, MaxVersion = CacheVersion.HaloOnline700123)]
                        public StringId FullName;
                        public StringId StateName;

                        [TagField(MaxVersion = CacheVersion.HaloOnline700123, Flags = Padding, Length = 2)]
                        public byte[] Padding0;

                        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
                        public sbyte IndexA;
                        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
                        public sbyte IndexB;

                        public List<Destination> Destinations;

                        [TagStructure(Size = 0x14, MaxVersion = CacheVersion.HaloOnline700123)]
                        [TagStructure(Size = 0xC, MinVersion = CacheVersion.HaloReach)]
                        public class Destination : TagStructure
						{
                            [TagField(Flags = TagFieldFlags.Label, MaxVersion = CacheVersion.HaloOnline700123)]
                            public StringId FullName;

                            public StringId ModeName;
                            public StringId StateName;

                            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
                            public FrameEventLinkValue FrameEventLink;

                            [TagField(MaxVersion = CacheVersion.HaloOnline700123, Flags = Padding, Length = 1)]
                            public byte[] Padding1;

                            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
                            public sbyte IndexA;
                            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
                            public sbyte IndexB;

                            public short GraphIndex;
                            public short Animation;

                            public enum FrameEventLinkValue : sbyte
                            {
                                NoKeyframe,
                                KeyframeTypeA,
                                KeyframeTypeB,
                                KeyframeTypeC,
                                KeyframeTypeD
                            }
                        }
                    }

                    [TagStructure(Size = 0x4)]
                    public class PrecacheListBlock : TagStructure
					{
                        public int CacheBlockIndex;
                    }
                }

                [TagStructure(Size = 0x10)]
                public class SyncActionGroup : TagStructure
				{
                    [TagField(Flags = TagFieldFlags.Label)]
                    public StringId Name;
                    public List<SyncAction> SyncActions;

                    [TagStructure(Size = 0x1C)]
                    public class SyncAction : TagStructure
					{
                        [TagField(Flags = TagFieldFlags.Label)]
                        public StringId Name;
                        public List<SameTypeParticipant> SameTypeParticipants;
                        public List<OtherParticipant> OtherParticipants;

                        [TagStructure(Size = 0x30)]
                        public class SameTypeParticipant : TagStructure
						{
                            public ParticipantFlags Flags;
                            public short GraphIndex;
                            public short Animation;
                            public RealPoint3d StartOffset;
                            public RealVector3d StartFacing;
                            public RealVector3d EndOffset;
                            public float TimeUntilHurt;

                            [Flags]
                            public enum ParticipantFlags : int
                            {
                                None = 0,
                                Initiator = 1 << 0,
                                CriticalParticipant = 1 << 1,
                                Disabled = 1 << 2,
                                Airborne = 1 << 3,
                                Partner = 1 << 4
                            }
                        }

                        [TagStructure(Size = 0x14)]
                        public class OtherParticipant : TagStructure
						{
                            public ParticipantFlags Flags;
                            [TagField(ValidTags = new[] { "unit", "scen" })]
                            public CachedTag ObjectType;

                            public enum ParticipantFlags : int
                            {
                                None = 0,
                                SupportsAnyType = 1 << 0,
                                Disabled = 1 << 1
                            }
                        }
                    }
                }
            }

            [TagStructure(Size = 0x8)]
            public class ModeIkBlock : TagStructure
			{
                public StringId Marker;
                public StringId AttachToMarker;
            }

            [TagStructure(Size = 0x4)]
            public class FootTrackingDefaultsBlock : TagStructure
            {
                public short Foot;
                public short DefaultState;
            }
        }

        [TagStructure(Size = 0x28, MaxVersion = CacheVersion.HaloOnline700123)]
        [TagStructure(Size = 0x30, MinVersion = CacheVersion.HaloReach)]
        public class VehicleSuspensionBlock : TagStructure
		{
            [TagField(Flags = TagFieldFlags.Label)]
            public StringId Label;
            public short GraphIndex;
            public short Animation;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public StringId FunctionName;
            public StringId MarkerName;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public StringId ContactMarker;
            public float MassPointOffset;
            public float FullExtensionGroundDepth;
            public float FullCompressionGroundDepth;
            public StringId RegionName;
            public float DestroyedMassPointOffset;
            public float DestroyedFullExtensionGroundDepth;
            public float DestroyedFullCompressionGroundDepth;
        }

        [TagStructure(Size = 0x14)]
        public class ObjectOverlay : TagStructure
		{
            [TagField(Flags = TagFieldFlags.Label)]
            public StringId Label;
            public short GraphIndex;
            public short Animation;

            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding0;

            public FunctionControlsValue FunctionControls;
            public StringId Function;

            [TagField(Flags = Padding, Length = 4)]
            public byte[] Padding1;

            public enum FunctionControlsValue : short
            {
                Frame,
                Scale
            }
        }

        [Flags]
        public enum InheritanceListFlags : uint
        {
            None = 0,
            TightenNodes = 1 << 0,
            Bit1 = 1 << 1,
            // ODST
            Bit2 = 1 << 2
        }

        [TagStructure(Size = 0x30)]
        public class Inheritance : TagStructure
		{
            [TagField(ValidTags = new[] { "jmad" })]
            public CachedTag InheritedGraph;
            public List<NodeMapBlock> NodeMap;
            public List<NodeMapFlag> NodeMapFlags;
            public float RootZOffset;
            public InheritanceListFlags Flags;

            [TagStructure(Size = 0x2)]
            public class NodeMapBlock : TagStructure
			{
                public short LocalNode;
            }

            [TagStructure(Size = 0x4)]
            public class NodeMapFlag : TagStructure
			{
                public int LocalNodeFlags;
            }
        }

        [TagStructure(Size = 0x8)]
        public class WeaponListBlock : TagStructure
		{
            [TagField(Flags = TagFieldFlags.Label)]
            public StringId WeaponName;
            public StringId WeaponClass;
        }

        [TagStructure(Size = 0x3C)]
        public class AdditionalNodeDataBlock : TagStructure
		{
            [TagField(Flags = TagFieldFlags.Label)]
            public StringId NodeName;
            public RealQuaternion DefaultRotation;
            public RealPoint3d DefaultTranslation;
            public float DefaultScale;
            public RealPoint3d MinimumBounds;
            public RealPoint3d MaximumBounds;
        }

        [TagStructure(Size = 0xC)]
        public class ResourceGroup : TagStructure
		{
            public int MemberCount;
            public TagResourceReference ResourceReference;
        }
    }
}
