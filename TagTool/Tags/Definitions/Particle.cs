using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;
using static TagTool.Tags.Definitions.Effect.Event.ParticleSystem.Emitter;
using static TagTool.Effects.EditableProperty;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "particle", Tag = "prt3", Size = 0x194, MaxVersion = CacheVersion.HaloOnline700123)]
    [TagStructure(Name = "particle", Tag = "prt3", Size = 0x1F0, MinVersion = CacheVersion.HaloReach)]
    public class Particle : TagStructure
	{
        [TagField(MaxVersion = CacheVersion.Halo3Retail)]
        public FlagsValueH3 FlagsH3;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public FlagsValue Flags;

        public List<Attachment> Attachments;

        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public AppearanceFlagsValue AppearanceFlags;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public AppearanceFlagsValueReach AppearanceFlagsReach;

        public ParticleBillboardStyleValue ParticleBillboardStyle;

        public short FirstSequenceIndex;
        public short SequenceCount;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public byte Version;

        [TagField(Length = 2, Flags = Padding, MaxVersion = CacheVersion.HaloOnline700123)]
        [TagField(Length = 1, Flags = Padding, MinVersion = CacheVersion.HaloReach)]
        public byte[] Padding2;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public float LowResolutionSwitchDistance;

        public RealPoint2d CenterOffset;
        public float Curvature; // 0=flat, 1=hemisphere
        public float AngleFadeRange;
        public float AngleFadeCutoff;
        public float MotionBlurTranslationScale = 1.0f;
        public float MotionBlurRotationScale = 1.0f;
        public float MotionBlurAspectScale = 0.5f;
        public RenderMethod RenderMethod;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public RealRgbColor BrightTint;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public RealRgbColor AmbientTint;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public float Contrast;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public float BlurWeight;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public float IntensityScale;
        [TagField(ValidTags = new[] { "bitm" }, MinVersion = CacheVersion.HaloReach)]
        public CachedTag Palette;

        public ParticlePropertyScalar AspectRatio;
        public ParticlePropertyScalar Color;
        public ParticlePropertyScalar Intensity;
        public ParticlePropertyScalar Alpha;
        public AnimationFlagsValue AnimationFlags;
        public ParticlePropertyScalar FrameIndex;
        public ParticlePropertyScalar AnimationRate;
        public ParticlePropertyScalar PaletteAnimation;

        [TagField(ValidTags = new[] { "pmdf" })]
        public CachedTag ParticleModel;

        public ParticlePropertyScalar.ParticleStatesFlags RuntimeMUsedParticleStates;
        public int RuntimeMConstantPerParticleProperties;
        public int RuntimeMConstantOverTimeProperties;
        public List<RuntimeMSpritesBlock> RuntimeMSprites;
        public List<RuntimeMFramesBlock> RuntimeMFrames;

        [Flags]
        public enum FlagsValue : int
        {
            None = 0,
            DiesAtRest = 1 << 0,
            DiesOnStructureCollision = 1 << 1,
            DiesInWater = 1 << 2,
            DiesInAir = 1 << 3,
            HasSweetener = 1 << 4,
            UseCheapShader = 1 << 5,
            NoAttachments = 1 << 6,
            HasAttachmentOnBirth = 1 << 7,
            HasAttachmentOnCollision = 1 << 8,
            HasAttachmentOnDeath = 1 << 9,
        }

        [Flags]
        public enum FlagsValueH3 : int
        {
            None = 0,
            DiesAtRest = 1 << 0,
            DiesOnStructureCollision = 1 << 1,
            DiesInWater = 1 << 2,
            DiesInAir = 1 << 3,
            HasSweetener = 1 << 4,
            NoAttachments = 1 << 5,
            HasAttachmentOnBirth = 1 << 6,
            HasAttachmentOnCollision = 1 << 7,
            HasAttachmentOnDeath = 1 << 8,
        }

        [TagStructure(Size = 0x14)]
        public class Attachment : TagStructure
		{
            [TagField(ValidTags = new[] { "effe", "snd!", "foot" }, Flags = Label)]
            public CachedTag Type;
            public TriggerValue Trigger;
            public byte Flags;
            public ParticlePropertyScalar.ParticleStates PrimaryScale;
            public ParticlePropertyScalar.ParticleStates SecondaryScale;

            public enum TriggerValue : sbyte
            {
                Birth,
                Collision,
                Death,
                FirstCollision
            }

            public void Postprocess()
            {
                // todo
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
            Fogged = 1 << 10,
            LightmapLit = 1 << 11,
            DepthFadeActive = 1 << 12,
            DistortionActive = 1 << 13,
            LdrOnly = 1 << 14,
            IsParticleModel = 1 << 15,
            Opaque = 1 << 16,
        }

        [Flags]
        public enum AppearanceFlagsValueReach : int
        {
            None = 0, //
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
            LowRes = 1 << 10,
            LowResTighterMask = 1 << 11,
            Fogged = 1 << 12,
            LightmapLit = 1 << 13,
            DepthFadeActive = 1 << 14,
            DistortionActive = 1 << 15,
            LdrOnly = 1 << 16,
            IsParticleModel = 1 << 17,
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
            CanAnimateBackwards = 1 << 1,
            DisableFrameBlending = 1 << 2,
            Bit3 = 1 << 3
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

        private ParticlePropertyScalar.ParticleStatesFlags ValidateAttachmentState(Attachment attachment)
        {
            ParticlePropertyScalar.ParticleStatesFlags usedStates = 0;
            if (attachment.PrimaryScale != ParticlePropertyScalar.ParticleStates.Velocity)
                usedStates |= (ParticlePropertyScalar.ParticleStatesFlags)(1 << (int)attachment.PrimaryScale);
            if (attachment.SecondaryScale != ParticlePropertyScalar.ParticleStates.Velocity)
                usedStates |= (ParticlePropertyScalar.ParticleStatesFlags)(1 << (int)attachment.SecondaryScale);

            // updates the states. probably not necessary since we are already dealing with postprocessed data
            //attachment.Postprocess();
            //
            //if (attachment.PrimaryScale != ParticlePropertyScalar.ParticleStates.Velocity)
            //    usedStates |= (ParticlePropertyScalar.ParticleStatesFlags)(1 << (int)attachment.PrimaryScale);
            //if (attachment.SecondaryScale != ParticlePropertyScalar.ParticleStates.Velocity)
            //    usedStates |= (ParticlePropertyScalar.ParticleStatesFlags)(1 << (int)attachment.SecondaryScale);

            return usedStates;
        }

        public ParticlePropertyScalar.ParticleStatesFlags ValidateUsedStates()
        {
            ParticlePropertyScalar.ParticleStatesFlags usedStates = ParticlePropertyScalar.ParticleStatesFlags.None;

            usedStates |= ParticleEditablePropertyEvaluate(AspectRatio, "AspectRatio",
                0x7FFFFFF, ParticlePropertyScalar.ParticleStates.Velocity);
            usedStates |= ParticleEditablePropertyEvaluate(Color, "Color",
                0x7FFFFFF, ParticlePropertyScalar.ParticleStates.Velocity);
            usedStates |= ParticleEditablePropertyEvaluate(Intensity, "Intensity",
                0x7FFFFFF, ParticlePropertyScalar.ParticleStates.Velocity);
            usedStates |= ParticleEditablePropertyEvaluate(Alpha, "Alpha",
                0x7FFFFFF, ParticlePropertyScalar.ParticleStates.Velocity);
            usedStates |= ParticleEditablePropertyEvaluate(FrameIndex, "FrameIndex",
                0x7FFFFFF, ParticlePropertyScalar.ParticleStates.Velocity);
            usedStates |= ParticleEditablePropertyEvaluate(AnimationRate, "AnimationRate",
                0x7FFFFFF, ParticlePropertyScalar.ParticleStates.Velocity);

            foreach (var attachment in Attachments)
            {
                usedStates |= ValidateAttachmentState(attachment);
            }

            return usedStates;
        }

        public void GetConstantStates(out int cppStates, out int cotStates)
        {
            cppStates = 0;

            if (this.AspectRatio.IsConstantPerParticle())
                cppStates |= 1;
            if (this.Color.IsConstantPerParticle())
                cppStates |= 2;
            if (this.Intensity.IsConstantPerParticle())
                cppStates |= 4;
            if (this.Alpha.IsConstantPerParticle())
                cppStates |= 8;
            if (this.FrameIndex.IsConstantPerParticle())
                cppStates |= 16;
            if (this.AnimationRate.IsConstantPerParticle())
                cppStates |= 32;
            if (this.PaletteAnimation.IsConstantPerParticle())
                cppStates |= 64;

            cotStates = 0;

            if (this.AspectRatio.IsConstantOverTime())
                cppStates |= 1;
            if (this.Color.IsConstantOverTime())
                cppStates |= 2;
            if (this.Intensity.IsConstantOverTime())
                cppStates |= 4;
            if (this.Alpha.IsConstantOverTime())
                cppStates |= 8;
            if (this.FrameIndex.IsConstantOverTime())
                cppStates |= 16;
            if (this.AnimationRate.IsConstantOverTime())
                cppStates |= 32;
            if (this.PaletteAnimation.IsConstantOverTime())
                cppStates |= 64;
        }
    }
}