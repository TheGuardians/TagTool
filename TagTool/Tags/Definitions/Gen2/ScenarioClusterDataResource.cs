using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "scenario_cluster_data_resource", Tag = "clu*", Size = 0x3C)]
    public class ScenarioClusterDataResource : TagStructure
    {
        public List<ScenarioClusterData> ClusterData;
        public List<StructureBackgroundSoundPaletteEntry> BackgroundSoundPalette;
        public List<StructureSoundEnvironmentPaletteEntry> SoundEnvironmentPalette;
        public List<StructureWeatherPaletteEntry> WeatherPalette;
        public List<ScenarioAtmosphericFogPaletteEntry> AtmosphericFogPalette;
        
        [TagStructure(Size = 0x50)]
        public class ScenarioClusterData : TagStructure
        {
            public CachedTag Bsp;
            public List<ScenarioClusterProperty> BackgroundSounds;
            public List<ScenarioClusterProperty> SoundEnvironments;
            public int BspChecksum;
            public List<RealPoint3d> ClusterCentroids;
            public List<ScenarioClusterProperty> WeatherProperties;
            public List<ScenarioClusterProperty> AtmosphericFogProperties;
            
            [TagStructure(Size = 0x4)]
            public class ScenarioClusterProperty : TagStructure
            {
                public short Type;
                [TagField(Flags = Padding, Length = 2)]
                public byte[] Padding1;
            }
            
            [TagStructure(Size = 0xC)]
            public class RealPoint3d : TagStructure
            {
                public RealPoint3d Centroid;
            }
        }
        
        [TagStructure(Size = 0x74)]
        public class StructureBackgroundSoundPaletteEntry : TagStructure
        {
            [TagField(Length = 32)]
            public string Name;
            public CachedTag BackgroundSound;
            public CachedTag InsideClusterSound; // Play only when player is inside cluster.
            [TagField(Flags = Padding, Length = 20)]
            public byte[] Padding1;
            public float CutoffDistance;
            public ScaleFlagsValue ScaleFlags;
            public float InteriorScale;
            public float PortalScale;
            public float ExteriorScale;
            public float InterpolationSpeed; // 1/sec
            [TagField(Flags = Padding, Length = 8)]
            public byte[] Padding2;
            
            [Flags]
            public enum ScaleFlagsValue : uint
            {
                OverrideDefaultScale = 1 << 0,
                UseAdjacentClusterAsPortalScale = 1 << 1,
                UseAdjacentClusterAsExteriorScale = 1 << 2,
                ScaleWithWeatherIntensity = 1 << 3
            }
        }
        
        [TagStructure(Size = 0x50)]
        public class StructureSoundEnvironmentPaletteEntry : TagStructure
        {
            [TagField(Length = 32)]
            public string Name;
            public CachedTag SoundEnvironment;
            public float CutoffDistance;
            public float InterpolationSpeed; // 1/sec
            [TagField(Flags = Padding, Length = 24)]
            public byte[] Padding1;
        }
        
        [TagStructure(Size = 0x98)]
        public class StructureWeatherPaletteEntry : TagStructure
        {
            [TagField(Length = 32)]
            public string Name;
            public CachedTag WeatherSystem;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding1;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding2;
            [TagField(Flags = Padding, Length = 32)]
            public byte[] Padding3;
            public CachedTag Wind;
            public RealVector3d WindDirection;
            public float WindMagnitude;
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Padding4;
            [TagField(Length = 32)]
            public string WindScaleFunction;
        }
        
        [TagStructure(Size = 0x100)]
        public class ScenarioAtmosphericFogPaletteEntry : TagStructure
        {
            public StringId Name;
            /// <summary>
            /// ATMOSPHERIC FOG
            /// </summary>
            public RealRgbColor Color;
            public float SpreadDistance; // World Units
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Padding1;
            public float MaximumDensity; // [0,1]
            public float StartDistance; // World Units
            public float OpaqueDistance; // World Units
            /// <summary>
            /// SECONDARY FOG
            /// </summary>
            public RealRgbColor Color1;
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Padding2;
            public float MaximumDensity2; // [0,1]
            public float StartDistance3; // World Units
            public float OpaqueDistance4; // World Units
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Padding3;
            /// <summary>
            /// PLANAR FOG OVERRIDE
            /// </summary>
            /// <remarks>
            /// Planar fog, if present, is interpolated toward these values.
            /// </remarks>
            public RealRgbColor PlanarColor;
            public float PlanarMaxDensity; // [0,1]
            public float PlanarOverrideAmount; // [0,1]
            public float PlanarMinDistanceBias; // World Units
            [TagField(Flags = Padding, Length = 44)]
            public byte[] Padding4;
            /// <summary>
            /// PATCHY FOG
            /// </summary>
            public RealRgbColor PatchyColor;
            [TagField(Flags = Padding, Length = 12)]
            public byte[] Padding5;
            public Bounds<float> PatchyDensity; // [0,1]
            public Bounds<float> PatchyDistance; // World Units
            [TagField(Flags = Padding, Length = 32)]
            public byte[] Padding6;
            public CachedTag PatchyFog;
            public List<ScenarioAtmosphericFogMixer> Mixers;
            /// <summary>
            /// BLOOM OVERRIDE
            /// </summary>
            public float Amount; // [0,1]
            public float Threshold; // [0,1]
            public float Brightness; // [0,1]
            public float GammaPower;
            /// <summary>
            /// CAMERA IMMERSION OVERRIDE
            /// </summary>
            public CameraImmersionFlagsValue CameraImmersionFlags;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding7;
            
            [TagStructure(Size = 0x10)]
            public class ScenarioAtmosphericFogMixer : TagStructure
            {
                [TagField(Flags = Padding, Length = 4)]
                public byte[] Padding1;
                public StringId AtmosphericFogSource; // From Scenario Atmospheric Fog Palette
                public StringId Interpolator; // From Scenario Interpolators
                [TagField(Flags = Padding, Length = 2)]
                public byte[] Unknown1;
                [TagField(Flags = Padding, Length = 2)]
                public byte[] Unknown2;
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

