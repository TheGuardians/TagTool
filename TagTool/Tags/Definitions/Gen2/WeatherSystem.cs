using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "weather_system", Tag = "weat", Size = 0xB0)]
    public class WeatherSystem : TagStructure
    {
        public List<GlobalParticleSystemLiteBlock> ParticleSystem;
        public List<GlobalWeatherBackgroundPlateBlock> BackgroundPlates;
        public GlobalWindModelStructBlock WindModel;
        public float FadeRadius;
        
        [TagStructure(Size = 0x8C)]
        public class GlobalParticleSystemLiteBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "bitm" })]
            public CachedTag Sprites;
            public float ViewBoxWidth;
            public float ViewBoxHeight;
            public float ViewBoxDepth;
            public float ExclusionRadius;
            public float MaxVelocity;
            public float MinMass;
            public float MaxMass;
            public float MinSize;
            public float MaxSize;
            public int MaximumNumberOfParticles;
            public RealVector3d InitialVelocity;
            public float BitmapAnimationSpeed;
            public GlobalGeometryBlockInfoStructBlock GeometryBlockInfo;
            public List<ParticleSystemLiteDataBlock> ParticleSystemData;
            public TypeValue Type;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public float MininumOpacity;
            public float MaxinumOpacity;
            public float RainStreakScale;
            public float RainLineWidth;
            [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
            public byte[] Padding1;
            [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
            public byte[] Padding2;
            [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
            public byte[] Padding3;
            
            [TagStructure(Size = 0x24)]
            public class GlobalGeometryBlockInfoStructBlock : TagStructure
            {
                public int BlockOffset;
                public int BlockSize;
                public int SectionDataSize;
                public int ResourceDataSize;
                public List<GlobalGeometryBlockResourceBlock> Resources;
                [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public short OwnerTagSectionOffset;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding1;
                [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
                public byte[] Padding2;
                
                [TagStructure(Size = 0x10)]
                public class GlobalGeometryBlockResourceBlock : TagStructure
                {
                    public TypeValue Type;
                    [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    public short PrimaryLocator;
                    public short SecondaryLocator;
                    public int ResourceDataSize;
                    public int ResourceDataOffset;
                    
                    public enum TypeValue : sbyte
                    {
                        TagBlock,
                        TagData,
                        VertexBuffer
                    }
                }
            }
            
            [TagStructure(Size = 0x30)]
            public class ParticleSystemLiteDataBlock : TagStructure
            {
                public List<ParticlesRenderDataBlock> ParticlesRenderData;
                public List<ParticlesUpdateDataBlock> ParticlesOtherData;
                [TagField(Length = 0x20, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                
                [TagStructure(Size = 0x14)]
                public class ParticlesRenderDataBlock : TagStructure
                {
                    public float PositionX;
                    public float PositionY;
                    public float PositionZ;
                    public float Size;
                    public ArgbColor Color;
                }
                
                [TagStructure(Size = 0x20)]
                public class ParticlesUpdateDataBlock : TagStructure
                {
                    public float VelocityX;
                    public float VelocityY;
                    public float VelocityZ;
                    [TagField(Length = 0xC, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    public float Mass;
                    public float CreationTimeStamp;
                }
            }
            
            public enum TypeValue : short
            {
                Generic,
                Snow,
                Rain,
                RainSplash,
                Bugs,
                SandStorm,
                Debris,
                Bubbles
            }
        }
        
        [TagStructure(Size = 0x3A8)]
        public class GlobalWeatherBackgroundPlateBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "bitm" })]
            public CachedTag Texture0;
            [TagField(ValidTags = new [] { "bitm" })]
            public CachedTag Texture1;
            [TagField(ValidTags = new [] { "bitm" })]
            public CachedTag Texture2;
            public float PlatePositions0;
            public float PlatePositions1;
            public float PlatePositions2;
            public RealVector3d MoveSpeed0;
            public RealVector3d MoveSpeed1;
            public RealVector3d MoveSpeed2;
            public float TextureScale0;
            public float TextureScale1;
            public float TextureScale2;
            public RealVector3d Jitter0;
            public RealVector3d Jitter1;
            public RealVector3d Jitter2;
            public float PlateZNear;
            public float PlateZFar;
            public float DepthBlendZNear;
            public float DepthBlendZFar;
            public float Opacity0;
            public float Opacity1;
            public float Opacity2;
            public FlagsValue Flags;
            public RealRgbColor TintColor0;
            public RealRgbColor TintColor1;
            public RealRgbColor TintColor2;
            public float Mass1;
            public float Mass2;
            public float Mass3;
            [TagField(Length = 0x2E0, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            
            [Flags]
            public enum FlagsValue : uint
            {
                ForwardMotion = 1 << 0,
                AutoPositionPlanes = 1 << 1,
                AutoScalePlanesautoUpdateSpeed = 1 << 2
            }
        }
        
        [TagStructure(Size = 0x9C)]
        public class GlobalWindModelStructBlock : TagStructure
        {
            public float WindTilingScale;
            public RealVector3d WindPrimaryHeadingPitchStrength;
            public float PrimaryRateOfChange;
            public float PrimaryMinStrength;
            [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
            public byte[] Padding1;
            [TagField(Length = 0xC, Flags = TagFieldFlags.Padding)]
            public byte[] Padding2;
            public RealVector3d WindGustingHeadingPitchStrength;
            public float GustDiretionalRateOfChange;
            public float GustStrengthRateOfChange;
            public float GustConeAngle;
            [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
            public byte[] Padding3;
            [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
            public byte[] Padding4;
            [TagField(Length = 0xC, Flags = TagFieldFlags.Padding)]
            public byte[] Padding5;
            [TagField(Length = 0xC, Flags = TagFieldFlags.Padding)]
            public byte[] Padding6;
            [TagField(Length = 0xC, Flags = TagFieldFlags.Padding)]
            public byte[] Padding7;
            [TagField(Length = 0xC, Flags = TagFieldFlags.Padding)]
            public byte[] Padding8;
            public float TurbulanceRateOfChange;
            public RealVector3d TurbulenceScaleXYZ;
            public float GravityConstant;
            public List<GloalWindPrimitivesBlock> WindPirmitives;
            [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
            public byte[] Padding9;
            
            [TagStructure(Size = 0x18)]
            public class GloalWindPrimitivesBlock : TagStructure
            {
                public RealVector3d Position;
                public float Radius;
                public float Strength;
                public WindPrimitiveTypeValue WindPrimitiveType;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                
                public enum WindPrimitiveTypeValue : short
                {
                    Vortex,
                    Gust,
                    Implosion,
                    Explosion
                }
            }
        }
    }
}

