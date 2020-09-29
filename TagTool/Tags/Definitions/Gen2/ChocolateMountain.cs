using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "chocolate_mountain", Tag = "gldf", Size = 0xC)]
    public class ChocolateMountain : TagStructure
    {
        public List<LightingVariablesBlock> LightingVariables;
        
        [TagStructure(Size = 0xA0)]
        public class LightingVariablesBlock : TagStructure
        {
            public ObjectAffectedValue ObjectAffected;
            /// <summary>
            /// Global lightmap sample
            /// </summary>
            public float LightmapBrightnessOffset;
            public PrimaryLightStruct PrimaryLight;
            public SecondaryLightStruct SecondaryLight;
            public AmbientLightStruct AmbientLight;
            public LightmapShadowsStruct LightmapShadows;
            
            [Flags]
            public enum ObjectAffectedValue : uint
            {
                All = 1 << 0,
                Biped = 1 << 1,
                Vehicle = 1 << 2,
                Weapon = 1 << 3,
                Equipment = 1 << 4,
                Garbage = 1 << 5,
                Projectile = 1 << 6,
                Scenery = 1 << 7,
                Machine = 1 << 8,
                Control = 1 << 9,
                LightFixture = 1 << 10,
                SoundScenery = 1 << 11,
                Crate = 1 << 12,
                Creature = 1 << 13
            }
            
            [TagStructure(Size = 0x28)]
            public class PrimaryLightStruct : TagStructure
            {
                /// <summary>
                /// Primary light
                /// </summary>
                public RealRgbColor MinLightmapColor;
                public RealRgbColor MaxLightmapColor;
                public float ExclusionAngleFromUp; // degrees from up the direct light cannot be
                /// <summary>
                /// Primary light function
                /// </summary>
                /// <remarks>
                /// input: accuracy, output: primary light scale
                /// </remarks>
                public FunctionDefinition Function;
                
                [TagStructure(Size = 0xC)]
                public class FunctionDefinition : TagStructure
                {
                    public List<Byte> Data;
                    
                    [TagStructure(Size = 0x1)]
                    public class Byte : TagStructure
                    {
                        public sbyte Value;
                    }
                }
            }
            
            [TagStructure(Size = 0x40)]
            public class SecondaryLightStruct : TagStructure
            {
                /// <summary>
                /// Secondary light
                /// </summary>
                public RealRgbColor MinLightmapColor;
                public RealRgbColor MaxLightmapColor;
                public RealRgbColor MinDiffuseSample;
                public RealRgbColor MaxDiffuseSample;
                public float ZAxisRotation; // degrees
                /// <summary>
                /// Secondary light function
                /// </summary>
                /// <remarks>
                /// input: accuracy, output: secondary light scale
                /// </remarks>
                public FunctionDefinition Function;
                
                [TagStructure(Size = 0xC)]
                public class FunctionDefinition : TagStructure
                {
                    public List<Byte> Data;
                    
                    [TagStructure(Size = 0x1)]
                    public class Byte : TagStructure
                    {
                        public sbyte Value;
                    }
                }
            }
            
            [TagStructure(Size = 0x24)]
            public class AmbientLightStruct : TagStructure
            {
                /// <summary>
                /// Ambient light
                /// </summary>
                public RealRgbColor MinLightmapSample;
                public RealRgbColor MaxLightmapSample;
                /// <summary>
                /// Ambient light function
                /// </summary>
                /// <remarks>
                /// Ambient light scale. (left side min brightness, right side max brightness). Before this scale it determines a global ambient scale, which added to either light will total ~1.0 scale. Then this scale modifies that.
                /// </remarks>
                public FunctionDefinition Function;
                
                [TagStructure(Size = 0xC)]
                public class FunctionDefinition : TagStructure
                {
                    public List<Byte> Data;
                    
                    [TagStructure(Size = 0x1)]
                    public class Byte : TagStructure
                    {
                        public sbyte Value;
                    }
                }
            }
            
            [TagStructure(Size = 0xC)]
            public class LightmapShadowsStruct : TagStructure
            {
                /// <summary>
                /// Lightmap shadows
                /// </summary>
                /// <remarks>
                /// Shadows generated by the lightmaps get direction from lightmap primary incoming light direction and darken based on how accurate that light is fed into the function below
                /// </remarks>
                public FunctionDefinition Function1;
                
                [TagStructure(Size = 0xC)]
                public class FunctionDefinition : TagStructure
                {
                    public List<Byte> Data;
                    
                    [TagStructure(Size = 0x1)]
                    public class Byte : TagStructure
                    {
                        public sbyte Value;
                    }
                }
            }
        }
    }
}

