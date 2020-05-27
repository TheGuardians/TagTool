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

        public uint Unknown1;
        public byte[] ImportScript1;

        public List<PuppetBlock> Puppets;
        public List<ShotBlock> Shots;
        public List<TextureCameraBlock> TextureCameras;

        public byte[] importScript2;
        public uint Unknown3;
        
        [TagStructure(Size = 0x74)]
        public class PuppetBlock : TagStructure
		{
            [TagField(Length = 32)]
            public string ImportName;

            [TagField(Flags = Label)]
            public StringId Name;
            public StringId Variant;
            public CachedTag PuppetAnimation;
            public CachedTag PuppetObject;

            public short Flags;
            public short Unknown1;
            public int Unknown2;
            public int Unknown3;

            public byte[] ImportScript;

            public List<UnknownBlock> Unknown7;

            [TagStructure(Size = 0x38)]
            public class UnknownBlock : TagStructure
			{
                public uint Unknown1;
                public uint Unknown2;
                public uint Unknown3;
                public uint Unknown4;
                public uint Unknown5;
                public uint Unknown6;
                public uint Unknown7;
                public uint Unknown8;
                public uint Unknown9;
                public uint Unknown10;
                public CachedTag Unknown11;
            }
        }

        [TagStructure(Size = 0xA4, MaxVersion = CacheVersion.Halo3Retail)]
        [TagStructure(Size = 0xBC, MinVersion = CacheVersion.Halo3ODST)]
        public class ShotBlock : TagStructure
		{
            public byte[] OpeningImportScripts;
            public int Unknown1;
            public uint Unknown2;
            public float Unknown3;
            public List<LightingBlock> Lighting;
            public List<ClipBlock> Clips;
            public List<SoundBlock> Sounds;
            public List<BackgroundSoundBlock> BackgroundSounds;
            public List<EffectBlock> Effects;
            public List<FunctionBlock> Functions;

            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public List<ScreenEffectBlock> ScreenEffects;

            public List<CortanaEffectBlock> CortanaEffects;
            public List<ImportScriptBlock> ImportScripts;

            //Used in C100 - ODST intro sequence
            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public List<UserInputConstraintsBlock> UserInputConstraints;

            public byte[] ImportScript1;
            public int FrameCount;
            public List<CameraFrame> CameraFrames;

            [TagStructure(Size = 0x18, MaxVersion = CacheVersion.Halo3Retail)]
            [TagStructure(Size = 0x1C, MinVersion = CacheVersion.Halo3ODST)]
            public class LightingBlock : TagStructure
			{
                [TagField(MinVersion = CacheVersion.Halo3ODST)]
                public uint Unknown;

                [TagField(Flags = Label)]
                public CachedTag CinematicLight;
                public int OwnerPuppetIndex;
                public StringId Marker;
            }

            [TagStructure(Size = 0x14)]
            public class UserInputConstraintsBlock : TagStructure
            {
                public short Unknown1;
                public short Unknown2;
                public int Ticks;
                public Rectangle2d ConstraintBoundaries;
                public float Unknown3; // friction?
            }

            [TagStructure(Size = 0x2C)]
            public class ClipBlock : TagStructure
			{
                public RealPoint3d PlaneCenter;
                public RealPoint3d PlaneDirection;
                public uint FrameStart;
                public uint FrameEnd;

                public List<ClipPuppetObject> PuppetObjects;

                [TagStructure(Size = 0x4)]
                public class ClipPuppetObject : TagStructure
				{
                    public uint PuppetIndex;
                }
            }

            [TagStructure(Size = 0x24)]
            public class SoundBlock : TagStructure
			{
                [TagField(Flags = Label)]
                public CachedTag Sound;
                public int Frame;
                public float Unknown1;
                public StringId Unknown2;
                public uint Unknown3;
                public StringId Unknown4;
            }

            [TagStructure(Size = 0x18)]
            public class BackgroundSoundBlock : TagStructure
			{
                public uint Unknown1;
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
                public int OwnerPuppetIndex;
            }

            [TagStructure(Size = 0x14)]
            public class FunctionBlock : TagStructure
			{
                public int OwnerPuppetIndex;
                [TagField(Flags = Label)]
                public StringId TargetFunctionName;
                public List<KeyFrame> KeyFrames;

                [TagStructure(Size = 0x10)]
                public class KeyFrame : TagStructure
				{
                    public uint Flags;
                    public int FrameIndex;
                    public float Value;
                    public float InterpolationTime;
                }
            }

            [TagStructure(Size = 0x18)]
            public class ScreenEffectBlock : TagStructure
			{
                [TagField(Flags = Label)]
                public CachedTag Effect;
                public int StartFrame;
                public int EndFrame;
            }

            [TagStructure(Size = 0x14)]
            public class CortanaEffectBlock : TagStructure
			{
                [TagField(Flags = Label)]
                public CachedTag Effect;
                public uint Unknown;
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
            public StringId Unknown;
            public List<CameraShotBlock> Shots;

            [TagStructure(Size = 0xC)]
            public class CameraShotBlock : TagStructure
			{
                public List<FrameBlock> Frames;

                [TagStructure(Size = 0x48)]
                public class FrameBlock : TagStructure
				{
                    public uint UnknownIndex;
                    public CameraFrame CameraFrame;
                }
            }
        }

        [TagStructure(Size = 0x44)]
        public class CameraFrame : TagStructure
        {
            public RealPoint3d Position;
            public RealVector3d Forward;
            public RealVector3d Up;

            public float Unknown7;
            public float Unknown8;
            public float FOV;

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