using System;
using TagTool.Common;

namespace TagTool.BlamFile.Reach
{
    public class ContentItemHistory
    {
        public DateTimeOffset Timestamp;
        public ulong AuthorUID;
        public string AuthorName;
        public bool AuthorIsOnline;

        public void Decode(BitStream stream)
        {
            var timestamp = (long)stream.ReadUnsigned64(64);  
            Timestamp = DateTimeOffset.FromUnixTimeSeconds(timestamp);
            AuthorUID = stream.ReadUnsigned(64);
            AuthorName = stream.ReadString(16);
            AuthorIsOnline = stream.ReadUnsigned(1) != 0;
        }
    }

    public class ReachContentItemMetadata
    {
        public ReachContentItemType Type;
        public int Size;
        public ulong Uid;
        public ulong ParentUid;
        public ulong RootUid;
        public ulong GameId;
        public int Activity;
        public int GameMode;
        public int GameEngineType;
        public int MapId = -1;
        public int MagaloCategoryIndex;
        public ContentItemHistory CreationHistory;
        public ContentItemHistory ModificationHistory;
        public int Filmduration;
        public int GameVariantIconIndex = -1;
        public int HopperId = -1;
        public int CampaignId = -1;
        public int CampaignDifficulty = -1;
        public int CampaignMetagameScoring;
        public int CampaignInsertionPoint = -1;
        public uint CampaignSkulls;

        public void Decode(BitStream bitstream)
        {
            Type = (ReachContentItemType)((int)bitstream.ReadUnsigned(4) - 1);
            Size = (int)bitstream.ReadUnsigned(32);
            Uid = bitstream.ReadUnsigned64(64);
            ParentUid = bitstream.ReadUnsigned64(64);
            RootUid = bitstream.ReadUnsigned64(64);
            GameId = bitstream.ReadUnsigned64(64);
            Activity = (int)bitstream.ReadUnsigned(3) - 1;
            GameMode = (int)bitstream.ReadUnsigned(3);
            GameEngineType = (int)bitstream.ReadUnsigned(3);
            MapId = (int)bitstream.ReadUnsigned(32);
            MagaloCategoryIndex = (sbyte)bitstream.ReadUnsigned(8);
            CreationHistory = new ContentItemHistory();
            ModificationHistory = new ContentItemHistory();
            CreationHistory.Decode(bitstream);
            ModificationHistory.Decode(bitstream);

            if(Type == ReachContentItemType.Film || Type == ReachContentItemType.FilmClip)
            {
                Filmduration = (int)bitstream.ReadUnsigned(32);
            }
            else if(Type == ReachContentItemType.GameVariant)
            {
                GameVariantIconIndex = (byte)bitstream.ReadUnsigned(8);
            }

            if (Activity == 2) // matchmaking
                HopperId = (short)bitstream.ReadUnsigned(16);

            if (GameMode == 1) // campaign
            {
                CampaignId = (int)bitstream.ReadUnsigned(8);
                CampaignDifficulty = (int)bitstream.ReadUnsigned(2);
                CampaignMetagameScoring = (int)bitstream.ReadUnsigned(2);
                CampaignInsertionPoint = (int)bitstream.ReadUnsigned(2);
                CampaignSkulls = bitstream.ReadUnsigned(32);
            }
            else if (GameMode == 2) // firefight
            {
                CampaignDifficulty = (int)bitstream.ReadUnsigned(8);
                CampaignSkulls = bitstream.ReadUnsigned(32);
            }
        }
    }

    public enum ReachContentItemType : int
    {
        Unknown0,
        Unknown1,
        Unknown2,
        Film,
        FilmClip,
        MapVariant,
        GameVariant
    }
}
