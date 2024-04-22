using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "game_globals_ordnance_list", Tag = "ggol", Size = 0x34)]
    public class GameGlobalsOrdnanceList : TagStructure
    {
        public float OrdnanceMapWidth; // world units
        public float RandomOrdnanceFanfareDuration; // seconds
        [TagField(ValidTags = new [] { "effe" })]
        public CachedTag DropPodCleanupEffect;
        public List<GameGlobalsOrdnanceBlock> Ordnances;
        public List<OrdnanceRemappingVariantBlock> OrdnanceRemappingTables;
        public float EquipmentInvulnerableSeconds;
        
        [TagStructure(Size = 0x4C)]
        public class GameGlobalsOrdnanceBlock : TagStructure
        {
            public StringId OrdnanceName;
            [TagField(Length = 32)]
            public string OrdnanceInternalName;
            public int ActivationPointCost;
            // is pod with this power weapon, else if blank is remote strike (remote strike equipment)
            public StringId DropPodVariantName;
            [TagField(ValidTags = new [] { "eqip" })]
            public CachedTag RemoteStrikeEquipment;
            public byte SequenceIndex;
            public byte EquipmentCount;
            public GuiOrdnancePrimiumFlag PremiumFlag;
            [TagField(Length = 0x1, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public RealPoint3d NavpointMarkerOffset;
            
            [Flags]
            public enum GuiOrdnancePrimiumFlag : byte
            {
                PremiumOrdnance = 1 << 0
            }
        }
        
        [TagStructure(Size = 0x30)]
        public class OrdnanceRemappingVariantBlock : TagStructure
        {
            public StringId Name;
            [TagField(Length = 32)]
            public string InternalName;
            public List<OrdnanceRemappingBlock> Remappings;
            
            [TagStructure(Size = 0x8)]
            public class OrdnanceRemappingBlock : TagStructure
            {
                // This must match one of the global ordnance objects.
                public StringId From;
                // This must match one of the global ordnance objects.
                public StringId To;
            }
        }
    }
}
