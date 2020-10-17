using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "scenario_cluster_data_resource", Tag = "clu*", Size = 0x28)]
    public class ScenarioClusterDataResource : TagStructure
    {
        public List<ScenarioClusterDataBlock> ClusterData;
        public List<StructureBspBackgroundSoundPaletteBlock> BackgroundSoundPalette;
        public List<StructureBspSoundEnvironmentPaletteBlock> SoundEnvironmentPalette;
        public List<StructureBspWeatherPaletteBlock> WeatherPalette;
        public List<ScenarioAtmosphericFogPalette> AtmosphericFogPalette;
        
        [TagStructure(Size = 0x34)]
        public class ScenarioClusterDataBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "sbsp" })]
            public CachedTag Bsp;
            public List<ScenarioClusterBackgroundSoundsBlock> BackgroundSounds;
            public List<ScenarioClusterSoundEnvironmentsBlock> SoundEnvironments;
            public int BspChecksum;
            public List<ScenarioClusterPointsBlock> ClusterCentroids;
            public List<ScenarioClusterWeatherPropertiesBlock> WeatherProperties;
            public List<ScenarioClusterAtmosphericFogPropertiesBlock> AtmosphericFogProperties;
            
            [TagStructure(Size = 0x4)]
            public class ScenarioClusterBackgroundSoundsBlock : TagStructure
            {
                public short Type;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
            }
            
            [TagStructure(Size = 0x4)]
            public class ScenarioClusterSoundEnvironmentsBlock : TagStructure
            {
                public short Type;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
            }
            
            [TagStructure(Size = 0xC)]
            public class ScenarioClusterPointsBlock : TagStructure
            {
                public RealPoint3d Centroid;
            }
            
            [TagStructure(Size = 0x4)]
            public class ScenarioClusterWeatherPropertiesBlock : TagStructure
            {
                public short Type;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
            }
            
            [TagStructure(Size = 0x4)]
            public class ScenarioClusterAtmosphericFogPropertiesBlock : TagStructure
            {
                public short Type;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
            }
        }
        
        [TagStructure(Size = 0x64)]
        public class StructureBspBackgroundSoundPaletteBlock : TagStructure
        {
            [TagField(Length = 32)]
            public string Name;
            [TagField(ValidTags = new [] { "lsnd" })]
            public CachedTag BackgroundSound;
            /// <summary>
            /// Play only when player is inside cluster.
            /// </summary>
            [TagField(ValidTags = new [] { "lsnd" })]
            public CachedTag InsideClusterSound;
            [TagField(Length = 0x14, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public float CutoffDistance;
            public ScaleFlagsValue ScaleFlags;
            public float InteriorScale;
            public float PortalScale;
            public float ExteriorScale;
            public float InterpolationSpeed; // 1/sec
            [TagField(Length = 0x8, Flags = TagFieldFlags.Padding)]
            public byte[] Padding1;
            
            [Flags]
            public enum ScaleFlagsValue : uint
            {
                OverrideDefaultScale = 1 << 0,
                UseAdjacentClusterAsPortalScale = 1 << 1,
                UseAdjacentClusterAsExteriorScale = 1 << 2,
                ScaleWithWeatherIntensity = 1 << 3
            }
        }
        
        [TagStructure(Size = 0x48)]
        public class StructureBspSoundEnvironmentPaletteBlock : TagStructure
        {
            [TagField(Length = 32)]
            public string Name;
            [TagField(ValidTags = new [] { "snde" })]
            public CachedTag SoundEnvironment;
            public float CutoffDistance;
            public float InterpolationSpeed; // 1/sec
            [TagField(Length = 0x18, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
        }
        
        [TagStructure(Size = 0x88)]
        public class StructureBspWeatherPaletteBlock : TagStructure
        {
            [TagField(Length = 32)]
            public string Name;
            [TagField(ValidTags = new [] { "weat" })]
            public CachedTag WeatherSystem;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding1;
            [TagField(Length = 0x20, Flags = TagFieldFlags.Padding)]
            public byte[] Padding2;
            [TagField(ValidTags = new [] { "wind" })]
            public CachedTag Wind;
            public RealVector3d WindDirection;
            public float WindMagnitude;
            [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
            public byte[] Padding3;
            [TagField(Length = 32)]
            public string WindScaleFunction;
        }
        
        [TagStructure(Size = 0xF4)]
        public class ScenarioAtmosphericFogPalette : TagStructure
        {
            public StringId Name;
            public RealRgbColor Color;
            /// <summary>
            /// How far fog spreads into adjacent clusters: 0 defaults to 1.
            /// </summary>
            public float SpreadDistance; // World Units
            [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            /// <summary>
            /// Fog density clamps to this value.
            /// </summary>
            public float MaximumDensity; // [0,1]
            /// <summary>
            /// Before this distance, there is no fog.
            /// </summary>
            public float StartDistance; // World Units
            /// <summary>
            /// Fog becomes opaque (maximum density) at this distance from viewer.
            /// </summary>
            public float OpaqueDistance; // World Units
            public RealRgbColor Color1;
            [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
            public byte[] Padding1;
            /// <summary>
            /// Fog density clamps to this value.
            /// </summary>
            public float MaximumDensity1; // [0,1]
            /// <summary>
            /// Before this distance, there is no fog.
            /// </summary>
            public float StartDistance1; // World Units
            /// <summary>
            /// Fog becomes opaque (maximum density) at this distance from viewer.
            /// </summary>
            public float OpaqueDistance1; // World Units
            [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
            public byte[] Padding2;
            /// <summary>
            /// Planar fog, if present, is interpolated toward these values.
            /// </summary>
            public RealRgbColor PlanarColor;
            public float PlanarMaxDensity; // [0,1]
            public float PlanarOverrideAmount; // [0,1]
            /// <summary>
            /// Don't ask.
            /// </summary>
            public float PlanarMinDistanceBias; // World Units
            [TagField(Length = 0x2C, Flags = TagFieldFlags.Padding)]
            public byte[] Padding3;
            public RealRgbColor PatchyColor;
            [TagField(Length = 0xC, Flags = TagFieldFlags.Padding)]
            public byte[] Padding4;
            public Bounds<float> PatchyDensity; // [0,1]
            public Bounds<float> PatchyDistance; // World Units
            [TagField(Length = 0x20, Flags = TagFieldFlags.Padding)]
            public byte[] Padding5;
            [TagField(ValidTags = new [] { "fpch" })]
            public CachedTag PatchyFog;
            public List<ScenarioAtmosphericFogMixerBlock> Mixers;
            public float Amount; // [0,1]
            public float Threshold; // [0,1]
            public float Brightness; // [0,1]
            public float GammaPower;
            public CameraImmersionFlagsValue CameraImmersionFlags;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding6;
            
            [TagStructure(Size = 0x10)]
            public class ScenarioAtmosphericFogMixerBlock : TagStructure
            {
                [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public StringId AtmosphericFogSource; // From Scenario Atmospheric Fog Palette
                public StringId Interpolator; // From Scenario Interpolators
                [TagField(Length = 0x2)]
                public byte[] Unknown;
                [TagField(Length = 0x2)]
                public byte[] Unknown1;
            }
            
            [Flags]
            public enum CameraImmersionFlagsValue : ushort
            {
                DisableAtmosphericFog = 1 << 0,
                DisableSecondaryFog = 1 << 1,
                DisablePlanarFog = 1 << 2,
                InvertPlanarFogPriorities = 1 << 3,
                DisableWater = 1 << 4
            }
        }
    }
}

