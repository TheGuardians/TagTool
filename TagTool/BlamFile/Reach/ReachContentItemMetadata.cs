using System;
using TagTool.Common;

namespace TagTool.BlamFile.Reach
{
    public class ContentItemHistory
    {
        public DateTime Timestamp;
        public ulong AuthorUID;
        public string AuthorName;
        public byte AuthorFlags;

        public void Decode(BitStream stream)
        {
            var timestamp = stream.ReadUnsigned64(64);
            Timestamp = new DateTime(1970, 1, 1).AddSeconds(timestamp);
            AuthorUID = stream.ReadUnsigned(64);
            AuthorName = stream.ReadString(16);
            AuthorFlags = (byte)stream.ReadUnsigned(1);
        }
    }

    public class ReachContentItemMetadata
    {
        public ContentItemType Type;
        public int Size;
        public ulong Uid;
        public ulong ParentUid;
        public ulong RootUid;
        public ulong GameId;
        public int Activity;
        public int GameMode;
        public int GameEngineType;
        public int MapId;
        public int MagaloCategoryIndex;
        public ContentItemHistory CreationHistory;
        public ContentItemHistory ModificationHistory;

        public void Decode(BitStream bitstream)
        {
            Type = (ContentItemType)((int)bitstream.ReadUnsigned(4) - 1);
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
        }
    }
}
