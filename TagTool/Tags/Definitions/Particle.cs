using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "particle", Tag = "prt3", Size = 0x194, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Name = "particle", Tag = "prt3", Size = 0x1A0, MinVersion = CacheVersion.HaloOnline106708)]
    public class Particle : TagStructure
	{
        public int Flags;
        public List<Attachment> Attachments;
        public AppearanceFlagsValue AppearanceFlags;
        public ParticleBillboardStyleValue ParticleBillboardStyle;
        public short RuntimeMTextureArraySize;
        public short FirstSequenceIndex;
        public short SequenceCount;
        public float LowResolutionSwitchDistance;
        public RealPoint2d CenterOffset;
        public float Curvature;
        public float AngleFadeRange;
        public float MotionBlurTranslationScale;
        public float MotionBlurRotationScale;
        public float MotionBlurAspectScale;
        public RenderMethod RenderMethod;
        public TagMapping AspectRatio;
        public TagMapping Color;
        public TagMapping Intensity;
        public TagMapping Alpha;
        public AnimationFlagsValue AnimationFlags;
        public TagMapping FrameIndex;
        public TagMapping AnimationRate;
        public TagMapping PaletteAnimation;
        public CachedTagInstance ParticleModel;
        public uint RuntimeMUsedParticleStates;
        public uint RuntimeMConstantPerParticleProperties;
        public uint RuntimeMConstantOverTimeProperties;
        public List<RuntimeMSpritesBlock> RuntimeMSprites;
        public List<RuntimeMFramesBlock> RuntimeMFrames;

        [TagField(Flags = Padding, Length = 12, MinVersion = CacheVersion.HaloOnline106708)]
        public byte[] Unused;

        [Flags]
        public enum FlagsValue : int
        {
            None = 0,
            DiesAtRest = 1 << 0,
            DiesOnStructureCollision = 1 << 1,
            DiesInWater = 1 << 2,
            DiesInAir = 1 << 3,
            HasSweetener = 1 << 4,
            UsesCheapShader = 1 << 5
        }

        [TagStructure(Size = 0x14)]
        public class Attachment : TagStructure
		{
            [TagField(Flags = Label)]
            public CachedTagInstance Type;
            public TriggerValue Trigger;
            public byte SkipFraction;
            public TagMapping.VariableTypeValue PrimaryScale;
            public TagMapping.VariableTypeValue SecondaryScale;

            public enum TriggerValue : sbyte
            {
                Birth,
                Collision,
                Death,
                FirstCollision
            }
        }

        [Flags]
        public enum AppearanceFlagsValue : int
        {
            None = 0,
            RandomUMirror = 1 << 0,
            RandomVMirror = 1 << 1,
            RandomStartingRotation = 1 << 2,
            TintFromLightmap = 1 << 3,
            TintFromDiffuseTexture = 1 << 4,
            BitmapAuthoredVertically = 1 << 5,
            IntensityAffectsAlpha = 1 << 6,
            FadeWhenViewedEdgeOn = 1 << 7,
            MotionBlur = 1 << 8,
            DoubleSided = 1 << 9,
            LowResolution = 1 << 10,
            LowResolutionTighterMask = 1 << 11,
            NeverKillVerticesOnGpu = 1 << 12,
            ParticleVelocityRelativeToCamera = 1 << 13,
            RenderWithWater = 1 << 14
        }

        public enum ParticleBillboardStyleValue : short
        {
            ScreenFacing,
            CameraFacing,
            ParallelToDirection,
            PerpendicularToDirection,
            Vertical,
            Horizontal,
            LocalVertical,
            LocalHorizontal,
            WorldParticleModels,
            VelocityHorizontalParticleModels,
            Local
        }

        [Flags]
        public enum AnimationFlagsValue : int
        {
            None = 0,
            FrameAnimationOneShot = 1 << 0,
            CanAnimateBackwards = 1 << 1
        }

        [TagStructure(Size = 0x10)]
        public class RuntimeMSpritesBlock : TagStructure
		{
            [TagField(Length = 4)]
            public float[] RuntimeGpuSpriteArray = new float[4];
        }

        [TagStructure(Size = 0x10)]
        public class RuntimeMFramesBlock : TagStructure
		{
            [TagField(Length = 4)]
            public float[] RuntimeMCount = new float[4];
        }
    }
}