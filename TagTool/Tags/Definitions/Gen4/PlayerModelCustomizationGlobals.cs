using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "player_model_customization_globals", Tag = "pmcg", Size = 0xC8)]
    public class PlayerModelCustomizationGlobals : TagStructure
    {
        // used for string list generation
        [TagField(ValidTags = new [] { "mode" })]
        public CachedTag SpartanRenderModel;
        [TagField(ValidTags = new [] { "mode" })]
        public CachedTag EliteRenderModel;
        public List<CustomizedModelSelectionBlock> HelmetSelections;
        public List<CustomizedModelSelectionBlock> ChestSelections;
        public List<CustomizedModelSelectionBlock> EliteSelections;
        public List<CustomizedModelSelectionBlock> LeftShoulder;
        public List<CustomizedModelSelectionBlock> RightShoulder;
        public List<CustomizedModelSelectionBlock> Arms;
        public List<CustomizedModelSelectionBlock> Legs;
        public List<CustomizedModelSelectionBlock> Unused5;
        public List<CustomizedModelSelectionBlock> SpartanArmorEffectSelections;
        public List<CustomizedModelSelectionBlock> EliteArmorEffectSelections;
        public List<CustomizedModelPlayerBitsBlock> MaleSpartanSelections;
        public List<CustomizedModelPlayerBitsBlock> FemaleSpartanSelections;
        public List<CustomizedModelPlayerBitsBlock> SpartanModelDefaults;
        public List<CustomizedModelPlayerBitsBlock> EliteModelDefaults;
        
        [TagStructure(Size = 0x14)]
        public class CustomizedModelSelectionBlock : TagStructure
        {
            public StringId SelectionName;
            // Which player-stats modifier should be activated when this item is equipped
            public StringId AppName;
            public List<CustomizedModelPlayerBitsBlock> CustomizedBits;
            
            [TagStructure(Size = 0xC)]
            public class CustomizedModelPlayerBitsBlock : TagStructure
            {
                public StringId RegionName;
                public StringId PermutationName;
                public ModelCustomizationPlayerPermutationFlags Flags;
                [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                
                [Flags]
                public enum ModelCustomizationPlayerPermutationFlags : byte
                {
                    // this permutation only applies to male players
                    MaleOnly = 1 << 0,
                    // this permutation only applies to female players
                    FemaleOnly = 1 << 1,
                    // this permutation is an elite with enclosed helmet
                    MandiblesHidden = 1 << 2
                }
            }
        }
        
        [TagStructure(Size = 0xC)]
        public class CustomizedModelPlayerBitsBlock : TagStructure
        {
            public StringId RegionName;
            public StringId PermutationName;
            public ModelCustomizationPlayerPermutationFlags Flags;
            [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            
            [Flags]
            public enum ModelCustomizationPlayerPermutationFlags : byte
            {
                // this permutation only applies to male players
                MaleOnly = 1 << 0,
                // this permutation only applies to female players
                FemaleOnly = 1 << 1,
                // this permutation is an elite with enclosed helmet
                MandiblesHidden = 1 << 2
            }
        }
    }
}
