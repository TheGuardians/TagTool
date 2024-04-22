using TagTool.Cache;
using TagTool.Common;
using System.Collections.Generic;
using System;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "cinematic_scene", Tag = "cisc", Size = 0x78, MinVersion = CacheVersion.Halo3Retail)]
    public class CinematicScene : TagStructure
	{
        public StringId Name;
        [TagField(Length = 32)]
        public string AnchorName;
        public SceneResetObjectLightingEnum ResetObjectLighting;
        [TagField(Length = 2, Flags = Padding)]
        public byte[] Padd;
        public byte[] ImportScriptHeader;
        public List<ObjectBlock> Objects;
        public List<ShotBlock> Shots;
        public List<TextureCameraBlock> TextureCameras;
        public byte[] ImportScriptFooter;
        public uint Version;

        public enum SceneResetObjectLightingEnum : short
        {
            Default,
            DontResetLighting,
            ResetLighting
        }

        [TagStructure(Size = 0x74)]
        public class ObjectBlock : TagStructure
		{
            [TagField(Length = 32)]
            public string ImportName;

            [TagField(Flags = Label)]
            public StringId Identifier;
            public StringId VariantName;
            public CachedTag PuppetAnimation;
            public CachedTag PuppetObject;
            public ObjectFlags Flags;
            public uint ShotsActiveFlags;
            public CinematicCoopTypeFlags OverrideCreationFlags;
            public byte[] ImportOverrideCreationScript;
            public List<AttachmentsBlock> Attachments;

            [Flags]
            public enum ObjectFlags : uint
            {
                None,
                PlacedManuallyInSapien = 1 << 0,
                ObjectComesFromGame = 1 << 1,
                NameIsFunctionCall = 1 << 2,
                EffectObject = 1 << 3,
                NoLightmapShadow = 1 << 4,
                UseMasterChiefPlayerAppearance = 1 << 5,
                UseDervishArbiterPlayerAppearance = 1 << 6
            }

            [Flags]
            public enum CinematicCoopTypeFlags : uint
            {
                SinglePlayer = 1 << 0,
                _2PlayerCoop = 1 << 1,
                _3PlayerCoop = 1 << 2,
                _4PlayerCoop = 1 << 3
            }

            [TagStructure(Size = 0x38)]
            public class AttachmentsBlock : TagStructure
			{
                public StringId ObjectMarkerName;
                [TagField(Length = 32)]
                public string AttachmentObjectName;
                public StringId AttachmentMarkerName;
                public CachedTag AttachmentType;
            }
        }

        [TagStructure(Size = 0xA4, MaxVersion = CacheVersion.Halo3Retail)]
        [TagStructure(Size = 0xBC, MinVersion = CacheVersion.Halo3ODST)]
        public class ShotBlock : TagStructure
		{
            public byte[] ImportScriptHeader;
            public ShotFlags Flags;
            public float EnvironmentDarken;
            public float ForcedExposure;
            public List<LightingBlock> Lighting;
            public List<ClipBlock> Clips;
            public List<DialogueBlock> Dialogue;
            public List<MusicBlock> Music;
            public List<EffectBlock> Effects;
            public List<FunctionBlock> Functions;

            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public List<ScreenEffectBlock> ScreenEffects;

            public List<CortanaEffectBlock> CortanaEffects;
            public List<ImportScriptBlock> ImportScripts;

            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public List<UserInputConstraintsBlock> UserInputConstraints;

            public byte[] ImportScriptFooter;
            public int FrameCount;
            public List<CameraFrame> CameraFrames;

            [Flags]
            public enum ShotFlags : int
            {
                None = 0,
                InstantAutoExposure = 1 << 0,
                ForceExposure = 1 << 1,
                GenerateLoopingScript = 1 << 2
            }

            [TagStructure(Size = 0x18, MaxVersion = CacheVersion.Halo3Retail)]
            [TagStructure(Size = 0x1C, MinVersion = CacheVersion.Halo3ODST)]
            public class LightingBlock : TagStructure
			{
                [TagField(MinVersion = CacheVersion.Halo3ODST)]
                public LightingFlags Flags;
                [Flags]
                public enum LightingFlags : int
                {
                    None = 0,
                    PersistsAcrossShots = 1 << 0
                }

                [TagField(Flags = Label)]
                public CachedTag CinematicLight;
                public int ObjectIndex;
                public StringId Marker;
            }

            [TagStructure(Size = 0x14)]
            public class UserInputConstraintsBlock : TagStructure
            {
                public int Frame;
                public int Ticks;
                public Rectangle2d MaximumLookAngles;
                public float FrictionalForce;
            }

            [TagStructure(Size = 0x2C)]
            public class ClipBlock : TagStructure
			{
                public RealPoint3d PlaneCenter;
                public RealPoint3d PlaneDirection;
                public uint FrameStart;
                public uint FrameEnd;

                public List<ClipObject> Objects;

                [TagStructure(Size = 0x4)]
                public class ClipObject : TagStructure
				{
                    public uint ObjectIndex;
                }
            }

            [TagStructure(Size = 0x24)]
            public class DialogueBlock : TagStructure
			{
                [TagField(Flags = Label)]
                public CachedTag Sound;
                public int Frame;
                public float Scale;
                public StringId LipsyncActor;
                public StringId DefaultSoundEffect;
                public StringId Subtitle;
            }

            [TagStructure(Size = 0x18)]
            public class MusicBlock : TagStructure
			{
                public MusicFlags Flags;
                [Flags]
                public enum MusicFlags : int
                {
                    None = 0,
                    StopMusicAtFrameRatherThanStartingIt = 1 << 0
                }

                [TagField(Flags = Label)]
                public CachedTag Sound;
                public int Frame;
            }

            [TagStructure(Size = 0x1C)]
            public class EffectBlock : TagStructure
			{
                [TagField(Flags = Label)]
                public CachedTag Effect;
                public int Frame;
                public StringId Marker;
                public int MarkerParent;
            }

            [TagStructure(Size = 0x14)]
            public class FunctionBlock : TagStructure
			{
                public int ObjectIndex;
                [TagField(Flags = Label)]
                public StringId TargetFunctionName;
                public List<KeyFrame> KeyFrames;

                [TagStructure(Size = 0x10)]
                public class KeyFrame : TagStructure
				{
                    public KeyFrameFlags Flags;
                    [Flags]
                    public enum KeyFrameFlags : int
                    {
                        None = 0,
                        ClearFunction = 1 << 0
                    }

                    public int Frame;
                    public float Value;
                    public float InterpolationTime;
                }
            }

            [TagStructure(Size = 0x18)]
            public class ScreenEffectBlock : TagStructure
			{
                [TagField(Flags = Label)]
                public CachedTag ScreenEffect;
                public int StartFrame;
                public int StopFrame;
            }

            [TagStructure(Size = 0x14)]
            public class CortanaEffectBlock : TagStructure
			{
                [TagField(Flags = Label)]
                public CachedTag CortanaEffect;
                public uint Frame;
            }

            [TagStructure(Size = 0x18)]
            public class ImportScriptBlock : TagStructure
			{
                public int Frame;
                public byte[] ImportScript;
            }
        }

        [TagStructure(Size = 0x14)]
        public class TextureCameraBlock : TagStructure
		{
            [TagField(Flags = Label)]
            public StringId Name;
            public StringId Type;
            public List<CameraShotBlock> Shots;

            [TagStructure(Size = 0xC)]
            public class CameraShotBlock : TagStructure
			{
                public List<FrameBlock> Frames;

                [TagStructure(Size = 0x48)]
                public class FrameBlock : TagStructure
                {
                    public CinematicExtraCameraFrameFlags Flags;
                    public CameraFrame CameraFrame;

                    [Flags]
                    public enum CinematicExtraCameraFrameFlags : uint
                    {
                        Enabled = 1 << 0
                    }
                }
            }
        }

        [TagStructure(Size = 0x44)]
        public class CameraFrame : TagStructure
        {
            public RealPoint3d CameraPosition;
            public RealVector3d CameraForward;
            public RealVector3d CameraUp;
            public float HorizontalFieldOfView;
            public float HorizontalFilmAperture;
            public float FocalLength;
            public FlagBits Flags;
            public float NearFocalPlaneDistance;
            public float FarFocalPlaneDistance;
            public float FocalDepth;
            public float BlurAmount;

            [Flags]
            public enum FlagBits : int
            {
                None,
                EnableDepthOfField = 1 << 0
            }
        }
    }
}