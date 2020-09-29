using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "weather_system", Tag = "weat", Size = 0xBC)]
    public class WeatherSystem : TagStructure
    {
        public List<ParticleSystemLite> ParticleSystem;
        public List<AnimatedBackgroundPlate> BackgroundPlates;
        public WindModelStruct WindModel;
        public float FadeRadius;
        
        [TagStructure(Size = 0x9C)]
        public class ParticleSystemLite : TagStructure
        {
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
            public GeometryBlockInfoStruct GeometryBlockInfo;
            public List<ParticleSystemDataBlock> ParticleSystemData;
            public TypeValue Type;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding1;
            public float MininumOpacity;
            public float MaxinumOpacity;
            public float RainStreakScale;
            public float RainLineWidth;
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Padding2;
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Padding3;
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Padding4;
            
            [TagStructure(Size = 0x28)]
            public class GeometryBlockInfoStruct : TagStructure
            {
                /// <summary>
                /// BLOCK INFO
                /// </summary>
                public int BlockOffset;
                public int BlockSize;
                public int SectionDataSize;
                public int ResourceDataSize;
                public List<GeometryBlockResource> Resources;
                [TagField(Flags = Padding, Length = 4)]
                public byte[] Padding1;
                public short OwnerTagSectionOffset;
                [TagField(Flags = Padding, Length = 2)]
                public byte[] Padding2;
                [TagField(Flags = Padding, Length = 4)]
                public byte[] Padding3;
                
                [TagStructure(Size = 0x10)]
                public class GeometryBlockResource : TagStructure
                {
                    public TypeValue Type;
                    [TagField(Flags = Padding, Length = 3)]
                    public byte[] Padding1;
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
            
            [TagStructure(Size = 0x38)]
            public class ParticleSystemDataBlock : TagStructure
            {
                public List<ParticleLiteRender> ParticlesRenderData;
                public List<ParticleLiteData> ParticlesOtherData;
                [TagField(Flags = Padding, Length = 32)]
                public byte[] Padding1;
                
                [TagStructure(Size = 0x14)]
                public class ParticleLiteRender : TagStructure
                {
                    public float PositionX;
                    public float PositionY;
                    public float PositionZ;
                    public float Size;
                    public ArgbColor Color;
                }
                
                [TagStructure(Size = 0x20)]
                public class ParticleLiteData : TagStructure
                {
                    public float VelocityX;
                    public float VelocityY;
                    public float VelocityZ;
                    [TagField(Flags = Padding, Length = 12)]
                    public byte[] Padding1;
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
        
        [TagStructure(Size = 0x3C0)]
        public class AnimatedBackgroundPlate : TagStructure
        {
            public CachedTag Texture0;
            public CachedTag Texture1;
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
            [TagField(Flags = Padding, Length = 736)]
            public byte[] Padding1;
            
            [Flags]
            public enum FlagsValue : uint
            {
                ForwardMotion = 1 << 0,
                AutoPositionPlanes = 1 << 1,
                AutoScalePlanesautoUpdateSpeed = 1 << 2
            }
        }
        
        [TagStructure(Size = 0xA0)]
        public class WindModelStruct : TagStructure
        {
            public float WindTilingScale;
            public RealVector3d WindPrimaryHeadingPitchStrength;
            public float PrimaryRateOfChange;
            public float PrimaryMinStrength;
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Padding1;
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Padding2;
            [TagField(Flags = Padding, Length = 12)]
            public byte[] Padding3;
            public RealVector3d WindGustingHeadingPitchStrength;
            public float GustDiretionalRateOfChange;
            public float GustStrengthRateOfChange;
            public float GustConeAngle;
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Padding4;
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Padding5;
            [TagField(Flags = Padding, Length = 12)]
            public byte[] Padding6;
            [TagField(Flags = Padding, Length = 12)]
            public byte[] Padding7;
            [TagField(Flags = Padding, Length = 12)]
            public byte[] Padding8;
            [TagField(Flags = Padding, Length = 12)]
            public byte[] Padding9;
            public float TurbulanceRateOfChange;
            public RealVector3d TurbulenceScaleXYZ;
            public float GravityConstant;
            public List<WindPrimitive> WindPirmitives;
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Padding10;
            
            [TagStructure(Size = 0x18)]
            public class WindPrimitive : TagStructure
            {
                public RealVector3d Position;
                public float Radius;
                public float Strength;
                public WindPrimitiveTypeValue WindPrimitiveType;
                [TagField(Flags = Padding, Length = 2)]
                public byte[] Padding1;
                
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

