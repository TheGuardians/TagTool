using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "multiplayer_object_type_list", Tag = "motl", Size = 0x70)]
    public class MultiplayerObjectTypeList : TagStructure
    {
        public List<MultiplayerObjectTypeBlock> ObjectTypes;
        public List<MultiplayerObjectCollectionStruct> Weapons;
        public List<MultiplayerObjectCollectionStruct> Vehicles;
        public List<MultiplayerObjectCollectionStruct> Grenades;
        public List<MultiplayerObjectCollectionStruct> Equipment;
        public List<MultiplayerWeaponRemapTableBlock> WeaponRemapping;
        public List<MultiplayerVehicleRemapTableBlock> VehicleRemapping;
        public List<MultiplayerEquipmentRemapTableBlock> EquipmentRemapping;
        public int RandomWeaponMenuSpriteFrame;
        public int RandomEquipmentMenuSpriteFrame;
        public StringId RandomWeaponIconStringId;
        public StringId RandomEquipmentIconStringId;
        
        [TagStructure(Size = 0x14)]
        public class MultiplayerObjectTypeBlock : TagStructure
        {
            public StringId Name;
            [TagField(ValidTags = new [] { "obje" })]
            public CachedTag Object;
        }
        
        [TagStructure(Size = 0x1C)]
        public class MultiplayerObjectCollectionStruct : TagStructure
        {
            public int ObjectType;
            public StringId ObjectDescription;
            public StringId ObjectHeaderText;
            public StringId ObjectHelpText;
            public float RandomWeight;
            public int LoadoutMenuSpriteFrame;
            public StringId IconStringId;
        }
        
        [TagStructure(Size = 0x10)]
        public class MultiplayerWeaponRemapTableBlock : TagStructure
        {
            public StringId Name;
            public List<MultiplayerWeaponRemapTableEntryBlock> RemapTable;
            
            [TagStructure(Size = 0x8)]
            public class MultiplayerWeaponRemapTableEntryBlock : TagStructure
            {
                public int FromObject;
                public int ToObject;
            }
        }
        
        [TagStructure(Size = 0x10)]
        public class MultiplayerVehicleRemapTableBlock : TagStructure
        {
            public StringId Name;
            public List<MultiplayerVehicleRemapTableEntryBlock> RemapTable;
            
            [TagStructure(Size = 0x8)]
            public class MultiplayerVehicleRemapTableEntryBlock : TagStructure
            {
                public int FromObject;
                public int ToObject;
            }
        }
        
        [TagStructure(Size = 0x10)]
        public class MultiplayerEquipmentRemapTableBlock : TagStructure
        {
            public StringId Name;
            public List<MultiplayerEquipmentRemapTableEntryBlock> RemapTable;
            
            [TagStructure(Size = 0x8)]
            public class MultiplayerEquipmentRemapTableEntryBlock : TagStructure
            {
                public int FromObject;
                public int ToObject;
            }
        }
    }
}
