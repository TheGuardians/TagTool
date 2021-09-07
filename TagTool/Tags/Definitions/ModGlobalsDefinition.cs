using System.Collections.Generic;
using TagTool.Cache;
using TagTool.Common;
using System;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "mod_globals", Tag = "modg", Size = 0x118)]
    public class ModGlobalsDefinition : TagStructure
    {
        public List<PlayerCharacterSet> PlayerCharacterSets;

        public List<PlayerCharacterCustomization> PlayerCharacterCustomizations;

        [TagField(Flags = TagFieldFlags.Padding, Length = 0x100)]
        public byte[] Unused = new byte[0x100];

        [TagStructure(Size = 0x34)]
        public class PlayerCharacterSet : TagStructure
        {
            [TagField(Length = 32)]
            public string DisplayName;
            public StringId Name;
            public float RandomChance;
            public List<PlayerCharacter> Characters;

            [TagStructure(Size = 0x28)]
            public class PlayerCharacter : TagStructure
            {
                [TagField(Length = 32)]
                public string DisplayName;
                public StringId Name;
                public float RandomChance;
            }
        }

        [TagStructure(Size = 0xC4)]
        public class PlayerCharacterCustomization : TagStructure
        {
            /// <summary>
            /// Index in Globals.PlayerCharacterType
            /// </summary>
            public int GlobalPlayerCharacterTypeIndex;

            /// <summary>
            /// StringId for the name, enter actual string in the unic tag.
            /// </summary>
            public StringId CharacterName;

            /// <summary>
            /// StringId for the description, enter actual string in the unic tag.
            /// </summary>
            public StringId CharacterDescription;

            [TagField(ValidTags = new[] { "chgd" })]
            public CachedTag HudGlobals;

            [TagField(ValidTags = new[] { "vmdx" })]
            public CachedTag VisionGlobals;

            [TagField(ValidTags = new[] { "pact" })]
            public CachedTag ActionSet;

            public List<PlayerCharacterRegionScript> RegionCameraScripts;

            public CharacterPositionInfo CharacterPositionData;

            public PlayerCharacterColors CharacterColors;

            [TagStructure(Size = 0x6C)]
            public class PlayerCharacterRegionScript : TagStructure
            {
                public int unused;
                [TagField(Length = 32)]
                public string RegionName;
                [TagField(Length = 32)]
                public string ScriptNameWidescreen;
                [TagField(Length = 32)]
                public string ScriptNameStandard;
                public float BipedRotation;
                public float RotationDuration;
            };

            [TagStructure(Size = 0x40)]
            public class PlayerCharacterColors : TagStructure
            {
                public ChangeColorFlagsValue ValidColorFlags;
                public ChangeColorFlagsValue TeamOverrideFlags;
                public short Unused;
                [TagField(Length = 5)]
                public ChangeColorBlock[] Colors;

                [Flags]
                public enum ChangeColorFlagsValue : byte
                {
                    None = 0,
                    PrimaryColor = 1 << 0,
                    SecondaryColor = 1 << 1,
                    TertiaryColor = 1 << 2,
                    QuaternaryColor = 1 << 3,
                    QuinaryColor = 1 << 4
                }

                [TagStructure(Size = 0xC)]
                public class ChangeColorBlock : TagStructure
                {
                    public StringId Name;
                    public StringId Description;
                    public ArgbColor Default;
                }
            };

            [TagStructure(Size = 0x3C)]
            public class CharacterPositionInfo : TagStructure
            {
                /// <summary>
                /// Character Flags
                /// </summary>
                public FlagsValue flags;

                /// <summary>
                /// Index to Object name in the mainmenu scenario
                /// </summary>
                public int BipedNameIndex;

                /// <summary>
                /// Index to Settings Camera in the mainmenu scenario
                /// </summary>
                public int SettingsCameraIndex;

                /// <summary>
                /// Index to the Character Platform name
                /// </summary>
                public int PlatformNameIndex;

                /// <summary>
                /// RealVector3D to offset the biped on the customization screen by a specificed amount
                /// </summary>
                public RealVector3d RelativeBipedPosition;

                /// <summary>
                /// Float to offset the bipeds rotation on the customization screen by a specificed amount
                /// </summary>
                public float RelativeBipedRotation;

                /// <summary>
                /// RealVector3D to point to the exact position of the biped in the customization screen with the relative flag not set
                /// </summary>
                public RealPoint3d BipedPositionWidescreen;

                public RealPoint3d BipedPositionStandard;

                /// <summary>
                /// float to point to the exact rotation of the biped with the relative flag not set
                /// </summary>
                public float BipedRotation;

                [Flags]
                public enum FlagsValue : int
                {
                    None = 0,
                    PlaceBipedRelativeToCamera = 1 << 0,
                    CanRotateOnMainMenu = 1 << 1,
                    HasPlatform = 1 << 2,
                    RotateInCustomization = 1 << 3
                }
            };
        }
    }
}