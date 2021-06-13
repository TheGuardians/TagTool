using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "loadout_globals_definition", Tag = "lgtd", Size = 0x30)]
    public class LoadoutGlobalsDefinition : TagStructure
    {
        public List<LoadoutDefinitionStruct> Loadouts;
        public List<LoadoutPaletteDefinitionBlock> LoadoutPalettes;
        public List<LoadoutNameBlock> LoadoutNames;
        public List<LoadoutDefinitionStruct> DefaultCustomLoadouts;
        
        [TagStructure(Size = 0x24)]
        public class LoadoutDefinitionStruct : TagStructure
        {
            public StringId LoadoutName;
            // name of an element from custom_app_globals/custom_apps.  Can be left empty.
            public StringId App1;
            // see above
            public StringId App2;
            // name of an element from global starting weapons block. Other values(empty) - unchanged
            // unchanged - unchanged
            // default - take from starting profiles in scenario
            // none - no weapon
            // random - use random item from valid starting weapons
            public StringId InitialPrimaryWeapon;
            public StringId InitialPrimaryWeaponVariantName;
            // see above
            public StringId InitialSecondaryWeapon;
            public StringId InitialSecondaryWeaponVariantName;
            // see above
            public StringId InitialEquipment;
            public PlayerTraitInitialGrenadeCountEnum InitialGrenadeCount;
            [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            
            public enum PlayerTraitInitialGrenadeCountEnum : sbyte
            {
                Unchanged,
                MapDefault,
                _0,
                _1Frag,
                _2Frag,
                _1Plasma,
                _2Plasma,
                _1Type2,
                _2Type2,
                _1Type3,
                _2Type3,
                _1Type4,
                _2Type4,
                _1Type5,
                _2Type5,
                _1Type6,
                _2Type6,
                _1Type7,
                _2Type7
            }
        }
        
        [TagStructure(Size = 0x10)]
        public class LoadoutPaletteDefinitionBlock : TagStructure
        {
            public StringId PaletteName;
            public List<LoadoutIndexBlock> LoadoutChoices;
            
            [TagStructure(Size = 0x4)]
            public class LoadoutIndexBlock : TagStructure
            {
                public short Loadout;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
            }
        }
        
        [TagStructure(Size = 0x4)]
        public class LoadoutNameBlock : TagStructure
        {
            public StringId DisplayName;
        }
    }
}
