using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "liquid", Tag = "tdtl", Size = 0x70)]
    public class Liquid : TagStructure
    {
        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
        public byte[] Padding;
        public TypeValue Type;
        public StringId AttachmentMarkerName;
        [TagField(Length = 0x38, Flags = TagFieldFlags.Padding)]
        public byte[] Padding1;
        public float FalloffDistanceFromCamera; // world units
        public float CutoffDistanceFromCamera; // world units
        [TagField(Length = 0x20, Flags = TagFieldFlags.Padding)]
        public byte[] Padding2;
        public List<LiquidArcBlock> Arcs;
        
        public enum TypeValue : short
        {
            Standard,
            WeaponToProjectile,
            ProjectileFromWeapon
        }
        
        [TagStructure(Size = 0xEC)]
        public class LiquidArcBlock : TagStructure
        {
            /// <summary>
            /// Note that if the type is not STANDARD, then the 'collide_with_stuff' and material effects will not have any effect. In
            /// addition, the 'natural_length' will not have an effect except as a means to compute the collision fraction.
            /// </summary>
            public FlagsValue Flags;
            public SpriteCountValue SpriteCount;
            public float NaturalLength; // world units
            public short Instances;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public Angle InstanceSpreadAngle; // degrees
            public float InstanceRotationPeriod; // seconds
            [TagField(Length = 0x8, Flags = TagFieldFlags.Padding)]
            public byte[] Padding1;
            [TagField(ValidTags = new [] { "foot" })]
            public CachedTag MaterialEffects;
            [TagField(ValidTags = new [] { "bitm" })]
            public CachedTag Bitmap;
            [TagField(Length = 0x8, Flags = TagFieldFlags.Padding)]
            public byte[] Padding2;
            /// <summary>
            /// In world units, how far the noise extends horizontally. By default the horizontal range is along the world XY plane.
            /// </summary>
            public ScalarFunctionStructBlock HorizontalRange;
            /// <summary>
            /// In world units, how far the noise extends vertically. By default the vertical range is along the world Z axis (up).
            /// </summary>
            public ScalarFunctionStructBlock1 VerticalRange;
            public float VerticalNegativeScale; // [0,1]
            /// <summary>
            /// Roughness controls how the different 'octaves' of noise get scaled. Usually it is in the range [0,1] but it can be
            /// slightly higher or lower and still make sense (zero is actually a pretty decent value). The mathematical equation used is
            /// 2^(-k*(1-r)) where 'k' is the octave index starting at 0 and 'r' is the roughness value.
            /// </summary>
            public ScalarFunctionStructBlock2 Roughness;
            [TagField(Length = 0x40, Flags = TagFieldFlags.Padding)]
            public byte[] Padding3;
            /// <summary>
            /// 4 sprites is 3 octaves
            /// 8 sprites is 4 octaves
            /// 16 sprites is 5 octaves
            /// 32 sprites is 6 octaves
            /// 64 sprites is 7 octaves
            /// 128
            /// sprites is 8 octaves
            /// 256 sprites is 9 octaves, etc.
            /// </summary>
            public float Octave1Frequency; // cycles/second
            public float Octave2Frequency; // cycles/second
            public float Octave3Frequency; // cycles/second
            public float Octave4Frequency; // cycles/second
            public float Octave5Frequency; // cycles/second
            public float Octave6Frequency; // cycles/second
            public float Octave7Frequency; // cycles/second
            public float Octave8Frequency; // cycles/second
            public float Octave9Frequency; // cycles/second
            [TagField(Length = 0x1C, Flags = TagFieldFlags.Padding)]
            public byte[] Padding4;
            public OctaveFlagsValue OctaveFlags;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding5;
            public List<LiquidCoreBlock> Cores;
            /// <summary>
            /// Scales range (amplitude) by collision fraction. The input to the function will be 1 if there is no collision, and 0 if
            /// the collision occurs at the origin.
            /// </summary>
            public ScalarFunctionStructBlock3 RangeScale;
            /// <summary>
            /// Scales brightness by collision fraction.
            /// </summary>
            public ScalarFunctionStructBlock4 BrightnessScale;
            
            [Flags]
            public enum FlagsValue : ushort
            {
                BasisMarkerRelative = 1 << 0,
                SpreadByExternalInput = 1 << 1,
                CollideWithStuff = 1 << 2,
                NoPerspectiveMidpoints = 1 << 3
            }
            
            public enum SpriteCountValue : short
            {
                _4Sprites,
                _8Sprites,
                _16Sprites,
                _32Sprites,
                _64Sprites,
                _128Sprites,
                _256Sprites
            }
            
            [TagStructure(Size = 0x8)]
            public class ScalarFunctionStructBlock : TagStructure
            {
                public MappingFunctionBlock Function;
                
                [TagStructure(Size = 0x8)]
                public class MappingFunctionBlock : TagStructure
                {
                    public List<ByteBlock> Data;
                    
                    [TagStructure(Size = 0x1)]
                    public class ByteBlock : TagStructure
                    {
                        public sbyte Value;
                    }
                }
            }
            
            [TagStructure(Size = 0x8)]
            public class ScalarFunctionStructBlock1 : TagStructure
            {
                public MappingFunctionBlock Function;
                
                [TagStructure(Size = 0x8)]
                public class MappingFunctionBlock : TagStructure
                {
                    public List<ByteBlock> Data;
                    
                    [TagStructure(Size = 0x1)]
                    public class ByteBlock : TagStructure
                    {
                        public sbyte Value;
                    }
                }
            }
            
            [TagStructure(Size = 0x8)]
            public class ScalarFunctionStructBlock2 : TagStructure
            {
                public MappingFunctionBlock Function;
                
                [TagStructure(Size = 0x8)]
                public class MappingFunctionBlock : TagStructure
                {
                    public List<ByteBlock> Data;
                    
                    [TagStructure(Size = 0x1)]
                    public class ByteBlock : TagStructure
                    {
                        public sbyte Value;
                    }
                }
            }
            
            [Flags]
            public enum OctaveFlagsValue : ushort
            {
                Octave1 = 1 << 0,
                Octave2 = 1 << 1,
                Octave3 = 1 << 2,
                Octave4 = 1 << 3,
                Octave5 = 1 << 4,
                Octave6 = 1 << 5,
                Octave7 = 1 << 6,
                Octave8 = 1 << 7,
                Octave9 = 1 << 8
            }
            
            [TagStructure(Size = 0x38)]
            public class LiquidCoreBlock : TagStructure
            {
                [TagField(Length = 0xC, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public short BitmapIndex;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding1;
                /// <summary>
                /// In world units.
                /// </summary>
                public ScalarFunctionStructBlock Thickness;
                public ColorFunctionStructBlock Color;
                /// <summary>
                /// Periodic function based on time.
                /// </summary>
                public ScalarFunctionStructBlock1 BrightnessTime;
                /// <summary>
                /// Brightness when facing perpendicular versus parallel.
                /// </summary>
                public ScalarFunctionStructBlock2 BrightnessFacing;
                /// <summary>
                /// Scale along-axis. Default should be 1.
                /// </summary>
                public ScalarFunctionStructBlock3 AlongAxisScale;
                
                [TagStructure(Size = 0x8)]
                public class ScalarFunctionStructBlock : TagStructure
                {
                    public MappingFunctionBlock Function;
                    
                    [TagStructure(Size = 0x8)]
                    public class MappingFunctionBlock : TagStructure
                    {
                        public List<ByteBlock> Data;
                        
                        [TagStructure(Size = 0x1)]
                        public class ByteBlock : TagStructure
                        {
                            public sbyte Value;
                        }
                    }
                }
                
                [TagStructure(Size = 0x8)]
                public class ColorFunctionStructBlock : TagStructure
                {
                    public MappingFunctionBlock Function;
                    
                    [TagStructure(Size = 0x8)]
                    public class MappingFunctionBlock : TagStructure
                    {
                        public List<ByteBlock> Data;
                        
                        [TagStructure(Size = 0x1)]
                        public class ByteBlock : TagStructure
                        {
                            public sbyte Value;
                        }
                    }
                }
                
                [TagStructure(Size = 0x8)]
                public class ScalarFunctionStructBlock1 : TagStructure
                {
                    public MappingFunctionBlock Function;
                    
                    [TagStructure(Size = 0x8)]
                    public class MappingFunctionBlock : TagStructure
                    {
                        public List<ByteBlock> Data;
                        
                        [TagStructure(Size = 0x1)]
                        public class ByteBlock : TagStructure
                        {
                            public sbyte Value;
                        }
                    }
                }
                
                [TagStructure(Size = 0x8)]
                public class ScalarFunctionStructBlock2 : TagStructure
                {
                    public MappingFunctionBlock Function;
                    
                    [TagStructure(Size = 0x8)]
                    public class MappingFunctionBlock : TagStructure
                    {
                        public List<ByteBlock> Data;
                        
                        [TagStructure(Size = 0x1)]
                        public class ByteBlock : TagStructure
                        {
                            public sbyte Value;
                        }
                    }
                }
                
                [TagStructure(Size = 0x8)]
                public class ScalarFunctionStructBlock3 : TagStructure
                {
                    public MappingFunctionBlock Function;
                    
                    [TagStructure(Size = 0x8)]
                    public class MappingFunctionBlock : TagStructure
                    {
                        public List<ByteBlock> Data;
                        
                        [TagStructure(Size = 0x1)]
                        public class ByteBlock : TagStructure
                        {
                            public sbyte Value;
                        }
                    }
                }
            }
            
            [TagStructure(Size = 0x8)]
            public class ScalarFunctionStructBlock3 : TagStructure
            {
                public MappingFunctionBlock Function;
                
                [TagStructure(Size = 0x8)]
                public class MappingFunctionBlock : TagStructure
                {
                    public List<ByteBlock> Data;
                    
                    [TagStructure(Size = 0x1)]
                    public class ByteBlock : TagStructure
                    {
                        public sbyte Value;
                    }
                }
            }
            
            [TagStructure(Size = 0x8)]
            public class ScalarFunctionStructBlock4 : TagStructure
            {
                public MappingFunctionBlock Function;
                
                [TagStructure(Size = 0x8)]
                public class MappingFunctionBlock : TagStructure
                {
                    public List<ByteBlock> Data;
                    
                    [TagStructure(Size = 0x1)]
                    public class ByteBlock : TagStructure
                    {
                        public sbyte Value;
                    }
                }
            }
        }
    }
}

