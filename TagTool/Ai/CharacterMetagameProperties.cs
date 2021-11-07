using System;
using TagTool.Tags;

namespace TagTool.Ai
{
    [TagStructure(Size = 0x8)]
    public class CharacterMetagameProperties : TagStructure
	{
        public CampaignMetagameBucketFlags Flags;
        public CampaignMetagameBucketTypeEnum Type;
        public CampaignMetagameBucketClassEnum Class;
        [TagField(Length = 1, Flags = TagFieldFlags.Padding)]
        public byte[] Padding;
        public short PointCount;
        [TagField(Length = 2, Flags = TagFieldFlags.Padding)]
        public byte[] Padding1;

        [Flags]
        public enum CampaignMetagameBucketFlags : byte
        {
            OnlyCountsWithRiders = 1 << 0
        }

        public enum CampaignMetagameBucketTypeEnum : sbyte
        {
            Brute,
            Grunt,
            Jackel,
            Marine,
            Bugger,
            Hunter,
            FloodInfection,
            FloodCarrier,
            FloodCombat,
            FloodPure,
            Sentinel,
            Elite,
            Turret,
            Mongoose,
            Warthog,
            Scorpion,
            Hornet,
            Pelican,
            Shade,
            Watchtower,
            Ghost,
            Chopper,
            Mauler,
            Wraith,
            Banshee,
            Phantom,
            Scarab,
            Guntower
        }

        public enum CampaignMetagameBucketClassEnum : sbyte
        {
            Infantry,
            Leader,
            Hero,
            Specialist,
            LightVehicle,
            HeavyVehicle,
            GiantVehicle,
            StandardVehicle
        }
    }
}