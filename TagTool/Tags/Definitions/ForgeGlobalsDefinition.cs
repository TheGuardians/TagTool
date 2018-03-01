using System;
using System.Collections.Generic;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Serialization;

namespace TagTool.Tags.Definitions
{
    /// <summary>
    /// ElDewrito Forge globals.
    /// </summary>
    [TagStructure(Name = "forge_globals_definition", Tag = "forg", Size = 0x80)]
    public struct ForgeGlobalsDefinition
    {
        /// <summary>
        /// The "invisible" render method.
        /// </summary>
        [TagField(ValidTags = new[] { "rm  " })]
        public CachedTagInstance InvisibleRenderMethod;

        /// <summary>
        /// The "default" render method used on ReForge objects.
        /// </summary>
        [TagField(ValidTags = new[] { "rm  " })]
        public CachedTagInstance DefaultRenderMethod;

        /// <summary>
        /// The collection of materials available to ReForge objects.
        /// </summary>
        public List<ReForgeMaterial> ReForgeMaterials;

        /// <summary>
        /// The object designated as the "prematch camera" handle.
        /// </summary>
        [TagField(ValidTags = new[] { "obje" })]
        public CachedTagInstance PrematchCameraObject;

        /// <summary>
        /// The object designated as the "map modifier" handle.
        /// </summary>
        [TagField(ValidTags = new[] { "obje" })]
        public CachedTagInstance ModifierObject;

        /// <summary>
        /// The object designated as the "kill volume" handle.
        /// </summary>
        [TagField(ValidTags = new[] { "obje" })]
        public CachedTagInstance KillVolumeObject;

        /// <summary>
        /// The object designed as the "garbage volume" object.
        /// </summary>
        [TagField(ValidTags = new[] { "obje" })]
        public CachedTagInstance GarbageVolumeObject;

        /// <summary>
        /// The list of items available to the Forge palette.
        /// </summary>
        public List<PaletteItem> Palette;

        /// <summary>
        /// The collection of weather effects used in Forge.
        /// </summary>
        public List<WeatherEffect> WeatherEffects;

        /// <summary>
        /// The collection of skies used in Forge.
        /// </summary>
        public List<Sky> Skies;

        /// <summary>
        /// A material reference for ReForge objects.
        /// </summary>
        [TagStructure(Size = 0x30)]
        public struct ReForgeMaterial
        {
            [TagField(Length = 32)]
            public string Name;

            [TagField(ValidTags = new[] { "rm  " })]
            public CachedTagInstance RenderMethod;
        }

        /// <summary>
        /// The category of a Forge object palette item.
        /// </summary>
        public enum PaletteItemCategory : short
        {
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

        /// <summary>
        /// A single item of the Forge object palette.
        /// </summary>
        [TagStructure(Size = 0x40)]
        public struct PaletteItem
        {
            /// <summary>
            /// The name of the palette item.
            /// </summary>
            [TagField(Length = 32)]
            public string Name;

            /// <summary>
            /// The category of the palette item.
            /// </summary>
            public PaletteItemCategory Category;

            /// <summary>
            /// The maximum number of the palette item allowed to be spawned.
            /// </summary>
            public ushort MaxAllowed;

            /// <summary>
            /// The object associated with the palette item.
            /// </summary>
            [TagField(ValidTags = new[] { "obje" })]
            public CachedTagInstance Object;

            /// <summary>
            /// The list of property setters of the palette item.
            /// </summary>
            public List<Setter> Setters;

            /// <summary>
            /// The type of a property being managed by a property setter.
            /// </summary>
            public enum SetterType : short
            {
                Boolean,
                Integer,
                Real
            }

            /// <summary>
            /// The flags of a property setter.
            /// </summary>
            [Flags]
            public enum SetterFlags : byte
            {
                None,
                Hidden = 1 << 0
            }

            /// <summary>
            /// A property setter descriptor.
            /// </summary>
            [TagStructure(Size = 0x2C)]
            public struct Setter
            {
                /// <summary>
                /// The target of the property setter.
                /// </summary>
                [TagField(Length = 32)]
                public string Target;

                /// <summary>
                /// The type of the property being set.
                /// </summary>
                public SetterType Type;

                /// <summary>
                /// The flags of the property
                /// </summary>
                public SetterFlags Flags;

                /// <summary>
                /// The boolean value of the property setter.
                /// </summary>
                public bool BooleanValue;

                /// <summary>
                /// The integer value of the property setter.
                /// </summary>
                public int IntegerValue;

                /// <summary>
                /// The real value of the property setter.
                /// </summary>
                public float RealValue;
            }
        }

        /// <summary>
        /// A weather effect reference descriptor.
        /// </summary>
        [TagStructure(Size = 0x30)]
        public struct WeatherEffect
        {
            /// <summary>
            /// The name of the weather effect.
            /// </summary>
            [TagField(Length = 32)]
            public string Name;

            /// <summary>
            /// The effect associated with the weather effect.
            /// </summary>
            [TagField(ValidTags = new[] { "effe" })]
            public CachedTagInstance Effect;
        }

        /// <summary>
        /// A Forge sky descriptor.
        /// </summary>
        [TagStructure(Size = 0xA8)]
        public struct Sky
        {
            /// <summary>
            /// The name of the sky.
            /// </summary>
            [TagField(Length = 32)]
            public string Name;
            
            /// <summary>
            /// The default translation of the sky.
            /// </summary>
            public RealPoint3d Translation;
            
            /// <summary>
            /// The default orientation of the sky.
            /// </summary>
            public RealEulerAngles3d Orientation;

            /// <summary>
            /// The object associated with the sky.
            /// </summary>
            [TagField(ValidTags = new[] { "scen" })]
            public CachedTagInstance Object;

            /// <summary>
            /// The parameters of the sky.
            /// </summary>
            [TagField(ValidTags = new[] { "skya" })]
            public CachedTagInstance Parameters;

            /// <summary>
            /// The wind of the sky.
            /// </summary>
            [TagField(ValidTags = new[] { "wind" })]
            public CachedTagInstance Wind;

            /// <summary>
            /// The camera effects of the sky.
            /// </summary>
            [TagField(ValidTags = new[] { "cfxs" })]
            public CachedTagInstance CameraFX;

            /// <summary>
            /// The screen effects of the sky.
            /// </summary>
            [TagField(ValidTags = new[] { "sefc" })]
            public CachedTagInstance ScreenFX;

            /// <summary>
            /// The global lighting of the sky.
            /// </summary>
            [TagField(ValidTags = new[] { "chmt" })]
            public CachedTagInstance GlobalLighting;

            /// <summary>
            /// The background sound of the sky.
            /// </summary>
            [TagField(ValidTags = new[] { "lsnd" })]
            public CachedTagInstance BackgroundSound;
        }
    }
}