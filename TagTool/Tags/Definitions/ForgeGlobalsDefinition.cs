using System;
using System.Collections.Generic;
using TagTool.Cache;
using TagTool.Common;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "forge_globals_definition", Tag = "forg", Size = 0x104)]
    public class ForgeGlobalsDefinition : TagStructure
    {
        public int version;

        [TagField(ValidTags = new[] { "rm  " })]
        public CachedTag DefaultBoundaryRenderMethod;

        [TagField(ValidTags = new[] { "rm  " })]
        public CachedTag InvisibleRenderMethod;

        [TagField(ValidTags = new[] { "rm  " })]
        public CachedTag DefaultRenderMethod;

        public List<ReForgeMaterial> ReForgeMaterials;
        public List<ReForgeMaterialType> ReForgeMaterialTypes;
        public List<TagReferenceBlock> ReForgeObjects;

        [TagField(ValidTags = new[] { "obje" })]
        public CachedTag PrematchCameraObject;

        [TagField(ValidTags = new[] { "obje" })]
        public CachedTag PostmatchObject;

        [TagField(ValidTags = new[] { "obje" })]
        public CachedTag ModifierObject;

        [TagField(ValidTags = new[] { "obje" })]
        public CachedTag KillVolumeObject;

        [TagField(ValidTags = new[] { "obje" })]
        public CachedTag GarbageVolumeObject;

        public List<Description> Descriptions;
        public List<PaletteCategory> PaletteCategories;
        public List<PaletteItem> Palette;
        public List<WeatherEffect> WeatherEffects;
        public List<Sky> Skies;

        [TagField(ValidTags = new[] { "obje" })]
        public CachedTag FxObject;

        [TagField(ValidTags = new[] { "obje" })]
        public CachedTag FxLight;

        [TagStructure(Size = 0x30)]
        public class ReForgeMaterial : TagStructure
        {
            [TagField(Length = 32)]
            public string Name;

            [TagField(ValidTags = new[] { "rm  " })]
            public CachedTag RenderMethod;
        }

        [TagStructure(Size = 0x24)]
        public class ReForgeMaterialType : TagStructure
        {
            [TagField(Length = 32)]
            public string Name;

            public short CollisionMaterialIndex;
            public short PhysicsMaterialIndex;
        }

        [TagStructure(Size = 0x100)]
        public class Description : TagStructure
        {
            [TagField(Length = 256)]
            public string Text;
        }

        public enum PaletteItemType : ushort
        {
            None,
            Tool,
            Prop,
            Light,
            Effects,
            Structure,
            Equipment,
            Weapon,
            Vehicle,
            Teleporter,
            Game,
            Assault,
            CaptureTheFlag,
            Infection,
            Juggernaut,
            KingOfTheHill,
            Territories,
            Slayer,
            VIP
        }

        [TagStructure(Size = 0x24)]
        public class PaletteCategory : TagStructure
        {
            [TagField(Length = 32)]
            public string Name;

            public short DescriptionIndex;
            public short ParentCategoryIndex;
        }

        [TagStructure(Size = 0x44)]
        public class PaletteItem : TagStructure
        {
            [TagField(Length = 32)]
            public string Name;

            public PaletteItemType Type;
            public short CategoryIndex;
            public short DescriptionIndex;
            public ushort MaxAllowed;

            [TagField(ValidTags = new[] { "obje" })]
            public CachedTag Object;

            public List<Setter> Setters;

            public enum SetterTarget : short
            {
                General_OnMapAtStart,
                General_Symmetry,
                General_RespawnRate,
                General_SpareClips,
                General_SpawnOrder,
                General_Team,
                General_TeleporterChannel,
                General_ShapeType,
                General_ShapeRadius,
                General_ShapeWidth,
                General_ShapeTop,
                General_ShapeBottom,
                General_ShapeDepth,
                General_Physics,
                General_EngineFlags,

                Reforge_Material,
                Reforge_Material_ColorR,
                Reforge_Material_ColorG,
                Reforge_Material_ColorB,
                Reforge_Material_TextureOverride,
                Reforge_Material_TextureScale,
                Reforge_Material_TextureOffsetX,
                Reforge_Material_TextureOffsetY,
                Reforge_MaterialAllowsProjectiles,
                Reforge_MaterialType,

                Light_Type,
                Light_ColorR,
                Light_ColorG,
                Light_ColorB,
                Light_Intensity,
                Light_Radius,
                Light_FieldOfView,
                Light_NearWidth,
                Light_IlluminationType,
                Light_IlluminationBase,
                Light_IlluminationFreq,

                Fx_Range,
                Fx_LightIntensity,
                Fx_Hue,
                Fx_Saturation,
                Fx_Desaturation,
                Fx_GammaIncrease,
                Fx_GammaDecrease,
                Fx_ColorFilterR,
                Fx_ColorFilterG,
                Fx_ColorFilterB,
                Fx_ColorFloorR,
                Fx_ColorFloorG,
                Fx_ColorFloorB,
                Fx_Tracing,

                GarbageVolume_CollectDeadBiped,
                GarbageVolume_CollectWeapons,
                GarbageVolume_CollectObjectives,
                GarbageVolume_CollectGrenades,
                GarbageVolume_CollectEquipment,
                GarbageVolume_CollectVehicles,
                GarbageVolume_Interval,

                KillVolume_AlwaysVisible,
                KillVolume_DestroyVehicles,
                KillVolume_DamageCause,

                Map_DisablePushBarrier,
                Map_DisableDeathBarrier,
                Map_PhysicsGravity,

                CameraFx_Exposure,
                CameraFx_LightIntensity,
                CameraFx_Bloom,
                CameraFx_LightBloom,

                AtmosphereProperties_Enabled,
                AtmosphereProperties_Weather,
                AtmosphereProperties_Brightness,
                AtmosphereProperties_FogDensity,
                AtmosphereProperties_FogVisibility,
                AtmosphereProperties_FogColorR,
                AtmosphereProperties_FogColorG,
                AtmosphereProperties_FogColorB,
                AtmosphereProperties_Skybox,
                AtmosphereProperties_SkyboxOffsetZ,
                AtmosphereProperties_SkyboxOverrideTransform,

                Budget_Minimum,
                Budget_Maximum
            }

            public enum SetterType : sbyte
            {
                Integer,
                Real
            }

            [Flags]
            public enum SetterFlags : byte
            {
                None,
                Hidden = 1 << 0
            }

            [TagStructure(Size = 0xC)]
            public class Setter : TagStructure
            {
                public SetterTarget Target;
                public SetterType Type;
                public SetterFlags Flags;
                public int IntegerValue;
                public float RealValue;
            }
        }

        [TagStructure(Size = 0x30)]
        public class WeatherEffect : TagStructure
        {
            [TagField(Length = 32)]
            public string Name;

            [TagField(ValidTags = new[] { "effe" })]
            public CachedTag Effect;
        }

        [Flags]
        public enum SkyFlags : int
        {
            None
        }

        [TagStructure(Size = 0xAC)]
        public class Sky : TagStructure
        {
            [TagField(Length = 32)]
            public string Name;

            public SkyFlags Flags;
            public RealPoint3d Translation;
            public RealEulerAngles3d Orientation;

            [TagField(ValidTags = new[] { "scen" })]
            public CachedTag Object;

            [TagField(ValidTags = new[] { "skya" })]
            public CachedTag Parameters;

            [TagField(ValidTags = new[] { "wind" })]
            public CachedTag Wind;

            [TagField(ValidTags = new[] { "cfxs" })]
            public CachedTag CameraFX;

            [TagField(ValidTags = new[] { "sefc" })]
            public CachedTag ScreenFX;

            [TagField(ValidTags = new[] { "chmt" })]
            public CachedTag GlobalLighting;

            [TagField(ValidTags = new[] { "lsnd" })]
            public CachedTag BackgroundSound;
        }
    }
}