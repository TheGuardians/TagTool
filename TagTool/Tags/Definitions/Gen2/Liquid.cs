using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "liquid", Tag = "tdtl", Size = 0x74)]
    public class Liquid : TagStructure
    {
        /// <summary>
        /// LIQUID
        /// </summary>
        [TagField(Flags = Padding, Length = 2)]
        public byte[] Padding1;
        public TypeValue Type;
        public StringId AttachmentMarkerName;
        [TagField(Flags = Padding, Length = 56)]
        public byte[] Padding2;
        public float FalloffDistanceFromCamera; // world units
        public float CutoffDistanceFromCamera; // world units
        [TagField(Flags = Padding, Length = 32)]
        public byte[] Padding3;
        public List<LiquidArc> Arcs;
        
        public enum TypeValue : short
        {
            Standard,
            WeaponToProjectile,
            ProjectileFromWeapon
        }
        
        [TagStructure(Size = 0x114)]
        public class LiquidArc : TagStructure
        {
            /// <summary>
            /// LIQUID ARC
            /// </summary>
            /// <remarks>
            /// Note that if the type is not STANDARD, then the 'collide_with_stuff' and material effects will not have any effect. In addition, the 'natural_length' will not have an effect except as a means to compute the collision fraction.
            /// </remarks>
            public FlagsValue Flags;
            public SpriteCountValue SpriteCount;
            public float NaturalLength; // world units
            public short Instances;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding1;
            public Angle InstanceSpreadAngle; // degrees
            public float InstanceRotationPeriod; // seconds
            [TagField(Flags = Padding, Length = 8)]
            public byte[] Padding2;
            public CachedTag MaterialEffects;
            public CachedTag Bitmap;
            [TagField(Flags = Padding, Length = 8)]
            public byte[] Padding3;
            /// <summary>
            /// HORIZONTAL RANGE
            /// </summary>
            /// <remarks>
            /// In world units, how far the noise extends horizontally. By default the horizontal range is along the world XY plane.
            /// </remarks>
            public FunctionDefinition HorizontalRange;
            /// <summary>
            /// VERTICAL RANGE
            /// </summary>
            /// <remarks>
            /// In world units, how far the noise extends vertically. By default the vertical range is along the world Z axis (up).
            /// </remarks>
            public FunctionDefinition VerticalRange;
            public float VerticalNegativeScale; // [0,1]
            /// <summary>
            /// ROUGHNESS
            /// </summary>
            /// <remarks>
            /// Roughness controls how the different 'octaves' of noise get scaled. Usually it is in the range [0,1] but it can be slightly higher or lower and still make sense (zero is actually a pretty decent value). The mathematical equation used is 2^(-k*(1-r)) where 'k' is the octave index starting at 0 and 'r' is the roughness value.
            /// </remarks>
            public FunctionDefinition Roughness;
            [TagField(Flags = Padding, Length = 64)]
            public byte[] Padding4;
            /// <summary>
            /// NOISE FREQUENCIES
            /// </summary>
            /// <remarks>
            /// 4 sprites is 3 octaves
            /// 8 sprites is 4 octaves
            /// 16 sprites is 5 octaves
            /// 32 sprites is 6 octaves
            /// 64 sprites is 7 octaves
            /// 128 sprites is 8 octaves
            /// 256 sprites is 9 octaves, etc.
            /// </remarks>
            public float Octave1Frequency; // cycles/second
            public float Octave2Frequency; // cycles/second
            public float Octave3Frequency; // cycles/second
            public float Octave4Frequency; // cycles/second
            public float Octave5Frequency; // cycles/second
            public float Octave6Frequency; // cycles/second
            public float Octave7Frequency; // cycles/second
            public float Octave8Frequency; // cycles/second
            public float Octave9Frequency; // cycles/second
            [TagField(Flags = Padding, Length = 28)]
            public byte[] Padding5;
            public OctaveFlagsValue OctaveFlags;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding6;
            public List<LiquidCore> Cores;
            /// <summary>
            /// RANGE-COLLISION SCALE
            /// </summary>
            /// <remarks>
            /// Scales range (amplitude) by collision fraction. The input to the function will be 1 if there is no collision, and 0 if the collision occurs at the origin.
            /// </remarks>
            public FunctionDefinition RangeScale;
            /// <summary>
            /// BRIGHTNESS-COLLISION SCALE
            /// </summary>
            /// <remarks>
            /// Scales brightness by collision fraction.
            /// </remarks>
            public FunctionDefinition BrightnessScale;
            
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
            
            [TagStructure(Size = 0xC)]
            public class FunctionDefinition : TagStructure
            {
                public FunctionDefinition Function;
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
            
            [TagStructure(Size = 0x4C)]
            public class LiquidCore : TagStructure
            {
                [TagField(Flags = Padding, Length = 12)]
                public byte[] Padding1;
                public short BitmapIndex;
                [TagField(Flags = Padding, Length = 2)]
                public byte[] Padding2;
                /// <summary>
                /// THICKNESS
                /// </summary>
                /// <remarks>
                /// In world units.
                /// </remarks>
                public FunctionDefinition Thickness;
                /// <summary>
                /// COLOR
                /// </summary>
                public FunctionDefinition Color;
                /// <summary>
                /// BRIGHTNESS/TIME
                /// </summary>
                /// <remarks>
                /// Periodic function based on time.
                /// </remarks>
                public FunctionDefinition BrightnessTime;
                /// <summary>
                /// BRIGHTNESS/FACING
                /// </summary>
                /// <remarks>
                /// Brightness when facing perpendicular versus parallel.
                /// </remarks>
                public FunctionDefinition BrightnessFacing;
                /// <summary>
                /// ALONG-AXIS SCALE
                /// </summary>
                /// <remarks>
                /// Scale along-axis. Default should be 1.
                /// </remarks>
                public FunctionDefinition AlongAxisScale;
                
                [TagStructure(Size = 0xC)]
                public class FunctionDefinition : TagStructure
                {
                    public FunctionDefinition Function;
                }
            }
        }
    }
}

