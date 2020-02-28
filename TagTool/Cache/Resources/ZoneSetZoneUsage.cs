using System;
using TagTool.Common;
using TagTool.Tags;

namespace TagTool.Cache.Resources
{
    [TagStructure(Size = 0x24)]
    public class ZoneSetZoneUsage : TagStructure
    {
        [TagField(Flags = TagFieldFlags.Label)]
        public StringId Name;

        public int BspGroupIndex;

        public BspFlagsValue ImportLoadedBsps;
        public BspFlagsValue LoadedBsps;

        public ZonesetFlagsValue LoadedDesignerZonesets;
        public ZonesetFlagsValue UnknownLoadedDesignerZonesets;
        public ZonesetFlagsValue UnloadedDesignerZonesets;
        public ZonesetFlagsValue LoadedCinematicZonesets;

        public int BspAtlasIndex;

        [Flags]
        public enum BspFlagsValue : int
        {
            None = 0,
            Bsp0 = 1 << 0,
            Bsp1 = 1 << 1,
            Bsp2 = 1 << 2,
            Bsp3 = 1 << 3,
            Bsp4 = 1 << 4,
            Bsp5 = 1 << 5,
            Bsp6 = 1 << 6,
            Bsp7 = 1 << 7,
            Bsp8 = 1 << 8,
            Bsp9 = 1 << 9,
            Bsp10 = 1 << 10,
            Bsp11 = 1 << 11,
            Bsp12 = 1 << 12,
            Bsp13 = 1 << 13,
            Bsp14 = 1 << 14,
            Bsp15 = 1 << 15,
            Bsp16 = 1 << 16,
            Bsp17 = 1 << 17,
            Bsp18 = 1 << 18,
            Bsp19 = 1 << 19,
            Bsp20 = 1 << 20,
            Bsp21 = 1 << 21,
            Bsp22 = 1 << 22,
            Bsp23 = 1 << 23,
            Bsp24 = 1 << 24,
            Bsp25 = 1 << 25,
            Bsp26 = 1 << 26,
            Bsp27 = 1 << 27,
            Bsp28 = 1 << 28,
            Bsp29 = 1 << 29,
            Bsp30 = 1 << 30,
            Bsp31 = 1 << 31
        }

        [Flags]
        public enum ZonesetFlagsValue : int
        {
            None = 0,
            Set0 = 1 << 0,
            Set1 = 1 << 1,
            Set2 = 1 << 2,
            Set3 = 1 << 3,
            Set4 = 1 << 4,
            Set5 = 1 << 5,
            Set6 = 1 << 6,
            Set7 = 1 << 7,
            Set8 = 1 << 8,
            Set9 = 1 << 9,
            Set10 = 1 << 10,
            Set11 = 1 << 11,
            Set12 = 1 << 12,
            Set13 = 1 << 13,
            Set14 = 1 << 14,
            Set15 = 1 << 15,
            Set16 = 1 << 16,
            Set17 = 1 << 17,
            Set18 = 1 << 18,
            Set19 = 1 << 19,
            Set20 = 1 << 20,
            Set21 = 1 << 21,
            Set22 = 1 << 22,
            Set23 = 1 << 23,
            Set24 = 1 << 24,
            Set25 = 1 << 25,
            Set26 = 1 << 26,
            Set27 = 1 << 27,
            Set28 = 1 << 28,
            Set29 = 1 << 29,
            Set30 = 1 << 30,
            Set31 = 1 << 31
        }
    }
}